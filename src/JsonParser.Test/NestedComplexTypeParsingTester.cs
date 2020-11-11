using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace JsonParser.Test
{
    public class NestedComplexTypeParsingTester
    {
        [Fact]
        public void CanParseNestedComplexType()
        {
            // Arrange
            string prop1 = "prop1";
            int prop2 = 1;
            DateTime prop3 = DateTime.Now;
            ComplexType complexType = new ComplexType(new NestedComplexType(prop1, prop2, prop3));
            string json = JsonConvert.SerializeObject(complexType);
            JToken jToken = JObject.Parse(json);

            // Act
            ComplexType result = jToken.Parse<ComplexType>();

            // Assert
            Assert.Equal(prop1, result.NestedComplexType.Prop1);
            Assert.Equal(prop2, result.NestedComplexType.Prop2);
            Assert.Equal(prop3, result.NestedComplexType.Prop3);
        }

        [Fact]
        public void CanParseComplexArray()
        {
            // Arrange
            string prop11 = "prop11";
            int prop12 = 1;
            DateTime prop13 = DateTime.Now;
            string prop21 = "prop21";
            int prop22 = 2;
            DateTime prop23 = DateTime.Now.AddDays(1);
            WithComplexArray withComplexArray = new WithComplexArray(new[]
            {
                new NestedComplexType(prop11, prop12, prop13),
                new NestedComplexType(prop21, prop22, prop23)
            });
            string json = JsonConvert.SerializeObject(withComplexArray);
            JToken jToken = JObject.Parse(json);

            // Act
            WithComplexArray result = jToken.Parse<WithComplexArray>();

            // Assert
            Assert.Equal(prop11, result.NestedComplexTypes[0].Prop1);
            Assert.Equal(prop12, result.NestedComplexTypes[0].Prop2);
            Assert.Equal(prop13, result.NestedComplexTypes[0].Prop3);
            Assert.Equal(prop21, result.NestedComplexTypes[1].Prop1);
            Assert.Equal(prop22, result.NestedComplexTypes[1].Prop2);
            Assert.Equal(prop23, result.NestedComplexTypes[1].Prop3);
        }
    }

    class ComplexType
    {
        public ComplexType(NestedComplexType nestedComplexType)
        {
            NestedComplexType = nestedComplexType;
        }

        public NestedComplexType NestedComplexType { get; }
    }

    internal class NestedComplexType
    {
        public NestedComplexType(string prop1, int prop2, DateTime prop3)
        {
            Prop1 = prop1;
            Prop2 = prop2;
            Prop3 = prop3;
        }

        public string Prop1 { get; }
        public int Prop2 { get; }
        public DateTime Prop3 { get; }
    }

    class WithComplexArray
    {
        public WithComplexArray(NestedComplexType[] nestedComplexTypes)
        {
            NestedComplexTypes = nestedComplexTypes;
        }

        public NestedComplexType[] NestedComplexTypes { get; }
    }
}