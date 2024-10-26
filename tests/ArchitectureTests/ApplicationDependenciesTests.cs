using FluentAssertions;
using TelegramBot.ArchitectureTests.Infrastructure;

namespace TelegramBot.ArchitectureTests;

public sealed class ApplicationDependenciesTests(SolutionFixture fixture) : ArchitectureTestBase(fixture)
{
    [Fact]
    public void DomainShouldNotDependOnAnotherProject()
    {
        var domainProject = ApplicationProjects
            .Single(x => x.Name == SolutionStructure.Module.Domain);

        var allApplicationProjectIds = ApplicationProjects.Select(x => x.Id).ToList();

        var wrongDependencies = domainProject.AllProjectReferences
            .Where(x => allApplicationProjectIds.Contains(x.ProjectId)
                        && x.ProjectId != domainProject.Id)
            .Select(x => Solution.GetProject(x.ProjectId)!.AssemblyName)
            .ToList();

        wrongDependencies.Should().BeEmpty($"{domainProject.AssemblyName} should not depend on any other non-framework project");
    }

    [Fact]
    public void DataShouldNotDependOnAnotherProject()
    {
        var domainProject = ApplicationProjects
            .Single(x => x.Name == SolutionStructure.Module.Data);
        var allowedDependencyNames = new[] { SolutionStructure.Module.Domain };

        var disallowedDependenciesProjectIds = ApplicationProjects
            .Where(x => !allowedDependencyNames.Contains(x.Name))
            .Select(x => x.Id)
            .ToList();

        var wrongDependencies = domainProject.AllProjectReferences
            .Where(x => disallowedDependenciesProjectIds.Contains(x.ProjectId)
                        && x.ProjectId != domainProject.Id)
            .Select(x => Solution.GetProject(x.ProjectId)!.AssemblyName)
            .ToList();

        wrongDependencies.Should().BeEmpty($"{domainProject.AssemblyName} should not depend on any other non-framework project, except {string.Join(", ", allowedDependencyNames)}");
    }

    [Fact]
    public void ApplicationShouldNotDependOnAnotherProject()
    {
        var applicationProject = ApplicationProjects
            .Single(x => x.Name == SolutionStructure.Module.Application);
        var allowedDependencyNames = new[] { SolutionStructure.Module.Domain };

        var disallowedDependenciesProjectIds = ApplicationProjects
            .Where(x => !allowedDependencyNames.Contains(x.Name))
            .Select(x => x.Id)
            .ToList();

        var wrongDependencies = applicationProject.AllProjectReferences
            .Where(x => disallowedDependenciesProjectIds.Contains(x.ProjectId)
                        && x.ProjectId != applicationProject.Id)
            .Select(x => Solution.GetProject(x.ProjectId)!.AssemblyName)
            .ToList();

        wrongDependencies.Should().BeEmpty($"{applicationProject.AssemblyName} should not depend on any other non-framework project, except {string.Join(", ", allowedDependencyNames)}");
    }

    [Fact]
    public void HostShouldNotDependOnAnotherProject()
    {
        var hostProject = ApplicationProjects
            .Single(x => x.Name == SolutionStructure.Module.Host);
        var allowedDependencyNames = new[]
        {
            SolutionStructure.Module.Domain, 
            SolutionStructure.Module.Application,
            SolutionStructure.Module.Data,
            SolutionStructure.Module.ServiceDefaults,
        };

        var disallowedDependenciesProjectIds = ApplicationProjects
            .Where(x => !allowedDependencyNames.Contains(x.Name))
            .Select(x => x.Id)
            .ToList();

        var wrongDependencies = hostProject.AllProjectReferences
            .Where(x => disallowedDependenciesProjectIds.Contains(x.ProjectId)
                        && x.ProjectId != hostProject.Id)
            .Select(x => Solution.GetProject(x.ProjectId)!.AssemblyName)
            .ToList();

        wrongDependencies.Should().BeEmpty($"{hostProject.AssemblyName} should not depend on any other non-framework project, except {string.Join(", ", allowedDependencyNames)}");
    }
}
