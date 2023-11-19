using Kysect.ScenarioLib.Abstractions;

namespace Kysect.ScenarioLib.Tests.Mocks;

public class SecondScenarioStepHandler : IScenarioStepExecutor<SecondScenarioStepHandler.Arguments>
{
    [ScenarioStep("Second.Scenario")]
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