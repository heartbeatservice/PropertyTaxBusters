using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTB.Entities.Utility;

namespace PTB.Entities
{
    public class PropertyModel
    {
        public long PropertyId { get; set; }
        public string Lot { get; set; }
        public string Section { get; set; }
        public string Block { get; set; }
        public string SalesDate { get; set; }
        public string SalesPrice { get; set; }
        public string Town { get; set; }

        public string OwnerName { get; set; }
        public string HouseNumber { get; set; }
        public string StName { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public List<PropertyAppealModel> PropertyAppealValues { get; set; }
        public List<PropertyAssessedModel> PropertyAssessedValues { get; set; }

        public PropertyModel(IDataReader dbReader)
        {
            if (dbReader == null) return;
            if (dbReader.HasColumn("Id") && dbReader["Id"] != DBNull.Value) PropertyId = (long)dbReader["Id"];
            if (dbReader.HasColumn("lot") && dbReader["lot"] != DBNull.Value) Lot = (string)dbReader["lot"];
            if (dbReader.HasColumn("section") && dbReader["section"] != DBNull.Value) Section = (string)dbReader["section"];
            if (dbReader.HasColumn("block") && dbReader["block"] != DBNull.Value) Block = (string)dbReader["block"];
            if (dbReader.HasColumn("SalesDate") && dbReader["SalesDate"] != DBNull.Value) SalesDate = (string)dbReader["SalesDate"];
            if (dbReader.HasColumn("SalesPrice") && dbReader["SalesPrice"] != DBNull.Value) SalesPrice = (string)dbReader["SalesPrice"];
            if (dbReader.HasColumn("Town") && dbReader["Town"] != DBNull.Value) Town = (string)dbReader["Town"];

            if (dbReader.HasColumn("owner_name") && dbReader["owner_name"] != DBNull.Value) OwnerName = (string)dbReader["owner_name"];
            if (dbReader.HasColumn("house_numb") && dbReader["house_numb"] != DBNull.Value) HouseNumber = (string)dbReader["house_numb"];
            if (dbReader.HasColumn("st_name") && dbReader["st_name"] != DBNull.Value) StName = (string)dbReader["st_name"];
            if (dbReader.HasColumn("city") && dbReader["city"] != DBNull.Value) City = (string)dbReader["city"];
            if (dbReader.HasColumn("zip_code") && dbReader["zip_code"] != DBNull.Value) ZipCode = (string)dbReader["zip_code"];
            PropertyAppealValues = new List<PropertyAppealModel>();
            PropertyAssessedValues = new List<PropertyAssessedModel>();
            //Customer = new Customer(dbReader);
            //PrimaryModel = new Model(dbReader);
        }
    }

    public class PropertyAppealModel
    {
        public string RollYear { get; set; }
        public string AppealType { get; set; }
        public string ReferenceNo { get; set; }
        public string Status { get; set; }
        public string UpdatedAssessment { get; set; }

        public PropertyAppealModel(IDataReader dbReader)
        {
            if (dbReader.HasColumn("RollYear") && dbReader["RollYear"] != DBNull.Value) RollYear = (string)dbReader["RollYear"];
            if (dbReader.HasColumn("AppealType") && dbReader["AppealType"] != DBNull.Value) AppealType = (string)dbReader["AppealType"];
            if (dbReader.HasColumn("ReferenceNo") && dbReader["ReferenceNo"] != DBNull.Value) ReferenceNo = (string)dbReader["ReferenceNo"];
            if (dbReader.HasColumn("Status") && dbReader["Status"] != DBNull.Value) Status = (string)dbReader["Status"];
            if (dbReader.HasColumn("UpdatedAssessment") && dbReader["UpdatedAssessment"] != DBNull.Value) UpdatedAssessment = (string)dbReader["UpdatedAssessment"];
        }

    }

    public class PropertyAssessedModel
    {
        public string Year { get; set; }
        public long AssessedValue { get; set; }
        public string TaxRollStatus { get; set; }

        public PropertyAssessedModel(IDataReader dbReader)
        {
            if (dbReader.HasColumn("Year") && dbReader["Year"] != DBNull.Value) Year = (string)dbReader["Year"];
            if (dbReader.HasColumn("AssessedValue") && dbReader["AssessedValue"] != DBNull.Value) AssessedValue = (long)dbReader["AssessedValue"];
            if (dbReader.HasColumn("TaxRollStatus") && dbReader["TaxRollStatus"] != DBNull.Value) TaxRollStatus = (string)dbReader["TaxRollStatus"];
        }
    }
}
