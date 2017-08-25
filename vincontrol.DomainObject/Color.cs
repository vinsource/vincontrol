using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.DomainObject
{
    public class Color
    {
        private GenericColor[] genericColorField;

        private int[] styleIdField;

        private InstallationCause installedField;

        private string colorCodeField;

        private string colorNameField;

        private string rgbValueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("genericColor")]
        public GenericColor[] genericColor
        {
            get
            {
                return this.genericColorField;
            }
            set
            {
                this.genericColorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("styleId")]
        public int[] styleId
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
        public InstallationCause installed
        {
            get
            {
                return this.installedField;
            }
            set
            {
                this.installedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string colorCode
        {
            get
            {
                return this.colorCodeField;
            }
            set
            {
                this.colorCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string colorName
        {
            get
            {
                return this.colorNameField;
            }
            set
            {
                this.colorNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string rgbValue
        {
            get
            {
                return this.rgbValueField;
            }
            set
            {
                this.rgbValueField = value;
            }
        }
    }

    public class GenericColor
    {
        private InstallationCause installedField;

        private string nameField;

        private bool primaryField;

        private bool primaryFieldSpecified;

        /// <remarks/>
        public InstallationCause installed
        {
            get
            {
                return this.installedField;
            }
            set
            {
                this.installedField = value;
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

    public class InstallationCause
    {
        private InstallationCauseCause causeField;

        private string detailField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public InstallationCauseCause cause
        {
            get
            {
                return this.causeField;
            }
            set
            {
                this.causeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string detail
        {
            get
            {
                return this.detailField;
            }
            set
            {
                this.detailField = value;
            }
        }
    }

    public enum InstallationCauseCause
    {
        /// <remarks/>
        Engine,

        /// <remarks/>
        RelatedCategory,

        /// <remarks/>
        RelatedColor,

        /// <remarks/>
        CategoryLogic,

        /// <remarks/>
        OptionLogic,

        /// <remarks/>
        OptionCodeBuild,

        /// <remarks/>
        ExteriorColorBuild,

        /// <remarks/>
        InteriorColorBuild,

        /// <remarks/>
        EquipmentDescriptionInput,

        /// <remarks/>
        ExteriorColorInput,

        /// <remarks/>
        InteriorColorInput,

        /// <remarks/>
        OptionCodeInput,

        /// <remarks/>
        BaseEquipment,

        /// <remarks/>
        VIN,

        /// <remarks/>
        NonFactoryEquipmentInput,
    }
}
