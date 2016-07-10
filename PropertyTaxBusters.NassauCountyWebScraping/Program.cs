using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using PropertyTaxBusters.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyTaxBusters.NassauCountyWebScraping
{
    class Program
    {
        static void Main(string[] args)
        {
            long counter = 0;
            while (true)
            {
                //PropertyTaxBustersEntities entity1 = new PropertyTaxBustersEntities();
                //string tblName = ConfigurationManager.AppSettings["TableName"]
                #region scrape data
                try
                {
                    Console.WriteLine(DateTime.Now.ToString());
                    ChromeOptions options = new ChromeOptions();

                    ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                    service.SuppressInitialDiagnosticInformation = true;

                    DateTime startDate = DateTime.Now;

                    using (var driver = new ChromeDriver(service, options))
                    {
                        driver.Manage().Window.Maximize();
                        // Go to the home page

                        driver.Navigate().GoToUrl("https://lrv.nassaucountyny.gov/login/");

                        // Get the page elements
                        var userNameField = driver.FindElementByName("user");
                        var userPasswordField = driver.FindElementByName("pass");
                        var loginButton = driver.FindElementById("singlebutton");

                        // Type user name and password
                        userNameField.SendKeys(ConfigurationManager.AppSettings["User"]);// "myyaaz@yahoo.com");
                        userPasswordField.SendKeys(ConfigurationManager.AppSettings["Password"]);// "myyaaz123");

                        // and click the login button
                        loginButton.Click();
                        System.Threading.Thread.Sleep(2000);
                        try
                        {
                            var usr = driver.FindElementByName("user");
                            if (usr != null)
                            {
                                Console.WriteLine("Login unsuccessfull!");
                                Console.WriteLine("Press any key to continue");
                                Console.ReadKey();
                                //return;
                                continue;
                            }

                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            var usr1 = driver.FindElementByXPath("//li[@class='pull-right dropdown']/a/b/span").Text;
                            if (usr1 != "MYYAAZ@YAHOO.COM")
                            {
                                Console.WriteLine("Login unsuccessfull!");
                                Console.WriteLine("Press any key to continue");
                                Console.ReadKey();
                                //return;
                                continue;
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Login unsuccessfull!");
                            Console.WriteLine("Press any key to continue");
                            Console.ReadKey();
                            //return;
                            continue;
                        }
                        // Extract the text and save it into result.txt
                        //var result = driver.FindElementByXPath("//div[@id='case_login']/h3").Text;
                        //File.WriteAllText("result.txt", result);

                        // Take a screenshot and save it into screen.png
                        PropertyTaxBustersEntities entity = new PropertyTaxBustersEntities();
                        entity.Database.CommandTimeout = 180;
                        string tblName = ConfigurationManager.AppSettings["TableName"];
                        entity.ExecLoadSP(ConfigurationManager.AppSettings["SPName"], ConfigurationManager.AppSettings["Section"]); //entity.PropertyScrapeData_TBL.ToList();
                        List<PropertyScrapeData_TBL> scrapItems = entity.GetScrapeDataByTableName(tblName).ToList(); //entity.PropertyScrapeData_TBL.ToList();
                        if (scrapItems.Count == 0)
                        {
                            Console.WriteLine(DateTime.Now.ToString());
                            Console.WriteLine(startDate - DateTime.Now);
                            Console.WriteLine("Section " + ConfigurationManager.AppSettings["Section"] + "is finished!");
                            Console.ReadKey();
                            return;
                        }
                        foreach (var item in scrapItems)
                        {
                            counter++;
                            driver.Navigate().GoToUrl("https://lrv.nassaucountyny.gov/");

                            try
                            {
                                var section = driver.FindElementById("sec");
                                var block = driver.FindElementById("blk");
                                var lot = driver.FindElementById("lot");

                                section.SendKeys(item.Section);
                                block.SendKeys(item.Block);
                                lot.SendKeys(item.Lot);

                                var searchbtn = driver.FindElementByName("subad");
                                Actions actions = new Actions(driver);
                                actions.MoveToElement(searchbtn);
                                actions.Perform();
                                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(" + searchbtn.Location.X.ToString() + ", " + searchbtn.Location.Y.ToString() + ")");
                                searchbtn.Click();
                                Property_TBL property = entity.Property_TBL.Where(i => i.section == item.Section &&
                                                                   i.lot == item.Lot &&
                                                                   i.block == item.Block).FirstOrDefault();

                                if (property != null)
                                {
                                    try
                                    {
                                        var errorMessage = GetElementValues(driver, "//div[@class='alert alert-danger spacer']");
                                        if (string.IsNullOrEmpty(errorMessage))
                                        {
                                            var year1 = GetElementValues(driver, "//div[@id='infovaltab']/div/table/thead/tr/th[2]");
                                            year1 = (string.IsNullOrEmpty(year1) ? "" : year1.Substring(0, year1.IndexOf('-')).Trim());

                                            var year2 = GetElementValues(driver, "//div[@id='infovaltab']/div/table/thead/tr/th[3]");
                                            year2 = (string.IsNullOrEmpty(year2) ? "" : year2.Substring(0, year2.IndexOf('-')).Trim());

                                            var year3 = GetElementValues(driver, "//div[@id='infovaltab']/div/table/thead/tr/th[4]");
                                            year3 = (string.IsNullOrEmpty(year3) ? "" : year3.Substring(0, year3.IndexOf('-')).Trim());

                                            var assesValue1 = GetElementValues(driver, "//div[@id='infovaltab']/div/table/tbody/tr[3]/td[2]");
                                            var assesValue2 = GetElementValues(driver, "//div[@id='infovaltab']/div/table/tbody/tr[3]/td[3]");
                                            var assesValue3 = GetElementValues(driver, "//div[@id='infovaltab']/div/table/tbody/tr[3]/td[4]");

                                            var taxRollStatus1 = GetElementValues(driver, "//div[@id='infovaltab']/div/table/tbody/tr[4]/td[2]");
                                            var taxRollStatus2 = GetElementValues(driver, "//div[@id='infovaltab']/div/table/tbody/tr[4]/td[3]");
                                            var taxRollStatus3 = GetElementValues(driver, "//div[@id='infovaltab']/div/table/tbody/tr[4]/td[4]");
                                            int result;
                                            if (int.TryParse(taxRollStatus1, out result) || taxRollStatus1.Equals(string.Empty))
                                            {
                                                taxRollStatus1 = GetElementValues(driver, "//div[@id='infovaltab']/div/table/tbody/tr[5]/td[2]");
                                            }
                                            if (int.TryParse(taxRollStatus2, out result) || taxRollStatus2.Equals(string.Empty))
                                            {
                                                taxRollStatus2 = GetElementValues(driver, "//div[@id='infovaltab']/div/table/tbody/tr[5]/td[3]");
                                            }
                                            if (int.TryParse(taxRollStatus3, out result) || taxRollStatus3.Equals(string.Empty))
                                            {
                                                taxRollStatus3 = GetElementValues(driver, "//div[@id='infovaltab']/div/table/tbody/tr[5]/td[4]");
                                            }
                                            var result1 = "Year 1:" + year1 + ", Year 2:" + year2 + ",Year 3" + year3;//driver.FindElementByXPath("//div[@id='infovaltab']/div/table").Text;
                                            result1 += "\n Assessed Value 1:" + assesValue1 + ", Assessed Value 2:" + assesValue2 + ", Assessed Value 3:" + assesValue3 + "\n";

                                            try
                                            {
                                                var a1 = driver.FindElementByClassName("inline");
                                                driver.Navigate().GoToUrl(a1.GetAttribute("href"));
                                            }
                                            catch (Exception)
                                            {
                                            }

                                            var rollYear1 = GetElementValues(driver, "//div[@class='table-responsive']/table/tbody/tr[2]/td[1]");
                                            var rollYear2 = GetElementValues(driver, "//div[@class='table-responsive']/table/tbody/tr[3]/td[1]");

                                            var appealType1 = GetElementValues(driver, "//div[@class='table-responsive']/table/tbody/tr[2]/td[2]");
                                            var appealType2 = GetElementValues(driver, "//div[@class='table-responsive']/table/tbody/tr[3]/td[2]");

                                            var referenceNo1 = GetElementValues(driver, "//div[@class='table-responsive']/table/tbody/tr[2]/td[3]");
                                            var referenceNo2 = GetElementValues(driver, "//div[@class='table-responsive']/table/tbody/tr[3]/td[3]");

                                            var status1 = GetElementValues(driver, "//div[@class='table-responsive']/table/tbody/tr[2]/td[4]");
                                            var status2 = GetElementValues(driver, "//div[@class='table-responsive']/table/tbody/tr[3]/td[4]");

                                            var updatedAssessment1 = GetElementValues(driver, "//div[@class='table-responsive']/table/tbody/tr[2]/td[5]");
                                            var updatedAssessment2 = GetElementValues(driver, "//div[@class='table-responsive']/table/tbody/tr[3]/td[5]");
                                            #region Assessed Value Update & Insert
                                            var assValue1Tbl = entity.PropertyAssessedValue_TBL.Where(i => i.PropertyId == property.Id && i.Year == year1).ToList();
                                            if (assValue1Tbl != null && assValue1Tbl.Count > 0)
                                            {
                                                foreach (var a1 in assValue1Tbl)
                                                {
                                                    a1.AssessedValue = Convert.ToInt64(assesValue1 == "" ? "0" : assesValue1);
                                                    a1.Year = year1;
                                                    a1.Modified = DateTime.Now;
                                                    a1.ModifyBy = 2;
                                                    a1.PropertyId = property.Id;
                                                    a1.TaxRollStatus = taxRollStatus1;
                                                }
                                            }
                                            else
                                            {
                                                entity.PropertyAssessedValue_TBL.Add(
                                                    InsertAssessedValue(assesValue1 == "" ? "0" : assesValue1, year1, taxRollStatus1, property.Id)
                                                    );
                                            }
                                            var assValue2Tbl = entity.PropertyAssessedValue_TBL.Where(i => i.PropertyId == property.Id && i.Year == year2).ToList();
                                            if (assValue2Tbl != null && assValue2Tbl.Count > 0)
                                            {
                                                foreach (var a in assValue2Tbl)
                                                {
                                                    a.AssessedValue = Convert.ToInt64(assesValue2 == "" ? "0" : assesValue2);
                                                    a.Year = year2;
                                                    a.Modified = DateTime.Now;
                                                    a.ModifyBy = 2;
                                                    a.PropertyId = property.Id;
                                                    a.TaxRollStatus = taxRollStatus2;
                                                }
                                            }
                                            else
                                            {
                                                entity.PropertyAssessedValue_TBL.Add(
                                                InsertAssessedValue(assesValue2 == "" ? "0" : assesValue2, year2, taxRollStatus2, property.Id)
                                                );
                                            }
                                            var assValue3Tbl = entity.PropertyAssessedValue_TBL.Where(i => i.PropertyId == property.Id && i.Year == year3).ToList();
                                            if (assValue3Tbl != null && assValue3Tbl.Count > 0)
                                            {
                                                foreach (var a1 in assValue3Tbl)
                                                {
                                                    a1.AssessedValue = Convert.ToInt64(assesValue3 == "" ? "0" : assesValue3);
                                                    a1.Year = year3;
                                                    a1.Modified = DateTime.Now;
                                                    a1.ModifyBy = 2;
                                                    a1.PropertyId = property.Id;
                                                    a1.TaxRollStatus = taxRollStatus3;
                                                }
                                            }
                                            else
                                            {
                                                entity.PropertyAssessedValue_TBL.Add(
                                                InsertAssessedValue(assesValue3 == "" ? "0" : assesValue3, year3, taxRollStatus3, property.Id)
                                                );
                                            }
                                            #endregion
                                            #region Appeal value UPdate & Insert
                                            if (!string.IsNullOrEmpty(appealType1) && !string.IsNullOrEmpty(rollYear1))
                                            {
                                                var appValue1Tbl = entity.PropertyAppealValues_TBL.Where(i => i.PropertyId == property.Id && i.RollYear == rollYear1).ToList();
                                                if (appValue1Tbl != null && appValue1Tbl.Count > 0)
                                                {
                                                    foreach (var a in appValue1Tbl)
                                                    {
                                                        a.AppealType = appealType1;
                                                        a.Modified = DateTime.Now;
                                                        a.ModifyBy = 2;
                                                        a.PropertyId = property.Id;
                                                        a.ReferenceNo = referenceNo1;
                                                        a.UpdatedAssessment = updatedAssessment1;
                                                        a.Status = status1;
                                                        a.RollYear = rollYear1;
                                                    }
                                                }
                                                else
                                                {
                                                    entity.PropertyAppealValues_TBL.Add(InsertAppealValue(appealType1, rollYear1, status1, referenceNo1, updatedAssessment1, property.Id));
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(appealType2) && !string.IsNullOrEmpty(rollYear2))
                                            {
                                                var appValue2Tbl = entity.PropertyAppealValues_TBL.Where(i => i.PropertyId == property.Id && i.RollYear == rollYear2).ToList();
                                                if (appValue2Tbl != null && appValue2Tbl.Count > 0)
                                                {
                                                    foreach (var a in appValue2Tbl)
                                                    {
                                                        a.AppealType = appealType2;
                                                        a.Modified = DateTime.Now;
                                                        a.ModifyBy = 2;
                                                        a.PropertyId = property.Id;
                                                        a.ReferenceNo = referenceNo2;
                                                        a.UpdatedAssessment = updatedAssessment2;
                                                        a.Status = status2;
                                                        a.RollYear = rollYear2;
                                                    }
                                                }
                                                else
                                                {
                                                    entity.PropertyAppealValues_TBL.Add(InsertAppealValue(appealType2, rollYear2, status2, referenceNo2, updatedAssessment2, property.Id));
                                                }
                                            }
                                            #endregion
                                            var scrapTbl = entity.PropertyScrapeDetail_TBL.Where(i => i.PropertyId == property.Id).ToList();
                                            if (scrapTbl != null && scrapTbl.Count > 0)
                                            {
                                                foreach (var a in scrapTbl)
                                                {
                                                    a.Created = DateTime.Now;
                                                    a.IsFound = true;
                                                }
                                            }
                                            else
                                            {
                                                entity.PropertyScrapeDetail_TBL.Add(InsertScrapeData(true, property.Id));
                                            }
                                        }
                                        else
                                        {
                                            var scrapTbl = entity.PropertyScrapeDetail_TBL.Where(i => i.PropertyId == property.Id).ToList();
                                            if (scrapTbl != null && scrapTbl.Count > 0)
                                            {
                                                foreach (var a in scrapTbl)
                                                {
                                                    a.Created = DateTime.Now;
                                                    a.IsFound = false;
                                                }
                                            }
                                            else
                                            {
                                                entity.PropertyScrapeDetail_TBL.Add(InsertScrapeData(false, property.Id));
                                            }
                                        }
                                        //entity.PropertyScrapeDetail_TBL.Add(InsertScrapeData(true, property.Id));
                                        Console.WriteLine(DateTime.Now.ToString() + " Counter:" + counter.ToString());
                                        //File.WriteAllText("result" + item.Id + ".txt", result);
                                    }
                                    catch (Exception ex)
                                    {
                                        var scrapTbl = entity.PropertyScrapeDetail_TBL.Where(i => i.PropertyId == property.Id).ToList();
                                        if (scrapTbl != null && scrapTbl.Count > 0)
                                        {
                                            foreach (var a in scrapTbl)
                                            {
                                                a.Created = DateTime.Now;
                                                a.IsFound = false;
                                            }
                                        }
                                        else
                                        {
                                            entity.PropertyScrapeDetail_TBL.Add(InsertScrapeData(false, property.Id));
                                        }
                                        string[] errlog = { "Property ID:" + property.Id + ", " + DateTime.Now + " Exception:" + ex.Message + " Stack Trace:" + ex.StackTrace };
                                        File.AppendAllLines(ConfigurationManager.AppSettings["ErrorLogFile"], errlog);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                string[] errlog = { DateTime.Now + " Exception:" + ex.Message + " Stack Trace:" + ex.StackTrace };
                                File.AppendAllLines(ConfigurationManager.AppSettings["ErrorLogFile"], errlog);
                                string[] errData = { DateTime.Now + ", Section:" + item.Section + ", Block:" + item.Block + ",Lot:" + item.Lot };
                                File.AppendAllLines(ConfigurationManager.AppSettings["ErrorData"], errData);
                            }                            
                        }
                        entity.SaveChanges();
                        Console.WriteLine(DateTime.Now.ToString());
                        Console.WriteLine(startDate - DateTime.Now);
                        string[] log = { DateTime.Now + " Start Time:" + startDate.ToString() + " End Time:" + DateTime.Now.ToString() };
                        File.AppendAllLines(ConfigurationManager.AppSettings["LogFile"], log);
                        //driver.GetScreenshot().SaveAsFile(@"screen.png", ImageFormat.Png);
                    }
                }
                catch (Exception ex)
                {
                    string[] errlog = { DateTime.Now + " Exception:" + ex.Message + " Stack Trace:" + ex.StackTrace };
                    File.AppendAllLines(ConfigurationManager.AppSettings["ErrorLogFile"], errlog);
                }
                #endregion
            }

        }

        static string GetElementValues(ChromeDriver driver, string xpath)
        {
            try
            {
                return driver.FindElementByXPath(xpath).Text;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        static PropertyAssessedValue_TBL InsertAssessedValue(string assd, string year, string status, long propId)
        {
            PropertyAssessedValue_TBL assdValue = new PropertyAssessedValue_TBL();
            assdValue.AssessedValue = Convert.ToInt64(assd);
            assdValue.Year = year;
            assdValue.Created = DateTime.Now;
            assdValue.CreatedBy = 1;
            assdValue.PropertyId = propId;
            assdValue.TaxRollStatus = status;
            return assdValue;
        }

        static PropertyAppealValues_TBL InsertAppealValue(string appealType, string year, string status, string refNo, string updatedAssesment, long propId)
        {
            PropertyAppealValues_TBL appealValue = new PropertyAppealValues_TBL();
            appealValue.AppealType = appealType;
            appealValue.Created = DateTime.Now;
            appealValue.CreatedBy = 1;
            appealValue.PropertyId = propId;
            appealValue.ReferenceNo = refNo;
            appealValue.UpdatedAssessment = updatedAssesment;
            appealValue.Status = status;
            appealValue.RollYear = year;
            return appealValue;
        }

        static PropertyScrapeDetail_TBL InsertScrapeData(bool isFound, long propId)
        {
            PropertyScrapeDetail_TBL scrapeDetail = new PropertyScrapeDetail_TBL();
            scrapeDetail.Created = DateTime.Now;
            scrapeDetail.IsFound = isFound;
            scrapeDetail.PropertyId = propId;
            return scrapeDetail;
        }
    }
}