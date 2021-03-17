using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringCalculatorBlank
{
    public class Calculator
    {
        private ILogger _logger;
        private IWebService _webService;

        public Calculator(ILogger logger, IWebService webService)
        {
            this._logger = logger;
            _webService = webService;
        }

        public int Add(string numbers)
        {
            if(numbers == "")
            {
                return 0;
            } else
            {
                // going to cheat and get to the good parts
                var response = numbers.Split(',')
                        .Select(int.Parse)
                        .Sum();

                try
                {
                    _logger.Log(response);
                }
                catch (Exception ex)
                {

                _webService.LoggingFailure(ex.Message); // EVIL!
                   // gulp
                }
                return response;
            }
        }

    }
}
