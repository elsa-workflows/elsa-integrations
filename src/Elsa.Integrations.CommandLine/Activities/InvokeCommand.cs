using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using CliWrap;
using CliWrap.Buffered;
using Elsa.Integrations.CommandLine.Contracts;
using Elsa.Integrations.CommandLine.Options;
using Elsa.Integrations.CommandLine.Services;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using Elsa.Workflows.UIHints;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Elsa.Integrations.CommandLine.Activities;

/// <summary>
/// Activity that invokes a command line process using CliWrap.
/// See CliWrap documentation for more information: <see cref="https://github.com/Tyrrrz/CliWrap"/>
/// </summary>
[Activity(
    "Elsa.Integrations.CommandLine",
    DisplayName = "Invoke Command",
    Description = "Invoke a command line process.",
    Category = "Commands",
    Kind = ActivityKind.Task
)]
[UsedImplicitly]
public class InvokeCommand : Activity
{
    private ILogger<InvokeCommand> _logger = null!;
    private ICommandRunner _commandRunner = null!;
    private CommandLineOptions? _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="InvokeCommand"/> class.
    /// </summary>
    public InvokeCommand([CallerFilePath] string? source = default, [CallerLineNumber] int? line = default) : base(source, line)
    {
    }

    /// <summary>
    /// The command to execute.
    /// Can be passed as a string of the executable name or a CliWrap.Command object.
    /// </summary>
    [Input(
        Description = "The command to execute. Can be passed as a string of the executable name or a CliWrap.Command object.",
        UIHandler = typeof(InvokeCommandUIHandler),
        UIHint = InputUIHints.DropDown
    )]
    public required Input<object> Command { get; set; }

    /// <summary>
    /// The arguments to pass to the command. This can be an array of strings or a single string.
    /// </summary>
    [Input(
        Description = "The arguments to pass to the command. Can be an array/collection of strings or a single string."
    )]
    public Input<object> Arguments { get; set; } = null!;

    /// <summary>
    /// The working directory to execute the command in.
    /// </summary>
    [Input(
        Name = "Working Directory",
        Description = "The working directory to execute the command in."
    )]
    public Input<string?> WorkingDirectory { get; set; } = null!;
    
    /// <summary>
    /// Environment variables to set for the command.
    /// </summary>
    [Input(
        Name = "Environment Variables",
        Description = "Environment variables to set for the command as key-value pairs."
    )]
    public Input<IDictionary<string, string?>?> EnvironmentVariables { get; set; } = null!;

    /// <summary>
    /// Credentials to run the command under of type NetworkCredential.
    /// </summary>
    [Input(
        Description = "Credentials to run the command under of type NetworkCredential."
    )]
    public Input<NetworkCredential> Credentials { get; set; } = null!;

    /// <summary>
    /// Useful for piping the StandardOutputPipe or StandardErrorPipe from another InvokeCommand activity into this command.
    /// Possible types include Stream, File, byte array, string (file path or raw text), or another CliWrap.Command object.
    /// </summary>
    [Input(
        Name = "Standard Input Pipe",
        Description = "Useful for piping the StandardOutputPipe or StandardErrorPipe from another InvokeCommand activity into this command. Possible types include Stream, File, byte array, string (file path or raw text), or another CliWrap.Command object.")]
    public Input<object> StandardInputPipe { get; set; } = null!;

    /// <summary>
    /// The expected exit code for a successful command execution. If null, exit code validation is disabled.
    /// </summary>
    [Input(
        Name = "Successful Exit Code",
        Description = "The expected exit code for a successful command execution. Exit code 0 is assumed successful by default."
    )]
    public Input<int?> SuccessfulExitCode { get; set; } = null!;

    /// <summary>
    /// A string contained in the standard output for a successful command execution.
    /// If blank, output text validation is disabled. Enable RegEx by adding leading and trailing '/' characters to your expression.
    /// </summary>
    [Input(
        Name = "Successful Output Text",
        Description = "A string contained in the standard output for a successful command execution. If blank, output text validation is disabled. RegEx is enabled by adding a leading and trailing '/' character to the expression."
    )]
    public Input<string> SuccessfulOutputText { get; set; } = null!;

    /// <summary>
    /// The timeout for the command execution. Accepts TimeSpan object or TimeSpan format string or int (seconds). If blank, no timeout is applied.
    /// </summary>
    [Input(
        Description = "The timeout for the command execution. Accepts TimeSpan object or TimeSpan format string or int (seconds). If blank, no timeout is applied."
    )]
    public Input<object> Timeout { get; set; } = null!;

    /// <summary>
    /// Determines how cancellation requests are handled. When true, allows processes to shut down gracefully.
    /// When false (default), processes are terminated immediately.
    /// </summary>
    [Input(
        Name = "Graceful Cancellation",
        Description = "When true, allows processes to shut down gracefully on its own terms. When false (default), processes are terminated immediately."
    )]
    public Input<bool> GracefulCancellation { get; set; } = new(false);

    /// <summary>
    /// The CliWrap command instance that was used to invoke the CLI command.
    /// </summary>
    [Output(
        Name = "Executed Command",
        Description = "The CliWrap Command instance that was used to invoke the CLI command.")]
    public Output<Command> ExecutedCommand { get; set; } = null!;

    /// <summary>
    /// Indicates whether the command execution was successful based on the configured validation criteria.
    /// </summary>
    [Output(
        Name = "Success",
        Description = "Indicates whether the command execution was successful based on the configured validation criteria.")]
    public Output<bool> Success { get; set; } = null!;

    /// <summary>
    /// The standard output string from the command execution.
    /// </summary>
    [Output(
        Name = "Standard Output String",
        Description = "The standard output string from the command execution.")]
    public Output<string> StandardOutputString { get; set; } = null!;

    /// <summary>
    /// The standard output pipe from the command execution.
    /// Useful for piping the standard output of this command into other InvokeCommand activities.
    /// </summary>
    [Output(
        Name = "Standard Output Pipe",
        Description = "The standard output pipe from the command execution. Useful for piping the standard output of this command into other InvokeCommand activities.")]
    public Output<object> StandardOutputPipe { get; set; } = null!;

    /// <summary>
    /// The standard error string from the command execution.
    /// </summary>
    [Output(
        Name = "Standard Error String",
        Description = "The standard error string from the command execution.")]
    public Output<string> StandardErrorString { get; set; } = null!;

    /// <summary>
    /// The standard error output pipe from the command execution.
    /// Useful for piping the standard output of this command into other InvokeCommand activities.
    /// </summary>
    [Output(
        Name = "Standard Error Pipe",
        Description = "The standard error output pipe from the command execution. Useful for piping the standard output of this command into other InvokeCommand activities.")]
    public Output<object> StandardErrorPipe { get; set; } = null!;

    /// <summary>
    /// The exit code from the command execution.
    /// </summary>
    [Output(
        Name = "Exit Code",
        Description = "The exit code from the command execution.")]
    public Output<int> ExitCode { get; set; } = null!;

    /// <summary>
    /// The start time of the command execution.
    /// </summary>
    [Output(
        Name = "Start Time",
        Description = "The start time of the command execution.")]
    public Output<DateTimeOffset> StartTime { get; set; } = null!;

    /// <summary>
    /// The exit time of the command execution.
    /// </summary>
    [Output(
        Name = "Exit Time",
        Description = "The exit time of the command execution.")]
    public Output<DateTimeOffset> ExitTime { get; set; } = null!;

    /// <summary>
    /// The runtime of the command execution.
    /// </summary>
    [Output(
        Name = "Run Time",
        Description = "The runtime of the command execution.")]
    public Output<TimeSpan> RunTime { get; set; } = null!;

    /// <summary>
    /// Convenience pipeline used to call the next InvokeCommand activity to pipe the result of this activity into.
    /// Connects the standard output of this command to the standard input of the next command.
    /// </summary>
    [Port] public InvokeCommand? Pipeline { get; set; }

    /// <summary>
    /// The activity to execute when this command exits with an error.
    /// </summary>
    [Port] public IActivity? Error { get; set; }

    /// <inheritdoc />
    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        _logger = context.GetRequiredService<ILogger<InvokeCommand>>();
        _commandRunner = context.GetRequiredService<ICommandRunner>();
        _options = context.GetRequiredService<IOptions<CommandLineOptions>>().Value;

        Command cmd = BuildCommand(context);

        try
        {
            CancellationToken cancellationToken = GetCancellationToken(context);
            bool gracefulCancellation = context.Get(GracefulCancellation);
            BufferedCommandResult result = await _commandRunner.ExecuteCommandAsync(cmd, gracefulCancellation, cancellationToken);

            // Store outputs
            context.Set(ExecutedCommand, cmd);
            context.Set(Success, ValidateExecution(context, result));
            context.Set(StandardOutputString, result.StandardOutput);
            context.Set(StandardOutputPipe, cmd.StandardOutputPipe);
            context.Set(StandardErrorString, result.StandardError);
            context.Set(StandardErrorPipe, cmd.StandardErrorPipe);
            context.Set(ExitCode, result.ExitCode);
            context.Set(StartTime, result.StartTime);
            context.Set(ExitTime, result.ExitTime);
            context.Set(RunTime, result.RunTime);

            if (Pipeline != null)
            {
                Pipeline.StandardInputPipe = new Input<object>(cmd);
                await context.ScheduleActivityAsync(Pipeline, CompletionCallback);
            }

            _logger.LogInformation("Command: {Command} {Arguments}\nFinished in {RunTime} with exit code {ExitCode}.",
                cmd.TargetFilePath, cmd.Arguments, result.RunTime, result.ExitCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing command {Command}", cmd.ToString());
            context.Set(StandardErrorString, ex.ToString());
            context.Set(Success, false);

            if (Error != null)
            {
                await context.ScheduleActivityAsync(Error, OnErrorCompletedAsync);
            }
        }

        await context.CompleteActivityAsync("Done");
    }

    private async ValueTask CompletionCallback(ActivityCompletedContext context) =>
        await context.TargetContext.CompleteActivityAsync("Done");

    private async ValueTask OnErrorCompletedAsync(ActivityCompletedContext context) =>
        await context.TargetContext.CompleteActivityAsync("Error");

    /// <summary>
    /// Builds the command to execute based on the activity inputs.
    /// </summary>
    /// <param name="context">The activity execution context.</param>
    /// <returns>The command to be executed.</returns>
    private Command BuildCommand(ActivityExecutionContext context)
    {
        // Get the command to execute
        object? commandInput = context.Get(Command);
        Command cmd = commandInput switch
        {
            Command command => command,
            string stringCommand => Cli.Wrap(stringCommand),
            _ => throw new ArgumentException("Command must be either a CliWrap Command object or a string.", nameof(Command))
        };

        // Build standard input pipe if available
        object? standardInputPipe = context.Get(StandardInputPipe);
        if(standardInputPipe != null)
        {
            PipeSource pipeSource = BuildPipeSource(standardInputPipe);
            cmd = cmd.WithStandardInputPipe(pipeSource);
        }

        // Build standard output pipe if available
        object? standardOutputPipe = context.Get(StandardOutputPipe);
        if (standardOutputPipe != null)
        {
            PipeTarget pipeTarget = BuildPipeTarget(standardOutputPipe);
            cmd = cmd.WithStandardOutputPipe(pipeTarget);
        }

        // Build standard error pipe if available
        object? standardErrorPipe = context.Get(StandardErrorPipe);
        if (standardErrorPipe != null)
        {
            PipeTarget pipeTarget = BuildPipeTarget(standardErrorPipe);
            cmd = cmd.WithStandardErrorPipe(pipeTarget);
        }

        // Validation is handled by the command runner
        cmd.WithValidation(CommandResultValidation.None);

        // Apply arguments
        object? arguments = context.Get(Arguments);
        if (arguments != null)
        {
            if (arguments is string[] argsArray)
            {
                cmd = cmd.WithArguments(argsArray);
            }
            else if (arguments is ICollection<string> argsCollection)
            {
                cmd = cmd.WithArguments(argsCollection);
            }
            else if (arguments is string argsStr)
            {
                cmd = cmd.WithArguments(argsStr);
            }
            else if (arguments is IEnumerable<string> argsEnumerable)
            {
                cmd = cmd.WithArguments(argsEnumerable.ToArray());
            }
        }

        // Apply working directory
        string? workingDirectory = context.Get(WorkingDirectory);
        if (!string.IsNullOrEmpty(workingDirectory))
        {
            cmd = cmd.WithWorkingDirectory(workingDirectory);
        }
        else if (!string.IsNullOrEmpty(_options?.DefaultWorkingDirectory))
        {
            cmd = cmd.WithWorkingDirectory(_options.DefaultWorkingDirectory);
        }

        // Merge environment variables
        IDictionary<string, string?>? environmentVariables = context.Get(EnvironmentVariables);
        Dictionary<string, string?> mergedVariables = new();

        // First add default environment variables from options
        if (_options?.DefaultEnvironmentVariables is { Count: > 0 })
        {
            foreach (KeyValuePair<string, string> kvp in _options.DefaultEnvironmentVariables)
            {
                mergedVariables[kvp.Key] = kvp.Value;
            }
        }

        // Then add (and override) with activity-specific environment variables
        if (environmentVariables is { Count: > 0 })
        {
            foreach (KeyValuePair<string, string> kvp in environmentVariables)
            {
                mergedVariables[kvp.Key] = kvp.Value;
            }
        }

        // Apply the merged environment variables
        if (mergedVariables.Count > 0)
        {
            cmd = cmd.WithEnvironmentVariables(mergedVariables);
        }
        
        // Apply credentials if specified
        NetworkCredential? credentials = context.Get(Credentials);
        if (credentials != null)
        {
            cmd = cmd.WithCredentials(new Credentials(
                credentials.Domain, 
                credentials.UserName, 
                credentials.Password));
        }

        return cmd;
    }

    /// <summary>
    /// Builds a pipe source based on the input object.
    /// </summary>
    /// <param name="inputPipe">The input pipe used to create the PipeSource.</param>
    private PipeSource BuildPipeSource(object inputPipe) =>
        inputPipe switch
        {
            PipeSource pipeSourceInputPipe => pipeSourceInputPipe,
            Stream streamInputPipe => PipeSource.FromStream(streamInputPipe),
            byte[] byteArrayInputPipe => PipeSource.FromBytes(byteArrayInputPipe),
            Command commandInputPipe => PipeSource.FromCommand(commandInputPipe),
            FileInfo fileInfoInputPipe => PipeSource.FromFile(fileInfoInputPipe.FullName),
            string stringInputPipe => PipeSource.FromString(stringInputPipe),
            _ => PipeSource.Null
        };

    /// <summary>
    /// Builds a pipe target based on the output object.
    /// </summary>
    /// <param name="outputPipe">The output pipe used to create the PipeTarget.</param>
    private PipeTarget BuildPipeTarget(object outputPipe) =>
        outputPipe switch
        {
            PipeTarget pipeTargetOutputPipe => pipeTargetOutputPipe,
            Stream streamOutputPipe => PipeTarget.ToStream(streamOutputPipe),
            string stringOutputPipe => PipeTarget.ToFile(stringOutputPipe),
            StringBuilder stringBuilderOutputPipe => PipeTarget.ToStringBuilder(stringBuilderOutputPipe),
            _ => PipeTarget.Null
        };

    /// <summary>
    /// Gets the cancellation token for the command execution.
    /// </summary>
    /// <param name="context">The activity execution context.</param>
    private CancellationToken GetCancellationToken(ActivityExecutionContext context)
    {
        object? timeout = context.Get(Timeout);

        if (timeout == null && _options?.DefaultTimeout is null)
        {
            return context.CancellationToken;
        }

        // Create a timeout-based cancellation token source
        CancellationTokenSource timeoutCts = new();
        if (timeout != null)
        {
            timeoutCts.CancelAfter(timeout switch
            {
                TimeSpan timeSpan => timeSpan,
                int seconds => TimeSpan.FromSeconds(seconds),
                string str => TimeSpan.Parse(str),
                _ => throw new ArgumentException("Timeout must be either a TimeSpan or an int.")
            });
        }
        else if (_options?.DefaultTimeout is null)
        {
            timeoutCts.CancelAfter(_options!.DefaultTimeout!.Value);
        }

        CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
            context.CancellationToken,
            timeoutCts.Token);

        return cancellationTokenSource.Token;
    }

    /// <summary>
    /// Validates the command execution based on the configured validation criteria.
    /// </summary>
    /// <param name="context">The activity execution context.</param>
    /// <param name="result">The buffered command result to validate.</param>
    /// <returns>True if the execution result is valid, false otherwise.</returns>
    private bool ValidateExecution(ActivityExecutionContext context, BufferedCommandResult result)
    {
        // If no validation criteria are specified, consider the execution successful
        bool exitCodeValid = result.IsSuccess;
        bool outputTextValid = result.IsSuccess;

        // Validate exit code if specified
        int? successfulExitCode = context.Get(SuccessfulExitCode);
        if (successfulExitCode.HasValue)
        {
            exitCodeValid = result.ExitCode == successfulExitCode.Value;
        }

        // Validate output text if specified
        string? successfulOutputText = context.Get(SuccessfulOutputText);

        if (!string.IsNullOrWhiteSpace(successfulOutputText))
        {
            try
            {
                successfulOutputText = successfulOutputText.Trim();

                // Check if it's a regex pattern
                if (successfulOutputText.StartsWith("/") && successfulOutputText.EndsWith("/") && successfulOutputText.Length > 2)
                {
                    string pattern = successfulOutputText.Substring(1, successfulOutputText.Length - 2);
                    outputTextValid = Regex.IsMatch(result.StandardOutput, pattern);
                }
                else
                {
                    // Simple string contains check
                    outputTextValid = result.StandardOutput.Contains(successfulOutputText);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating output text with pattern: {Pattern}", successfulOutputText);
                outputTextValid = false;
            }
        }

        return exitCodeValid && outputTextValid;
    }
}