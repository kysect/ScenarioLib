using Kysect.ScenarioLib.Abstractions;
using System;
using System.Reflection;

namespace Kysect.ScenarioLib;

public static class ScenarioStepExecutorReflectionExtensions
{
    public static void ExecuteUnsafe(this IScenarioStepExecutor executor, IScenarioStep scenarioStep)
    {
        string methodName = nameof(IScenarioStepExecutor<IScenarioStep>.Execute);
        MethodInfo? executeMethod = executor.GetType().GetMethod(methodName);
        if (executeMethod is null)
            throw new ArgumentException($"Cannot find method {methodName} in type {executor.GetType().FullName}");

        executeMethod.Invoke(executor, new object[] { scenarioStep });
    }
}