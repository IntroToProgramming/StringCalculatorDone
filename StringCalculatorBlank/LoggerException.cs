using System;
using System.Runtime.Serialization;

namespace StringCalculatorBlank
{
  
    public class LoggerException : Exception
    {
       

        public LoggerException(string message) : base(message)
        {
            // some stuff I haven't shown you. Good luck. (evil laugh)
        }

       
    }
}