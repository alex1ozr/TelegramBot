namespace TelegramBot.ArchitectureTests.Infrastructure;

internal static class SolutionStructure
{
    public const string SolutionFileName = "TelegramBot.sln";

    public static class Module
    {
        public const string Application = nameof(Application);
        public const string Data = nameof(Data);
        public const string Domain = nameof(Domain);
        public const string Host = nameof(Host);
        public const string AppHost = nameof(AppHost);
        public const string ServiceDefaults = nameof(ServiceDefaults);
    }
}
