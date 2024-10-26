using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using TelegramBot.Framework.Utilities.System;

namespace TelegramBot.ArchitectureTests.Infrastructure;

public sealed class SolutionFixture : IAsyncLifetime
{
    private MSBuildWorkspace? _msWorkspace;
    private Solution? _solution;
    private IReadOnlyList<Project>? _frameworkProjects;
    private IReadOnlyList<Project>? _applicationProjects;

    public Solution Solution => _solution.Required();
    public IReadOnlyList<Project> ApplicationProjects => _applicationProjects.Required();
    public IReadOnlyList<Project> FrameworkProjects => _frameworkProjects.Required();

    public async Task InitializeAsync()
    {
        var solutionDirectoryPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../.."));
        var solutionFilePath = Path.Combine(solutionDirectoryPath, SolutionStructure.SolutionFileName);

        _msWorkspace = MSBuildWorkspace.Create();
        _solution = await _msWorkspace.OpenSolutionAsync(solutionFilePath).ConfigureAwait(false);

        _applicationProjects = _solution.Projects
            .Where(x => !x.FilePath!.Contains("src/Framework"))
            .ToList();

        _frameworkProjects = _solution.Projects
            .Where(x => x.FilePath!.Contains("src/Framework"))
            .ToList();
    }

    public Task DisposeAsync()
    {
        _msWorkspace?.Dispose();
        return Task.CompletedTask;
    }
}
