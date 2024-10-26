namespace TelegramBot.ArchitectureTests.Infrastructure;

[CollectionDefinition(Name)]
public class ArchitectureTestCollection : ICollectionFixture<SolutionFixture>
{
    public const string Name = "Architecture tests collection";

    public const string Category = "ArchitectureTests";
}
