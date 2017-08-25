using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.ViewModels.ManheimAuctionManagement;
using vincontrol.Data.Interface;
using vincontrol.Data.Model;
using vincontrol.Data.Repository;
using vincontrol.DomainObject;

namespace vincontrol.Application.Forms.ManheimAuctionManagement
{
    public class AuctionManagement : BaseForm, IAuctionManagement
    {
        #region Constructors
        public AuctionManagement() : this(new SqlUnitOfWork()) { }

        public AuctionManagement(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        #endregion

        #region IAuctionManagement Members

        public IList<StateViewModel> GetAllAuctions()
        {
            var records = UnitOfWork.ManheimAuction.GetAllAuctions();
            var models = records.Select(i => i.State).OrderBy(i => i).Distinct().Select(i => new StateViewModel() { Name = i, Auctions = new List<AuctionViewModel>() }).ToList();
            foreach (var region in records)
            {
                var matchModel = models.FirstOrDefault(i => i.Name.Equals(region.State));
                matchModel.Auctions.Add(new AuctionViewModel()
                                           {
                                               Code = region.Code,
                                               Name = region.Name,
                                               State = region.State
                                           });
                
            }

            return models;
        }

        public IList<RegionActionSummarizeViewModel> GetAllRegionsWithAuctionStatistics()
        {
            return UnitOfWork.ManheimAuction.GetAllRegionsWithAuctionStatistics().Select(i => new RegionActionSummarizeViewModel(i)).ToList();
        }

        public IList<VehicleViewModel> GetVehicles(string auctionCode, int dealershipId, int userStamp)
        {
            var records = UnitOfWork.ManheimAuction.GetVehicles(auctionCode, dealershipId, userStamp);
            return records.Select(i => new VehicleViewModel(i) { }).ToList();
        }

        public IList<VehicleViewModel> GetVehicles(string auctionCode, DateTime date, int dealershipId, int userStamp)
        {
            var records = UnitOfWork.ManheimAuction.GetVehicles(auctionCode, date, dealershipId, userStamp);
            return records.Select(i => new VehicleViewModel(i) { }).ToList();
        }

        public IList<VehicleViewModel> GetSoldVehicles(string auctionCode)
        {
            var records = UnitOfWork.ManheimAuction.GetSoldVehicles(auctionCode);
            return records.Count == 0 ? new List<VehicleViewModel>() : records.Select(i => new VehicleViewModel(i)).ToList();
        }

        public IQueryable<AnalysisItemViewModel> GetSoldVehiclesQuery(List<DateTime> date)
        {
            return UnitOfWork.ManheimAuction.GetSoldVehiclesQuery(date).Select(i => new AnalysisItemViewModel { Model = i.Model, Make = i.Make, Year = i.Year, Mmr = i.Mmr, DateStampSold = i.DateStampSold });
        }

        public IList<VehicleViewModel> GetSoldVehicles(string auctionCode, DateTime date)
        {
            var records = UnitOfWork.ManheimAuction.GetSoldVehicles(auctionCode, date);
            return records.Count == 0 ? new List<VehicleViewModel>() : records.Select(i => new VehicleViewModel(i)).ToList();
        }
        
        public IList<VehicleViewModel> GetSoldVehicles(DateTime date)
        {
            var records = UnitOfWork.ManheimAuction.GetSoldVehicles(date);
            return records.Count == 0 ? new List<VehicleViewModel>() : records.Select(i => new VehicleViewModel(i)).ToList();
        }

        public IList<VehicleViewModel> GetSoldVehicles(List<DateTime> dates)
        {
            var records = UnitOfWork.ManheimAuction.GetSoldVehicles(dates);
            return records.Count == 0 ? new List<VehicleViewModel>() : records.Select(i => new VehicleViewModel(i)).ToList();
        }

        public IList<VehicleViewModel> GetFavoriteVehicles(int dealershipId, int userStamp)
        {
            var favorites = UnitOfWork.ManheimAuction.GetFavoriteVehicles(dealershipId, userStamp);
            var soldFavorites = UnitOfWork.ManheimAuction.GetSoldFavoriteVehicles(dealershipId, userStamp);

            var result = new List<VehicleViewModel>();
            result.AddRange(favorites.Select(i => new VehicleViewModel(i) { IsSold = false }).ToList());
            result.AddRange(soldFavorites.Select(i => new VehicleViewModel(i) { IsSold = true }).ToList());

            return result.OrderByDescending(i => i.DateStamp).ThenByDescending(i => i.Year).ToList();
        }

        public IList<VehicleViewModel> GetNotedVehicles(int dealershipId, int userStamp)
        {
            var notes = UnitOfWork.ManheimAuction.GetNotedVehicles(dealershipId, userStamp);
            var soldNotes = UnitOfWork.ManheimAuction.GetSoldNotedVehicles(dealershipId, userStamp);

            var result = new List<VehicleViewModel>();
            result.AddRange(notes.Select(i => new VehicleViewModel(i) { HasNote = true, IsFavorite = UnitOfWork.ManheimAuction.IsFavorite(i.Id, dealershipId, userStamp) }).ToList());
            result.AddRange(soldNotes.Select(i => new VehicleViewModel(i) { HasNote = true, IsFavorite = UnitOfWork.ManheimAuction.IsFavorite(i.Id, dealershipId, userStamp) }).ToList());

            return result.OrderByDescending(i => i.DateStamp).ThenByDescending(i => i.Year).ToList();
        }

        public bool IsFavorite(int vehicleId, int dealershipId, int userStamp)
        {
            return UnitOfWork.ManheimAuction.IsFavorite(vehicleId, dealershipId, userStamp);
        }

        public string GetNote(int vehicleId, int dealershipId, int userStamp)
        {
            return UnitOfWork.ManheimAuction.GetNote(vehicleId, dealershipId, userStamp);
        }

        public bool HasNote(string vin, int dealershipId, int userStamp)
        {
            return UnitOfWork.ManheimAuction.HasNote(vin, dealershipId, userStamp);
        }

        public void MarkFavorite(int vehicleId, int dealershipId, int userStamp)
        {
            UnitOfWork.ManheimAuction.MarkFavorite(vehicleId, dealershipId, userStamp);
            UnitOfWork.CommitVinSell();
        }

        public void MarkNote(int vehicleId, string note, int dealershipId, int userStamp)
        {
            UnitOfWork.ManheimAuction.MarkNote(vehicleId, note, dealershipId, userStamp);
            UnitOfWork.CommitVinSell();
        }

        public void DeleteNote(int vehicleId, string vin, int dealershipId, int userStamp)
        {
            UnitOfWork.ManheimAuction.DeleteNote(vehicleId, vin, dealershipId, userStamp);
            UnitOfWork.CommitVinSell();
        }

        public VehicleViewModel GetVehicle(int id)
        {
            var newVehicle = UnitOfWork.ManheimAuction.GetVehicle(id);
            if (newVehicle != null)
            {
                var auction = UnitOfWork.ManheimAuction.GetAuction(newVehicle.Auction);
                return new VehicleViewModel(newVehicle) { State = auction.State, RegionName = auction.Name, NextId = UnitOfWork.ManheimAuction.GetNextVehicleId(newVehicle), PreviousId = UnitOfWork.ManheimAuction.GetPreviousVehicleId(newVehicle) };
            }
            else
            {
                var soldVehicle = UnitOfWork.ManheimAuction.GetSoldVehicleByOriginalId(id);
                if (soldVehicle == null) return null;

                var auction = UnitOfWork.ManheimAuction.GetAuction(soldVehicle.Auction);
                return new VehicleViewModel(soldVehicle) { State = auction.State, RegionName = auction.Name };
            }
        }

        public VehicleViewModel GetVehicle(string vin)
        {
            var newVehicle = UnitOfWork.ManheimAuction.GetVehicle(vin);
            if (newVehicle != null)
                return new VehicleViewModel(newVehicle);

            var soldVehicle = UnitOfWork.ManheimAuction.GetSoldVehicle(vin);
            return soldVehicle != null ? new VehicleViewModel(soldVehicle) : null;
        }

        public AdvancedSearchViewModel IntializeAdvancedSearchForm()
        {
            var model = new AdvancedSearchViewModel()
                            {
                                YearsFrom = UnitOfWork.ManheimAuction.GetYears(),
                                YearsTo = UnitOfWork.ManheimAuction.GetYears(),
                                Regions = UnitOfWork.ManheimAuction.GetRegions(),
                                BodyStyles = UnitOfWork.ManheimAuction.GetBodyStyles(),
                                ExteriorColors = UnitOfWork.ManheimAuction.GetExteriorColors()
                            };
            return model;
        }

        public AdvancedSearchViewModel IntializeAdvancedSearchForm(AdvancedSearchViewModel model)
        {
            var newModel = new AdvancedSearchViewModel()
            {
                YearsFrom = UnitOfWork.ManheimAuction.GetYears(),
                YearsTo = UnitOfWork.ManheimAuction.GetYears(),
                Regions = UnitOfWork.ManheimAuction.GetRegions(),
                BodyStyles = UnitOfWork.ManheimAuction.GetBodyStyles(),
                ExteriorColors = UnitOfWork.ManheimAuction.GetExteriorColors()
            };

            newModel.SelectedYearFrom = model.SelectedYearFrom;
            newModel.SelectedYearTo = model.SelectedYearTo;
            newModel.MileageFrom = model.MileageFrom;
            newModel.MileageTo = model.MileageTo;
            newModel.SelectedCr = model.SelectedCr;
            newModel.SelectedMmr = model.SelectedMmr;
            newModel.SelectedTransmission = model.SelectedTransmission;
            newModel.SaleDateFrom = model.SaleDateFrom;
            newModel.SaleDateTo = model.SaleDateTo;
            newModel.HasCarfax1Owner = model.HasCarfax1Owner;
            newModel.Text = model.Text;

            if (newModel.SelectedYearFrom > 0 || newModel.SelectedYearTo > 0)
            {
                newModel.Makes = UnitOfWork.ManheimAuction.GetMakes(new int[] { newModel.SelectedYearFrom > 0 ? newModel.SelectedYearFrom : newModel.SelectedYearTo });
            }

            if (!String.IsNullOrEmpty(model.SelectedMake))
            {
                newModel.SelectedMake = model.SelectedMake;
                foreach (var item in newModel.Makes)
                {
                    item.Selected = model.SelectedMake.Contains(item.Text);
                }
                newModel.Models = UnitOfWork.ManheimAuction.GetModels(GetArrayFromString(newModel.SelectedMake));
            }

            if (!String.IsNullOrEmpty(model.SelectedModel))
            {
                newModel.SelectedModel = model.SelectedModel;
                foreach (var item in newModel.Models)
                {
                    item.Selected = model.SelectedModel.Contains(item.Text);
                }
                newModel.Trims = UnitOfWork.ManheimAuction.GetTrims(GetArrayFromString(newModel.SelectedModel));
            }

            if (!String.IsNullOrEmpty(model.SelectedTrim))
            {
                newModel.SelectedTrim = model.SelectedTrim;
                foreach (var item in newModel.Trims)
                {
                    item.Selected = model.SelectedTrim.Contains(item.Text);
                }

            }

            if (!String.IsNullOrEmpty(model.SelectedBodyStyle))
            {
                newModel.SelectedBodyStyle = model.SelectedBodyStyle;
                foreach (var item in newModel.BodyStyles)
                {
                    item.Selected = model.SelectedBodyStyle.Contains(item.Text);
                }
                
            }

            if (!String.IsNullOrEmpty(model.SelectedExteriorColor))
            {
                newModel.SelectedExteriorColor = model.SelectedExteriorColor;
                foreach (var item in newModel.ExteriorColors)
                {
                    item.Selected = model.SelectedExteriorColor.Contains(item.Text);
                }
                
            }

            if (!String.IsNullOrEmpty(model.SelectedRegion))
            {
                newModel.SelectedRegion = model.SelectedRegion;
                foreach (var item in newModel.Regions)
                {
                    item.Selected = model.SelectedRegion.Contains(item.Text);
                }
                newModel.States = UnitOfWork.ManheimAuction.GetStates(GetArrayFromString(newModel.SelectedRegion));
            }

            if (!String.IsNullOrEmpty(model.SelectedState))
            {
                newModel.SelectedState = model.SelectedState;
                foreach (var item in newModel.States)
                {
                    item.Selected = model.SelectedState.Contains(item.Text);
                }
                newModel.Auctions = UnitOfWork.ManheimAuction.GetAuctions(GetArrayFromString(newModel.SelectedState));
            }

            if (!String.IsNullOrEmpty(model.SelectedAuction))
            {
                newModel.SelectedAuction = model.SelectedAuction;
                foreach (var item in newModel.Auctions)
                {
                    item.Selected = model.SelectedAuction.Contains(item.Text);
                }
                newModel.Sellers = UnitOfWork.ManheimAuction.GetSellers(GetArrayFromString(newModel.SelectedAuction));
            }

            if (!String.IsNullOrEmpty(model.SelectedSeller))
            {
                newModel.SelectedSeller = model.SelectedSeller;
                foreach (var item in newModel.Sellers)
                {
                    item.Selected = model.SelectedSeller.Contains(item.Text);
                }
                
            }

            return newModel;
        }

        public AdvancedSearchViewModel IntializeAdvancedSearchForm(int year, string make, string model)
        {
            var newModel = new AdvancedSearchViewModel()
            {
                YearsFrom = UnitOfWork.ManheimAuction.GetYears(),
                YearsTo = UnitOfWork.ManheimAuction.GetYears(),
                //Makes = UnitOfWork.ManheimAuction.GetMakes(),
                Regions = UnitOfWork.ManheimAuction.GetRegions(),
                BodyStyles = UnitOfWork.ManheimAuction.GetBodyStyles(),
                ExteriorColors = UnitOfWork.ManheimAuction.GetExteriorColors()
            };

            newModel.SelectedYearFrom = year;
            newModel.SelectedYearTo = year;
            if (newModel.SelectedYearFrom > 0)
            {
                newModel.Makes = UnitOfWork.ManheimAuction.GetMakes(new int[] {newModel.SelectedYearFrom});
            }

            if (!String.IsNullOrEmpty(make))
            {
                newModel.SelectedMake = "," + make;
                foreach (var item in newModel.Makes)
                {
                    item.Selected = make.Equals(item.Text);
                }
                newModel.Models = UnitOfWork.ManheimAuction.GetModels(GetArrayFromString(newModel.SelectedMake));
            }

            if (!String.IsNullOrEmpty(model))
            {
                newModel.SelectedModel = "," + model;
                foreach (var item in newModel.Models)
                {
                    item.Selected = model.Equals(item.Text);
                }
                newModel.Trims = UnitOfWork.ManheimAuction.GetTrims(GetArrayFromString(newModel.SelectedModel));
            }

            return newModel;
        }
        
        public IList<ExtendedSelectListItem> GetMakes(int[] years)
        {
            return UnitOfWork.ManheimAuction.GetMakes(years);
        }

        public IList<ExtendedSelectListItem> GetModels(string[] makes)
        {
            return UnitOfWork.ManheimAuction.GetModels(makes);
        }

        public IList<ExtendedSelectListItem> GetTrims(string[] models)
        {
            return UnitOfWork.ManheimAuction.GetTrims(models);
        }

        public IList<ExtendedSelectListItem> GetStates(string[] regions)
        {
            return UnitOfWork.ManheimAuction.GetStates(regions);
        }

        public IList<ExtendedSelectListItem> GetAuctions(string[] states)
        {
            return UnitOfWork.ManheimAuction.GetAuctions(states);
        }

        public IList<ExtendedSelectListItem> GetSellers(string[] auctions)
        {
            return UnitOfWork.ManheimAuction.GetSellers(auctions);
        }

        #region With Paging

        public IList<VehicleViewModel> SearchVehicles(int[] years, string[] makes, string[] models, string[] trims, int mmr, int cr, string[] regions, string[] states, string[] auctions, string[] sellers, string[] bodyStyles, string[] exteriorColors, string text, int pageNumber, int recordsPerPage)
        {
            var records = UnitOfWork.ManheimAuction.SearchVehicles(years, makes, models, trims, mmr, cr, regions, states, auctions, sellers, bodyStyles, exteriorColors, text, pageNumber, recordsPerPage);
            return records.Count > 0 ? records.Select(i => new VehicleViewModel(i)).ToList() : new List<VehicleViewModel>();
        }

        public List<DateTime> GetNext7AuctionDays(string auctionCode)
        {
            return UnitOfWork.ManheimAuction.GetNext7AuctionDays(auctionCode);
        }

        public int NumberOfVehicles(string auctionCode)
        {
            return UnitOfWork.ManheimAuction.NumberOfVehicles(auctionCode);
        }

        public int NumberOfVehicles(string auctionCode, DateTime saleDate)
        {
            return UnitOfWork.ManheimAuction.NumberOfVehicles(auctionCode, saleDate);
        }

        public int NumberOfVehicles(string auctionCode, DateTime saleDate, int year, string make, string model, string trim, int lane, int run, string seller)
        {
            return UnitOfWork.ManheimAuction.NumberOfVehicles(auctionCode, saleDate, year, make, model, trim, lane, run, seller);
        }

        public IList<VehicleViewModel> GetVehicles(string auctionCode, DateTime saleDate, int dealershipId, int userStamp, int pageNumber, int recordsPerPage)
        {
            var records = UnitOfWork.ManheimAuction.GetVehicles(auctionCode, saleDate, dealershipId, userStamp, pageNumber, recordsPerPage);
            return records.Count > 0 ? records.Select(i => new VehicleViewModel(i)).ToList() : new List<VehicleViewModel>();
        }

        public IList<VehicleViewModel> GetVehicles(string auctionCode, DateTime saleDate, int year, string make, string model,
                                            string trim, int lane, int run, string seller, int dealershipId,
                                            int userStamp, int pageNumber, int recordsPerPage)
        {
            var records = UnitOfWork.ManheimAuction.GetVehicles(auctionCode, saleDate, year, make, model, trim, lane,
                                                                run, seller, dealershipId, userStamp, pageNumber,
                                                                recordsPerPage);
            return records.Count > 0 ? records.Select(i => new VehicleViewModel(i)).ToList() : new List<VehicleViewModel>();
        }

        #endregion

        #endregion

        #region Helper

        private string[] GetArrayFromString(string dataString)
        {
            return String.IsNullOrEmpty(dataString.Trim()) ? new[] { "" } : dataString.Split(new[] { ',', '|' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
        }

        private int[] GetIntArrayFromString(string dataString)
        {
            return String.IsNullOrEmpty(dataString.Trim()) ? null : dataString.Split(new[] { ',', '|' }, StringSplitOptions.RemoveEmptyEntries).Select(i => Convert.ToInt32(i)).ToArray();
        }

        #endregion
    }
}
