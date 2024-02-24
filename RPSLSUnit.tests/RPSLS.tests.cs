using System;
using System.Collections.Generic;
using System.Text;
using RPSLS.RPSLS;
using Xunit;

namespace RPSLSUnit.Tests
{
    public class RPSLSTests
    {
        [Fact]
        public void ParseInput_InvalidInput()
        {
            // Arrange, Act, Assert
            int expected = -1;
            int actual = RPSLS.ParseInput("badinput");
            Assert.Equal(expected, actual);
        }
    }
}
