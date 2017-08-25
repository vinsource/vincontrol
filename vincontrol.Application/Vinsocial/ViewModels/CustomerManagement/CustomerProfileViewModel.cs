using System;
using System.Collections.Generic;
using System.Linq;
using vincontrol.Application.Vinsocial.ViewModels.SurveyManagement;
using vincontrol.Data.Model;
using vincontrol.DomainObject;

namespace vincontrol.Application.Vinsocial.ViewModels.CustomerManagement
{
    public class CustomerProfileViewModel
    {
        public CustomerProfileViewModel() { Communications = new List<CustomerCommunication>(); }

        public CustomerProfileViewModel(Survey obj)
        {
            if (obj != null && obj.Customer != null)
            {
                CustomerInfo = new CustomerInformationViewModel(obj.Customer);
                if (obj.Customer.CustomerVehicles != null && obj.Customer.CustomerVehicles.Count > 0)
                    CustomerVehicle = new CustomerVehicleViewModel(obj.Customer.CustomerVehicles.FirstOrDefault());

                Communications = new List<CustomerCommunication>();
                if (obj.Communications != null && obj.Communications.Count > 0)
                {
                    foreach (var item in obj.Communications.OrderByDescending(x => x.Datestamp).ThenBy(x => x.CommunicationType.Name))
                        Communications.Add(new CustomerCommunication(item));
                }

                Comments = new List<CustomerComment>();
                if (obj.Comments1 != null && obj.Comments1.Count > 0)
                {
                    foreach (var item in obj.Comments1.OrderByDescending(x => x.DateStamp))
                        Comments.Add(new CustomerComment(item));
                }
            }
        }

        public CustomerInformationViewModel CustomerInfo { get; set; }
        public CustomerVehicleViewModel CustomerVehicle { get; set; }
        public SurveyViewModel Survey { get; set; }
        public List<CustomerComment> Comments { get; set; }
        public List<CustomerCommunication> Communications { get; set; }
        public List<SelectListItem> CommunicationTypes { get; set; }
        public List<SelectListItem> CustomerLevels { get; set; }
        public string EncryptedCustomerId { get; set; }
        public string EncryptedSurveyId { get; set; }
        public string SurveyType { get; set; }
    }

    public class CustomerInformationViewModel
    {
        public CustomerInformationViewModel() {}

        public CustomerInformationViewModel(Customer obj)
        {
            CustomerInformationId = obj.CustomerId;
            CustomerLevelId = obj.CustomerLevelId;
            if (obj.CustomerLevel != null)
                CustomerLevelName = obj.CustomerLevel.Name;
            FirstName = obj.FirstName;
            LastName = obj.LastName;
            Email = obj.Email;
            CellNumber = obj.CellNumber;
            HomeNumber = obj.HomeNumber;
        }

        public int CustomerInformationId { get; set; }
        public int? CustomerLevelId { get; set; }
        public string CustomerLevelName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CellNumber { get; set; }
        public string HomeNumber { get; set; }
        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }
    }

    public class CustomerVehicleViewModel
    {
        public CustomerVehicleViewModel(){}

        public CustomerVehicleViewModel(CustomerVehicle obj)
        {
            CustomerVehicleId = obj.CustomerVehicleId;
            CustomerInformationId = obj.CustomerId;
            Year = obj.Year;
            Make = obj.Make;
            Model = obj.Model;
            Trim = obj.Trim;
        }

        public int CustomerVehicleId { get; set; }
        public int CustomerInformationId { get; set; }
        public int Year { get; set; }
        public int Make { get; set; }
        public int Model { get; set; }
        public int Trim { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string TrimName { get; set; }
    }

    public class CustomerCommunication
    {
        public CustomerCommunication() {}
        public CustomerCommunication(Communication obj)
        {
            if (obj == null) return;
            CommunicationId = obj.CommunicationId;
            CommunicationTypeId = obj.CommunicationTypeId;
            CommunicationStatusId = obj.CommunicationStatusId.GetValueOrDefault();
            ScriptId = obj.ScriptId.GetValueOrDefault();
            if (obj.CommunicationType != null)
                CommunicationTypeName = obj.CommunicationType.Name;
            if (obj.CommunicationStatu != null)
                CommunicationStatusName = obj.CommunicationStatu.Name;
            if (obj.Script != null)
                ScriptName = obj.Script.Name;
            Datestamp = obj.Datestamp;
            Date = obj.Datestamp;
            Time = obj.Datestamp;
            Notes = obj.Notes;
            NoteTypeId = obj.NoteTypeId;
            UserId = obj.UserId;
            SurveyResult = obj.SurveyResult.GetValueOrDefault();
            ManagerId = obj.ManagerId;
            CreatedBy = obj.CreatedBy;

            if (obj.Survey != null && obj.Survey.Customer != null)
            {
                var customer = obj.Survey.Customer;
                CustomerId = customer.CustomerId;
                CustomerInfo = new CustomerInformationViewModel(customer);
                if (customer.CustomerVehicles != null && customer.CustomerVehicles.Count > 0)
                    CustomerVehicle = new CustomerVehicleViewModel(customer.CustomerVehicles.FirstOrDefault());
            }
        }
        public CustomerCommunication(Customer obj)
        {
            if (obj != null)
            {
                CustomerInfo = new CustomerInformationViewModel(obj);
                if (obj.CustomerVehicles != null && obj.CustomerVehicles.Count > 0)
                    CustomerVehicle = new CustomerVehicleViewModel(obj.CustomerVehicles.FirstOrDefault());
            }
        }

        public CustomerInformationViewModel CustomerInfo { get; set; }
        public CustomerVehicleViewModel CustomerVehicle { get; set; }
        public int CommunicationId { get; set; }
        public int CommunicationTypeId { get; set; }
        public int ScriptId { get; set; }
        public int CommunicationStatusId { get; set; }
        public string CommunicationTypeName { get; set; }
        public string CommunicationStatusName { get; set; }
        public string ScriptName { get; set; }
        public int CustomerId { get; set; }
        public DateTime Datestamp { get; set; }
        public string Notes { get; set; }
        public List<SelectListItem> CommunicationTypes { get; set; }
        public string ScriptsJson { get; set; }
        public List<SelectListItem> CommunicationStatuses{ get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Time { get; set; }
        public string Call { get; set; }
        public int? NoteTypeId { get; set; }
        public int UserId { get; set; }
        public int ManagerId { get; set; }
        public string EmployeeName { get; set; }
        public string ManagerName { get; set; }
        public int SurveyId { get; set; }
        public int SurveyStatusId { get; set; }
        public double SurveyResult { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedUserName { get; set; }
    }

    public class CustomerComment
    {
        public CustomerComment() { }
        public CustomerComment(Comment obj)
        {
            CommentId = obj.CommentId;
            DateStamp = obj.DateStamp;
            SurveyId = obj.SurveyId;
            Text = obj.Text;
            UserId = obj.UserId;
        }

        public int CommentId { get; set; }
        public DateTime DateStamp { get; set; }
        public int SurveyId { get; set; }
        public string Text { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
