using Microsoft.Extensions.Logging;

namespace Elsa.Integrations.CommandLine.Services;

/// <summary>
/// Helper class to find commands in PATH for autocomplete.
/// </summary>
public class CommandFinder
{
    private readonly ILogger<CommandFinder> _logger;
    private readonly Lazy<IEnumerable<string>> _availableCommands;

    /// <summary>
    /// Gets all available commands from the PATH environment variable.
    /// </summary>
    public IEnumerable<string> GetAvailableCommands() => _availableCommands.Value;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandFinder"/> class.
    /// </summary>
    public CommandFinder(ILogger<CommandFinder> logger)
    {
        _logger = logger;
        _availableCommands = new Lazy<IEnumerable<string>>(GetCommandsFromPath);
    }

    private IEnumerable<string> GetCommandsFromPath()
    {
        string? pathVariable = Environment.GetEnvironmentVariable("PATH");

        if (string.IsNullOrEmpty(pathVariable))
        {
            _logger.LogWarning("PATH environment variable is empty or not available. No executable commands could be found.");
            return [];
        }

        string[] pathDirectories = pathVariable.Split(Path.PathSeparator);
        HashSet<string> uniqueCommands = new(StringComparer.OrdinalIgnoreCase);

        foreach (string directory in pathDirectories)
        {
            try
            {
                if (Directory.Exists(directory))
                {
                    string[] files = Directory.GetFiles(directory);

                    foreach (string file in files)
                    {
                        string fileName = Path.GetFileName(file);

                        if (IsExecutable(fileName))
                        {
                            string commandName = Path.GetFileNameWithoutExtension(fileName);
                            uniqueCommands.Add(commandName);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Continue with next directory
            }
        }

        _logger.LogInformation("Found {Count} commands in PATH", uniqueCommands.Count);

        return uniqueCommands.OrderBy(cmd => cmd);
    }

    private static bool IsExecutable(string fileName)
    {
        List<string> executableExtensions = [];

        switch (Environment.OSVersion.Platform)
        {
            case PlatformID.Win32NT or PlatformID.Win32S or PlatformID.Win32Windows or PlatformID.WinCE:
                executableExtensions = [".exe", ".cmd", ".bat", ".ps1", ".vbs", ""];
                break;
            case PlatformID.Unix or PlatformID.MacOSX:
                executableExtensions = ["", ".sh"];
                break;
        }

        string extension = Path.GetExtension(fileName).ToLower();
        return executableExtensions.Contains(extension);
    }
}