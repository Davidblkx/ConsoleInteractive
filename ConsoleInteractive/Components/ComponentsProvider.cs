using System;
using System.Collections.Generic;
using ConsoleInteractive.InputRender;

namespace ConsoleInteractive.Components
{
    /// <summary>
    /// Provide components based on name or key
    /// </summary>
    public class ComponentsProvider
    {
        private readonly Dictionary<string, IRender> _items
            = new Dictionary<string, IRender>();

        public bool Register(string name, IRender render, bool overwrite = true) {
            if(_items.ContainsKey(name) && !overwrite) return false;
            _items[name] = render;
            return true;
        }

        public bool Register<TKey>(TKey key, IRender render, bool overwrite = true) where TKey : Enum {
            return Register(BuildName(key), render, overwrite);
        }

        public IRender GetRender(string name) {
            if (_items.ContainsKey(name) && _items[name] is IRender r) return r;
            throw new Exception($"Render not found for {name}");
        }
        public IRender GetRender<TKey>(TKey key) where TKey : Enum
            => GetRender(BuildName(key));

        public IRender<TValue> GetRender<TValue>(string name) {
            var type = typeof(TValue);
            if (_items.ContainsKey(name) && _items[name] is IRender<TValue> r) return r;
            throw new Exception($"Render not found for {name} with type: {type}");
        }

        public IRender GetRender<TKey, TValue>(TKey key) where TKey : Enum
            => GetRender<TValue>(BuildName(key));

        private string BuildName<TKey>(TKey key) where TKey : Enum {
            var typeName = typeof(TKey).Name;
            var value = key.ToString();
            return $"{typeName}:{value}";
        }

        /// <summary>
        /// Default render
        /// </summary>
        /// <returns></returns>
        public static ComponentsProvider Global => new ComponentsProvider();
    }
}