using System;
using Xunit;
using ConsoleInteractive.Question.Formats;

using static ConsoleInteractive.Question.QuestionFactoryProvider;
using System.Collections.Generic;

namespace ConsoleInteractive.Tests.Question
{
    public class QuestionFactoryProviderTests
    {
        [Fact]
        public void TestRegisterProvider() {
            RegisterQuestionFormat(new StringQuestion());
            var target = TryGetQuestionFactory<string>();
            Assert.NotNull(target);
        }

        [Fact]
        public void TestRegisterDefaultProviders() {
            RegisterDefaultProviders();

            Assert.NotNull(TryGetQuestionFactory<string>());
            Assert.NotNull(TryGetQuestionFactory<long>());
            Assert.NotNull(TryGetQuestionFactory<int>());
            Assert.NotNull(TryGetQuestionFactory<double>());
            Assert.NotNull(TryGetQuestionFactory<float>());
            Assert.NotNull(TryGetQuestionFactory<decimal>());
            Assert.NotNull(TryGetQuestionFactory<ulong>());
            Assert.NotNull(TryGetQuestionFactory<uint>());
            Assert.NotNull(TryGetQuestionFactory<List<string>>());
            Assert.NotNull(TryGetQuestionFactory<List<long>>());
            Assert.NotNull(TryGetQuestionFactory<List<int>>());
            Assert.NotNull(TryGetQuestionFactory<List<double>>());
            Assert.NotNull(TryGetQuestionFactory<List<float>>());
            Assert.NotNull(TryGetQuestionFactory<List<decimal>>());
            Assert.NotNull(TryGetQuestionFactory<List<ulong>>());
            Assert.NotNull(TryGetQuestionFactory<List<uint>>());
        }

        [Fact]
        public void TestGetQuestionFactoryFail() {
            Assert.Null(TryGetQuestionFactory<TestInternalClass>());
            Assert.Throws<Exception>(() => GetQuestionFactory<TestInternalClass>());
        }

        private class TestInternalClass {}
    }
}