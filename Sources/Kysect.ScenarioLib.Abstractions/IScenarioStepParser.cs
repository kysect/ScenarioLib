namespace Kysect.ScenarioLib.Abstractions;

public interface IScenarioStepParser
{
    IScenarioStep ParseScenarioStep(ScenarioStepArguments scenarioStepArguments);
}