using System.Threading.Tasks;
using ConsoleInteractive.InputValidation;
using Xunit;

namespace ConsoleInteractive.Tests.InputValidation
{
    public class IValidatorCollectionTests
    {
        [Fact]
        public async Task ValidatorCollectionTests() {
            var error10 = "Must equal 10";
            var error20 = "Must equal 20";

            var collection = new ValidatorCollection<int>()
                .Add(e => (e == 10, error10))
                .Add(e => (e == 20, error20));

            IValidatorCollection<int> target1 = collection;
            IValidatorCollection target2 = collection;

            Assert.NotNull(target1);
            var (_, test1_10) = await target1.ValidateInput(40);
            var (_, test1_20) = await target1.ValidateInput(10);
            Assert.Equal(error10, test1_10);
            Assert.Equal(error20, test1_20);

            Assert.NotNull(target2);
            var (_, test2_10) = await target2.ValidateRawInput(40);
            var (_, test2_20) = await target2.ValidateRawInput(10);
            Assert.Equal(error10, test2_10);
            Assert.Equal(error20, test2_20);
        }
    }
}