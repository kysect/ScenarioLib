using Kysect.CommonLib.DependencyInjection;
using Kysect.ScenarioLib.Abstractions;
using Kysect.ScenarioLib.Tests.Mocks;
using Kysect.ScenarioLib.Tests.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace Kysect.ScenarioLib.Tests;

public class ScenarioStepReflectionHandlerTests
{
    [Fact]
    public void Create_ForTestAssembly_ReturnInitializedHandler()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddAllImplementationOf<IScenarioStepExecutor>(TestConstants.CurrentAssembly);
        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        var scenarioStepReflectionHandler = ScenarioStepReflectionHandler.Create(serviceProvider, TestConstants.CurrentAssembly);
        scenarioStepReflectionHandler.Handle(new ScenarioContext(), new FirstScenarioStepHandler.Arguments("Some name"));
    }
}