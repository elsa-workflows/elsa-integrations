using System.Reflection;
using Elsa.Workflows.UIHints.CodeEditor;

// ReSharper disable once CheckNamespace
namespace Elsa.Integrations.Sql.Activities;

internal class SqlCodeOptionsProvider : CodeEditorOptionsProviderBase
{
    protected override string GetLanguage(PropertyInfo propertyInfo, object? context) => "sql";
}