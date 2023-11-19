using Kysect.ScenarioLib.Abstractions;

namespace Kysect.ScenarioLib.Tests.Mocks;

public class FirstScenarioStepHandler : IScenarioStepExecutor<FirstScenarioStepHandler.Arguments>
{
    [ScenarioStep("First.Scenario")]
    public class Arguments : IScenarioStep
    {
        public string Name { get; }

        public Arguments(string name)
        {
            Name = name;
        }
    }

    public void Execute(Arguments request)
    {
    }
}