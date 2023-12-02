using Kysect.ScenarioLib.Abstractions;

namespace Kysect.ScenarioLib.Tests.Mocks;

public class ScenarioWithBoolStepHandler : IScenarioStepExecutor<ScenarioWithBoolStepHandler.Arguments>
{
    [ScenarioStep("Scenario.WithBool")]
    public class Arguments : IScenarioStep
    {
        public bool Value { get; set; }

        public Arguments(bool value)
        {
            Value = value;
        }
    }

    public void Execute(ScenarioContext context, Arguments request)
    {
    }
}