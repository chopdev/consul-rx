using System.Collections.Generic;

namespace ConsulRx
{
    public class ConsulDependencies
    {
        public HashSet<string> Keys { get; } = new HashSet<string>();
        public HashSet<string> ConfigurationKeys { get; } = new HashSet<string>();
        public HashSet<string> KeyPrefixes { get; } = new HashSet<string>();
        public HashSet<string> Services { get; } = new HashSet<string>();

        public void CopyTo(ConsulDependencies other)
        {
            other.Keys.UnionWith(Keys);
            other.ConfigurationKeys.UnionWith(ConfigurationKeys);
            other.KeyPrefixes.UnionWith(KeyPrefixes);
            other.Services.UnionWith(Services);
        }
    }
}