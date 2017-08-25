using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.DomainObject
{
    public class VehicleConfiguration : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private int IdField;

        private string VINField;

        //private WhitmanEnterpriseMVC.KBBServiceEndPoint.IdStringPair YearField;

        //private WhitmanEnterpriseMVC.KBBServiceEndPoint.IdStringPair MakeField;

        //private WhitmanEnterpriseMVC.KBBServiceEndPoint.IdStringPair ModelField;

        //private WhitmanEnterpriseMVC.KBBServiceEndPoint.IdStringPair TrimField;

        private int MileageField;

        //private WhitmanEnterpriseMVC.KBBServiceEndPoint.EquipmentOption[] OptionalEquipmentField;

        private System.DateTime ConfiguredDateField;

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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
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

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public string VIN
        {
            get
            {
                return this.VINField;
            }
            set
            {
                if ((object.ReferenceEquals(this.VINField, value) != true))
                {
                    this.VINField = value;
                    this.RaisePropertyChanged("VIN");
                }
            }
        }

        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        //public WhitmanEnterpriseMVC.KBBServiceEndPoint.IdStringPair Year
        //{
        //    get
        //    {
        //        return this.YearField;
        //    }
        //    set
        //    {
        //        if ((object.ReferenceEquals(this.YearField, value) != true))
        //        {
        //            this.YearField = value;
        //            this.RaisePropertyChanged("Year");
        //        }
        //    }
        //}

        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 3)]
        //public WhitmanEnterpriseMVC.KBBServiceEndPoint.IdStringPair Make
        //{
        //    get
        //    {
        //        return this.MakeField;
        //    }
        //    set
        //    {
        //        if ((object.ReferenceEquals(this.MakeField, value) != true))
        //        {
        //            this.MakeField = value;
        //            this.RaisePropertyChanged("Make");
        //        }
        //    }
        //}

        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 4)]
        //public WhitmanEnterpriseMVC.KBBServiceEndPoint.IdStringPair Model
        //{
        //    get
        //    {
        //        return this.ModelField;
        //    }
        //    set
        //    {
        //        if ((object.ReferenceEquals(this.ModelField, value) != true))
        //        {
        //            this.ModelField = value;
        //            this.RaisePropertyChanged("Model");
        //        }
        //    }
        //}

        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 5)]
        //public WhitmanEnterpriseMVC.KBBServiceEndPoint.IdStringPair Trim
        //{
        //    get
        //    {
        //        return this.TrimField;
        //    }
        //    set
        //    {
        //        if ((object.ReferenceEquals(this.TrimField, value) != true))
        //        {
        //            this.TrimField = value;
        //            this.RaisePropertyChanged("Trim");
        //        }
        //    }
        //}

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 6)]
        public int Mileage
        {
            get
            {
                return this.MileageField;
            }
            set
            {
                if ((this.MileageField.Equals(value) != true))
                {
                    this.MileageField = value;
                    this.RaisePropertyChanged("Mileage");
                }
            }
        }

        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 7)]
        //public WhitmanEnterpriseMVC.KBBServiceEndPoint.EquipmentOption[] OptionalEquipment
        //{
        //    get
        //    {
        //        return this.OptionalEquipmentField;
        //    }
        //    set
        //    {
        //        if ((object.ReferenceEquals(this.OptionalEquipmentField, value) != true))
        //        {
        //            this.OptionalEquipmentField = value;
        //            this.RaisePropertyChanged("OptionalEquipment");
        //        }
        //    }
        //}

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 8)]
        public System.DateTime ConfiguredDate
        {
            get
            {
                return this.ConfiguredDateField;
            }
            set
            {
                if ((this.ConfiguredDateField.Equals(value) != true))
                {
                    this.ConfiguredDateField = value;
                    this.RaisePropertyChanged("ConfiguredDate");
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
