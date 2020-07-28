using System;
using System.Collections.Generic;
using System.Text;

namespace Titan.UFC.GraphQL.Model
{
    public class Country
    {
        public string _id { get; set; }
        public string name { get; set; }
    }

    public class CountryList
    {
        public List<Country> Country { get; set; }
    }
}
