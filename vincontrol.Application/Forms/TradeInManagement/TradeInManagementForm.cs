using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.ViewModels.TradeInManagement;
using vincontrol.Data.Interface;
using vincontrol.Data.Model;
using vincontrol.Data.Repository;
using vincontrol.DomainObject.TradeIn;

namespace vincontrol.Application.Forms.TradeInManagement
{
    public class TradeInManagementForm : BaseForm, ITradeInManagementForm
    {
        #region Constructors
        public TradeInManagementForm() : this(new SqlUnitOfWork()) { }

        public TradeInManagementForm(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        #endregion

        #region ITradeInManagementForm' Members
        
        public TradeInVehicleModel GetInventory(string vin, int dealerId)
        {
            var existingInventory = UnitOfWork.Inventory.GetInventory(vin, dealerId);
            return existingInventory != null ? new TradeInVehicleModel(existingInventory) : new TradeInVehicleModel();
        }


        public TradeInVehicleModel GetInventory(int inventoryId)
        {
            var existingInventory = UnitOfWork.Inventory.GetInventory(inventoryId);
            return existingInventory != null ? new TradeInVehicleModel(existingInventory) : new TradeInVehicleModel();
        }

        public TradeInVehicleModel GetInventory(int year, string make, string model, string trim, int dealerId)
        {
            var existingInventory = UnitOfWork.Inventory.GetInventory(year, make, model, trim, dealerId);
            return existingInventory != null ? new TradeInVehicleModel(existingInventory) : new TradeInVehicleModel();
        }

        public List<CustomerReview> GetComments(int dealerId)
        {
            var comments = UnitOfWork.TradeIn.GetComments(dealerId);
            return comments.Any() ? comments.Select(i => new CustomerReview(i)).ToList() : new List<CustomerReview>();
        }
        
        public List<ExtendedEquipmentOption> GetOptionalEquipments(int trimId)
        {
            return UnitOfWork.TradeIn.GetOptionalEquipments(trimId);
        }

        public string GetStockImage(int trimId)
        {
            return UnitOfWork.TradeIn.GetStockImage(trimId);
        }

        public void SaveTradeInCustomer(TradeInVehicleModel vehicle)
        {
            var tradeInCustomer = new TradeInCustomer()
                {
                    FirstName = vehicle.CustomerFirstName,
                    LastName = vehicle.CustomerLastName,
                    Email = vehicle.CustomerEmail,
                    Phone = vehicle.CustomerPhone,
                    Year = vehicle.SelectedYear,
                    Make = vehicle.SelectedMakeValue,
                    Model = vehicle.SelectedModelValue,
                    Trim = vehicle.SelectedTrimValue,
                    Mileage = vehicle.MileageNumber,
                    Vin =vehicle.Vin,
                    SelectedOptions = vehicle.SelectedOptions,
                    Condition = vehicle.Condition,
                    TradeInFairValue = vehicle.TradeInFairPrice,
                    TradeInMaxValue = vehicle.TradeInGoodPrice,
                    DealerId = vehicle.DealerId,
                    DateStamp = DateTime.Now,
                    Source = "Dealer Website"
                   

                };

            UnitOfWork.TradeIn.SaveTradeInCustomer(tradeInCustomer);
            
        }

        public void AddNewContact(ContactViewModel viewmodel)
        {
            var newContact = new VPContactInfo()
                                 {
                                     DealerId = viewmodel.dealerId,
                                     Vin = viewmodel.vinnumber,
                                     RegisterDate = viewmodel.RegistDate,

                                     ContactType = viewmodel.contact_type,
                                     ContactPrefer = viewmodel.contact_prefer,
                                     FirstName = viewmodel.firstname,
                                     LastName = viewmodel.lastname,
                                     Email = viewmodel.email_address,
                                     Phone = viewmodel.phone_number,
                                     ScheduleDate = viewmodel.schedule_date,
                                     ScheduleTime = viewmodel.schedule_time,
                                     OfferValue = viewmodel.offer_value,
                                     FriendEmail = viewmodel.friendemail,
                                     FriendName = viewmodel.friendname,
                                     Year = viewmodel.ModelYear,
                                     Make = viewmodel.Make,
                                     Model = viewmodel.Model,
                                     Trim = viewmodel.Trim,

                                     Engine = viewmodel.engine,
                                     Transmission = viewmodel.transmission,
                                     Mileage = viewmodel.mileage,
                                     Condition = viewmodel.condition,

                                     ExteriorColor = viewmodel.exterior_color,
                                     Address = viewmodel.address,
                                     City = viewmodel.city,
                                     State = viewmodel.state,
                                     Postal = viewmodel.zipcode,
                                     Comment = viewmodel.comment
                                 };
            UnitOfWork.TradeIn.AddNewContact(newContact);
            UnitOfWork.CommitVincontrolModel();
        }

        public void AddNewPriceAlert(PriceAlertViewModel viewmodel)
        {
            var newAlert = new TradeInPriceAlert()
                               {
                                   DealerId = viewmodel.DealerId,
                                   VehicleId = viewmodel.VehicleId,
                                   Type = viewmodel.Type,
                                   FirstName = viewmodel.FirstName,
                                   LastName = viewmodel.LastName,
                                   Phone = viewmodel.Phone,
                                   Email = viewmodel.Email,
                                   DateStamp = DateTime.Now
                               };
            UnitOfWork.TradeIn.AddNewPriceAlert(newAlert);
            UnitOfWork.CommitVincontrolModel();
        }

        #endregion
    }
}
