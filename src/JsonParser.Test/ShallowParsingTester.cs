using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace JsonParser.Test
{
    public class ShallowParsingTester
    {
        [Fact]
        public void CanParseShallowObjects()
        {
            // Arrange
            string prop1 = "prop1";
            int prop2 = 1;
            DateTime prop3 = DateTime.Now;
            Shallow shallow = new Shallow(prop1, prop2, prop3);
            string json = JsonConvert.SerializeObject(shallow);
            JToken jToken = JObject.Parse(json);

            // Act
            Shallow result = jToken.Parse<Shallow>();

            // Assert
            Assert.Equal(prop1, result.Prop1);
            Assert.Equal(prop2, result.Prop2);
            Assert.Equal(prop3, result.Prop3);
        }

        [Fact]
        public void CanParsePrimitiveArray()
        {
            // Arrange
            string sanityCheck = "sanityCheck";
            string[] prop1 = {"A", "B"};
            int[] prop2 = {1, 2};
            WithPrimitiveArray withPrimitiveArray = new WithPrimitiveArray(sanityCheck, prop1, prop2);
            string json = JsonConvert.SerializeObject(withPrimitiveArray);
            JToken jToken = JObject.Parse(json);

            // Act
            WithPrimitiveArray result = jToken.Parse<WithPrimitiveArray>();

            // Assert
            Assert.Equal(sanityCheck, result.SanityCheck);
            Assert.Equal(prop1, result.Prop1);
            Assert.Equal(prop2, result.Prop2);
        }
    }

    class WithPrimitiveArray
    {
        public WithPrimitiveArray(string sanityCheck, string[] prop1, int[] prop2)
        {
            SanityCheck = sanityCheck;
            Prop1 = prop1;
            Prop2 = prop2;
        }

        public string SanityCheck { get; }
        public string[] Prop1 { get; }
        public int[] Prop2 { get; }
    }

    class Shallow
    {
        public Shallow(string prop1, int prop2, DateTime prop3)
        {
            Prop1 = prop1;
            Prop2 = prop2;
            Prop3 = prop3;
        }

        public string Prop1 { get; }
        public int Prop2 { get; }
        public DateTime Prop3 { get; }
    }
}