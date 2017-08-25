using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.DomainObject
{
    public class OptionValuation : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal PriceField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal PriceAdjustmentField;

        //[System.Runtime.Serialization.OptionalFieldAttribute()]
        //private WhitmanEnterpriseMVC.KBBServiceEndPoint.PriceType PriceTypeField;

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
        public decimal Price
        {
            get
            {
                return this.PriceField;
            }
            set
            {
                if ((this.PriceField.Equals(value) != true))
                {
                    this.PriceField = value;
                    this.RaisePropertyChanged("Price");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal PriceAdjustment
        {
            get
            {
                return this.PriceAdjustmentField;
            }
            set
            {
                if ((this.PriceAdjustmentField.Equals(value) != true))
                {
                    this.PriceAdjustmentField = value;
                    this.RaisePropertyChanged("PriceAdjustment");
                }
            }
        }

        //[System.Runtime.Serialization.DataMemberAttribute()]
        //public WhitmanEnterpriseMVC.KBBServiceEndPoint.PriceType PriceType
        //{
        //    get
        //    {
        //        return this.PriceTypeField;
        //    }
        //    set
        //    {
        //        if ((this.PriceTypeField.Equals(value) != true))
        //        {
        //            this.PriceTypeField = value;
        //            this.RaisePropertyChanged("PriceType");
        //        }
        //    }
        //}

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
