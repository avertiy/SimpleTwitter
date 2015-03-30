using System;
using System.Collections.Generic;

namespace SimpleTwitter.ReadSide.Data.Redis
{
    public interface IUrnResolver
    {
        string ResolveUrnPrefix(Type type);
        string ResolveUrn(Type type, string key);
    }
    
    public class UrnResolver : IUrnResolver
    {
        private readonly Dictionary<Type,string> _dict = new Dictionary<Type, string>();

        public void RegisterUrnPrefix<T>(string urnPrefix) where T : IKVEntity
        {
            if (string.IsNullOrEmpty(urnPrefix))
            {
                throw new ArgumentNullException("urnPrefix");
            }
            var type = typeof (T);
            if (_dict.ContainsKey(type))
            {
                throw new ArgumentException(string.Format("Type {0} is already registered", type.Name));
            }
            _dict.Add(type,urnPrefix);
        }

        public string ResolveUrnPrefix(Type type)
        {
            if (!_dict.ContainsKey(type))
            {
                throw new ArgumentException(string.Format("Type {0} was not registered",type.Name));
            }
            return _dict[type];
        }

        public string ResolveUrn(Type type, string key)
        {
            return ResolveUrnPrefix(type) + ":" + key;
        }
    }
    public interface IKVEntity
    {
        string Key { get; }
    }
}