using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace JsonParser.Test
{
    public class ListParsingTester
    {
        [Fact]
        public void CanParsePrimitiveList()
        {
            // Arrange
            List<string> prop1 = new List<string> {"A", "B"};
            PrimitiveList primitiveList = new PrimitiveList(prop1);
            string json = JsonConvert.SerializeObject(primitiveList);
            JToken jToken = JObject.Parse(json);

            // Act
            PrimitiveList result = jToken.Parse<PrimitiveList>();

            // Assert
            Assert.Equal(prop1, result.Prop1);
        }
        
        [Fact]
        public void CanParseComplexList()
        {
            // Arrange
            string prop11 = "prop11";
            int prop12 = 1;
            DateTime prop13 = DateTime.Now;
            string prop21 = "prop12";
            int prop22 = 2;
            DateTime prop23 = DateTime.Now.AddDays(1);
            ComplexList complexList = new ComplexList(new List<NestedComplexList>
            {
                new NestedComplexList(prop11, prop12, prop13),
                new NestedComplexList(prop21, prop22, prop23)
            });
            string json = JsonConvert.SerializeObject(complexList);
            JToken jToken = JObject.Parse(json);
            
            // Act
            ComplexList result = jToken.Parse<ComplexList>();

            // Assert
            Assert.Equal(prop11, result.NestedComplexLists[0].Prop1);
            Assert.Equal(prop12, result.NestedComplexLists[0].Prop2);
            Assert.Equal(prop13, result.NestedComplexLists[0].Prop3);
            Assert.Equal(prop21, result.NestedComplexLists[1].Prop1);
            Assert.Equal(prop22, result.NestedComplexLists[1].Prop2);
            Assert.Equal(prop23, result.NestedComplexLists[1].Prop3);
        }
    }

    class PrimitiveList
    {
        public PrimitiveList(List<string> prop1)
        {
            Prop1 = prop1;
        }

        public List<string> Prop1 { get; }
    }

    class ComplexList
    {
        public ComplexList(List<NestedComplexList> nestedComplexLists)
        {
            NestedComplexLists = nestedComplexLists;
        }

        public List<NestedComplexList> NestedComplexLists { get; }
    }

    internal class NestedComplexList
    {
        public NestedComplexList(string prop1, int prop2, DateTime prop3)
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