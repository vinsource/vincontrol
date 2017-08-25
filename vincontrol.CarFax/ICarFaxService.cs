using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Data.Model;

namespace vincontrol.CarFax
{
    public interface ICarFaxService
    {
        CarFaxViewModel XmlSerializeCarFax(string vin, string carFaxUsername, string carFaxPassword);
        XmlDocument MakeApiCall(string vin, string carFaxUsername, string carFaxPassword);
        string GetVinNumberFromDetailUrl(string detailUrl);
        CarFaxViewModel ConvertXmlToCarFaxModelAndSave(string vin, string carFaxUsername, string carFaxPassword);
        CarFaxViewModel ConvertXmlToCarFaxModelAndSave(int vehicleId, string vin, string carFaxUsername, string carFaxPassword);
        CarFaxViewModel GetCarFaxReportInDatabase(string vin);
        CarFaxViewModel GetCarFaxReportInDatabase(int vehicleId);
        string GetCarFaxReportLinkFromAutoTrader(int autotraderId);
        CarFaxViewModel ConvertXmlToCarFaxModelForVinsell(int vehicleId, string vin, string carFaxUsername,
            string carFaxPassword);
    }
}
