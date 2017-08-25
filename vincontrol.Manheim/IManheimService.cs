using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Application.ViewModels.ManheimAuctionManagement;
using vincontrol.Data.Model;

namespace vincontrol.Manheim
{
    public interface IManheimService
    {
        void LogOn(string userName, string password);
        void Execute(string vin);
        void Execute(string userName, string password);
        void ExecuteByVin(string userName, string password, string vin);
        void Execute(string country, string year, string make, string model, string body, string region, int pageIndex = 1, int pageSize = 10);
        void Execute(string country, string year, string make, string model, string body, string region, string userName, string password, int pageIndex = 1, int pageSize = 10);
        void Execute(string trim, string region, string url);
        void GetTrim(string year, string make, int[] models);
        void GetTrim(string year, string make, int[] models, string userName, string password);
        int MatchingMake(string make);
        int MatchingModel(string model, int makeServiceId);
        int[] MatchingModels(string model, int makeServiceId);
        int MatchingTrim(string trim, int modelServiceId);
        List<vincontrol.Data.Model.ManheimTrim> MatchingTrimsByModelId(int modelServiceId);
        List<SelectItem> GetRegion();
        string WebRequestGet(string url);
        XmlDocument DownloadDocument(string content);
        void PostSimulcastData(string url, SimulcastContract contract);
        string GetSimulcastUrl(string content);
        ManheimAuction GetManheimAuctionFromUrl(string url);
        void GetManheimAuctionDataInRegion(string auctionSaleLink, string regionCode, XmlDocument xmlDocument);
        manheim_vehicles GetManheimAuctionDataWithUrl(ManheimAuction manheimAuctionPar, XmlDocument xmlDocument, VinsellEntities context);
        List<ManehimRegion> GetAuctionSaleLinks(XmlDocument xmlDocument);
        ManheimCredential GetManheimCredentialByDate();
        List<ManheimWholesaleViewModel> ManheimReportForAppraisal(Appraisal appraisal, string userName, string password);
        List<ManheimWholesaleViewModel> ManheimReportNew(VincontrolEntities context, VehicleViewModel inventory, string userName, string password);
        List<ManheimWholesaleViewModel> ManheimReportNew(VinsellEntities context, VehicleViewModel inventory, string userName, string password);
    }
}
