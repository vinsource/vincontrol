using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.ViewModels.ManheimAuctionManagement;
using vincontrol.Data.Model;
using vincontrol.DomainObject;

namespace vincontrol.Application.Forms.ManheimAuctionManagement
{
    public interface IAuctionManagement
    {
        IList<StateViewModel> GetAllAuctions();
        IList<VehicleViewModel> GetVehicles(string auctionCode, int dealershipId, int userStamp);
        IList<VehicleViewModel> GetVehicles(string auctionCode, DateTime date, int dealershipId, int userStamp);
        IList<VehicleViewModel> GetFavoriteVehicles(int dealershipId, int userStamp);
        IList<VehicleViewModel> GetNotedVehicles(int dealershipId, int userStamp);
        IList<RegionActionSummarizeViewModel> GetAllRegionsWithAuctionStatistics();
        void MarkFavorite(int vehicleId, int dealershipId, int userStamp);
        void MarkNote(int vehicleId, string note, int dealershipId, int userStamp);
        void DeleteNote(int vehicleId, string vin, int dealershipId, int userStamp);
        string GetNote(int vehicleId, int dealershipId, int userStamp);
        bool IsFavorite(int vehicleId, int dealershipId, int userStamp);
        bool HasNote(string vin, int dealershipId, int userStamp);
        VehicleViewModel GetVehicle(int id);
        VehicleViewModel GetVehicle(string vin);
        AdvancedSearchViewModel IntializeAdvancedSearchForm();
        AdvancedSearchViewModel IntializeAdvancedSearchForm(AdvancedSearchViewModel model);
        AdvancedSearchViewModel IntializeAdvancedSearchForm(int year, string make, string model);
        IList<VehicleViewModel> SearchVehicles(int[] years, string[] makes, string[] models, string[] trims, int mmr, int cr, string[] regions, string[] states, string[] auctions, string[] sellers, string[] bodyStyles, string[] exteriorColors, string text, int pageNumber, int recordsPerPage);
        IList<ExtendedSelectListItem> GetMakes(int[] years);
        IList<ExtendedSelectListItem> GetModels(string[] makes);
        IList<ExtendedSelectListItem> GetTrims(string[] models);
        IList<ExtendedSelectListItem> GetStates(string[] regions);
        IList<ExtendedSelectListItem> GetAuctions(string[] states);
        IList<ExtendedSelectListItem> GetSellers(string[] auctions);
        List<DateTime> GetNext7AuctionDays(string auctionCode);
        int NumberOfVehicles(string auctionCode);
        int NumberOfVehicles(string auctionCode, DateTime saleDate);
        int NumberOfVehicles(string auctionCode, DateTime saleDate, int year, string make, string model, string trim, int lane, int run, string seller);
        IList<VehicleViewModel> GetVehicles(string auctionCode, DateTime saleDate, int dealershipId, int userStamp, int pageNumber, int recordsPerPage);
        IList<VehicleViewModel> GetVehicles(string auctionCode, DateTime saleDate, int year, string make, string model,
                                            string trim, int lane, int run, string seller, int dealershipId,
                                            int userStamp, int pageNumber, int recordsPerPage);

        IList<VehicleViewModel> GetSoldVehicles(string auctionCode);
        IList<VehicleViewModel> GetSoldVehicles(string auctionCode, DateTime date);
        IList<VehicleViewModel> GetSoldVehicles(DateTime date);
        IList<VehicleViewModel> GetSoldVehicles(List<DateTime> dates);
        IQueryable<AnalysisItemViewModel> GetSoldVehiclesQuery(List<DateTime> date);

    }
}
