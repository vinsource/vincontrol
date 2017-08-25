using System.Collections.Generic;
using vincontrol.Application.ViewModels.TradeInManagement;
using vincontrol.DomainObject.TradeIn;

namespace vincontrol.Application.Forms.TradeInManagement
{
    public interface ITradeInManagementForm
    {
        TradeInVehicleModel GetInventory(int inventoryId);
        TradeInVehicleModel GetInventory(string vin, int dealerId);
        TradeInVehicleModel GetInventory(int year, string make, string model, string trim, int dealerId);
        List<CustomerReview> GetComments(int dealerId);
        List<ExtendedEquipmentOption> GetOptionalEquipments(int trimId);
        string GetStockImage(int trimId);
        void SaveTradeInCustomer(TradeInVehicleModel vehicle);
        void AddNewContact(ContactViewModel contact);
        void AddNewPriceAlert(PriceAlertViewModel viewmodel);
    }
}
