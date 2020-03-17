using System;
using System.Threading.Tasks;
using ConsoleInteractive.InputValidation;
using Xunit;

namespace ConsoleInteractive.Tests.InputValidation
{
    public class IValidatorTests
    {
        [Fact]
        public async Task IValidatorTest() {
            var message = "Length must be at least 10";
            var v = new Validator<string>(e => (e.Length > 9, message));
            IValidator<string> target1 = v;
            IValidator target2 = v;

            // Tests target 1
            Assert.NotNull(target1);
            var (t1Success, t1Message) = await target1.Validate("1234567890");
            Assert.True(t1Success);
            Assert.Equal(message, t1Message);

            // Tests target 2
            Assert.NotNull(target2);
            Assert.Equal(typeof(string), target2.Target);
            var (t2Success, t2Message) = await target2.RawValidate("1234567890");
            Assert.True(t2Success);
            Assert.Equal(message, t2Message);
        }
    }
}