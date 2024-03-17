using Kysect.ScenarioLib.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Kysect.ScenarioLib.Tests.Mocks;

public class FirstScenarioStepHandler : IScenarioStepExecutor<FirstScenarioStepHandler.Arguments>
{
    [ScenarioStep("First.Scenario")]
    public record Arguments([property: Required] string Name) : IScenarioStep;

    public void Execute(ScenarioContext context, Arguments request)
    {
    }
}