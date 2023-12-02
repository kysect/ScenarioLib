using Kysect.ScenarioLib.Abstractions;

namespace Kysect.ScenarioLib.Tests.Mocks;

public class ScenarioWithoutArgumentsStepHandler : IScenarioStepExecutor<ScenarioWithoutArgumentsStepHandler.Arguments>
{
    [ScenarioStep("Scenario.WithoutArguments")]
    public class Arguments : IScenarioStep
    {
    }

    public void Execute(ScenarioContext context, Arguments request)
    {
    }
}