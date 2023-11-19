using System.Reflection;

namespace Kysect.ScenarioLib.Tests.Tools;

public class TestConstants
{
    public static Assembly CurrentAssembly { get; } = typeof(ScenarioStepReflectionHandlerTests).Assembly;
}