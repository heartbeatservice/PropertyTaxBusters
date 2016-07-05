using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyTaxBusters.DataAccess
{
    public class PropertyInfoDataAccess
    {
        public List<PropertyScrapeData_TBL> GetScrapeDataByTableName(string tableName)
        {
            PropertyTaxBustersEntities entity = new PropertyTaxBustersEntities();
            var tbl = entity.GetScrapeDataByTableName(tableName);
            return tbl.ToList();
        }
        public string UpdatePropertyInfo(DataTable dt, string filename, DateTime startDate)
        {
            int insertedCount = 0;
            int updatedCount = 0;
            PropertyTaxBustersEntities entity = new PropertyTaxBustersEntities();
            StringBuilder falloutArray = new StringBuilder();

            foreach (DataRow item in dt.Rows)
            {

                try
                {
                    string lot, sec, block;
                    lot = GetValue(dt, item, "lot");
                    sec = GetValue(dt, item, "sec");
                    block = GetValue(dt, item, "block");
                    Property_TBL proptbl = entity.Property_TBL.Where(i => i.lot == lot && i.section == sec && i.block == block).FirstOrDefault();
                    if (proptbl == null)
                    {
                        FallOut_TBL ftbl = new FallOut_TBL();
                        ftbl.Block = block;
                        ftbl.CreatedDate = DateTime.Now;
                        ftbl.FileName = filename;
                        ftbl.Lot = lot;
                        ftbl.Section = sec;
                        entity.FallOut_TBL.Add(ftbl);
                        //AddToFallOutCSVFile(e.Name + "," + lot + "," + sec + "," + block + "," + DateTime.Now.ToString("yyyy-MM-ddTHH':'mm':'sszzz"));
                        //falloutArray.AppendLine(filename + "," + lot + "," + sec + "," + block + "," + DateTime.Now.ToString("yyyy-MM-ddTHH':'mm':'sszzz"));
                        //proptbl = new Property_TBL();
                        //isnewEntry = true;

                        insertedCount++;
                    }
                    else
                    {

                        #region Set the Fields from Excel
                        proptbl.SalesDate = GetValue(dt, item, "salesdate");
                        proptbl.SalesPrice = GetValue(dt, item, "salespric");
                        if (Convert.ToString(proptbl.SalesPrice).Equals(string.Empty))
                        {
                            proptbl.SalesPrice = GetValue(dt, item, "salespric");
                        }
                        //proptbl.Town = GetValue(dt, item, "Town");
                        //proptbl.old_name = GetValue(dt, item, "seller");
                        proptbl.owner_name = GetValue(dt, item, "buyer");
                        #endregion
                        updatedCount++;
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog_TBL etbl1 = new ErrorLog_TBL();
                    etbl1.Exception = ex.Message;
                    etbl1.CreatedDate = DateTime.Now;
                    etbl1.FileName = filename;
                    etbl1.StackTrace = ex.StackTrace;
                    
                    entity.ErrorLog_TBL.Add(etbl1);
                    string[] errlog = { DateTime.Now + " File Name:" + filename + " Exception:" + ex.Message + " Stack Trace:" + ex.StackTrace };
                    
                }
                //entity.Property_TBL.Add(proptbl);
            }
            ErrorLog_TBL etbl = new ErrorLog_TBL();
            etbl.Exception = "Started From : " + startDate.ToString() + " End Date : " + DateTime.Now.ToString();
            etbl.CreatedDate = DateTime.Now;
            etbl.FileName = filename;
            etbl.StackTrace = "Started From : " + startDate.ToString() + " End Date : " + DateTime.Now.ToString();

            entity.ErrorLog_TBL.Add(etbl);

            entity.SaveChanges();
            return falloutArray.ToString();
        }

        public static string GetValue(DataTable dt, DataRow dr, string columnName)
        {
            //for (int i = 0; i < dt.Columns.Count; i++)
            //{
            //    if (dt.Columns[i].ColumnName.Equals(columnName, StringComparison.InvariantCultureIgnoreCase))

            //}
            try
            {
                return Convert.ToString(dr[columnName]);
            }
            catch 
            {
                return string.Empty;
            }

        }
    }
}
