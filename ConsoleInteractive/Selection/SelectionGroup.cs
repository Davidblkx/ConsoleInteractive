using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleInteractive.Selection
{
    public interface ISelectionGroup<T> : IEnumerable<SelectionOption<T>>, IEnumerable {
        public SelectionOption<T> this[int index] { get; }
    }

    public class SelectionGroup<T> : ISelectionGroup<T>
    {
        private readonly List<SelectionOption<T>> _items =
            new List<SelectionOption<T>>();

        /// <summary>
        /// Add a collection of options
        /// </summary>
        public SelectionGroup<T> AddRange(IEnumerable<SelectionOption<T>> options) {
            _items.AddRange(options);
            return this;
        }

        /// <summary>
        /// Add a new option
        /// </summary>
        public SelectionGroup<T> Add(SelectionOption<T> option) {
            _items.Add(option);
            return this;
        }

        /// <summary>
        /// Add a new option
        /// </summary>
        public SelectionGroup<T> Add(T value, string? text = default) {
            _items.Add(new SelectionOption<T>(value, text));
            return this;
        }

        public int Count => _items.Count;

        public SelectionOption<T> this[int index] => _items[index];

        public IEnumerator<SelectionOption<T>> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public static class SelectionGroup {

        /// <summary>
        /// Create list of options from an Enum
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        public static SelectionGroup<T> FromEnum<T>() where T : Enum {
            var group = new SelectionGroup<T>();

            foreach(var n in Enum.GetNames(typeof(T))) {
                group.Add((T)Enum.Parse(typeof(T), n), n);
            }

            return group;
        }

        /// <summary>
        /// Select value from a list of options
        /// </summary>
        /// <param name="options"></param>
        /// <param name="max">max allowed to selected</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> Select<T>(SelectionGroup<T> options, int max = 1) {
            var selected = new List<int>();
            var startAt = 0;
            var row = 0;
            var exit = false;

            void moveUp() {
                row--;
                if (row < 0) {
                    row = 0;
                    startAt--;
                    if (startAt < 0) startAt = 0;
                }
            };

            void moveDown() {
                row++;
                var max = options.Count;
                if (max > 5) max = 5;
                if (row > max) {
                    row = max;
                    startAt++;
                    if (startAt > options.Count - 6) startAt = options.Count - 6;
                    if (startAt < 0) startAt = 0;
                }
            };

            void triggerRow() {
                var val = row + startAt;
                if (selected.Contains(val)) {
                    selected.Remove(val);
                } else {
                    selected.Add(val);
                }
                if (selected.Count > max)
                    selected.RemoveAt(0);
            }

            Console.CursorVisible = false;
            ConsoleBuffer.MemoriseBufferPosition(BUFFER_NAME);

            do {
                exit = false;
                RenderSelectionOptions(options, selected, startAt, row);
                
                var key = Console.ReadKey();

                switch (key.Key) {
                    case ConsoleKey.UpArrow:
                        moveUp();
                        break;
                    
                    case ConsoleKey.DownArrow:
                        moveDown();
                        break;

                    case ConsoleKey.Spacebar:
                        triggerRow();
                        break;

                    case ConsoleKey.Enter:
                        exit = true;
                        break;
                }
            } while(!exit || selected.Count == 0);
            Console.CursorVisible = true;

            return options
                .Where((options, index) => selected.Contains(index))
                .Select(o => o.Value);
        }

        private static readonly string BUFFER_NAME = "INTERNAL_BUFFER_NAME#SELECTION_GROUP";

        private static void RenderSelectionOptions<T>(SelectionGroup<T> options, IEnumerable<int> selected, int startAt = 0, int row = 0) {
            ConsoleBuffer.ClearBufferFrom(BUFFER_NAME);

            int renderUntil = options.Count();
            if (renderUntil > startAt + 6) renderUntil = startAt + 6;
            
            for(var i = startAt; i < renderUntil; i++) {
                var rowSel = row == (i - startAt) ? ">" : " ";
                var rowCheck = selected.Contains(i) ? "(*)" : "( )";

                Console.WriteLine($"{rowSel}{rowCheck} {options[i].Text}");
            }
            Console.WriteLine("\nPress [SPACE] to select, [ENTER] to acept selection");
        }
    }
}