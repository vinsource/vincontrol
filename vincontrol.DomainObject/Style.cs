using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.DomainObject
{
    public class Style
    {
        private IdentifiedString divisionField;

        private IdentifiedString subdivisionField;

        private IdentifiedString modelField;

        private Price basePriceField;

        private StyleBodyType[] bodyTypeField;

        private IdentifiedString marketClassField;

        private StyleStockImage stockImageField;

        private MediaGallery mediaGalleryField;

        private int idField;

        private int modelYearField;

        private string nameField;

        private string nameWoTrimField;

        private string trimField;

        private string mfrModelCodeField;

        private bool fleetOnlyField;

        private bool fleetOnlyFieldSpecified;

        private bool modelFleetField;

        private bool modelFleetFieldSpecified;

        private int passDoorsField;

        private bool passDoorsFieldSpecified;

        private string altModelNameField;

        private string altStyleNameField;

        private string altBodyTypeField;

        private DriveTrain drivetrainField;

        private bool drivetrainFieldSpecified;

        /// <remarks/>
        public IdentifiedString division
        {
            get
            {
                return this.divisionField;
            }
            set
            {
                this.divisionField = value;
            }
        }

        /// <remarks/>
        public IdentifiedString subdivision
        {
            get
            {
                return this.subdivisionField;
            }
            set
            {
                this.subdivisionField = value;
            }
        }

        /// <remarks/>
        public IdentifiedString model
        {
            get
            {
                return this.modelField;
            }
            set
            {
                this.modelField = value;
            }
        }

        /// <remarks/>
        public Price basePrice
        {
            get
            {
                return this.basePriceField;
            }
            set
            {
                this.basePriceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("bodyType")]
        public StyleBodyType[] bodyType
        {
            get
            {
                return this.bodyTypeField;
            }
            set
            {
                this.bodyTypeField = value;
            }
        }

        /// <remarks/>
        public IdentifiedString marketClass
        {
            get
            {
                return this.marketClassField;
            }
            set
            {
                this.marketClassField = value;
            }
        }

        /// <remarks/>
        public StyleStockImage stockImage
        {
            get
            {
                return this.stockImageField;
            }
            set
            {
                this.stockImageField = value;
            }
        }

        /// <remarks/>
        public MediaGallery mediaGallery
        {
            get
            {
                return this.mediaGalleryField;
            }
            set
            {
                this.mediaGalleryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int modelYear
        {
            get
            {
                return this.modelYearField;
            }
            set
            {
                this.modelYearField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nameWoTrim
        {
            get
            {
                return this.nameWoTrimField;
            }
            set
            {
                this.nameWoTrimField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string trim
        {
            get
            {
                return this.trimField;
            }
            set
            {
                this.trimField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string mfrModelCode
        {
            get
            {
                return this.mfrModelCodeField;
            }
            set
            {
                this.mfrModelCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool fleetOnly
        {
            get
            {
                return this.fleetOnlyField;
            }
            set
            {
                this.fleetOnlyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool fleetOnlySpecified
        {
            get
            {
                return this.fleetOnlyFieldSpecified;
            }
            set
            {
                this.fleetOnlyFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool modelFleet
        {
            get
            {
                return this.modelFleetField;
            }
            set
            {
                this.modelFleetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool modelFleetSpecified
        {
            get
            {
                return this.modelFleetFieldSpecified;
            }
            set
            {
                this.modelFleetFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int passDoors
        {
            get
            {
                return this.passDoorsField;
            }
            set
            {
                this.passDoorsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool passDoorsSpecified
        {
            get
            {
                return this.passDoorsFieldSpecified;
            }
            set
            {
                this.passDoorsFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string altModelName
        {
            get
            {
                return this.altModelNameField;
            }
            set
            {
                this.altModelNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string altStyleName
        {
            get
            {
                return this.altStyleNameField;
            }
            set
            {
                this.altStyleNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string altBodyType
        {
            get
            {
                return this.altBodyTypeField;
            }
            set
            {
                this.altBodyTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public DriveTrain drivetrain
        {
            get
            {
                return this.drivetrainField;
            }
            set
            {
                this.drivetrainField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool drivetrainSpecified
        {
            get
            {
                return this.drivetrainFieldSpecified;
            }
            set
            {
                this.drivetrainFieldSpecified = value;
            }
        }
    }

    public class Price
    {
        private bool unknownField;

        private bool unknownFieldSpecified;

        private double invoiceField;

        private bool invoiceFieldSpecified;

        private double msrpField;

        private bool msrpFieldSpecified;

        private double destinationField;

        private bool destinationFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool unknown
        {
            get
            {
                return this.unknownField;
            }
            set
            {
                this.unknownField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool unknownSpecified
        {
            get
            {
                return this.unknownFieldSpecified;
            }
            set
            {
                this.unknownFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double invoice
        {
            get
            {
                return this.invoiceField;
            }
            set
            {
                this.invoiceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool invoiceSpecified
        {
            get
            {
                return this.invoiceFieldSpecified;
            }
            set
            {
                this.invoiceFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double msrp
        {
            get
            {
                return this.msrpField;
            }
            set
            {
                this.msrpField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool msrpSpecified
        {
            get
            {
                return this.msrpFieldSpecified;
            }
            set
            {
                this.msrpFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double destination
        {
            get
            {
                return this.destinationField;
            }
            set
            {
                this.destinationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool destinationSpecified
        {
            get
            {
                return this.destinationFieldSpecified;
            }
            set
            {
                this.destinationFieldSpecified = value;
            }
        }
    }

    public class StyleBodyType : IdentifiedString
    {
        private bool primaryField;

        private bool primaryFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool primary
        {
            get
            {
                return this.primaryField;
            }
            set
            {
                this.primaryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool primarySpecified
        {
            get
            {
                return this.primaryFieldSpecified;
            }
            set
            {
                this.primaryFieldSpecified = value;
            }
        }
    }

    public class StyleStockImage : Image
    {
        private string filenameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string filename
        {
            get
            {
                return this.filenameField;
            }
            set
            {
                this.filenameField = value;
            }
        }
    }

    public class Image
    {

        private string urlField;

        private int widthField;

        private bool widthFieldSpecified;

        private int heightField;

        private bool heightFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string url
        {
            get
            {
                return this.urlField;
            }
            set
            {
                this.urlField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int width
        {
            get
            {
                return this.widthField;
            }
            set
            {
                this.widthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool widthSpecified
        {
            get
            {
                return this.widthFieldSpecified;
            }
            set
            {
                this.widthFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int height
        {
            get
            {
                return this.heightField;
            }
            set
            {
                this.heightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool heightSpecified
        {
            get
            {
                return this.heightFieldSpecified;
            }
            set
            {
                this.heightFieldSpecified = value;
            }
        }
    }

    public class MediaGallery
    {
        private MediaGalleryView[] viewField;

        private MediaGalleryColorized[] colorizedField;

        private int styleIdField;

        private bool styleIdFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("view")]
        public MediaGalleryView[] view
        {
            get
            {
                return this.viewField;
            }
            set
            {
                this.viewField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("colorized")]
        public MediaGalleryColorized[] colorized
        {
            get
            {
                return this.colorizedField;
            }
            set
            {
                this.colorizedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int styleId
        {
            get
            {
                return this.styleIdField;
            }
            set
            {
                this.styleIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool styleIdSpecified
        {
            get
            {
                return this.styleIdFieldSpecified;
            }
            set
            {
                this.styleIdFieldSpecified = value;
            }
        }
    }

    public class MediaGalleryView : Image
    {
        private string shotCodeField;

        private string backgroundDescriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string shotCode
        {
            get
            {
                return this.shotCodeField;
            }
            set
            {
                this.shotCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string backgroundDescription
        {
            get
            {
                return this.backgroundDescriptionField;
            }
            set
            {
                this.backgroundDescriptionField = value;
            }
        }
    }

    public class MediaGalleryColorized : Image
    {
        private string primaryColorOptionCodeField;

        private string secondaryColorOptionCodeField;

        private bool matchField;

        private bool matchFieldSpecified;

        private string shotCodeField;

        private string backgroundDescriptionField;

        private string primaryRGBHexCodeField;

        private string secondaryRGBHexCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string primaryColorOptionCode
        {
            get
            {
                return this.primaryColorOptionCodeField;
            }
            set
            {
                this.primaryColorOptionCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string secondaryColorOptionCode
        {
            get
            {
                return this.secondaryColorOptionCodeField;
            }
            set
            {
                this.secondaryColorOptionCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool match
        {
            get
            {
                return this.matchField;
            }
            set
            {
                this.matchField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool matchSpecified
        {
            get
            {
                return this.matchFieldSpecified;
            }
            set
            {
                this.matchFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string shotCode
        {
            get
            {
                return this.shotCodeField;
            }
            set
            {
                this.shotCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string backgroundDescription
        {
            get
            {
                return this.backgroundDescriptionField;
            }
            set
            {
                this.backgroundDescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string primaryRGBHexCode
        {
            get
            {
                return this.primaryRGBHexCodeField;
            }
            set
            {
                this.primaryRGBHexCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string secondaryRGBHexCode
        {
            get
            {
                return this.secondaryRGBHexCodeField;
            }
            set
            {
                this.secondaryRGBHexCodeField = value;
            }
        }
    }

    public enum DriveTrain
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Front Wheel Drive")]
        FrontWheelDrive,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Rear Wheel Drive")]
        RearWheelDrive,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("All Wheel Drive")]
        AllWheelDrive,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Four Wheel Drive")]
        FourWheelDrive,
    }
}
