using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;
using vincontrol.DomainObject.TradeIn;

namespace vincontrol.Data.Repository.Interface
{
    public interface ITradeInRepository
    {
        List<TradeInComment> GetComments(int dealerId);
        List<ExtendedEquipmentOption> GetOptionalEquipments(int trimId);
        string GetStockImage(int trimId);
        void SaveTradeInCustomer(TradeInCustomer customer);
        void AddNewContact(VPContactInfo contact);
        void AddNewPriceAlert(TradeInPriceAlert obj);
    }
}
