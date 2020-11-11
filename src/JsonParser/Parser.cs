using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace JsonParser
{
    public static class Parser
    {
        public static T Parse<T>(this JToken jToken)
        {
            T t = (T) FormatterServices.GetUninitializedObject(typeof(T));

            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                JToken jProp = jToken[property.Name];
                if (jProp == null)
                    continue;

                FieldInfo field = t.GetType().GetField($"<{property.Name}>k__BackingField",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                
                if (field == null)
                    throw new InvalidOperationException($"Failed to find backing field for property with name {property.Name}");

                Parse(field, jProp, t);
            }

            return t;
        }

        private static void Parse<T>(FieldInfo field, JToken jProp, T t)
        {
            switch (jProp.Type)
            {
                case JTokenType.Object:
                    object value = ParseGeneric(field.FieldType, jProp);
                    field.SetValue(t, value);
                    break;
                case JTokenType.Array when field.FieldType.IsArray:
                    ParseArray(field, jProp, t);
                    break;
                case JTokenType.Array when field.FieldType.IsGenericType:
                    ParseList(field, jProp, t);
                    break;
                default:
                    field.SetValue(t, Convert.ChangeType(jProp, field.FieldType));
                    break;
            }
        }

        private static void ParseList<T>(FieldInfo field, JToken jProp, T t)
        {
            Type genericType = field.FieldType.GetGenericArguments()[0];
            Type concreteType = typeof(List<>).MakeGenericType(genericType);
            object listInstance = Activator.CreateInstance(concreteType);
            foreach (var token in jProp)
            {
                var value = token.Type == JTokenType.Object
                    ? ParseGeneric(genericType, token)
                    : Convert.ChangeType(token, genericType);
                listInstance.GetType().GetMethod("Add")!.Invoke(listInstance, new[] {value});
            }

            field.SetValue(t, listInstance);
        }

        private static void ParseArray<T>(FieldInfo field, JToken jProp, T t)
        {
            Type arrayType = field.FieldType.GetElementType();
            if (arrayType == null)
                throw new InvalidOperationException($"Could not create Array type from {field.FieldType.Name}");

            Array arrayInstance = Array.CreateInstance(arrayType, jProp.Count());
            for (int i = 0; i < arrayInstance.Length; i++)
            {
                JToken token = jProp[i];
                var value = token!.Type == JTokenType.Object
                    ? ParseGeneric(arrayType, token)
                    : Convert.ChangeType(token, arrayType);
                arrayInstance.SetValue(value, i);
            }

            field.SetValue(t, arrayInstance);
        }

        private static object ParseGeneric(Type targetType, JToken jToken)
        {
            var method = typeof(Parser).GetMethod(nameof(Parse));
            method = method!.MakeGenericMethod(targetType);
            var dAsThing = method.Invoke(null, new object[] {jToken});
            return dAsThing;
        }
    }
}