namespace Kysect.ScenarioLib.Abstractions;

public interface IScenarioContentStepDeserializer
{
    IScenarioStep ParseScenarioStep(ScenarioStepArguments arguments);
}