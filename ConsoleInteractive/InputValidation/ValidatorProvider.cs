using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleInteractive.Tools;

namespace ConsoleInteractive.InputValidation
{
    /// <summary>
    /// Allow to store and retrieve validatorCollections by name
    /// </summary>
    public class ValidatorProvider
    {
        private readonly Dictionary<string, IValidatorCollection> _data =
            new Dictionary<string, IValidatorCollection>();

        /// <summary>
        /// Register a new collection with a name
        /// </summary>
        /// <param name="name">name to store collection</param>
        /// <param name="coll">collection to store</param>
        /// <param name="overwrite">set if should replace existing name</param>
        /// <returns>true, if saved</returns>
        public bool Register(string name, IValidatorCollection coll, bool overwrite = true) {
            if (_data.ContainsKey(name) && !overwrite) return false;
            _data[name] = coll;
            return true;
        }

        /// <summary>
        /// Register a new collection with a name
        /// </summary>
        /// <param name="key">Enum value to store</param>
        /// <param name="coll">collection to store</param>
        /// <param name="overwrite">set if should replace existing name</param>
        /// <typeparam name="T">Enum type</typeparam>
        /// <returns>true, if saved</returns>
        public bool Register<T>(T key, IValidatorCollection coll, bool overwrite = true) where T : Enum {
            return Register(key.ToString(), coll, overwrite);
        }
        
        /// <summary>
        /// Get collection for name, if no collection is found, 
        /// a empty one is returned
        /// </summary>
        /// <param name="name">reference name</param>
        /// <returns></returns>
        public IValidatorCollection GetCollection(string name) {
            if (!_data.ContainsKey(name)) return ValidatorCollection.Create<object>();

            return _data[name];
        }

        /// <summary>
        /// Get collection for key from Enum, if enum has flags,
        /// multiple collections are merge into one
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IValidatorCollection GetCollection<T>(T key) where T : Enum {
            return EnumTools.GetActiveValues(key)
                .Select(e => GetCollection(e.ToString()))
                .Aggregate((prev, next) => prev is null ? next : prev.MergeCollection(next));
        }

        /// <summary>
        /// Get collection for name, if no collection is found, 
        /// a empty one is returned.null If cant cast to type, an 
        /// Exception is raized
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="TTarget"></typeparam>
        /// <returns></returns>
        public IValidatorCollection<TTarget> GetCollection<TTarget>(string name) {
            var collection = GetCollection(name);
            if (collection is IValidatorCollection<TTarget> c) return c;
            var type = typeof(TTarget);
            throw new InvalidCastException($"Can't cast collection [{name}] to target [{type}]");
        }

        /// <summary>
        /// Get collection for name, if no collection is found, 
        /// a empty one is returned.null If cant cast to type, an 
        /// Exception is raized
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="TTarget"></typeparam>
        /// <returns></returns>
        public IValidatorCollection<TTarget> GetCollection<TKey, TTarget>(TKey key) where TKey : Enum {
            return EnumTools.GetActiveValues(key)
                .Select(e => GetCollection<TTarget>(e.ToString()))
                .Aggregate((prev, next) => prev is null ? next : prev.MergeCollection(next));
        }

        /// <summary>
        /// Use collection with [name] to validate [value]
        /// </summary>
        /// <returns></returns>
        public Task<(bool, string?)> Validate(string name, object value) {
            return GetCollection(name).ValidateRawInput(value);
        }

        /// <summary>
        /// Use collection with [name] to validate [value]
        /// </summary>
        /// <returns></returns>
        public Task<(bool, string?)> Validate<T>(T key, object value) where T : Enum {
            return GetCollection(key).ValidateRawInput(value);
        }

        /// <summary>
        /// Use collection with [name] to validate [value]
        /// </summary>
        /// <returns></returns>
        public Task<(bool, string?)> Validate<TKey, TTarget>(TKey key, TTarget value) where TKey : Enum {
            return GetCollection<TKey, TTarget>(key).ValidateInput(value);
        }

        /// <summary>
        /// Use collection with [name] to validate [value]
        /// </summary>
        /// <returns></returns>
        public Task<(bool, string?)> Validate<TTarget>(string name, TTarget value) {
            return GetCollection<TTarget>(name).ValidateInput(value);
        }

        /// <summary>
        /// Global validator provider
        /// </summary>
        /// <returns></returns>
        public readonly static ValidatorProvider Global = new ValidatorProvider();
    }
}