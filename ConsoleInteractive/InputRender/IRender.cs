using System.Threading.Tasks;

namespace ConsoleInteractive.InputRender
{
    /// <summary>
    /// Responsable to request a input and convert to a valid format
    /// </summary>
    public interface IRender
    {
        /// <summary>
        /// Message to show before request input
        /// </summary>
        /// <value></value>
        string Message { get; set; }

        /// <summary>
        /// Render data and request input
        /// </summary>
        /// <returns></returns>
        Task<object?> RequestInput();
    }

    /// <summary>
    /// Responsable to request a input and convert to a valid format
    /// </summary>
    public interface IRender<T> : IRender
    {
        /// <summary>
        /// Render data and request input
        /// </summary>
        /// <returns></returns>
        new Task<T> RequestInput();
    }
}