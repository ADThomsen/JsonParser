using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace JsonParser.Test
{
    public class MissingPropertyTester
    {
        [Fact]
        public void CanParseIfPropertyIsMissingInJson()
        {
            // Arrange
            string prop2 = "prop2";
            MissingProp allProps = new MissingProp(prop2);
            string json = JsonConvert.SerializeObject(allProps);
            JToken jToken = JObject.Parse(json);
            
            // Act
            AllProps result = jToken.Parse<AllProps>();

            // Assert
            Assert.Equal(prop2, result.Prop2);
        }
    }

    class AllProps
    {
        public AllProps(string prop1, string prop2)
        {
            Prop1 = prop1;
            Prop2 = prop2;
        }

        public string Prop1 { get; }
        public string Prop2 { get; }
    }

    class MissingProp
    {
        public MissingProp(string prop2)
        {
            Prop2 = prop2;
        }

        public string Prop2 { get; }
    }
}