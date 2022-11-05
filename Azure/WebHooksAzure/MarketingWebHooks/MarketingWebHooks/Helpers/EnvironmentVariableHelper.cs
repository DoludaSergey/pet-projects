namespace MarketingWebHooks.Helpers
{
    public sealed class EnvironmentVariableHelper
    {
        public static string GetEnvironmentVariableOrDefaulf(string environmentVariableName, string defaultValue)
        {
            return Environment.GetEnvironmentVariable(environmentVariableName) ?? defaultValue;
        }

        public static int GetEnvironmentVariableOrDefaulf(string environmentVariableName, int defaultValue)
        {
            if (int.TryParse(Environment.GetEnvironmentVariable(environmentVariableName), out var value))
            {
                return value;
            }

            return defaultValue;
        }
    }
}
