using PTB.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTB.DataAccess
{
    public class PropertyRepository
    {
        protected string PTBConnectionString { get; set; }
        public PropertyRepository()
        {
            PTBConnectionString = ConfigurationManager.ConnectionStrings["PTBConnectionString"].ConnectionString;
        }

        public PropertySearchModel SearchProperty(int pageNumber, int pageSize, string lot, string section, string block)
        {
            using (var conn = new SqlConnection(PTBConnectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand("SearchProperty", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@PageNbr", SqlDbType.NVarChar).Value = pageNumber;
                    cmd.Parameters.Add("@PageSize", SqlDbType.NVarChar).Value = pageSize;
                    cmd.Parameters.Add("@lot", SqlDbType.NVarChar).Value = lot;
                    cmd.Parameters.Add("@section", SqlDbType.NVarChar).Value = section;
                    cmd.Parameters.Add("@block", SqlDbType.NVarChar).Value = block;

                    PropertySearchModel propertySearchModel = new PropertySearchModel();
                    //var myReader = cmd.ExecuteReader();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                propertySearchModel.TotalResultCount = (int)myReader["TotalCount"];
                                propertySearchModel.Properties.Add(new PropertyModel(myReader));
                            }
                            //}

                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return propertySearchModel;
                }
            }
        }

        public PropertySearchModel SearchPropertyByClientInfo(int pageNumber, int pageSize, string firstName, string lastName, string phone, string address, string email)
        {
            using (var conn = new SqlConnection(PTBConnectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand("SearchPropertyByClientInfo", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@PageNbr", SqlDbType.NVarChar).Value = pageNumber;
                    cmd.Parameters.Add("@PageSize", SqlDbType.NVarChar).Value = pageSize;
                    cmd.Parameters.Add("@firstName", SqlDbType.NVarChar).Value = firstName;
                    cmd.Parameters.Add("@lastName", SqlDbType.NVarChar).Value = lastName;
                    cmd.Parameters.Add("@phone", SqlDbType.NVarChar).Value = phone;
                    cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = address;
                    cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;

                    PropertySearchModel propertySearchModel = new PropertySearchModel();
                    //var myReader = cmd.ExecuteReader();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                propertySearchModel.TotalResultCount = (int)myReader["TotalCount"];
                                propertySearchModel.Properties.Add(new PropertyModel(myReader));
                            }
                            //}

                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return propertySearchModel;
                }
            }
        }

        public PropertySearchModel AdvanceSearchProperty(int pageNumber, int pageSize, string houseNumber, string street, string city, string town, string zipCode, bool isSoldProperty)
        {
            using (var conn = new SqlConnection(PTBConnectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand("AdvanceSearchProperty", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@PageNbr", SqlDbType.NVarChar).Value = pageNumber;
                    cmd.Parameters.Add("@PageSize", SqlDbType.NVarChar).Value = pageSize;
                    cmd.Parameters.Add("@houseNumber", SqlDbType.NVarChar).Value = houseNumber;
                    cmd.Parameters.Add("@street", SqlDbType.NVarChar).Value = street;
                    cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = city;
                    cmd.Parameters.Add("@town", SqlDbType.NVarChar).Value = town;
                    cmd.Parameters.Add("@zipCode", SqlDbType.NVarChar).Value = zipCode;
                    cmd.Parameters.Add("@isSoldProperty", SqlDbType.Bit).Value = isSoldProperty;

                    PropertySearchModel propertySearchModel = new PropertySearchModel();
                    //var myReader = cmd.ExecuteReader();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                propertySearchModel.TotalResultCount = (int)myReader["TotalCount"];
                                propertySearchModel.Properties.Add(new PropertyModel(myReader));
                            }
                            //}

                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return propertySearchModel;
                }
            }
        }

        public PropertyModel GetPropertyById(long propertyId)
        {
            using (var conn = new SqlConnection(PTBConnectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand("GetPropertyById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = propertyId;

                    PropertyModel propertyModel = null;
                    //var myReader = cmd.ExecuteReader();
                    using (var myReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (myReader.Read())
                            {
                                propertyModel = new PropertyModel(myReader);
                            }

                            myReader.NextResult();
                            while (myReader.Read())
                            {
                                propertyModel.PropertyAppealValues.Add(new PropertyAppealModel(myReader));
                            }

                            myReader.NextResult();
                            while (myReader.Read())
                            {
                                propertyModel.PropertyAssessedValues.Add(new PropertyAssessedModel(myReader));
                            }

                        }
                        catch (Exception ex)
                        {
                            // LOG ERROR
                            throw ex;
                        }
                    }
                    return propertyModel;
                }
            }
        }

        public bool UpdateProperty(PropertyModel propertyModel)
        {
            using (var conn = new SqlConnection(PTBConnectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand("UpdateProperty", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = propertyModel.PropertyId;
                    cmd.Parameters.Add("@lot", SqlDbType.NVarChar).Value = propertyModel.Lot;
                    cmd.Parameters.Add("@section", SqlDbType.NVarChar).Value = propertyModel.Section;
                    cmd.Parameters.Add("@block", SqlDbType.NVarChar).Value = propertyModel.Block;

                    cmd.Parameters.Add("@owner_name", SqlDbType.NVarChar).Value = propertyModel.OwnerName;
                    cmd.Parameters.Add("@house_numb", SqlDbType.NVarChar).Value = propertyModel.HouseNumber;
                    cmd.Parameters.Add("@st_name", SqlDbType.NVarChar).Value = propertyModel.StName;
                    cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = propertyModel.City;
                    cmd.Parameters.Add("@zip_code", SqlDbType.NVarChar).Value = propertyModel.ZipCode;
                    cmd.Parameters.Add("@SalesDate", SqlDbType.NVarChar).Value = propertyModel.SalesDate;
                    cmd.Parameters.Add("@SalesPrice", SqlDbType.NVarChar).Value = propertyModel.SalesPrice;
                    cmd.Parameters.Add("@Town", SqlDbType.NVarChar).Value = propertyModel.Town;

                    var i = cmd.ExecuteNonQuery();

                    return true;
                }
            }
        }

        public bool DeleteProperty(long id)
        {
            using (var conn = new SqlConnection(PTBConnectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand("DeleteProperty", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = id;

                    var i = cmd.ExecuteNonQuery();

                    return true;
                }
            }
        }
    }
}

