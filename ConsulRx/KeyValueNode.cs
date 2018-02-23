using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ConsulRx.ConfigParsers;

namespace ConsulRx
{
    public class KeyValueNode
    {
        public string FullKey { get; }
        public string Value { get; }
        public byte[] RawValue { get; }

        public KeyValueNode(string fullKey, byte[] value)
        {
            FullKey = fullKey;

            if (value != null)
            {
                Value = Encoding.UTF8.GetString(value);
                RawValue = value;
            }
        }

        public KeyValueNode(string fullKey, string value)
        {
            FullKey = fullKey;

            if (value != null)
            {
                Value = value;
                RawValue = Encoding.UTF8.GetBytes(value);
            }
        }

        public string LeafKey
        {
            get
            {
                var lastSlashIndex = FullKey.LastIndexOf('/');
                if (lastSlashIndex < 0)
                    return FullKey;
                else
                    return FullKey.Substring(lastSlashIndex + 1);
            }
        }

        public string ParentKey
        {
            get
            {
                var lastSlashIndex = FullKey.LastIndexOf('/');
                if (lastSlashIndex < 0)
                    return FullKey;
                else
                    return FullKey.Remove(lastSlashIndex);
            }
        }

        public bool IsChildOf(string otherKey)
        {
            return otherKey.Equals(ParentKey);
        }

        public bool IsDescendentOf(string prefix)
        {
            return FullKey.StartsWith(prefix);
        }

        protected bool Equals(KeyValueNode other)
        {
            return string.Equals(FullKey, other.FullKey) && string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((KeyValueNode) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((FullKey != null ? FullKey.GetHashCode() : 0) * 397) ^ (Value != null ? Value.GetHashCode() : 0);
            }
        }

        public KeyValuePair<string, KeyValueNode> ToIndexedPair()
        {
            return new KeyValuePair<string, KeyValueNode>(FullKey, this);
        }
    }
}