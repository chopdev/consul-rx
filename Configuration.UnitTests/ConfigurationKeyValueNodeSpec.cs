using System;
using System.Collections.Generic;
using System.Text;
using ConsulRx.ConfigParsers;
using FluentAssertions.Common;
using Xunit;
using Xunit.Sdk;

namespace ConsulRx.Configuration.UnitTests
{
    public class ConfigurationKeyValueNodeSpec
    {
        [Fact]
        public void ConfigurationNodeParsesCorrect()
        {
            var json = @"{""key1"":""value1"",""key2"": {
        ""key3"": {
            ""key4"": ""val4"",
            ""key5"": ""val5""
        }
    },}";

            var retrievedNode = new KeyValueNode("myconfig", json);
            var configNodes = retrievedNode.ParseConfig(new JsonConfigurationParser());

            Assert.Equal(configNodes.Count, 3);
            Assert.Equal(configNodes[0].FullKey, "myconfig:key1");
            Assert.Equal(configNodes[1].FullKey, "myconfig:key2:key3:key4");
            Assert.Equal(configNodes[2].FullKey, "myconfig:key2:key3:key5");
            Assert.Equal(configNodes[0].Value, "value1");
            Assert.Equal(configNodes[1].Value, "val4");
            Assert.Equal(configNodes[2].Value, "val5");
        }

        [Fact]
        public void ConfigurationNodeParsesCorrectForIncorrectValue()
        {
            var json = @"key1";

            var retrievedNode = new KeyValueNode("myconfig", json);
            Assert.ThrowsAny<Exception>(() => retrievedNode.ParseConfig(new JsonConfigurationParser()));
        }

        [Fact]
        public void ConfigurationNodeParsesCorrectForEmptyValue()
        {
            var retrievedNode = new KeyValueNode("myconfig", new byte[0]);
            var configNodes = retrievedNode.ParseConfig(new JsonConfigurationParser());
            Assert.Equal(configNodes.Count, 1);
            Assert.Equal(configNodes[0].FullKey, "myconfig");
        }
    }
}
