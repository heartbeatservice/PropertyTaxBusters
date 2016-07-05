using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTB.Entities
{
    public class PropertySearchModel
    {
        public int TotalResultCount { get; set; }

        public string TotalResultCountFormatted
        {
            get
            {
                if (TotalResultCount > 0)
                    return string.Format("{0:##,#}", TotalResultCount);

                return string.Empty;
            }
        }

        public List<PropertyModel> Properties { get; set; }

        public PropertySearchModel()
        {
            Properties = new List<PropertyModel>();
            //BeneficiaryModels = new List<BeneficiaryModel>();
        }
    }
}
