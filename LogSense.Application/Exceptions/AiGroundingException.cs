using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSense.Application.Exceptions
{
    public class AiGroundingException : Exception
    {
        public AiGroundingException(string message)
            : base(message)
        {
        }
    }
}
