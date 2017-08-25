using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Interface;
using vincontrol.DomainObject;

namespace vincontrol.Data.Repository.Implementation
{
    public class ManheimAuctionRepository : IManheimAuctionRepository
    {
        private VinsellEntities _context;

        public ManheimAuctionRepository(VinsellEntities context)
        {
            _context = context;
        }

        #region IManheimAuctionRepository Members

        public IList<manheim_vehicles> GetAll()
        {
            return _context.manheim_vehicles.ToList();
        }

        public manheim_auctions GetAuction(string code)
        {
            return _context.manheim_auctions.FirstOrDefault(i => i.Code.Equals(code));
        }

        public IList<manheim_auctions> GetAllAuctions()
        {
            return _context.manheim_auctions.ToList();
        }

        public manheim_vehicles GetVehicle(int id)
        {
            return _context.manheim_vehicles.FirstOrDefault(i => i.Id == id);
        }

        public manheim_vehicles GetVehicle(string vin)
        {
            return _context.manheim_vehicles.FirstOrDefault(i => i.Vin.ToLower().Equals(vin.ToLower()));
        }

        public manheim_vehicles GetNextVehicle(manheim_vehicles currentVehicle)
        {
            return _context.manheim_vehicles.FirstOrDefault(i => i.Id > currentVehicle.Id && i.Auction.ToLower().Equals(currentVehicle.Auction.ToLower()));
        }

        public manheim_vehicles GetPreviousVehicle(manheim_vehicles currentVehicle)
        {
            return _context.manheim_vehicles.LastOrDefault(i => i.Id < currentVehicle.Id && i.Auction.ToLower().Equals(currentVehicle.Auction.ToLower()));
        }

        public int GetNextVehicleId(manheim_vehicles currentVehicle)
        {
            //var tmp = _context.manheim_vehicles.OrderBy(i => i.Id).ThenBy(i => i.Lane.HasValue ? i.Lane.Value : 0).ThenBy(i => i.Run.HasValue ? i.Run.Value : 0).FirstOrDefault(i => i.Id > currentVehicle.Id && i.Auction.ToLower().Equals(currentVehicle.Auction.ToLower()) && (currentVehicle.Lane.HasValue && i.Lane.HasValue && i.Lane.Value.Equals(currentVehicle.Lane.Value)) && (i.SaleDate.HasValue && currentVehicle.SaleDate.HasValue && i.SaleDate.Value.Equals(currentVehicle.SaleDate.Value)));
            var tmp = _context.manheim_vehicles.OrderBy(i => i.Id).ThenBy(i => i.Run.HasValue ? i.Run.Value : 0).ThenBy(i => i.Lane.HasValue ? i.Lane.Value : 0).FirstOrDefault(i => i.Id > currentVehicle.Id && i.Auction.ToLower().Equals(currentVehicle.Auction.ToLower()) && (currentVehicle.Run.HasValue && i.Run.HasValue && i.Run.Value.Equals(currentVehicle.Run.Value)) && (i.SaleDate.HasValue && currentVehicle.SaleDate.HasValue && i.SaleDate.Value.Equals(currentVehicle.SaleDate.Value)));
            return tmp == null ? 0 : tmp.Id;
        }

        public int GetPreviousVehicleId(manheim_vehicles currentVehicle)
        {
            //var tmp = _context.manheim_vehicles.OrderByDescending(i => i.Id).ThenBy(i => i.Lane.HasValue ? i.Lane.Value : 0).ThenBy(i => i.Run.HasValue ? i.Run.Value : 0).FirstOrDefault(i => i.Id < currentVehicle.Id && i.Auction.ToLower().Equals(currentVehicle.Auction.ToLower()) && (currentVehicle.Lane.HasValue && i.Lane.HasValue && i.Lane.Value.Equals(currentVehicle.Lane.Value)) && (i.SaleDate.HasValue && currentVehicle.SaleDate.HasValue && i.SaleDate.Value.Equals(currentVehicle.SaleDate.Value)));
            var tmp = _context.manheim_vehicles.OrderByDescending(i => i.Id).ThenBy(i => i.Run.HasValue ? i.Run.Value : 0).ThenBy(i => i.Lane.HasValue ? i.Lane.Value : 0).FirstOrDefault(i => i.Id < currentVehicle.Id && i.Auction.ToLower().Equals(currentVehicle.Auction.ToLower()) && (currentVehicle.Run.HasValue && i.Run.HasValue && i.Run.Value.Equals(currentVehicle.Run.Value)) && (i.SaleDate.HasValue && currentVehicle.SaleDate.HasValue && i.SaleDate.Value.Equals(currentVehicle.SaleDate.Value)));
            return tmp == null ? 0 : tmp.Id;
        }

        public manheim_vehicles_sold GetSoldVehicle(int id)
        {
            return _context.manheim_vehicles_sold.FirstOrDefault(i => i.Id == id);
        }

        public manheim_vehicles_sold GetSoldVehicleByOriginalId(int id)
        {
            return _context.manheim_vehicles_sold.FirstOrDefault(i => i.VehicleId == id);
        }

        public manheim_vehicles_sold GetSoldVehicle(string vin)
        {
            return _context.manheim_vehicles_sold.FirstOrDefault(i => i.Vin.ToLower().Equals(vin.ToLower()));
        }

        public IList<manheim_vehicles> GetVehicles(string auctionCode)
        {
            return _context.manheim_vehicles.Where(i => i.Auction.Equals(auctionCode) && EntityFunctions.TruncateTime(i.SaleDate) >= (Constant.CurrentDate)).OrderByDescending(i => i.Year).ToList();
        }

        public IList<manheim_vehicles> GetVehicles(string auctionCode, int dealershipId, int userStamp)
        {
            var favorites = _context.manheim_favorites.Where(i => i.DealerId == dealershipId && i.UserStamp==userStamp);
            var tmp = _context.manheim_vehicles.Where(
                i =>
                i.Auction.ToLower().Equals(auctionCode.ToLower()) && EntityFunctions.TruncateTime(i.SaleDate) >= (Constant.CurrentDate))
                .Select(o => new { vehicle = o, isFavorite = favorites.Any(ii => ii.Vin.Equals(o.Vin)) });
            return tmp.AsEnumerable().Select(o => new manheim_vehicles(o.vehicle)
            {
                IsFavorite = o.isFavorite
            }).OrderByDescending(i => i.Year).ToList();
        }

        public IList<manheim_vehicles> GetVehicles(string auctionCode, DateTime date)
        {
            return _context.manheim_vehicles.Where(i => i.Auction.Equals(auctionCode) && i.SaleDate.HasValue && i.SaleDate.Value.Equals(date)).OrderByDescending(i => i.Year).ToList();
        }

        public IList<manheim_vehicles> GetVehicles(string auctionCode, DateTime saleDate, int dealershipId, int userStamp)
        {
            var favorites = _context.manheim_favorites.Where(i => i.DealerId == dealershipId && i.UserStamp==userStamp);
            var tmp = _context.manheim_vehicles.Where(
                i =>
                i.Auction.ToLower().Equals(auctionCode.ToLower()) &&
                EntityFunctions.TruncateTime(i.SaleDate) == saleDate)
                .Select(o => new { vehicle = o, isFavorite = favorites.Any(ii => ii.Vin.Equals(o.Vin)) });
            return tmp.AsEnumerable().Select(o => new manheim_vehicles(o.vehicle)
            {
                IsFavorite = o.isFavorite
            }).OrderByDescending(i => i.Year).ToList();
        }

        public IList<manheim_vehicles_sold> GetSoldVehicles(string auctionCode)
        {
            return _context.manheim_vehicles_sold.Where(i => i.Auction.Equals(auctionCode)).OrderByDescending(i => i.Year).ToList();
        }

        public IList<manheim_vehicles_sold> GetSoldVehicles(string auctionCode, DateTime date)
        {
            return _context.manheim_vehicles_sold.Where(i => i.Auction.Equals(auctionCode) && EntityFunctions.TruncateTime(i.DateStampSold) == (date)).OrderByDescending(i => i.Year).ToList();
        }

        public IList<manheim_vehicles_sold> GetSoldVehicles(DateTime date)
        {
            return _context.manheim_vehicles_sold.Where(i => EntityFunctions.TruncateTime(i.DateStampSold) == (date)).OrderByDescending(i => i.Year).ToList();
        }

        public IList<manheim_vehicles_sold> GetSoldVehicles(List<DateTime> dates)
        {
            return _context.manheim_vehicles_sold.Where(i => EntityFunctions.TruncateTime(i.DateStampSold) >= (dates.Min()) && EntityFunctions.TruncateTime(i.DateStampSold) <= (dates.Max())).OrderByDescending(i => i.Year).ToList();
        }

        public IQueryable<manheim_vehicles_sold> GetSoldVehiclesQuery(List<DateTime> dates)
        {
           return _context.manheim_vehicles_sold.Where(
                i =>
                EntityFunctions.TruncateTime(i.DateStampSold) >= (dates.Min()) &&
                EntityFunctions.TruncateTime(i.DateStampSold) <= (dates.Max())).OrderByDescending(i => i.Year);
        }

        public IList<manheim_vehicles> GetFavoriteVehicles(int dealershipId, int userStamp)
        {
            var favorites = _context.manheim_favorites.Where(i => i.DealerId == dealershipId && i.UserStamp == userStamp && i.VehicleId >0).ToList();
            if (favorites.Any())
            {
                var auctionList = _context.manheim_auctions.ToList();

                return
                    favorites.Select(x => x.manheim_vehicles)
                        .Select(t => new manheim_vehicles(t, auctionList))
                        .ToList();
            }

             

            return new List<manheim_vehicles>();
        }

        public IList<manheim_vehicles_sold> GetSoldFavoriteVehicles(int dealershipId, int userStamp)
        {
            var favorites = _context.manheim_favorites.Where(i => i.DealerId == dealershipId && i.UserStamp == userStamp && i.SoldVehicleId > 0).ToList();
            if (favorites.Count > 0)
            {
                var auctionList = _context.manheim_auctions.ToList();

                return
                    favorites.Select(x => x.manheim_vehicles_sold)
                        .Select(t => new manheim_vehicles_sold(t, auctionList))
                        .ToList();

              
            }

            return new List<manheim_vehicles_sold>();
        }

        public IList<manheim_vehicles> GetNotedVehicles(int dealershipId, int userStamp)
        {
            var notes = _context.manheim_notes.Where(i => i.DealerId == dealershipId && i.UserStamp == userStamp && i.VehicleId > 0).ToList();
            if (notes.Any())
            {
                var auctionList = _context.manheim_auctions.ToList();

                return
                    notes.Select(x => x.manheim_vehicles)
                        .Select(t => new manheim_vehicles(t, auctionList))
                        .ToList();
            }

            return new List<manheim_vehicles>();
        }

        public IList<manheim_vehicles_sold> GetSoldNotedVehicles(int dealershipId, int userStamp)
        {
            var notes = _context.manheim_notes.Where(i => i.DealerId == dealershipId && i.UserStamp == userStamp && i.SoldVehicleId > 0).ToList();
            if (notes.Any())
            {
                var auctionList = _context.manheim_auctions.ToList();

                return
                    notes.Select(x => x.manheim_vehicles_sold)
                        .Select(t => new manheim_vehicles_sold(t, auctionList))
                        .ToList();

            }

            return new List<manheim_vehicles_sold>();
        }

        public IList<manheim_regions_auctions_summarize> GetAllRegionsWithAuctionStatistics()
        {
            return (from gr in
                       (from r in _context.manheim_auctions
                        join g in
                            (from a in _context.manheim_vehicles
                             where a.SaleDate.HasValue && EntityFunctions.TruncateTime(a.SaleDate) >= Constant.CurrentDate
                                    group a by a.Auction into groupedAuction
                                    select new { NumberofVehicles = groupedAuction.Count(), Key = groupedAuction.Key })
                             on r.Code equals g.Key
                                         select  new{r, g})
                                     group gr by gr.r.State into groupedResult
                         select new manheim_regions_auctions_summarize()
                                   {
                                       Auctions = groupedResult.Select(i => new auctions_summarize() { RegionCode = i.g.Key, NumberofVehicles = i.g.NumberofVehicles, RegionName = i.r.Name }),
                                       NumberOfRegions = groupedResult.Count(),
                                       State = groupedResult.Key
                                    }).ToList();
        
        }

        public void MarkFavorite(int vehicleId, int dealershipId, int userStamp)
        {
            var existingFavorite = _context.manheim_favorites.FirstOrDefault(i =>i.VehicleId == vehicleId && i.DealerId == dealershipId && i.UserStamp == userStamp);
            if (existingFavorite == null)
            {
                var newFavorite = new manheim_favorites()
                {
                    VehicleId = vehicleId,
                    DealerId = dealershipId,
                    UserStamp = userStamp,
                    DateStamp = DateTime.Now
                };
                _context.manheim_favorites.AddObject(newFavorite);
            }
            else
            {
                _context.manheim_favorites.DeleteObject(existingFavorite);
                
            }
        }

        public void MarkNote(int vehicleId, string note, int dealershipId,int userId)
        {
            var existingNote = _context.manheim_notes.FirstOrDefault(i => i.VehicleId == vehicleId && i.UserStamp == userId);
            if (existingNote != null)
            {
                if (String.IsNullOrEmpty(note))
                {
                    _context.manheim_notes.DeleteObject(existingNote);
                }
                else
                {
                    existingNote.Note = note;
                    existingNote.DateStamp = DateTime.Now;
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(note))
                {
                    var newNote = new manheim_notes()
                                      {
                                          VehicleId = vehicleId,
                                            DealerId = dealershipId,
                                          UserStamp = userId,
                                          DateStamp = DateTime.Now,
                                          Note = note
                                      };
                    _context.manheim_notes.AddObject(newNote);
                }
            }
        }
        
        public void DeleteNote(int vehicleId, string vin, int dealershipId, int userStamp)
        {
            var existingNote = _context.manheim_notes.FirstOrDefault(i => i.VehicleId == vehicleId && i.UserStamp == userStamp);
            if (existingNote != null)
            {
              _context.manheim_notes.DeleteObject(existingNote);
             
            }
        }

        public string GetNote(int vehicleId, int dealershipId, int userStamp)
        {
            using (var context = new VinsellEntities())
            {
                var note = context.manheim_notes.FirstOrDefault(i => i.VehicleId==vehicleId && i.UserStamp == userStamp);
                return note == null ? string.Empty : note.Note;
            }
        }

        public bool HasNote(string vin, int dealershipId, int userStamp)
        {
            using (var context = new VinsellEntities())
            {
                return context.manheim_notes.Any(i => i.Vin==vin &&  i.UserStamp == userStamp);
            }
        }

        public bool IsFavorite(int vehicleId, int dealershipId, int userStamp)
        {
            using (var context = new VinsellEntities())
            {
                return context.manheim_favorites.Any(i => i.VehicleId == vehicleId && i.UserStamp == userStamp);
            }
        }

        public IList<ExtendedSelectListItem> GetYears()
        {
            var records = _context.manheim_years.OrderByDescending(i => i.Year).ToList();
            return records.Count > 0 ? records.Select(i => new ExtendedSelectListItem() { Text = i.Year.ToString(), Value = i.Year.ToString(), Selected = false }).ToList() : new List<ExtendedSelectListItem>();
        }

        public IList<ExtendedSelectListItem> GetMakes()
        {
            var records = _context.manheim_makes.Select(i => i.Name).Distinct().OrderBy(i => i).ToList();
            return records.Count > 0 ? records.Select(i => new ExtendedSelectListItem() { Text = i, Value = i, Selected = false }).ToList() : new List<ExtendedSelectListItem>();
        }

        public IList<ExtendedSelectListItem> GetMakes(int[] years)
        {
            var records = _context.manheim_makes.Where(i => years.Contains(i.Year)).Select(i => i.Name).Distinct().OrderBy(i => i).ToList();
            return records.Count > 0 ? records.Select(i => new ExtendedSelectListItem() { Text = i, Value = i, Selected = false }).ToList() : new List<ExtendedSelectListItem>();
        }

        public IList<ExtendedSelectListItem> GetModels(string[] makes)
        {
            var records = _context.manheim_models.Where(i => makes.Contains(i.Make)).Select(i => i.Name).Distinct().OrderBy(i => i).ToList();
            return records.Count > 0 ? records.Select(i => new ExtendedSelectListItem() { Text = i, Value = i, Selected = false }).ToList() : new List<ExtendedSelectListItem>();
        }

        public IList<ExtendedSelectListItem> GetModels(int[] years, string[] makes)
        {
            var filterredMakes = GetMakes(years).Select(i => i.Text);
            var records = _context.manheim_models.Where(i => makes.Contains(i.Make) && filterredMakes.Contains(i.Make)).Select(i => i.Name).Distinct().OrderBy(i => i).ToList();
            return records.Count > 0 ? records.Select(i => new ExtendedSelectListItem() { Text = i, Value = i, Selected = false }).ToList() : new List<ExtendedSelectListItem>();
        }

        public IList<ExtendedSelectListItem> GetTrims(string[] models)
        {
            var records = _context.manheim_trims.Where(i => models.Contains(i.Model)).Select(i => i.Name).Distinct().OrderBy(i => i).ToList();
            return records.Count > 0 ? records.Select(i => new ExtendedSelectListItem() { Text = i, Value = i, Selected = false }).ToList() : new List<ExtendedSelectListItem>();
        }

        public IList<ExtendedSelectListItem> GetTrims(int[] years, string[] makes, string[] models)
        {
            var filterredModels = GetModels(years, makes).Select(i => i.Text);
            var records = _context.manheim_trims.Where(i => models.Contains(i.Model) && filterredModels.Contains(i.Model)).Select(i => i.Name).Distinct().OrderBy(i => i).ToList();
            return records.Count > 0 ? records.Select(i => new ExtendedSelectListItem() { Text = i, Value = i, Selected = false }).ToList() : new List<ExtendedSelectListItem>();
        }

        public IList<ExtendedSelectListItem> GetRegions()
        {
            var records = _context.manheim_regions.OrderBy(i => i.Name).ToList();
            return records.Count > 0 ? records.Select(i => new ExtendedSelectListItem() { Text = i.Name, Value = i.Name, Selected = false }).ToList() : new List<ExtendedSelectListItem>();
        }

        public IList<ExtendedSelectListItem> GetStates()
        {
            var records = _context.manheim_states.OrderBy(i => i.Name).ToList();
            return records.Count > 0 ? records.Select(i => new ExtendedSelectListItem() { Text = i.Name, Value = i.Name, Selected = false }).ToList() : new List<ExtendedSelectListItem>();
        }

        public IList<ExtendedSelectListItem> GetStates(string[] regions)
        {
            var records = _context.manheim_states.Where(i => regions.Contains(i.Region)).OrderBy(i => i.Name).ToList();
            return records.Count > 0 ? records.Select(i => new ExtendedSelectListItem() { Text = i.Name, Value = i.Name, Selected = false }).ToList() : new List<ExtendedSelectListItem>();
        }

        public IList<ExtendedSelectListItem> GetAuctions()
        {
            var records = _context.manheim_auctions.OrderBy(i => i.Name).ToList();
            return records.Count > 0 ? records.Select(i => new ExtendedSelectListItem() { Text = i.Name, Value = i.Code, Selected = false }).ToList() : new List<ExtendedSelectListItem>();
        }

        public IList<ExtendedSelectListItem> GetAuctions(string[] states)
        {
            var records = _context.manheim_auctions.Where(i => states.Contains(i.State)).OrderBy(i => i.Name).ToList();
            return records.Count > 0 ? records.Select(i => new ExtendedSelectListItem() { Text = i.Name, Value = i.Code, Selected = false }).ToList() : new List<ExtendedSelectListItem>();
        }

        public IList<ExtendedSelectListItem> GetSellers()
        {
            var records = _context.manheim_sellers.Select(i => i.Name).Distinct().OrderBy(i => i).ToList();
            return records.Count > 0 ? records.Select(i => new ExtendedSelectListItem() { Text = i, Value = i, Selected = false }).ToList() : new List<ExtendedSelectListItem>();
        }

        public IList<ExtendedSelectListItem> GetSellers(string[] auctions)
        {
            var records = _context.manheim_sellers.Where(i => auctions.Contains(i.Auction)).Select(i => i.Name).Distinct().OrderBy(i => i).ToList();
            return records.Count > 0 ? records.Select(i => new ExtendedSelectListItem() { Text = i, Value = i, Selected = false }).ToList() : new List<ExtendedSelectListItem>();
        }

        public IList<ExtendedSelectListItem> GetBodyStyles()
        {
            var records = _context.manheim_bodystyles.Select(i => i.Name).Distinct().OrderBy(i => i).ToList();
            return records.Count > 0 ? records.Select(i => new ExtendedSelectListItem(){ Text = i, Value = i, Selected = false }).ToList() : new List<ExtendedSelectListItem>();
        }

        public IList<ExtendedSelectListItem> GetExteriorColors()
        {
            var records = _context.manheim_exteriorcolors.Select(i => i.Name).Distinct().OrderBy(i => i).ToList();
            return records.Count > 0 ? records.Select(i => new ExtendedSelectListItem() { Text = i, Value = i, Selected = false }).ToList() : new List<ExtendedSelectListItem>();
        }

        #region With Paging

        public IList<manheim_vehicles> SearchVehicles(int[] years, string[] makes, string[] models, string[] trims, int mmr, int cr, string[] regions, string[] states, string[] auctions, string[] sellers, string[] bodyStyles, string[] exteriorColors, string text, int pageNumber, int recordsPerPage)
        {
            var flag = false;
            var records = _context.manheim_vehicles.AsQueryable();
            
            if (regions.Any())
            {
                flag = true;
                var selectedStates = _context.manheim_states.Where(i => regions.Contains(i.Region));
                if (states.Any())
                    selectedStates = selectedStates.Where(i => states.Contains(i.Name));
                var stateNames = selectedStates.Select(i => i.Name).Distinct().ToList();
                if (auctions.Any())
                {
                    records = records.Where(i => auctions.Contains(i.Auction));
                }
                else
                {
                    var auctionNames = _context.manheim_auctions.Where(i => stateNames.Contains(i.State)).Select(i => i.Code).ToList();
                    records = records.Where(i => auctionNames.Contains(i.Auction));
                }
            }

            if (years.Count() > 0)
            {
                flag = true;
                records = records.Where(i => i.Year >= years.Min() && i.Year <= years.Max());
            }

            if (makes.Any())
            {
                flag = true;
                records = records.Where(i => makes.Contains(i.Make));
            }

            if (models.Any())
            {
                flag = true;
                records = records.Where(i => models.Contains(i.Model));
            }

            if (trims.Any())
            {
                flag = true;
                records = records.Where(i => trims.Contains(i.Trim));
            }
            
            if (sellers.Any())
            {
                flag = true;
                records = records.Where(i => sellers.Contains(i.Seller));
            }

            if (bodyStyles.Any())
            {
                flag = true;
                records = records.Where(i => bodyStyles.Contains(i.BodyStyle));
            }

            if (exteriorColors.Any())
            {
                flag = true;
                records = records.Where(i => exteriorColors.Contains(i.ExteriorColor));
            }

            if (mmr > 0)
            {
                flag = true;
                records = records.Where(i => i.Mmr < mmr);
            }

            if (cr > 0)
            {
                flag = true;
                records = records.Where(i => i.Cr.CompareTo(SqlFunctions.StringConvert((double)cr)) > 1);
            }

            if (!String.IsNullOrEmpty(text))
            {
                flag = true;
                records = records.Where(i => i.Make.ToLower().Contains(text.ToLower())
                    || i.Model.ToLower().Contains(text.ToLower())
                    || i.Trim.ToLower().Contains(text.ToLower())
                    || i.Vin.ToLower().Contains(text.ToLower())
                    || i.Seller.ToLower().Contains(text.ToLower())
                    );
            }

            if (records.Any() && flag)
            {
                var tmp = records.Join(_context.manheim_auctions,
                                       v => v.Auction,
                                       a => a.Code,
                                       (s, c) => new {vehicle = s, auction = c.Name}).Select(o => new { o.vehicle, o.auction });
                return tmp.AsEnumerable().OrderByDescending(i => i.vehicle.Year).Skip(pageNumber * recordsPerPage).Take(recordsPerPage).Select(o => new manheim_vehicles(o.vehicle)
                                           {
                                               AuctionName = o.auction
                                           }).ToList();
            }

            return new List<manheim_vehicles>();
        }

        public List<DateTime> GetNext7AuctionDays(string auctionCode)
        {
            return _context.manheim_vehicles.Where(i => i.Auction.Equals(auctionCode) && EntityFunctions.TruncateTime(i.SaleDate) >= (DateTime.Now) && i.SaleDate.HasValue).OrderBy(i => i.SaleDate).Select(i => i.SaleDate.Value).Distinct().Skip(0).Take(7).ToList();
        }

        public int NumberOfVehicles(string auctionCode)
        {
            return _context.manheim_vehicles.Where(i => i.Auction.ToLower().Equals(auctionCode.ToLower())).Count();
        }

        public int NumberOfVehicles(string auctionCode, DateTime saleDate)
        {
            return _context.manheim_vehicles.Where(i => i.Auction.ToLower().Equals(auctionCode.ToLower()) && EntityFunctions.TruncateTime(i.SaleDate) == saleDate).Count();
        }

        public int NumberOfVehicles(string auctionCode, DateTime saleDate, int year, string make, string model, string trim, int lane, int run, string seller)
        {
            return _context.manheim_vehicles.Where(i => i.Auction.ToLower().Equals(auctionCode.ToLower())
                                                        && EntityFunctions.TruncateTime(i.SaleDate) == saleDate
                                                        && (year.Equals(0) ? true : i.Year.Equals(year))
                                                        && (lane.Equals(0) ? true : i.Lane.Equals(lane))
                                                        && (run.Equals(0) ? true : i.Run.Equals(run))
                                                        && (String.IsNullOrEmpty(make) ? true : i.Make.Equals(make))
                                                        && (String.IsNullOrEmpty(model) ? true : i.Model.Equals(model))
                                                        && (String.IsNullOrEmpty(trim) ? true : i.Trim.Equals(trim))
                                                        && (String.IsNullOrEmpty(seller) ? true : i.Seller.Equals(seller))).Count();
        }

        public IList<manheim_vehicles> GetVehicles(string auctionCode, DateTime saleDate, int dealershipId, int userStamp, int pageNumber, int recordsPerPage)
        {
            var tmp = _context.manheim_vehicles.Where(
                i =>
                i.Auction.ToLower().Equals(auctionCode.ToLower()) &&
                EntityFunctions.TruncateTime(i.SaleDate) == saleDate)
                .Join(
                    _context.manheim_favorites.Where(i => i.DealerId == dealershipId && i.UserStamp==userStamp),
                    v => v.Vin,
                    a => a.Vin,
                    (s, c) => new {vehicle = s, vin = c.Vin}).Select(o => new {o.vehicle, o.vin});
            return tmp.AsEnumerable().Select(o => new manheim_vehicles(o.vehicle)
                                                      {
                                                          IsFavorite = o.vehicle.Vin == o.vin
                                                      }).OrderByDescending(i => i.Year).Skip(pageNumber*recordsPerPage).Take(recordsPerPage).ToList();
        }

        public IList<manheim_vehicles> GetVehicles(string auctionCode, DateTime saleDate, int year, string make, string model, string trim, int lane, int run, string seller, int dealershipId, int userStamp, int pageNumber, int recordsPerPage)
        {
            var tmp = _context.manheim_vehicles.Where(
                i =>
                i.Auction.ToLower().Equals(auctionCode.ToLower())
                && EntityFunctions.TruncateTime(i.SaleDate) == saleDate
                && (year.Equals(0) ? true : i.Year.Equals(year))
                && (lane.Equals(0) ? true : i.Lane.Equals(lane))
                && (run.Equals(0) ? true : i.Run.Equals(run))
                && (String.IsNullOrEmpty(make) ? true : i.Make.Equals(make))
                && (String.IsNullOrEmpty(model) ? true : i.Model.Equals(model))
                && (String.IsNullOrEmpty(trim) ? true : i.Trim.Equals(trim))
                && (String.IsNullOrEmpty(seller) ? true : i.Seller.Equals(seller)))
                .Join(
                    _context.manheim_favorites.Where(i => i.DealerId == dealershipId && i.UserStamp.Equals(userStamp)),
                    v => v.Vin,
                    a => a.Vin,
                    (s, c) => new {vehicle = s, vin = c.Vin}).Select(o => new {o.vehicle, o.vin});
            return tmp.AsEnumerable().Select(o => new manheim_vehicles(o.vehicle)
            {
                IsFavorite = o.vehicle.Vin == o.vin
            }).OrderByDescending(i => i.Year).Skip(pageNumber * recordsPerPage).Take(recordsPerPage).ToList();
        }

        #endregion

        #endregion
    }
}