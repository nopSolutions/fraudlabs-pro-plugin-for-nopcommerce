using System;
using Newtonsoft.Json;

namespace Nop.Plugin.Misc.FraudLabsPro.Services
{
    /// <summary>
    /// Represents StringConverter that allows to deserialize string values as boolean or formatted string
    /// </summary>
    class StringConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var source = (string)reader.Value;
            var res = string.Empty;
            switch (source)
            {
                case "N":
                    res = "No";
                    break;
                case "Y":
                    res = "Yes";
                    break;
                case "NA":
                    res = "N/A";
                    break;
                default:
                    res = source;
                    break;
            }
            return res;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
