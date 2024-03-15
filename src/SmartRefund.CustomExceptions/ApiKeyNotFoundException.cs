
using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.CustomExceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class ApiKeyNotFoundException : Exception
    {
        private string? EnvVariable { get; init; }
        public ApiKeyNotFoundException(string envVariable)
            : base($"API key not found. It's necessary to set an environmental variable called {envVariable}.")
        {
            EnvVariable = envVariable;
        }

        public ApiKeyNotFoundException()
            : base("The API key env variable needs to be configured on appsettings.json")
        {
        }
    }
}
