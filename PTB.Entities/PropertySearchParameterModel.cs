using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTB.Entities
{
    public class PropertySearchParameterModel
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string Lot { get; set; }
        public string Section { get; set; }
        public string Block { get; set; }
    }

    public class PropertyClientInfoSearchModel
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string email { get; set; }
    }

    public class PropertyAdvanceSearchModel
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string houseNumber { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string town { get; set; }
        public string zipCode { get; set; }
    }
}
