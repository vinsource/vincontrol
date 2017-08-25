using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vincontrol.Application.Forms.ManheimAuctionManagement;
using vincontrol.VinSell.Handlers;
using vincontrol.Application.ViewModels.ManheimAuctionManagement;
using vincontrol.Helper;

namespace vincontrol.VinSell.Controllers
{
    public class SearchController : BaseController
    {
        private IAuctionManagement _manheimAuctionManagement;

        public SearchController()
        {
            _manheimAuctionManagement = new AuctionManagement();
        }

        public ActionResult Search(string year, string make, string model)
        {
            if (SessionHandler.AdvancedSearchViewModel != null && String.IsNullOrEmpty(year))
            {
                var newModel = _manheimAuctionManagement.IntializeAdvancedSearchForm(SessionHandler.AdvancedSearchViewModel);
                SessionHandler.AdvancedSearchViewModel = null;
                return View(newModel);
            }

            return View(_manheimAuctionManagement.IntializeAdvancedSearchForm(String.IsNullOrEmpty(year) ? 0 : Convert.ToInt32(year), make, model));
        }

        [HttpPost]
        public ActionResult Search(AdvancedSearchViewModel model)
        {
            SessionHandler.AdvancedSearchViewModel = model;
            var viewmodel = new AuctionListViewModel(ExecuteSearch(model, 0, model.PagingRecords));
            return PartialView("SearchData", viewmodel);
        }

        public JsonResult SearchLazyLoading(string pageNumbers, string recordsPerPage)
        {
            return Json(ExecuteSearch(SessionHandler.AdvancedSearchViewModel, Convert.ToInt32(pageNumbers), Convert.ToInt32(recordsPerPage)), JsonRequestBehavior.AllowGet);
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

        #region Private Methods

        private IList<VehicleViewModel> ExecuteSearch(AdvancedSearchViewModel model, int pageNumber, int recordsPerPage)
        {
            var records = _manheimAuctionManagement.SearchVehicles(
                model.SelectedYearFrom == 0 && model.SelectedYearTo == 0 ? new int[] {} : new int[] { model.SelectedYearFrom == 0 ? 1800 : model.SelectedYearFrom, model.SelectedYearTo == 0 ? 2999 : model.SelectedYearTo },
                String.IsNullOrEmpty(model.SelectedMake) ? new string[] { } : CommonHelper.GetArrayFromString(model.SelectedMake),
                String.IsNullOrEmpty(model.SelectedModel) ? new string[] { } : CommonHelper.GetArrayFromString(model.SelectedModel),
                String.IsNullOrEmpty(model.SelectedTrim) ? new string[] { } : CommonHelper.GetArrayFromString(model.SelectedTrim),
                model.SelectedMmr,
                (model.SelectedCr),
                String.IsNullOrEmpty(model.SelectedRegion) ? new string[] { } : CommonHelper.GetArrayFromString(model.SelectedRegion),
                String.IsNullOrEmpty(model.SelectedState) ? new string[] { } : CommonHelper.GetArrayFromString(model.SelectedState),
                String.IsNullOrEmpty(model.SelectedAuction) ? new string[] { } : CommonHelper.GetArrayFromString(model.SelectedAuction),
                String.IsNullOrEmpty(model.SelectedSeller) ? new string[] { } : CommonHelper.GetArrayFromString(model.SelectedSeller),
                String.IsNullOrEmpty(model.SelectedBodyStyle) ? new string[] { } : CommonHelper.GetArrayFromString(model.SelectedBodyStyle),
                String.IsNullOrEmpty(model.SelectedExteriorColor) ? new string[] { } : CommonHelper.GetArrayFromString(model.SelectedExteriorColor),
                model.Text,
                pageNumber,
                recordsPerPage
                );
            return records;
        }

        #endregion
    }
}
