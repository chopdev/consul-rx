using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConsulRx.ConfigParsers;
using Microsoft.Extensions.Configuration;

namespace ConsulRx
{
    public static class KeyValueNodeExtensions
    {
        public static Dictionary<string, string> GetValueAsDictionary(this KeyValueNode node, IConfigurationParser parser)
        {
            if(node.FullKey == null)
                throw new ArgumentException("Key is empty");

            if (string.IsNullOrWhiteSpace(node.Value))
            {
                return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) { { node.FullKey, null } };
            }

            using (var configStream = new MemoryStream(node.RawValue))
            {
                return new Dictionary<string, string>(
                    parser.Parse(configStream),
                    StringComparer.OrdinalIgnoreCase);
            }
        }

        public static IReadOnlyList<KeyValueNode> ParseConfig(this KeyValueNode node, IConfigurationParser parser)
        {
            if (node.FullKey == null)
                throw new ArgumentException("Key is empty");

            if (string.IsNullOrWhiteSpace(node.Value))
            {
                return new []{ new KeyValueNode(node.FullKey, null as string) };
            }

            var config = node.GetValueAsDictionary(parser);

            var result = config.Select(x=> new KeyValueNode(ConfigurationPath.Combine(node.FullKey, x.Key), x.Value)).ToList();
            return result;
        }
    }
}
