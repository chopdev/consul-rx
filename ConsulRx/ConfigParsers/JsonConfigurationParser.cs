using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsulRx.ConfigParsers
{
    /// <inheritdoc />
    /// <summary>
    ///     Implemenation of <see cref="IConfigurationParser" /> for parsing JSON Configuration
    /// </summary>
    public sealed class JsonConfigurationParser : IConfigurationParser
    {
        /// <inheritdoc />
        public IDictionary<string, string> Parse(Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                jsonReader.DateParseHandling = DateParseHandling.None;
                JObject jsonConfig = JObject.Load(jsonReader);
                return jsonConfig.Flatten();
            }
        }
    }

    public static class JsonConfigurationParserExtensions
    {
        internal static IDictionary<string, string> Flatten(this JObject jObject)
        {
            var jsonPrimitiveVisitor = new JsonPrimitiveVisitor();
            IDictionary<string, string> data = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (KeyValuePair<string, string> primitive in jsonPrimitiveVisitor.VisitJObject(jObject))
            {
                if (data.ContainsKey(primitive.Key))
                {
                    throw new FormatException($"Key {primitive.Key} is duplicated in json");
                }

                data.Add(primitive);
            }

            return data;
        }
    }
}
