using System;
using System.Collections.Generic;
using System.Linq;
using vincontrol.Application.ViewModels.AccountManagement;
using vincontrol.Application.ViewModels.CommonManagement;
using vincontrol.Application.ViewModels.FacebookManagement;
using vincontrol.Application.ViewModels.VideoTrackingManagement;
using vincontrol.Application.ViewModels.YoutubeManagement;
using vincontrol.Application.VinMarket.ViewModels.CommonManagement;
using vincontrol.Application.Vinsocial.ViewModels.CustomerManagement;
using vincontrol.Application.Vinsocial.ViewModels.ReviewManagement;
using vincontrol.Application.Vinsocial.ViewModels.SurveyManagement;
using vincontrol.Application.Vinsocial.ViewModels.TeamManagement;
using vincontrol.Application.Vinsocial.ViewModels.TemplateManagement;
using vincontrol.Data.Model;
using vincontrol.DomainObject;

namespace vincontrol.Application
{
    public static class MappingHandler
    {
        public static FBPost ConvertViewModelToFBPost(FacebookPostViewModel viewModel)
        {
            return new FBPost()
            {
                RealPostId = viewModel.RealPostId,
                DealerId = viewModel.DealerId,
                UserId = viewModel.UserId,
                Content = viewModel.Content,
                Link = viewModel.Link,
                Picture = viewModel.Picture,                
                FBSharedWithId = viewModel.SharedWithId,
                PublishDate = viewModel.PublishDate,
                LocationId = viewModel.LocationId,
                LocationName = viewModel.LocationName,
            };
        }

        public static FBPostTracking ConvertViewModelToFBPostTracking(FacebookPostViewModel viewModel)
        {
            return new FBPostTracking()
            {
                RealPostId = viewModel.RealPostId,
                InventoryId = viewModel.InventoryId,
                UserId = viewModel.UserId,
                Content = viewModel.Content,
                Picture = viewModel.Picture,
                PublishDate = DateTime.Now,
                LocationId = viewModel.LocationId,
                LocationName = viewModel.LocationName,
                
            };
        }


        public static FBCredential ConvertViewModelToFBCredential(FacebookCredentialViewModel viewModel)
        {
            return new FBCredential()
            {
                Category = viewModel.Category,
                Name = viewModel.Name,
                PageId = viewModel.PageId,
                AccessToken = viewModel.AccessToken,
                ExpiredDate = DateTime.Now.AddHours(1.5)
            };
        }

        public static FBCredential ConvertViewModelToFBCredential(FacebokPersonalInfo viewModel)
        {
            return new FBCredential()
            {
                DealerId = viewModel.DealerId,
                Category = viewModel.Category,
                Name = viewModel.Name,
                PageId = viewModel.Id,
                PageUrl = viewModel.Link,
                About = viewModel.About,
                Website = viewModel.Website,
                Phone = viewModel.Phone,
                ExpiredDate = DateTime.Now.AddHours(1.5)
            };
        }

        public static Survey ConvertViewModelToSurvey(SurveyViewModel viewModel)
        {
            return new Survey()
                {
                    UserId = viewModel.UserId,
                    CustomerId = viewModel.CustomerVehicle.CustomerInformationId,
                    Rating = viewModel.Rating,
                    Description = viewModel.Description,
                    Comments = viewModel.Comments,
                    ManagerId = viewModel.ManagerId,
                    BDCId = viewModel.BDCId,
                    DepartmentId = viewModel.DepartmentId,
                    SurveyTemplateId = viewModel.TemplateId,
                    DateStamp = DateTime.Now
                };
        }

        public static SurveyGoal ConvertViewModelToSurveyGoal(SurveysGoalViewModel viewmodel)
        {
            return new SurveyGoal()
                {
                    DealerId = viewmodel.DealerId,
                    Goal = viewmodel.Goal,
                    DepartmentId = viewmodel.DeparmentId
                };
        }

        public static Customer ConvertViewModelToCustomer(CustomerInformationViewModel viewModel)
        {
            return new Customer()
                {
                    CustomerLevelId = viewModel.CustomerLevelId,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Email = viewModel.Email,
                    CellNumber = viewModel.CellNumber,
                    HomeNumber = viewModel.HomeNumber
                };
        }

        public static CustomerVehicle ConvertViewModelToCustomerVehicle(CustomerVehicleViewModel viewModel)
        {
            return new CustomerVehicle()
                {
                    CustomerId = viewModel.CustomerInformationId,
                    Year = viewModel.Year,
                    Make = viewModel.Make,
                    Model = viewModel.Model,
                    Trim = viewModel.Trim
                };
        }

        public static Data.Model.CustomerAnswer ConvertViewModelToCustomerAnswer(Vinsocial.ViewModels.SurveyManagement.CustomerAnswer viewModel)
        {
            return new Data.Model.CustomerAnswer()
                {
                    SurveyId = viewModel.SurveyId,
                    SurveyAnswerId = viewModel.SurveyAnswerId,
                    SurveyQuestionId = viewModel.SurveyQuestionId
                };
        }

        public static Team ConvertViewModelToTeam(TeamViewModel viewModel)
        {
            return new Team()
                {
                    Name = viewModel.TeamName,
                    ManagerId = viewModel.ManagerId,
                    DateStamp = DateTime.Now,
                    DealerId = viewModel.DealerId,
                };
        }

        public static DealerReview ConvertViewModelToDealerReview(DealerReviewViewModel viewModel)
        {
            return new DealerReview()
                {
                    DealerId = viewModel.DealerId,
                    Url = viewModel.Url,
                    CategoryReviewId = viewModel.CategoryId,
                    SiteId = viewModel.SiteId,
                    OverallScore = viewModel.OverallScore,
                    DateStamp = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
        }

        public static UserReview ConvertViewModelToUserReview(UserReviewViewModel viewModel)
        {
            return new UserReview()
            {
                DealerReviewId = viewModel.DealerReviewId,
                Author = viewModel.Author,
                ReviewDate = viewModel.ReviewDate,
                Rating = viewModel.Rating,
                Comment = viewModel.Comment,
                DateStamp = DateTime.Now,
                IsFilterred = viewModel.Filtered,
                CategoryReviewId = viewModel.DepartmentId,
                UserId = viewModel.UserId,
                DepartmentId = viewModel.DepartmentId
            };
        }

        public static User ConvertViewModelToUser(UserViewModel viewModel)
        {
            return new User()
            {
                UserName = viewModel.UserName,
                Password = viewModel.Password,
                Expiration = DateTime.Now.AddYears(10),
                Name = viewModel.Name,
                Email = viewModel.Email,
                CellPhone = viewModel.Phone,
                Active = true,
                DefaultLogin = viewModel.HomeDealerId > 0 ? viewModel.HomeDealerId : viewModel.DealerId,
                DealerGroupId = viewModel.DealerGroupId,
                Photo = viewModel.Photo,
                Description = viewModel.Description,
                TeamId = viewModel.TeamId.Equals(0) || !viewModel.RoleId.Equals(Constant.Constanst.RoleType.Employee) ? (int?)null : viewModel.TeamId,
                DepartmentId = viewModel.DepartmentId.Equals(0) ? (int?)null : viewModel.DepartmentId
            };
        }

        public static Communication ToEntity(this CustomerCommunication model)
        {
            var entity = new Communication
            {
                UserId = model.UserId,
                ManagerId = model.ManagerId,
                SurveyId = model.SurveyId,
                ScriptId = model.ScriptId,
                CommunicationStatusId = model.CommunicationStatusId,
                CommunicationTypeId = model.CommunicationTypeId,
                Call = model.Call,
                Notes = model.Notes,
                NoteTypeId = model.NoteTypeId,
                CreatedBy = model.CreatedBy
            };

            if (model.CommunicationTypeId != Constant.Constanst.CommunicationType.InboundCall && model.CommunicationTypeId != Constant.Constanst.CommunicationType.OutboundCall)
                entity.NoteTypeId = null;

            return entity;
        }

        public static void ToEntity(this CustomerCommunication model, Communication destination)
        {
            destination.UserId = model.UserId;
            destination.ManagerId = model.ManagerId;
            destination.SurveyId = model.SurveyId;
            destination.ScriptId = model.ScriptId;
            destination.CommunicationStatusId = model.CommunicationStatusId;
            destination.CommunicationTypeId = model.CommunicationTypeId;
            destination.Call = model.Call;
            destination.Notes = model.Notes;
            destination.NoteTypeId = model.NoteTypeId;
            destination.CreatedBy = model.CreatedBy;

            if (model.Date != null && model.Time != null)
                destination.Datestamp = new DateTime(model.Date.Value.Year, model.Date.Value.Month, model.Date.Value.Day, model.Time.Value.Hour, model.Time.Value.Minute, model.Time.Value.Second);

            if (model.CommunicationTypeId != Constant.Constanst.CommunicationType.InboundCall && model.CommunicationTypeId != Constant.Constanst.CommunicationType.OutboundCall)
                destination.NoteTypeId = null;
        }

        public static SurveyTemplate ConvertViewModelToSurveyTemplate(TemplateViewModel viewModel)
        {
            return new SurveyTemplate()
                {
                    Name = viewModel.Name,
                    DealerId = viewModel.DealerId,
                    DepartmentId = viewModel.DepartmentId,
                    EmailContent = viewModel.EmailContent,
                    Question = string.Empty,
                    DateStamp = DateTime.Now,
                    TotalPoints = viewModel.TotalPoints,
                };
        }

        public static Data.Model.SurveyQuestion ConvertViewModelToSurveyQuestion(Vinsocial.ViewModels.SurveyManagement.SurveyQuestion viewModel)
        {
            return new Data.Model.SurveyQuestion()
                {
                    Content = viewModel.Content,
                    SurveyTemplateId = viewModel.SurveyTemplateId,
                    Point = viewModel.Point,
                    Order = viewModel.Order,
                    DateStamp = DateTime.Now
                };
        }

        public static Script ToEntity(this ScriptViewModel model)
        {
            return new Script
            {
                CommunicationTypeId = model.CommunicationTypeId,
                ScriptId = model.ScriptId,
                DepartmentId = model.DepartmentId,
                Name = model.Name,
                Text = model.Text,
                DealerId = model.DealerId,
            };
        }

        public static void ToEntity(this ScriptViewModel model, Script destination)
        {
            destination.CommunicationTypeId = model.CommunicationTypeId;
            destination.ScriptId = model.ScriptId;
            destination.DepartmentId = model.DepartmentId;
            destination.Name = model.Name;
            destination.Text = model.Text;
            destination.DealerId = model.DealerId;
        }

        public static VPContactInfo ToEntity(ContactViewModel viewmodel)
        {
            return new VPContactInfo()
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
                Comment = viewmodel.comment,
                Options = viewmodel.Options
            };
        }

        public static YoutubeVideo ToEntity(this YoutubeVideoViewModel model)
        {
            return new YoutubeVideo
            {
                VideoId = model.VideoId,
                Categories = string.Join(",", model.Categories),
                CommentCounts = model.CommentCounts,
                Description = model.Description,
                DislikeCounts = model.DislikeCounts,
                LikeCounts = model.LikeCounts,
                Rating = model.Rating,
                Thumbnail = model.Thumbnail,
                Title = model.Title,
                ViewCounts = model.ViewCounts,
            };
        }

        public static void ToEntity(this YoutubeVideoViewModel model, YoutubeVideo destination)
        {
            destination.VideoId = model.VideoId;
            destination.Categories = string.Join(",", model.Categories);
            destination.CommentCounts = model.CommentCounts;
            destination.Description = model.Description;
            destination.DislikeCounts = model.DislikeCounts;
            destination.LikeCounts = model.LikeCounts;
            destination.Rating = model.Rating;
            destination.Thumbnail = model.Thumbnail;
            destination.Title = model.Title;
            destination.ViewCounts = model.ViewCounts;
        }

        public static VSRSchedule ToEntity(VSRScheduleViewModel model)
        {
            return new VSRSchedule()
            {
                DealerId = model.DealerId,
                TeamId = model.TeamId,
                StartTime = model.StartTime,
                FinishTime = model.FinishTime,
                Day = model.Day,
                Status = model.Status
            };
        }

        public static Data.Model.Truck.CommercialTruckDealer ToEntity(CommercialTruckDealerViewModel model)
        {
            return new Data.Model.Truck.CommercialTruckDealer() 
            {
                CommercialTruckDealerId = model.TruckDealerId,
                DealerGroupId = model.DealerGroupId,
                Name = model.Name,
                Address = model.Address,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                Phone = model.Phone,
                Email = model.Email,
                Longitude = model.Longitude,
                Latitude = model.Latitude,
                DateStamp = model.DateStamp
            };
        }

        public static Data.Model.Truck.CommercialTruck ToEntity(CommercialTruckViewModel model)
        {
            return new Data.Model.Truck.CommercialTruck() 
            {
                CommercialTruckId = model.CommercialTruckId,
                DealerId = model.DealerId.Equals(0) ? (int?)null : model.DealerId,
                Year = model.Year,
                Make = model.Make,
                Model = model.Model,
                Trim = model.Trim,
                Vin = model.Vin,
                Stock = model.Stock,
                ExteriorColor = model.ExteriorColor,
                InteriorColor = model.InteriorColor,
                BodyStyle = model.BodyStyle,
                Litter = model.Litter,
                Engine = model.Engine,
                Fuel = model.Fuel,
                Transmission = model.Transmission,
                DriveTrain = model.Drivetrain,
                Price = model.Price,
                Mileage = model.Mileage,
                Description = model.Description,
                Package = model.Package,
                IsNew = model.IsNew,
                Images = model.Images,
                Category = model.Category,
                Class = model.Class,
                Url = model.Url,
                DateStamp = model.DateStamp,
                Updated = model.Updated
            };
        }

        public static Data.Model.Truck.CommercialTruckSoldOut ToCommercialTruckSoldOut(CommercialTruckViewModel model)
        {
            return new Data.Model.Truck.CommercialTruckSoldOut()
            {
                TruckId = model.TruckId,
                CommercialTruckId = model.CommercialTruckId,
                DealerId = model.DealerId.Equals(0) ? (int?)null : model.DealerId,
                Year = model.Year,
                Make = model.Make,
                Model = model.Model,
                Trim = model.Trim,
                Vin = model.Vin,
                Stock = model.Stock,
                ExteriorColor = model.ExteriorColor,
                InteriorColor = model.InteriorColor,
                BodyStyle = model.BodyStyle,
                Litter = model.Litter,
                Engine = model.Engine,
                Fuel = model.Fuel,
                Transmission = model.Transmission,
                DriveTrain = model.Drivetrain,
                Price = model.Price,
                Mileage = model.Mileage,
                Description = model.Description,
                Package = model.Package,
                IsNew = model.IsNew,
                Images = model.Images,
                Category = model.Category,
                Class = model.Class,
                Url = model.Url,
                DateStamp = model.DateStamp
            };
        }

        public static List<MarketCarInfo> ToEntities(List<Data.Model.Truck.CommercialTruck> objectSet)
        {
            return objectSet.Select(i => new MarketCarInfo()
            {
                Address = i.CommercialTruckDealer != null ? i.CommercialTruckDealer.Address + " " +i.CommercialTruckDealer.City +", " +i.CommercialTruckDealer.State + " " + i.CommercialTruckDealer.ZipCode : string.Empty,
                AutoTrader = false,
                AutoTraderListingURL = i.Url ?? string.Empty,
                AutoTraderStockNo = i.Stock ?? string.Empty,
                AutoTraderThumbnailURL = !String.IsNullOrEmpty(i.Images) ? i.Images.Split('|')[0] : string.Empty,
                BodyStyle = i.BodyStyle ?? string.Empty,
                CarsCom = false,
                CarsComThumbnailURL = !String.IsNullOrEmpty(i.Images) ? i.Images.Split('|')[0] : string.Empty,
                CarsComListingURL = i.Url ?? string.Empty,
                CarsComStockNo = i.Stock ?? string.Empty,
                Certified = false,
                CurrentPrice = i.Price,
                DateAdded = i.DateStamp,
                Dealershipname = i.CommercialTruckDealer != null ? i.CommercialTruckDealer.Name : "Private Seller",
                DriveType = i.DriveTrain ?? string.Empty,
                Engine = i.Engine ?? string.Empty,
                ExteriorColor = i.ExteriorColor ?? string.Empty,
                Franchise = false,
                FuelType = i.Fuel ?? string.Empty,
                InteriorColor = i.InteriorColor ?? string.Empty,
                Latitude = i.CommercialTruckDealer != null && i.CommercialTruckDealer.Latitude!=null ? (double) i.CommercialTruckDealer.Latitude : 0,
                Longitude = i.CommercialTruckDealer != null && i.CommercialTruckDealer.Longitude!=null ? (double) i.CommercialTruckDealer.Longitude : 0,
                Year = i.Year,
                Make = i.Make,
                Mileage = i.Mileage,
                Model = i.Model,
                //MoonRoof = i.MoonRoof,
                //MSRP = i.MSRP,
                RegionalListingId = i.CommercialTruckId,
                StartingPrice = i.Price,
                State = i.CommercialTruckDealer != null ? i.CommercialTruckDealer.State : string.Empty,
                //SunRoof = i.SunRoof,
                Tranmission = i.Transmission ?? string.Empty,
                Trim = i.Trim ?? string.Empty,
                Vin = i.Vin ?? string.Empty,
                AutoTraderListingId = i.CommercialTruckId.ToString(),
                CarscomListingId = i.CommercialTruckId.ToString(),
                AutoTraderDealerId = i.CommercialTruckDealer != null ? i.CommercialTruckDealer.CommercialTruckDealerId : 0,
                CommercialTruck = true,
                CommercialTruckListingUrl = i.Url,
                LastUpdatedDate = i.Updated
            }).ToList();
        }

        public static CarMaxStore ToEntity(CarMaxStoreViewModel model)
        {
            return new CarMaxStore()
            {
                CarMaxStoreId = model.CarMaxStoreId,
                Name = model.Name,
                FullName = model.FullName,
                Url = model.Url,
                Address = model.Address,
                City = model.City,
                State =model.State,
                ZipCode = model.ZipCode,
                Phone = model.Phone,
                CreatedDate = model.CreatedDate,
                UpdatedDate = model.UpdatedDate,
                Longitude = model.Longitude,
                Latitude = model.Latitude
            };
        }

        public static CarMaxVehicle ToEntity(CarMaxVehicleViewModel obj)
        {
            return new CarMaxVehicle()
            {
                CarMaxVehicleId = obj.CarMaxVehicleId,
                StoreId = obj.StoreId.Equals(0) ? (int?)null : obj.StoreId,
                Year = obj.Year,
                Make = obj.Make,
                Model = obj.Model,
                Trim = obj.Trim,
                Price = obj.Price,
                Miles = obj.Miles,
                Vin = obj.Vin,
                Stock = obj.Stock,
                DriveTrain = obj.DriveTrain,
                Transmission = obj.Transmission,
                ExteriorColor = obj.ExteriorColor,
                InteriorColor = obj.InteriorColor,
                MPGHighway = obj.MPGHighway,
                MPGCity = obj.MPGCity,
                Rating = obj.Rating,
                Features = obj.Features,
                ThumbnailPhotos = obj.ThumbnailPhotos,
                FullPhotos = obj.FullPhotos,
                Certified = obj.Certified,
                Used = obj.Used,
                Url = obj.Url,
                CreatedDate = obj.CreatedDate,
                UpdatedDate = obj.UpdatedDate
            };
        }

        public static VideoTracking ToEntity(VideoTrackingViewModel obj)
        {
            return new VideoTracking()
            {
                DealerId = obj.DealerId,
                InventoryId = obj.InventoryId,
                Url = obj.Url,
                IsPosted = obj.IsPosted,
                IsSucceed = obj.IsSucceeded,
                CreatedDate = obj.CreatedDate,
                LastDate = obj.LastDate
            };
        }
    }
}
