namespace ConsoleInteractive.Components
{
    internal static class DefaultRenderProviders
    {
        /// <summary>
        /// Register default providers in global ComponentsProvider
        /// </summary>
        public static void Register() {
            Register("");
            Register(0);
            Register(0L);
            Register(0.0);
            Register(0.0f);
            Register(0.0d);
        }

        private static void Register<T>(T defaultValue) {
            ComponentsProvider.Global.Register(
                typeof(T).ToString(), InputText.Create<T>("", defaultValue));
        }
    }
}