using System;
using System.Collections.Generic;
using System.Text;

namespace Titan.UFC.GraphQL.Model
{
    public class Data
    {
        public string login { get; set; }
    }

    public class LoginResponse
    {
        public Data data { get; set; }
        public String Error { get; set; }
    }
}
