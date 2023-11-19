using System.Reflection;

namespace Kysect.ScenarioLib.Tests.Tools;

public static class TestConstants
{
    public static Assembly CurrentAssembly { get; } = typeof(ScenarioStepReflectionHandlerTests).Assembly;
}