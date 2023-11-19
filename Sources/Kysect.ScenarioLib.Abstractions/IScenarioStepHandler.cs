namespace Kysect.ScenarioLib.Abstractions;

public interface IScenarioStepHandler
{
    void Handle(IScenarioStep scenarioStep);
}