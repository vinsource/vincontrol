using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vincontrol.Application.Forms.ManheimAuctionManagement;
using vincontrol.DomainObject;
using vincontrol.Helper;
using vincontrol.VinSell.Handlers;

namespace vincontrol.VinSell.Controllers
{
    public class AjaxController : Controller
    {
        private IAuctionManagement _manheimAuctionManagement;

        public AjaxController()
        {
            _manheimAuctionManagement = new AuctionManagement();
        }

        public ActionResult SingleMakes(string year)
        {
            SessionHandler.ManheimYearMakeModelList.Make = LinqHelper.GetManheimMakeForAdvancedSearch(Convert.ToInt32(year));

            return PartialView();
        }

        public ActionResult SingleModels(string year, string make)
        {
            SessionHandler.ManheimYearMakeModelList.Model = LinqHelper.GetManheimModelForAdvancedSearch(Convert.ToInt32(year), make);

            return PartialView();
        }

        public ActionResult Makes(string year, string type)
        {
            var list = SessionHandler.VehiclesInAuctionsByDate;
            if (type.ToLower().Equals("note"))
                list = SessionHandler.NotedAuctions;
            else if (type.ToLower().Equals("favorite"))
                list = SessionHandler.FavoriteAuctions;

            var convertedYear = Convert.ToInt32(String.IsNullOrEmpty(year) ? "0" : year);
            return PartialView("Makes", list.Where(i => convertedYear.Equals(i.Year)).OrderBy(i => i.Make).Select(i => i.Make).Distinct().Select(i => new ExtendedSelectListItem() { Value = i.ToString(), Text = i.ToString(), Selected = false }).ToList());
        }

        public ActionResult Models(string year, string make, string type)
        {
            var list = SessionHandler.VehiclesInAuctionsByDate;
            if (type.ToLower().Equals("note"))
                list = SessionHandler.NotedAuctions;
            else if (type.ToLower().Equals("favorite"))
                list = SessionHandler.FavoriteAuctions;

            var convertedYear = Convert.ToInt32(String.IsNullOrEmpty(year) ? "0" : year);
            return PartialView("Models", list.Where(i => make.ToLower().Equals(i.Make.ToLower()) && convertedYear.Equals(i.Year)).OrderBy(i => i.Model).Select(i => i.Model).Distinct().Select(i => new ExtendedSelectListItem() { Value = i.ToString(), Text = i.ToString(), Selected = false }).ToList());
        }

        public ActionResult Trims(string year, string make, string model, string type)
        {
            var list = SessionHandler.VehiclesInAuctionsByDate;
            if (type.ToLower().Equals("note"))
                list = SessionHandler.NotedAuctions;
            else if (type.ToLower().Equals("favorite"))
                list = SessionHandler.FavoriteAuctions;

            var convertedYear = Convert.ToInt32(String.IsNullOrEmpty(year) ? "0" : year);
            return PartialView("Trims", list.Where(i => model.ToLower().Equals(i.Model.ToLower()) && make.ToLower().Equals(i.Make.ToLower()) && convertedYear.Equals(i.Year)).OrderBy(i => i.Trim).Select(i => i.Trim).Distinct().Select(i => new ExtendedSelectListItem() { Value = i.ToString(), Text = i.ToString(), Selected = false }).ToList());
        }

        public ActionResult GenerateStates(string regions)
        {
            return PartialView(_manheimAuctionManagement.GetStates(CommonHelper.GetArrayFromString(regions)));
        }

        public ActionResult GenerateMakes(string years)
        {
            return PartialView(_manheimAuctionManagement.GetMakes(CommonHelper.GetIntArrayFromString(years)));
        }

        public ActionResult GenerateModels(string makes)
        {
            return PartialView(_manheimAuctionManagement.GetModels(CommonHelper.GetArrayFromString(makes)));
        }

        public ActionResult GenerateTrims(string models)
        {
            return PartialView(_manheimAuctionManagement.GetTrims(CommonHelper.GetArrayFromString(models)));
        }

        public ActionResult GenerateAuctions(string states)
        {
            return PartialView(_manheimAuctionManagement.GetAuctions(CommonHelper.GetArrayFromString(states)));
        }

        public ActionResult GenerateSellers(string auctions)
        {
            return PartialView(_manheimAuctionManagement.GetSellers(CommonHelper.GetArrayFromString(auctions)));
        }
    }
}
