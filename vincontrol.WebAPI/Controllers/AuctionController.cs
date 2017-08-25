using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Application.ViewModels.ManheimAuctionManagement;
using vincontrol.Helper;
using vincontrol.Data.Model;

namespace vincontrol.WebAPI.Controllers
{
    public class AuctionController : ApiController
    {
        //
        // GET: /Auction/


        [HttpPost]
        [HttpGet]
        public List<AuctionTransactionStatistic> AuctionData(int listingId, int dealerId, int vehicleStatus)
        {
            List<AuctionTransactionStatistic> result;
            try
            {
                using (var context = new VincontrolEntities())
                {
                    var query = context.Dealers.FirstOrDefault(x => x.DealerId == dealerId);

                    var dealer = new DealershipViewModel(query);

                    result = MarketHelper.GetManheimTransactionWebApi(listingId, dealer, vehicleStatus);
                
                }

            }
            catch (Exception)
            {
                result = new List<AuctionTransactionStatistic>();
            }

            return result;
        }

        [HttpPost]
        [HttpGet]
        public List<AuctionTransactionStatistic> AuctionDataOnVinsell(int listingId, int dealerId)
        {
            List<AuctionTransactionStatistic> result;
            try
            {
                using (var context = new VincontrolEntities())
                {
                    var query = context.Dealers.FirstOrDefault(x => x.DealerId == dealerId);

                    var dealer = new DealershipViewModel(query);

                    result = MarketHelper.GetManheimTransactionWebApi(listingId, dealer, 4);

                }

            }
            catch (Exception)
            {
                result = new List<AuctionTransactionStatistic>();
            }

            return result;
        }
    }
}
