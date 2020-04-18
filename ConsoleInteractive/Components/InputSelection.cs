using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsoleInteractive.InputRender;
using ConsoleInteractive.InputValidation;
using ConsoleInteractive.InputConverter;

namespace ConsoleInteractive.Components
{
    public class InputSelection<T> : BaseInputRender<IEnumerable<T>>
    {
        private static readonly string BUFFER_NAME = "#INTERNAL.COMPONENT.INPUTSELECT.TOP";
        private readonly List<SelectOption> _options = new List<SelectOption>();

        public IEnumerable<SelectOption> Options => _options;

        /// <summary>
        /// Max number of items that can be selected
        /// </summary>
        /// <value></value>
        public int MaxSelected { get; set; } = 1;

        /// <summary>
        /// Max number of items to render
        /// </summary>
        /// <value></value>
        public int MaxItemsToRender { get; set; } = 6;

        /// <summary>
        /// Caret to render
        /// </summary>
        /// <value></value>
        public char CaretChar { get; set; } = '>';

        /// <summary>
        /// Message to show before requesting input
        /// </summary>
        /// <value></value>
        public string Message { get; set; } = "Select an option";
        
        /// <summary>
        /// Add options to current list
        /// </summary>
        /// <param name="option"></param>
        public InputSelection<T> AddOption(params SelectOption[] option) {
            _options.AddRange(option);
            return this;
        }

        /// <summary>
        /// Add options to current list
        /// </summary>
        /// <param name="option"></param>
        public InputSelection<T> AddOption(IEnumerable<T> options) {
            _options.AddRange(
                options.Select(o => new SelectOption(o)));
            return this;
        }

        /// <summary>
        /// Clear current list of options
        /// </summary>
        public InputSelection<T> ResetOptions() {
            _options.Clear();
            return this;
        }

        public InputSelection<T> SetMessage(string message) {
            Message = message;
            return this;
        }

        public InputSelection<T> SetMaxSelected(int max) {
            MaxSelected = max;
            return this;
        }
        
        public override async Task<IEnumerable<T>> RequestInput()
        {
            var selected = RenderAndSelectOptions();
            var data = GetOptionsFromSelection(selected);
            var (valid, message) = await Validators.ValidateInput(data);
            
            if (!valid) throw new Exception(message ?? "Error validating input");
            return data;
        }

        /// <summary>
        /// Class to represent an option
        /// </summary>
        public class SelectOption {
            public T Value { get; set; }
            public string Text { get; set; }

            public SelectOption(T value, string? text = default) {
                Value = value;
                Text = text ?? value?.ToString() ?? "Unkown";
            }

            public static implicit operator SelectOption(T value) => new SelectOption(value);
            public static implicit operator SelectOption((T, string) opt) => 
                new SelectOption(opt.Item1, opt.Item2);
        }

        /// <summary>
        /// Render options and return selected ones
        /// </summary>
        /// <returns></returns>
        private IEnumerable<int> RenderAndSelectOptions() {
            List<int> selected = new List<int>();
            var startAt = 0;
            var row = 0;
            Console.CursorVisible = false;
            ConsoleBuffer.MemoriseBufferPosition(BUFFER_NAME);

            bool exit = false;
            while(!exit || selected.Count == 0)
            {
                RenderOptions(selected, startAt, row);
                var key = Console.ReadKey();
                // Update params according to key press
                (exit, startAt, row, selected) = 
                    ExecuteAction(key.Key, startAt, row, selected);
            }
            Console.CursorVisible = true;

            return selected;
        }

        /// <summary>
        /// Execute action for a key press
        /// </summary>
        /// <param name="key"></param>
        /// <param name="startAt"></param>
        /// <param name="row"></param>
        /// <param name="selected"></param>
        /// <returns></returns>
        private (bool exit, int startAt, int row, List<int> selected) ExecuteAction(
            ConsoleKey key, int startAt, int row, List<int> selected) {
                bool exit = false;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        (startAt, row) = MoveCaret(-1, startAt, row);
                    break;

                    case ConsoleKey.DownArrow:
                        (startAt, row) = MoveCaret(1, startAt, row);
                    break;

                    case ConsoleKey.Spacebar:
                        selected = TriggerSelected(row + startAt, selected);
                    break;

                    case ConsoleKey.Enter:
                        exit = true;
                    break;
                }
                return (exit, startAt, row, selected);
        }

        /// <summary>
        /// Return values from options, that have index present in list
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private IEnumerable<T> GetOptionsFromSelection(IEnumerable<int> list) {
            return _options
                .Where((opt, index) => list.Contains(index))
                .Select(o => o.Value);
        }

        /// <summary>
        /// Add or remove index from a list
        /// </summary>
        /// <param name="index"></param>
        /// <param name="selected"></param>
        /// <returns></returns>
        private List<int> TriggerSelected(int index, List<int> selected) {
            if (selected.Contains(index))
                selected.Remove(index);
            else 
                selected.Add(index);
            // Remove first value if max number of selected was reached
            if (selected.Count > MaxSelected) selected.RemoveAt(0);

            return selected;
        }

        /// <summary>
        /// Move row index by a defined value.
        /// </summary>
        /// <param name="step">number of steps to move, positive number move down, negative up</param>
        /// <param name="startAt">postion to start render</param>
        /// <param name="row">current postion</param>
        /// <returns></returns>
        private (int startAt, int row) MoveCaret(int step, int startAt, int row) {
            var lastRow = CalculateLastOptionToRender(startAt) - 1; // base 0
            row+=step;
            if (row < 0) row = 0;
            if (row > lastRow) row = lastRow;
            return (CalculateStartAt(startAt+step), row);
        }

        /// <summary>
        /// Checks if start postion is out of boundaries and calculate right value
        /// </summary>
        /// <param name="startAt">target position</param>
        /// <returns></returns>
        private int CalculateStartAt(int startAt) {
            // Max index allowed to render
            int lastStartAt = _options.Count - MaxItemsToRender;
            if (startAt > lastStartAt) startAt = lastStartAt;
            if (startAt < 0) startAt = 0;
            return startAt;
        }

        /// <summary>
        /// Render option list
        /// </summary>
        /// <param name="selected">selected items</param>
        /// <param name="startAt">position to start show</param>
        /// <param name="row">current position</param>
        private void RenderOptions(IEnumerable<int> selected, int startAt = 0, int row = 0) {
            ConsoleBuffer.ClearBufferFrom(BUFFER_NAME);

            int renderUntil = CalculateLastOptionToRender(startAt);
            
            for(var i = startAt; i < renderUntil; i++) {
                var caret = GetCursorForPosition(startAt - i, row);
                var isSelected = selected.Contains(i) ? "(*)" : "( )";

                Console.WriteLine($"{caret}{isSelected} {_options[i].Text}");
            }
            Console.WriteLine("\nPress [SPACE] to select, [ENTER] to accept selection");
        }

        /// <summary>
        /// Calculate last option index to render
        /// </summary>
        /// <param name="start">index of postion to start render</param>
        /// <returns></returns>
        private int CalculateLastOptionToRender(int start) {
            int maxRender = start + MaxItemsToRender;
            return _options.Count > maxRender ? maxRender : _options.Count;
        }

        /// <summary>
        /// returns the string to render on cursor position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="cursorPostion"></param>
        /// <returns></returns>
        private string GetCursorForPosition(int position, int cursorPostion) {
            return cursorPostion == position ? CaretChar.ToString() : " ";
        }
    }

    public static class InputSelection {
        /// <summary>
        /// Create intput from enum
        /// </summary>
        public static InputSelection<T> FromEnum<T>(
            string? message = default,
            IValidatorCollection<IEnumerable<T>>? validators = default, 
            StringConverterProvider? provider = default
        ) where T : Enum {
            var input = From(message, validators, provider);

            foreach(var n in Enum.GetNames(typeof(T))) {
                input.AddOption(((T)Enum.Parse(typeof(T), n), n));
            }

            return input;
        }

        /// <summary>
        /// create a InputSelection from a type T
        /// </summary>
        public static InputSelection<T> From<T>(
            string? message = default,
            IValidatorCollection<IEnumerable<T>>? validators = default, 
            StringConverterProvider? provider = default
        ) {
            return new InputSelection<T> {
                Message = message ?? "Select an option",
                Validators = validators ?? ValidatorCollection.Create<IEnumerable<T>>(),
                ConverterProvider = provider ?? StringConverterProvider.Global
            };
        }
    }
}