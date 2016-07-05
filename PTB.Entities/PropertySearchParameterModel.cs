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
}
