using System;
using System.Threading.Tasks;
using ConsoleInteractive.InputValidation;
using Xunit;

namespace ConsoleInteractive.Tests.InputValidation
{
    public class ValidationProviderTests
    {
        [Fact]
        public void TestRegisterRetriveValid() {
            var provider = GetProvider();
            var target = "TEST1";
            
            provider.Register(target, ValidatorCollection.Create<long>());

            var subject = provider.GetCollection(target);
            Assert.NotNull(subject);
            Assert.False(subject is IValidatorCollection<object>);
            Assert.True(subject is IValidatorCollection<long>);
            Assert.NotNull(provider.GetCollection<long>(target));
        }

        [Fact]
        public void TestRegisterRetriveInvalid() {
            var provider = GetProvider();
            var target = "TEST1";
            var validTarget = "BATATAS";
            
            provider.Register(validTarget, ValidatorCollection.Create<long>());

            var subject = provider.GetCollection(target);
            Assert.NotNull(subject);
            Assert.True(subject is IValidatorCollection<object>);
            Assert.False(subject is IValidatorCollection<long>);
            Assert.ThrowsAny<InvalidCastException>(() => provider.GetCollection<long>(target));
            Assert.ThrowsAny<InvalidCastException>(() => provider.GetCollection<string>(validTarget));
        }

        [Fact]
        public async Task TestRetriveFlagEnum() {
            var provider = GetProvider();

            var target1 = StringValidator.NotEmpty;
            var target2 = StringValidator.NotEmpty | StringValidator.MinLength3;
            var target3 = StringValidator.NotEmpty | StringValidator.MinLength3 | StringValidator.MinLength7;
            var target4 = StringValidator.NotEmpty | StringValidator.MinLength3 | StringValidator.MinLength7 | StringValidator.Lowercase;

            var subject1 = provider.GetCollection<StringValidator, string>(target1);
            var subject2 = provider.GetCollection<StringValidator, string>(target2);
            var subject3 = provider.GetCollection<StringValidator, string>(target3);
            var subject4 = provider.GetCollection<StringValidator, string>(target4);

            Assert.Equal(1, subject1.Count);
            Assert.Equal(2, subject2.Count);
            Assert.Equal(3, subject3.Count);
            Assert.Equal(4, subject4.Count);

            var (test1_0, _) = await provider.Validate(target1, "");
            var (test1_1, _) = await provider.Validate(target1, "BA");
            Assert.False(test1_0);
            Assert.True(test1_1);

            var (test2_0, _) = await provider.Validate(target2, "");
            var (test2_1, _) = await provider.Validate(target2, "BA");
            var (test2_2, _) = await provider.Validate(target2, "BATA");
            Assert.False(test2_0);
            Assert.False(test2_1);
            Assert.True(test2_2);

            var (test3_0, _) = await provider.Validate(target3, "");
            var (test3_1, _) = await provider.Validate(target3, "BA");
            var (test3_2, _) = await provider.Validate(target3, "BATA");
            var (test3_3, _) = await provider.Validate(target3, "BATATAS");
            Assert.False(test3_0);
            Assert.False(test3_1);
            Assert.False(test3_2);
            Assert.True(test3_3);

            var (test4_0, _) = await provider.Validate(target4, "");
            var (test4_1, _) = await provider.Validate(target4, "BA");
            var (test4_2, _) = await provider.Validate(target4, "BATA");
            var (test4_3, _) = await provider.Validate(target4, "BATATAS");
            var (test4_4, _) = await provider.Validate(target4, "batatas");
            Assert.False(test4_0);
            Assert.False(test4_1);
            Assert.False(test4_2);
            Assert.False(test4_3);
            Assert.True(test4_4);
        }

        private ValidatorProvider GetProvider() {
            var provider = new ValidatorProvider();

            provider.Register(
                StringValidator.NotEmpty,
                ValidatorCollection.Create<string>()
                    .Add(e => (!string.IsNullOrEmpty(e), "Can't be empty")));

            provider.Register(
                StringValidator.MinLength3,
                ValidatorCollection.Create<string>()
                    .Add(e => (e.Length >= 3, "Length must be at least 3")));

            provider.Register(
                StringValidator.MinLength7,
                ValidatorCollection.Create<string>()
                    .Add(e => (e.Length >= 7, "Length must be at least 7")));

            provider.Register(
                StringValidator.Lowercase,
                ValidatorCollection.Create<string>()
                    .Add(e => (e == e.ToLower(), "Must be in lowercase")));

            return provider;
        }
    }

    [Flags]
    enum StringValidator {
        NotEmpty = 1,
        MinLength3 = 2,
        MinLength7 = 4,
        Lowercase = 8,
    }
}