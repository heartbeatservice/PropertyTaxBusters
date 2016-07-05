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
            try
            {
                Console.WriteLine(DateTime.Now.ToString());
                ChromeOptions options = new ChromeOptions();

                ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                service.SuppressInitialDiagnosticInformation = true;

                DateTime startDate = DateTime.Now;
                PropertyTaxBustersEntities entity = new PropertyTaxBustersEntities();
                string tblName = ConfigurationManager.AppSettings["TableName"];
                List<PropertyScrapeData_TBL> scrapItems = entity.GetScrapeDataByTableName(tblName).ToList(); //entity.PropertyScrapeData_TBL.ToList();

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

                    // Extract the text and save it into result.txt
                    //var result = driver.FindElementByXPath("//div[@id='case_login']/h3").Text;
                    //File.WriteAllText("result.txt", result);

                    // Take a screenshot and save it into screen.png

                    foreach (var item in scrapItems)
                    {
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
                                        year2 = (string.IsNullOrEmpty(year1) ? "" : year2.Substring(0, year2.IndexOf('-')).Trim());

                                        var year3 = GetElementValues(driver, "//div[@id='infovaltab']/div/table/thead/tr/th[4]");
                                        year3 = (string.IsNullOrEmpty(year1) ? "" : year3.Substring(0, year3.IndexOf('-')).Trim());

                                        var assesValue1 = GetElementValues(driver, "//div[@id='infovaltab']/div/table/tbody/tr[3]/td[2]");
                                        var assesValue2 = GetElementValues(driver, "//div[@id='infovaltab']/div/table/tbody/tr[3]/td[3]");
                                        var assesValue3 = GetElementValues(driver, "//div[@id='infovaltab']/div/table/tbody/tr[3]/td[4]");

                                        var taxRollStatus1 = GetElementValues(driver, "//div[@id='infovaltab']/div/table/tbody/tr[4]/td[2]");
                                        var taxRollStatus2 = GetElementValues(driver, "//div[@id='infovaltab']/div/table/tbody/tr[4]/td[3]");
                                        var taxRollStatus3 = GetElementValues(driver, "//div[@id='infovaltab']/div/table/tbody/tr[4]/td[4]");

                                        var result = "Year 1:" + year1 + ", Year 2:" + year2 + ",Year 3" + year3;//driver.FindElementByXPath("//div[@id='infovaltab']/div/table").Text;
                                        result += "\n Assessed Value 1:" + assesValue1 + ", Assessed Value 2:" + assesValue2 + ", Assessed Value 3:" + assesValue3 + "\n";

                                        var a1 = driver.FindElementByClassName("inline");

                                        driver.Navigate().GoToUrl(a1.GetAttribute("href"));

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

                                        entity.PropertyAssessedValue_TBL.Add(InsertAssessedValue(assesValue1, year1, taxRollStatus1, property.Id));
                                        entity.PropertyAssessedValue_TBL.Add(InsertAssessedValue(assesValue2, year2, taxRollStatus2, property.Id));
                                        entity.PropertyAssessedValue_TBL.Add(InsertAssessedValue(assesValue3, year3, taxRollStatus3, property.Id));
                                        if (!string.IsNullOrEmpty(appealType1) && !string.IsNullOrEmpty(rollYear1))
                                        {
                                            entity.PropertyAppealValues_TBL.Add(InsertAppealValue(appealType1, rollYear1, status1, updatedAssessment1, property.Id));
                                        }
                                        if (!string.IsNullOrEmpty(appealType2) && !string.IsNullOrEmpty(rollYear2))
                                            entity.PropertyAppealValues_TBL.Add(InsertAppealValue(appealType2, rollYear2, status2, updatedAssessment2, property.Id));
                                    }
                                    else
                                    {
                                        entity.PropertyScrapeDetail_TBL.Add(InsertScrapeData(false, property.Id));
                                    }
                                    entity.PropertyScrapeDetail_TBL.Add(InsertScrapeData(true, property.Id));
                                    Console.WriteLine(DateTime.Now.ToString());
                                    //File.WriteAllText("result" + item.Id + ".txt", result);
                                }
                                catch (Exception ex)
                                {
                                    entity.PropertyScrapeDetail_TBL.Add(InsertScrapeData(false, property.Id));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string[] errlog = { DateTime.Now + " Exception:" + ex.Message + " Stack Trace:" + ex.StackTrace };
                            File.AppendAllLines(ConfigurationManager.AppSettings["ErrorLogFile"], errlog);
                            string[] errData = { DateTime.Now + ", Section:" + item.Section + ", Block:" + item.Block + ",Lot:" + item.Lot};
                            File.AppendAllLines(ConfigurationManager.AppSettings["ErrorData"], errData);
                        }
                    }
                    entity.SaveChanges();
                    Console.WriteLine(DateTime.Now.ToString());
                    Console.WriteLine(startDate - DateTime.Now);
                    Console.ReadKey();
                    //driver.GetScreenshot().SaveAsFile(@"screen.png", ImageFormat.Png);
                }
            }
            catch (Exception ex)
            {
                string[] errlog = { DateTime.Now + " Exception:" + ex.Message + " Stack Trace:" + ex.StackTrace };
                File.AppendAllLines(ConfigurationManager.AppSettings["ErrorLogFile"], errlog);
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

        static PropertyAppealValues_TBL InsertAppealValue(string appealType, string year, string status, string updatedAssesment, long propId)
        {
            PropertyAppealValues_TBL appealValue = new PropertyAppealValues_TBL();
            appealValue.AppealType = appealType;
            appealValue.Created = DateTime.Now;
            appealValue.CreatedBy = 1;
            appealValue.PropertyId = propId;
            appealValue.ReferenceNo = status;
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