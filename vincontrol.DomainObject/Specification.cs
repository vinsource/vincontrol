using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.DomainObject
{
    public partial class Specification : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        //[System.Runtime.Serialization.OptionalFieldAttribute()]
        //private WhitmanEnterpriseMVC.KBBServiceEndPoint.SpecificationCat CategoryField;

        //[System.Runtime.Serialization.OptionalFieldAttribute()]
        //private WhitmanEnterpriseMVC.KBBServiceEndPoint.SpecificationCat[] CategoryHeirarchyField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string OptionAvailabilityField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UnitsField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ValueField;

        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }

        //[System.Runtime.Serialization.DataMemberAttribute()]
        //public WhitmanEnterpriseMVC.KBBServiceEndPoint.SpecificationCat Category
        //{
        //    get
        //    {
        //        return this.CategoryField;
        //    }
        //    set
        //    {
        //        if ((object.ReferenceEquals(this.CategoryField, value) != true))
        //        {
        //            this.CategoryField = value;
        //            this.RaisePropertyChanged("Category");
        //        }
        //    }
        //}

        //[System.Runtime.Serialization.DataMemberAttribute()]
        //public WhitmanEnterpriseMVC.KBBServiceEndPoint.SpecificationCat[] CategoryHeirarchy
        //{
        //    get
        //    {
        //        return this.CategoryHeirarchyField;
        //    }
        //    set
        //    {
        //        if ((object.ReferenceEquals(this.CategoryHeirarchyField, value) != true))
        //        {
        //            this.CategoryHeirarchyField = value;
        //            this.RaisePropertyChanged("CategoryHeirarchy");
        //        }
        //    }
        //}

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id
        {
            get
            {
                return this.IdField;
            }
            set
            {
                if ((this.IdField.Equals(value) != true))
                {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.NameField, value) != true))
                {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string OptionAvailability
        {
            get
            {
                return this.OptionAvailabilityField;
            }
            set
            {
                if ((object.ReferenceEquals(this.OptionAvailabilityField, value) != true))
                {
                    this.OptionAvailabilityField = value;
                    this.RaisePropertyChanged("OptionAvailability");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Units
        {
            get
            {
                return this.UnitsField;
            }
            set
            {
                if ((object.ReferenceEquals(this.UnitsField, value) != true))
                {
                    this.UnitsField = value;
                    this.RaisePropertyChanged("Units");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Value
        {
            get
            {
                return this.ValueField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ValueField, value) != true))
                {
                    this.ValueField = value;
                    this.RaisePropertyChanged("Value");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
