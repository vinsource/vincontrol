using System.Collections.Generic;
using System.Linq;
using VINCapture.UploadImage.Interface;
using VINCapture.UploadImage.Models;
using vincontrol.Data.Model;

namespace VINCapture.UploadImage.ViewModels
{
    class EmailListViewModel
    {
        private IView _view;
        public List<EmailItem> Emails { get; set; }
        public EmailListViewModel(IView view, List<EmailItem> emailList, int dealerID)
        {
            _view = view;
            Init(emailList, dealerID);
            view.SetDataContext(this);
        }

        private void Init(List<EmailItem> emailList, int dealerID)
        {
            if (!emailList.Any())
            {
                var context = new VincontrolEntities();
                emailList.AddRange(context.Users.Where(i => i.DefaultLogin == dealerID).Select(
                    i => new EmailItem {Email = i.Email, UserName = i.Name}).ToList().Distinct(new EmailComparer()));
            }


            Emails = emailList;
        }
    }
}
