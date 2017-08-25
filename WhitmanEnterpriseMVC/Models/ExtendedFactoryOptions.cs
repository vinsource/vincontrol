using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhitmanEnterpriseMVC.Models
{
    public class ExtendedFactoryOptions
    {

        public ExtendedFactoryOptions()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private string m_categoryName;

        public string getCategoryName()
        {
            return m_categoryName;
        }
        public void setCategoryName(string m_categoryName)
        {
            this.m_categoryName = m_categoryName;
            
        }

            private string m_MSRP;

            public string getMSRP()
            {
                return m_MSRP;
            }
            public void setMSRP(string m_MSRP)
            {
                this.m_MSRP = m_MSRP;
            }

            private string m_Name;


            public string getName()
            {
                return m_Name;
            }
            public void setName(string m_Name)
            {
                this.m_Name = m_Name;
            }

            private bool m_Installed;
            
            public bool getInstalled()
            {
                return m_Installed;
            }
            public void setInstalled(bool m_Installed)
            {
                this.m_Installed = m_Installed;
            }

            private bool m_Standard;

            public bool getStandard()
            {
                return m_Standard;
            }
            public void setStandard(bool m_Standard)
            {
                this.m_Standard = m_Standard;
            }

            public string Description { get; set; }
           
        
    }
}
