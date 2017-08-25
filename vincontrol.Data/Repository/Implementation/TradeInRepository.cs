using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Interface;
using vincontrol.DomainObject.TradeIn;

namespace vincontrol.Data.Repository.Implementation
{
    public class TradeInRepository : ITradeInRepository
    {
        private VincontrolEntities _context;

        public TradeInRepository(VincontrolEntities context)
        {
            _context = context;
        }

        #region ITradeInRepository' Members

        public List<TradeInComment> GetComments(int dealerId)
        {
            return _context.TradeInComments.Where(i => i.DealerId == dealerId).ToList();
        }

        public List<ExtendedEquipmentOption> GetOptionalEquipments(int trimId)
        {
            return _context.TrimOptions.Where(o => o.TrimId == trimId)
                .Join(_context.Options,
                      v => v.OptionId,
                      a => a.OptionId,
                      (s, c) => new { c.Value, c.OptionId })
                .ToList().Select(i => new ExtendedEquipmentOption()
                                          {
                                              VehicleOptionId = i.OptionId,
                                              DisplayName = i.Value,
                                              IsSelected = false
                                          }).ToList();
        }

        public string GetStockImage(int trimId)
        {
            return _context.Trims.FirstOrDefault(i => i.TrimId == trimId).DefaultImageUrl;
        }

        public void SaveTradeInCustomer(TradeInCustomer vehicle)
        {
            _context.AddToTradeInCustomers(vehicle);
            _context.SaveChanges();
        }

        public void AddNewContact(VPContactInfo contact)
        {
            _context.AddToVPContactInfoes(contact);
        }

        public void AddNewPriceAlert(TradeInPriceAlert obj)
        {
            _context.AddToTradeInPriceAlerts(obj);
        }

        #endregion
    }
}
