using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Application.Forms;
using vincontrol.Application.Vinsocial.ViewModels.ReviewManagement;
using vincontrol.Data.Interface;
using vincontrol.Data.Repository;

namespace vincontrol.Application.Vinsocial.Forms.CustomerManagement
{
    public class InsertFormsManagement : BaseForm, IInsertFormsManagement
    {
        public InsertFormsManagement() : this(new SqlUnitOfWork()) { }

        public InsertFormsManagement(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public void AddNewContact(ContactViewModel viewModel)
        {
            var newContact = MappingHandler.ToEntity(viewModel);
            UnitOfWork.Contact.AddNewContact(newContact);
            UnitOfWork.CommitVincontrolModel();
        }
    }
}
