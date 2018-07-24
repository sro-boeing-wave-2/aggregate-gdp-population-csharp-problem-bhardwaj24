using System;
using Xunit;
using System.IO;
using AggregateGDPPopulation;
using Newtonsoft.Json.Linq;

namespace AggregateGDPPopulation.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Class1 c = new Class1();
            c.calculateaggregate();
            JObject actual = JObject.Parse(File.ReadAllText(@"../../../../output.json"));
            JObject expected = JObject.Parse(File.ReadAllText(@"../../../../AggregateGDPPopulation.Tests/expected-output.json"));
            Assert.Equal(actual, expected);
        }

    }
}
