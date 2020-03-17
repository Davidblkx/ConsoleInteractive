using System.Linq;
using System;
using ConsoleInteractive.Tools;
using Xunit;

namespace ConsoleInteractive.Tests.Tools
{
    public class EnumToolsTests
    {
        [Fact]
        public void HasFlagAttributeTest() {
            Assert.True(EnumTools.HasFlagsAttribute<FlagsEnum>());
            Assert.False(EnumTools.HasFlagsAttribute<NoFlagsEnum>());
        }

        [Fact]
        public void GetActiveValuesTests() {
            var value = FlagsEnum.P1 | FlagsEnum.P2;
            var subject = EnumTools.GetActiveValues(value);

            Assert.Contains(FlagsEnum.P1, subject);
            Assert.Contains(FlagsEnum.P2, subject);
            Assert.DoesNotContain(FlagsEnum.P3, subject);
            Assert.DoesNotContain(FlagsEnum.P4, subject);
        }
    }

    [Flags]
    enum FlagsEnum {
        P1 = 1,
        P2 = 2,
        P3 = 4,
        P4 = 8
    }

    enum NoFlagsEnum {
        P1 = 1,
        P2 = 2,
        P3 = 3,
        P4 = 4
    }
}