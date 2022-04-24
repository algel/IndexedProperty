using System.Collections.Generic;
using Algel.IndexedProperty;
using FluentAssertions;
using Xunit;

namespace IndexedProperty.Tests;

public class SimpleIndexPropertyTests
{
    public SimpleIndexPropertyTests()
    {
        SampleInstance = new Sample();
    }

    private Sample SampleInstance { get; set; }

    [Fact]
    public void ValueByName_SetAndGet_Test()
    {
        SampleInstance.ValueByName["name"] = "value";

        SampleInstance.ValueByName["name"].Should().Be("value");
    }

    [Fact]
    public void ValueByName_GetNotSet_Test()
    {
        var func = () => SampleInstance.ValueByName["name"];

        func.Should().Throw<KeyNotFoundException>();
    }

    [Fact]
    public void SafeValueByName_GetNotSet_Test()
    {
        SampleInstance.SafeValueByName["name"].Should().BeNull();
    }

    [Fact]
    public void SafeValueByName_SetAndGet_Test()
    {
        SampleInstance.SafeValueByName["name"] = "value";

        SampleInstance.SafeValueByName["name"].Should().Be("value");
    }

    #region Sample class

    private class Sample
    {
        private readonly Dictionary<string, string> _dictionary = new();

        public Sample()
        {
            ValueByName = new IndexProperty<string, string>(
                name => _dictionary[name], 
                (name, value) => _dictionary[name] = value);

            SafeValueByName = new IndexProperty<string, string>(
                name => SafeGetValueByName(name),
                (name, value) => _dictionary[name] = value);
        }

        public IndexProperty<string, string> ValueByName { get; }

        public IndexProperty<string, string> SafeValueByName { get; }

        private string? SafeGetValueByName(string name) => _dictionary.TryGetValue(name, out var value) ? value : null;
    }

    #endregion
}