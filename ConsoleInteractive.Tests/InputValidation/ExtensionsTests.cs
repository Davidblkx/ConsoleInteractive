using System.Collections.Generic;
using Xunit;
using ConsoleInteractive.InputValidation;
using System.Threading.Tasks;

namespace ConsoleInteractive.Tests.InputValidation
{
    public class ExtensionsTests
    {
        [Fact]
        public async Task ToValidatorCollectionTest() {
            var target1 = (true, "BATATAS1");
            var target2 = (true, "BATATAS2");
            var target3 = (false, "BATATAS3");

            var list = new List<IValidator<string>>
            {
                (Validator<string>)((string s) => target1),
                (Validator<string>)((string s) => target2),
                (Validator<string>)((string s) => target3)
            };

            var collection = list.ToValidatorCollection();

            Assert.NotNull(collection);
            // Should keep size
            Assert.Equal(3, collection.Count);
            // Should keep order
            Assert.Equal(target1, await collection[0].Validate(""));
            Assert.Equal(target2, await collection[1].Validate(""));
            Assert.Equal(target3, await collection[2].Validate(""));
        }
    }
}