using ConsoleInteractive.InputConverter;
using Xunit;

namespace ConsoleInteractive.Tests.InputConverterTests
{
    public class InputConverterFactoryTests
    {
        [Theory]
        [InlineData("batatas")]
        [InlineData(10)]
        [InlineData(15L)]
        [InlineData(10.5F)]
        [InlineData(5.5D)]
        [InlineData(true)]
        [InlineData('t')]
        public void ConvertStringTest(object input) {
            var converter = StringConverterFactory.Create(input.GetType());
            var toString = converter.ToString(input);
            var toSource = converter.ToObject(toString);

            Assert.Equal(input, toSource);
        }
    }
}