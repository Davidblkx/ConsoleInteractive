using System.Threading.Tasks;

namespace ConsoleInteractive.InputRender
{
    /// <summary>
    /// Responsable to request a input and convert to a valid format
    /// </summary>
    public interface IRender
    {
        /// <summary>
        /// Render data and request input
        /// </summary>
        /// <returns></returns>
        Task<object> Request();
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
        new Task<T> Request();
    }
}