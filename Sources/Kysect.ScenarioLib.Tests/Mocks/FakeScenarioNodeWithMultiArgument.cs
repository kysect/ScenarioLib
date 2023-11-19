using Kysect.ScenarioLib.Abstractions;

namespace Kysect.ScenarioLib.Tests.Mocks;

[ScenarioStep("Some.Action")]
public class FakeScenarioNodeWithMultiArgument : IScenarioStep
{
    public IReadOnlyCollection<string> Names { get; }
    public string RepositoryPath { get; }

    public FakeScenarioNodeWithMultiArgument(IReadOnlyCollection<string> names, string repositoryPath)
    {
        Names = names;
        RepositoryPath = repositoryPath;
    }
}