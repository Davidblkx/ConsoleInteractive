using System.Reflection;
using System.Collections.Generic;
using ConsoleInteractive.InputRender;
using System.Threading.Tasks;
using System.Linq;
using ConsoleInteractive.Components;
using ConsoleInteractive.InputValidation;
using System;
using System.Collections;

namespace ConsoleInteractive.Form
{
    public class ConsoleForm<T> where T : new()
    {
        private readonly Dictionary<PropertyInfo, (IRender, ulong)> _entries
            = new Dictionary<PropertyInfo, (IRender, ulong)>();

        /// <summary>
        /// Instantiate <T> to call initialization
        /// </summary>
        /// <returns></returns>
        public ConsoleForm() { var _ = new T(); }
            
        /// <summary>
        /// Add a new FormEntry for a property
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="component"></param>
        /// <param name="priority">Render priority, 0 is max priority</param>
        public void AddFormEntry(PropertyInfo prop, IRender component, ulong priority = ulong.MaxValue) {
            _entries[prop] = (component, priority);
        }

        /// <summary>
        /// Add a new FormEntry for a property
        /// </summary>
        /// <param name="propName">name of property</param>
        /// <param name="component"></param>
        /// <param name="priority">Render priority, 0 is max priority</param>
        public void AddFormEntry(string propName, IRender component, ulong priority = ulong.MaxValue) {
            var prop = typeof(T).GetProperty(propName);
            AddFormEntry(prop, component, priority);
        }

        /// <summary>
        /// Render all components and build response
        /// </summary>
        /// <returns></returns>
        public async Task<T> Request() {
            var result = new T();

            var list = GetEntriesByPriority();

            foreach (var (prop, component) in list) {
                var res = await component.RequestInput();
                if (res is null) continue;
                SetValue(prop, result, res);
            }

            return result;
        }

        private void SetValue(PropertyInfo prop, T obj, Object value) {
            if(prop.PropertyType.IsEnum) {
                if (value is IEnumerable v) {
                    var e = v.GetEnumerator();
                    e.MoveNext();
                    prop.SetValue(obj, e.Current);
                    return;
                }
            }
            prop.SetValue(obj, value);
        }

        /// <summary>
        /// Get list of renders, by priority
        /// </summary>
        /// <returns></returns>
        private IEnumerable<(PropertyInfo, IRender)> GetEntriesByPriority() {
            return _entries
                .OrderBy(e => e.Value.Item2)
                .Select(e => (e.Key, e.Value.Item1));
        }
    }

    public static class ConsoleForm {
        /// <summary>
        /// Create ConsoleForm for a type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ConsoleForm<T> BuildForm<T>() where T : new() {
            var form = new ConsoleForm<T>();

            foreach (var prop in typeof(T).GetProperties()) {
                var attr = prop.GetCustomAttribute<FormEntry>();
                if (attr is null) continue;

                var component = BuildComponent(attr, prop);
                if (component is null) continue;

                form.AddFormEntry(prop, component, attr.Priority);
            }

            return form;
        }

        private static IRender? BuildComponent(FormEntry entry, PropertyInfo prop) {
            var componentKey = entry.ProviderKey ?? prop.PropertyType.ToString();
            var component = ComponentsProvider.Global.GetRender(componentKey);
            
            var val = GetValidator(entry.ValidatorsKey);
            if (!(val is null)) component?.SetValidator(val);
            
            if (!(component is null)) {
                component.Message = entry.Message ?? component.Message;
            }

            return component;
        }

        private static IValidatorCollection? GetValidator(string? key) {
            if (string.IsNullOrEmpty(key)) return null;
            return ValidatorProvider.Global.GetCollection(key!);
        }
    }
}