using Kysect.CommonLib.Reflection;
using Kysect.ScenarioLib.Abstractions;
using System;
using System.Reflection;

namespace Kysect.ScenarioLib;

public class ScenarioStepExecutorReflectionDecorator
{
    private const string ExecuteMethodName = nameof(IScenarioStepExecutor<IScenarioStep>.Execute);

    private readonly IScenarioStepExecutor _executor;
    private readonly MethodInfo _executeMethod;

    public ScenarioStepExecutorReflectionDecorator(IScenarioStepExecutor executor)
    {
        Type executorType = executor.GetType();
        MethodInfo? executeMethod = executorType.GetMethod(ExecuteMethodName);

        _executor = executor;
        _executeMethod = executeMethod ?? throw new ReflectionException($"Cannot get method {executeMethod} from type {executorType}");
    }

    public void Execute(IScenarioStep scenarioStep)
    {
        _executeMethod.Invoke(_executor, new object[] { scenarioStep });
    }
}