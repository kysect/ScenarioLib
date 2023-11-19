namespace Kysect.ScenarioLib.Abstractions;

public interface IScenarioStepExecutor
{
}

public interface IScenarioStepExecutor<T> : IScenarioStepExecutor where T : IScenarioStep
{
    void Execute(T request);
}