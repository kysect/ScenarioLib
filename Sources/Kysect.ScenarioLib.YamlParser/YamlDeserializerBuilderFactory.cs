using YamlDotNet.Serialization;

namespace Kysect.ScenarioLib.YamlParser;

public static class YamlDeserializerBuilderFactory
{
    public static IDeserializer Create()
    {
        return new DeserializerBuilder()
            .WithNodeDeserializer(new KeyValuePairNodeDeserializer())
            .Build();
    }
}