using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;
using vincontrol.DomainObject;

namespace vincontrol.Data.Repository.Interface
{
    public interface IManheimAuctionRepository
    {
        IList<manheim_vehicles> GetAll();
        manheim_auctions GetAuction(string code);
        IList<manheim_auctions> GetAllAuctions();
        manheim_vehicles GetVehicle(int id);
        manheim_vehicles GetVehicle(string vin);
        manheim_vehicles GetNextVehicle(manheim_vehicles currentVehicle);
        manheim_vehicles GetPreviousVehicle(manheim_vehicles currentVehicle);
        int GetNextVehicleId(manheim_vehicles currentVehicle);
        int GetPreviousVehicleId(manheim_vehicles currentVehicle);
        manheim_vehicles_sold GetSoldVehicle(int id);        
        manheim_vehicles_sold GetSoldVehicle(string vin);
        manheim_vehicles_sold GetSoldVehicleByOriginalId(int id);
        IList<manheim_vehicles> GetVehicles(string auctionCode);
        IList<manheim_vehicles> GetVehicles(string auctionCode, int dealershipId, int userStamp);
        IList<manheim_vehicles> GetVehicles(string auctionCode, DateTime date);
        IList<manheim_vehicles> GetVehicles(string auctionCode, DateTime saleDate, int dealershipId, int userStamp);
        IList<manheim_vehicles_sold> GetSoldVehicles(string auctionCode);
        IList<manheim_vehicles_sold> GetSoldVehicles(string auctionCode, DateTime date);
        IList<manheim_vehicles_sold> GetSoldVehicles(DateTime date);
        IList<manheim_vehicles_sold> GetSoldVehicles(List<DateTime> dates);
        IQueryable<manheim_vehicles_sold> GetSoldVehiclesQuery(List<DateTime> dates);
        IList<manheim_vehicles> GetFavoriteVehicles(int dealershipId, int userStamp);
        IList<manheim_vehicles_sold> GetSoldFavoriteVehicles(int dealershipId, int userStamp);
        IList<manheim_vehicles> GetNotedVehicles(int dealershipId, int userStamp);
        IList<manheim_vehicles_sold> GetSoldNotedVehicles(int dealershipId, int userStamp);
        IList<manheim_regions_auctions_summarize> GetAllRegionsWithAuctionStatistics();
        void MarkFavorite(int vehicleId, int dealershipId, int userStamp);
        void MarkNote(int vehicleId, string note, int dealershipId, int userStamp);
        void DeleteNote(int vehicleId, string vin, int dealershipId, int userStamp);
        string GetNote(int vehicleId, int dealershipId, int userStamp);
        bool HasNote(string vin, int dealershipId, int userStamp);
        bool IsFavorite(int vehicleId, int dealershipId, int userStamp);
        IList<ExtendedSelectListItem> GetYears();
        IList<ExtendedSelectListItem> GetMakes();
        IList<ExtendedSelectListItem> GetMakes(int[] years);
        IList<ExtendedSelectListItem> GetModels(string[] makes);
        IList<ExtendedSelectListItem> GetModels(int[] years, string[] makes);
        IList<ExtendedSelectListItem> GetTrims(string[] models);
        IList<ExtendedSelectListItem> GetTrims(int[] years, string[] makes, string[] models);
        IList<ExtendedSelectListItem> GetRegions();
        IList<ExtendedSelectListItem> GetStates();
        IList<ExtendedSelectListItem> GetStates(string[] regions);
        IList<ExtendedSelectListItem> GetAuctions();
        IList<ExtendedSelectListItem> GetAuctions(string[] states);
        IList<ExtendedSelectListItem> GetSellers();
        IList<ExtendedSelectListItem> GetSellers(string[] auctions);
        IList<ExtendedSelectListItem> GetBodyStyles();
        IList<ExtendedSelectListItem> GetExteriorColors();
        IList<manheim_vehicles> SearchVehicles(int[] years, string[] makes, string[] models, string[] trims, int mmr, int cr, string[] regions, string[] states, string[] auctions, string[] sellers, string[] bodyStyles, string[] exteriorColors, string text, int pageNumber, int recordsPerPage);
        List<DateTime> GetNext7AuctionDays(string auctionCode);
        int NumberOfVehicles(string auctionCode);
        int NumberOfVehicles(string auctionCode, DateTime saleDate);
        int NumberOfVehicles(string auctionCode, DateTime saleDate, int year, string make, string model, string trim, int lane, int run, string seller);
        IList<manheim_vehicles> GetVehicles(string auctionCode, DateTime saleDate, int dealershipId, int userStamp, int pageNumber, int recordsPerPage);
        IList<manheim_vehicles> GetVehicles(string auctionCode, DateTime saleDate, int year, string make, string model,
                                            string trim, int lane, int run, string seller, int dealershipId,
                                            int userStamp, int pageNumber, int recordsPerPage);
    }
}
