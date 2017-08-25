using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Data;
using vincontrol.Backend.Data;
using vincontrol.DataFeed.Model;

namespace vincontrol.DataFeed.Helper
{
    public class ExportConvertHelper
    {
        private MySQLHelper _mySqlHelper;
        private ExportXMLHelper _exportXMLHelper;
        private EmailHelper _emailHelper;
        private const string TOKEN_DEALERID = "[DealerId]";
        private const string TOKEN_DEALERNAME = "[DealerName]";
        private const string TOKEN_DATETIMENOW = "[DateTime.Now]";
        private const string TOKEN_PROFILEID = "[ProfileId]";
        private const string TOKEN_COMPANYNAME = "[CompanyName]";
        private const string TOKEN_NEW = "[New]";
        private const string TOKEN_USED = "[Used]";
        private const string TOKEN_RECEIVERID = "[ReceiverId]";
        private LogFile _log;

        public ExportConvertHelper()
        {
            _mySqlHelper = new MySQLHelper();
            _exportXMLHelper = new ExportXMLHelper();
            _emailHelper = new EmailHelper();
        }

        #region Public Methods
        
        public DataTable InitializeFileWithHeader(string name, List<string> columns)
        {
            var workTable = new DataTable(name);
            foreach (var column in columns)
            {
                workTable.Columns.Add(column, typeof(String));
            }

            return workTable;
        }
        
        public void ExportToFile(int companyProfileId, bool upToSvn)
        {
            var listExportDealer = _mySqlHelper.GetDealerExportList(companyProfileId);
            var mappingTemplate = _exportXMLHelper.LoadMappingTemplateByCompany(companyProfileId);
            if (String.IsNullOrEmpty(mappingTemplate.ExportFileType))
                throw new InvalidOperationException("Invalid file type");
            
            var companyProfile = _mySqlHelper.LoadCompanyProfile(companyProfileId);
            
            var localFolder = ConfigurationManager.AppSettings["LocalBackupExportDataFeed"] + "\\" + companyProfile.CompanyName;
            var localFtpFolder = ConfigurationManager.AppSettings["VinFtpExportDataFeed"];

            if (!Directory.Exists(localFolder))
            {
                Directory.CreateDirectory(localFolder);
            }

            var errors = new List<DealerViewModel>();
            
            if (companyProfile.Bundle.GetValueOrDefault())
            {
                if (String.IsNullOrEmpty(companyProfile.FileName))
                {
                    throw new InvalidOperationException("Invalid file name");
                }

                // Apply template for file name
                companyProfile.FileName = companyProfile.FileName.Replace(TOKEN_PROFILEID, companyProfile.Id.ToString());
                companyProfile.FileName = companyProfile.FileName.Replace(TOKEN_RECEIVERID, companyProfile.Id.ToString());
                companyProfile.FileName = companyProfile.FileName.Replace(TOKEN_COMPANYNAME, companyProfile.CompanyName);
                companyProfile.FileName = companyProfile.FileName.Replace(TOKEN_DEALERID, companyProfile.Id.ToString());
                companyProfile.FileName = companyProfile.FileName.Replace(TOKEN_DEALERNAME, companyProfile.CompanyName);
                companyProfile.FileName = companyProfile.FileName.Replace(TOKEN_DATETIMENOW, DateTime.Now.ToString("MMddyyy"));
                companyProfile.FileName = companyProfile.FileName.Replace(TOKEN_NEW, "NEW");
                companyProfile.FileName = companyProfile.FileName.Replace(TOKEN_USED, "USED");

                companyProfile.FileName += "." + mappingTemplate.ExportFileType.ToLower();
                var fullFilePath = localFolder + "\\" + companyProfile.FileName;

                _log = new LogFile(ConfigurationManager.AppSettings["DataFeedLog"], companyProfile.FileName);
                _log.ErrorLog("START: Export data feed for company " + companyProfile.CompanyName);

                var exportHistory = new exportservicehistory();
                if (upToSvn)
                {
                    using (var context = new vincontrolwarehouseEntities())
                    {
                        exportHistory.RunningDate = DateTime.Now;
                        exportHistory.Status = RunningStatus.Running;
                        exportHistory.DatafeedProfileId = companyProfile.Id;
                        exportHistory.ArchiveFileName = companyProfile.FileName;

                        context.AddToexportservicehistories(exportHistory);
                        context.SaveChanges();
                    }
                }
                var inventoryList = new List<VehicleViewModel>();
                foreach (var dealer in listExportDealer)
                {
                    inventoryList.AddRange(GetInventoryListByStatus(dealer.Id, mappingTemplate.InventoryStatus));
                }

                if (mappingTemplate.ExportFileType.ToLower().Equals("csv"))
                {
                    //var csvWriterDealerAds = new CsvExport<VehicleViewModel>(inventoryList);
                    //csvWriterDealerAds.ExportToFileWithHeader(fullFilePath, mappingTemplate.HasHeader);
                    var csvWriter = new CSVWriter();
                    csvWriter.GenerateCsv(mappingTemplate, inventoryList, fullFilePath, mappingTemplate.HasHeader, ",");
                }
                else
                {
                    var csvWriter = new CSVWriter();
                    csvWriter.GenerateCsv(mappingTemplate, inventoryList, fullFilePath, mappingTemplate.HasHeader, mappingTemplate.Delimeter);
                }

                var backupFileName = DateTime.Now.ToString("MMddyyyhhmmss") + "_" + companyProfile.FileName;

                if (upToSvn)
                {
                    // Upload file to company's server
                    try
                    {
                        FTPHelper.ConnectToFtpServer(companyProfile.FTPServer.Trim(), 21, companyProfile.DefaultUserName.Trim(), companyProfile.DefaultPassword.Trim());
                        //FTPHelper.FtpClient.ChangeDirectory(localFtpFolder);
                        FTPHelper.UploadToFtpServer(fullFilePath, companyProfile.FileName);
                    }
                    catch (Exception ex)
                    {
                        MarkExportTaskCompleted(exportHistory.Id, backupFileName, RunningStatus.FTPError);
                        _log.ErrorLog("ERROR: FTP Server/Account is not correct for " + companyProfile.CompanyName);
                        throw new InvalidOperationException("ERROR: FTP Server/Account is not correct for " + companyProfile.CompanyName);
                    }
                    
                    FileStream objFileStream = File.Open(fullFilePath, FileMode.Open);
                    FTPHelper.StoreBackupFileOnLocal(localFolder, backupFileName, CommonHelper.GetBytes(objFileStream));
                }
                
                // mark this running task is complete
                if (upToSvn)
                    MarkImportTaskCompleted(exportHistory.Id, backupFileName);
                
                _log.ErrorLog("END: Export data feed for company " + companyProfile.CompanyName);
            }
            else
            {
                foreach (var dealer in listExportDealer)
                {
                    var dealerExportFileName = _mySqlHelper.GetDealerExportFileName(dealer.Id, companyProfile);
                    if (String.IsNullOrEmpty(dealerExportFileName))
                        dealerExportFileName = companyProfile.CompanyName + "_" + dealer.Id;
                    dealerExportFileName += "." + mappingTemplate.ExportFileType.ToLower();

                    dealerExportFileName = dealerExportFileName.Replace(TOKEN_PROFILEID, companyProfile.Id.ToString());
                    dealerExportFileName = dealerExportFileName.Replace(TOKEN_DEALERID, dealer.Id.ToString());
                    dealerExportFileName = dealerExportFileName.Replace(TOKEN_DEALERNAME, dealer.Name);
                    dealerExportFileName = dealerExportFileName.Replace(TOKEN_DATETIMENOW, DateTime.Now.ToString("MMddyyy"));
                    dealerExportFileName = dealerExportFileName.Replace(TOKEN_NEW, "NEW");
                    dealerExportFileName = dealerExportFileName.Replace(TOKEN_USED, "USED");

                    _log = new LogFile(ConfigurationManager.AppSettings["DataFeedLog"], dealerExportFileName);
                    _log.ErrorLog("START: Export data feed for dealer " + dealer.Name);
                    
                    try
                    {
                        var combinedlocalFolder = localFolder + "\\" + dealer.Id;
                        if (!Directory.Exists(combinedlocalFolder))
                        {
                            Directory.CreateDirectory(combinedlocalFolder);
                        }
                        var fullFilePath = combinedlocalFolder + "\\" + dealerExportFileName;

                        var exportHistory = new exportservicehistory();
                        if (upToSvn)
                        {
                            using (var context = new vincontrolwarehouseEntities())
                            {
                                exportHistory.RunningDate = DateTime.Now;
                                exportHistory.Status = RunningStatus.Running;
                                exportHistory.DatafeedProfileId = companyProfile.Id;
                                exportHistory.DealerId = dealer.Id;
                                exportHistory.ArchiveFileName = dealerExportFileName;

                                context.AddToexportservicehistories(exportHistory);
                                context.SaveChanges();
                            }
                        }
                        var inventoryList = GetInventoryListByStatus(dealer.Id, mappingTemplate.InventoryStatus);

                        if (mappingTemplate.ExportFileType.ToLower().Equals("csv"))
                        {
                            //var csvWriterDealerAds = new CsvExport<VehicleViewModel>(inventoryList);
                            //csvWriterDealerAds.ExportToFileWithHeader(fullFilePath, mappingTemplate.HasHeader);
                            var csvWriter = new CSVWriter();
                            csvWriter.GenerateCsv(mappingTemplate, inventoryList, fullFilePath, mappingTemplate.HasHeader, ",");
                        }
                        else
                        {
                            var csvWriter = new CSVWriter();
                            csvWriter.GenerateCsv(mappingTemplate, inventoryList, fullFilePath, mappingTemplate.HasHeader, mappingTemplate.Delimeter);
                        }
                        var backupFileName = DateTime.Now.ToString("MMddyyyhhmmss") + "_" + dealerExportFileName;

                        if (upToSvn)
                        {
                            // Upload file to company's server
                            try
                            {
                                FTPHelper.ConnectToFtpServer(companyProfile.FTPServer.Trim(), 21, companyProfile.DefaultUserName.Trim(), companyProfile.DefaultPassword.Trim());
                                //FTPHelper.FtpClient.ChangeDirectory(localFtpFolder);
                                FTPHelper.UploadToFtpServer(fullFilePath, dealerExportFileName);
                            }
                            catch (Exception)
                            {
                                MarkExportTaskCompleted(exportHistory.Id, backupFileName, RunningStatus.FTPError);
                                _log.ErrorLog("ERROR: FTP Server/Account is not correct for " + dealer.Name);
                                throw new InvalidOperationException("ERROR: FTP Server/Account is not correct for " + dealer.Name);
                            }
                            
                            FileStream objFileStream = File.Open(fullFilePath, FileMode.Open);
                            FTPHelper.StoreBackupFileOnLocal(localFolder + "\\" + dealer.Id, backupFileName, CommonHelper.GetBytes(objFileStream));
                        }

                        // mark this running task is complete
                        if (upToSvn)
                            MarkExportTaskCompleted(exportHistory.Id, backupFileName, RunningStatus.Completed);
                    }
                    catch (Exception ex)
                    {
                        errors.Add(dealer);
                        _log.ErrorLog("ERROR: Dealer " + dealer.Name + " " + ex.Message);
                    }

                    _log.ErrorLog("END: Export data feed for dealer " + dealer.Name);
                }
            }
            
            SendErrorNotificationEmails(errors, companyProfile);
        }
        
        #endregion

        #region Private Methods

        private List<VehicleViewModel> GetInventoryListByStatus(int dealerId, string status)
        {
            switch (status.ToUpper())
            {
                case "NEW":
                    return _mySqlHelper.GetDealerInventory(dealerId, true);
                    
                case "USED":
                    return _mySqlHelper.GetDealerInventory(dealerId, false);
                    
                default:
                    return _mySqlHelper.GetDealerInventory(dealerId);
                    
            }
        }

        private void MarkImportTaskCompleted(int id, string archiveFileName)
        {
            using (var context = new vincontrolwarehouseEntities())
            {
                var existingTask = context.exportservicehistories.FirstOrDefault(i => i.Id == id);
                if (existingTask != null)
                {
                    existingTask.Status = RunningStatus.Completed;
                    existingTask.CompletedDate = DateTime.Now;
                    existingTask.ArchiveFileName = archiveFileName;
                    context.SaveChanges();
                }
            }
        }

        private void MarkExportTaskCompleted(int id, string archiveFileName, string status)
        {
            using (var context = new vincontrolwarehouseEntities())
            {
                var existingTask = context.exportservicehistories.FirstOrDefault(i => i.Id == id);
                if (existingTask != null)
                {
                    existingTask.Status = status;
                    existingTask.CompletedDate = DateTime.Now;
                    existingTask.ArchiveFileName = archiveFileName;
                    context.SaveChanges();
                }
            }
        }

        private void SendErrorNotificationEmails(List<DealerViewModel> errors, datafeedprofile companyProfile)
        {
            if (errors.Count > 0 && !String.IsNullOrEmpty(companyProfile.Email))
            {
                var errorViewModel = new CompanyViewModel()
                {
                    Name = companyProfile.CompanyName,
                    Email = companyProfile.Email,
                    FtpHost = companyProfile.FTPServer,
                    FtpPassword = companyProfile.DefaultPassword,
                    FtpUserName = companyProfile.DefaultUserName,
                    Dealerships = errors
                };

                try
                {
                    _emailHelper.SendEmail(new MailAddress(errorViewModel.Email), "Data Feed Alert", _emailHelper.CreateEmailContent(errorViewModel), true);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Data Feed Export: Error when sending error notification email - " + ex.Message);
                }
                
            }
        }

        #endregion

        public void TestExportToFile(int profileId)
        {
           ExportToFile(profileId,false);
        }
    }
}
