using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.File;
using Microsoft.Azure;
using System.Data;
using System.IO;
using System.Text;

namespace ImportPropertyWorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("ImportPropertyWorkerRole is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("ImportPropertyWorkerRole has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("ImportPropertyWorkerRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("ImportPropertyWorkerRole has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                                CloudConfigurationManager.GetSetting("StorageConnectionString"));

                // Create a CloudFileClient object for credentialed access to File storage.
                CloudFileClient fileClient = storageAccount.CreateCloudFileClient();

                // Get a reference to the file share we created previously.
                CloudFileShare share = fileClient.GetShareReference("propertyexcels");

                // Ensure that the share exists.
                if (share.Exists())
                {
                    // Get a reference to the root directory for the share.
                    CloudFileDirectory rootDir = share.GetRootDirectoryReference();

                    // Get a reference to the directory we created previously.
                    CloudFileDirectory inputDir = rootDir.GetDirectoryReference("input");
                    CloudFileDirectory outputDir = rootDir.GetDirectoryReference("output");
                    // Ensure that the directory exists.
                    if (inputDir.Exists())
                    {
                        // Get a reference to the file we created previously.
                        IEnumerable<IListFileItem> files = inputDir.ListFilesAndDirectories();
                        //CloudFile file = inputDir.GetFileReference("1.csv");

                        // Ensure that the file exists.
                        foreach (IListFileItem file in files)
                        {
                            DateTime startDate = DateTime.Now;
                            string url = WebUtility.UrlDecode(file.Uri.AbsolutePath.Substring(file.Uri.AbsolutePath.LastIndexOf('/') + 1));
                            CloudFile infile = inputDir.GetFileReference(url);
                            Trace.TraceInformation(url);
                            
                            if (infile.Exists())
                            {
                                // Write the contents of the file to the console window.
                                Trace.TraceInformation(infile.Name);
                                
                                //string csvText = string.Empty;
                                //csvText = infile.DownloadText();

                                //byte[] csvByte = Encoding.UTF8.GetBytes(csvText);
                                //MemoryStream stream = new MemoryStream(csvByte);
                                DataTable dt = new DataTable();

                                MemoryStream memstream = new MemoryStream();
                                
                                await infile.DownloadToStreamAsync(memstream).ConfigureAwait(false);

                                if (infile.Name.EndsWith(".csv"))
                                    dt = GetDateTableFromCSV(memstream);
                                else if (infile.Name.EndsWith(".xlsx"))
                                    dt = GetDataTableFromExcel(memstream);
                                PropertyTaxBusters.DataAccess.PropertyInfoDataAccess pdataAccess = new PropertyTaxBusters.DataAccess.PropertyInfoDataAccess();
                                pdataAccess.UpdatePropertyInfo(dt, infile.Name, startDate);


                                //AddToFallOutCSVFile(falloutArray.ToString());
                                Trace.TraceInformation(dt.Rows.Count.ToString());
                                Trace.TraceInformation("Update Completed");
                                CloudFile outfile = outputDir.GetFileReference(infile.Name);
                                outfile.UploadText(infile.DownloadText());
                                infile.Delete();
                            }
                        }

                    }
                }
                Trace.TraceInformation("Working");
                await Task.Delay(100000);
            }
        }

        private DataTable GetDateTableFromCSV(Stream stream)
        {
            using (StreamReader sr = new StreamReader(stream))
            {
                string[] headers = sr.ReadLine().Split(',');
                DataTable dt = new DataTable();
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }
                return dt;
            }
        }

        public static DataTable GetDataTableFromExcel(Stream stream)
        {
            bool hasHeader = true;
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                try
                {                    
                    pck.Load(stream);
                    var ws = pck.Workbook.Worksheets.First();
                    DataTable tbl = new DataTable();
                    foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                    {
                        tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                    }
                    var startRow = hasHeader ? 2 : 1;
                    for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                    {
                        var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                        DataRow row = tbl.Rows.Add();
                        foreach (var cell in wsRow)
                        {
                            row[cell.Start.Column - 1] = cell.Text;
                        }
                    }
                    return tbl;
                }
                catch (Exception ex)
                {
                    
                }
            }
            return null;
        }
    }
}
