using FluentAssertions;
using TelegramBot.ArchitectureTests.Infrastructure;

namespace TelegramBot.ArchitectureTests;

public sealed class FrameworkDependenciesTests(SolutionFixture fixture) : ArchitectureTestBase(fixture)
{
    [Fact]
    public void FrameworkShouldNotDependOnAnyApplicationProject()
    {
        foreach (var project in FrameworkProjects)
        {
            var wrongDependencies = project.AllProjectReferences
                .Where(x => ApplicationProjects.Any(mp => mp.Id == x.ProjectId))
                .Select(x => Solution.GetProject(x.ProjectId)!.AssemblyName)
                .ToList();

            wrongDependencies.Should().BeEmpty($"Framework assembly ({project.AssemblyName}) should not depend on any non-framework project");
        }
    }
}
