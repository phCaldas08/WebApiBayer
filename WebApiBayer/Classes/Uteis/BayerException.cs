using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiBayer.Classes.Uteis
{
    public class BayerException : Exception
    {
        public BayerException(string message) : base(message)
        {

        }
    }
}