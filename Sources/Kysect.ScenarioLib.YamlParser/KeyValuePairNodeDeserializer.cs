using Kysect.CommonLib.BaseTypes.Extensions;
using System;
using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Kysect.ScenarioLib.YamlParser;

public class KeyValuePairNodeDeserializer : INodeDeserializer
{
    public bool Deserialize(IParser reader, Type expectedType, Func<IParser, Type, object?> nestedObjectDeserializer, out object? value)
    {
        reader.ThrowIfNull();
        expectedType.ThrowIfNull();
        nestedObjectDeserializer.ThrowIfNull();


        if (expectedType.IsGenericType && expectedType.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
        {
            reader.Consume<MappingStart>();

            var pairArgs = expectedType.GetGenericArguments();

            object? key = null;
            object? val = null;

            if (reader.Accept<Scalar>(out _))
                key = nestedObjectDeserializer(reader, pairArgs[0]);

            if (reader.Accept<Scalar>(out _))
                val = nestedObjectDeserializer(reader, pairArgs[1]);

            value = Activator.CreateInstance(expectedType, key, val);


            reader.Consume<MappingEnd>();
            return true;
        }

        value = null;
        return false;
    }
}