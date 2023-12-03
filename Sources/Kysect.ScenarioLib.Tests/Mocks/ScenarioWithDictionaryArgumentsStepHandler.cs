using Kysect.ScenarioLib.Abstractions;

namespace Kysect.ScenarioLib.Tests.Mocks;

public class ScenarioWithDictionaryArgumentsStepHandler : IScenarioStepExecutor<ScenarioWithDictionaryArgumentsStepHandler.Arguments>
{
    [ScenarioStep("Scenario.WithDictionaryArguments")]
    public class Arguments : IScenarioStep
    {
        public Dictionary<string, string> Values { get; }

        public Arguments(Dictionary<string, string> values)
        {
            Values = values;
        }
    }

    public void Execute(ScenarioContext context, Arguments request)
    {
    }
}