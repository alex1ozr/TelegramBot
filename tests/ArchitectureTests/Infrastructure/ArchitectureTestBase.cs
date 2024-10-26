using Microsoft.CodeAnalysis;

namespace TelegramBot.ArchitectureTests.Infrastructure;

[Trait("Category", ArchitectureTestCollection.Category)]
[Collection(ArchitectureTestCollection.Name)]
public abstract class ArchitectureTestBase(SolutionFixture fixture)
{
    protected readonly Solution Solution = fixture.Solution;

    protected readonly IReadOnlyList<Project> ApplicationProjects = fixture.ApplicationProjects;

    protected readonly IReadOnlyList<Project> FrameworkProjects = fixture.FrameworkProjects;
}
