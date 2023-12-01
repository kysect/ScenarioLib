using Kysect.ScenarioLib.Abstractions;

namespace Kysect.ScenarioLib.Tests.Mocks;

public class ScenarioWithArrayStepHandler : IScenarioStepExecutor<ScenarioWithArrayStepHandler.Arguments>
{
    [ScenarioStep("Scenario.WithArray")]
    public class Arguments : IScenarioStep
    {
        public IReadOnlyCollection<string> Values { get; }

        public Arguments(IReadOnlyCollection<string> values)
        {
            Values = values;
        }
    }

    public void Execute(ScenarioContext context, Arguments request)
    {
    }
}