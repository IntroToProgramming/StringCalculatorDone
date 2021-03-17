using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StringCalculatorBlank
{

    
    // https://osherove.com/tdd-kata-1
    public class CalculatorTests
    {
        ILogger dummyLogger = new Mock<ILogger>().Object;
        IWebService dummyWebService = new Mock<IWebService>().Object;
        [Fact]
        public void EmptyStringReturnsZero()
        {
            var calculator = new Calculator(dummyLogger, dummyWebService);

            var response = calculator.Add("");

            Assert.Equal(0, response);
        }

        [Theory]
        [InlineData("1", 1)]
        [InlineData("42", 42)]
        [InlineData("108", 108)]
        public void OneDigit(string numbers, int expected)
        {
            var calculator = new Calculator(dummyLogger, dummyWebService);

            var response = calculator.Add(numbers);

            Assert.Equal(expected, response);
        }

        [Theory]
        [InlineData("1,2" , 3)]
        public void TwoDigits(string numbers, int expected)
        {
            var calculator = new Calculator(dummyLogger, dummyWebService);

            var response = calculator.Add(numbers);

            Assert.Equal(expected, response);
        }

        [Theory]
        [InlineData("1,2", 3)]
        [InlineData("1,2,3,4,5,6,7,8,9", 45)]
        public void ResultsAreLogged(string numbers, int expected)
        {
            var mockedLogger = new Mock<ILogger>();
            var calculator = new Calculator(mockedLogger.Object, dummyWebService);

            calculator.Add(numbers);

            mockedLogger.Verify(o => o.Log(expected));
        }

        [Theory]
        [InlineData("1,2", "Blammo")]
        [InlineData("1,2", "OUCH!!")]
        public void CallsWebServiceOnLoggerException(string numbers, string message)
        {
            var stubbedLogger = new Mock<ILogger>();
            var mockedWebService = new Mock<IWebService>();
            var calculator = new Calculator(stubbedLogger.Object, mockedWebService.Object);
            stubbedLogger.Setup(m => m.Log(It.IsAny<int>()))
                    .Throws(new LoggerException(message));

            calculator.Add(numbers);

            mockedWebService.Verify(m => m.LoggingFailure(message));

        }

        [Fact]
        public void WebServiceNotCalledWhenNoException()
        {
            var stubbedLogger = new Mock<ILogger>();
            var mockedWebService = new Mock<IWebService>();
            var calculator = new Calculator(stubbedLogger.Object, mockedWebService.Object);
            //stubbedLogger.Setup(m => m.Log(It.IsAny<int>()))
            //        .Throws(new LoggerException("Blammo!"));

            calculator.Add("12");

            mockedWebService.Verify(m => m.LoggingFailure(It.IsAny<string>()), Times.Never);
        }
    }
}
