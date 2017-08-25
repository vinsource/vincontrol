using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhitmanEnterpriseMVC.ReportModel
{
    public class MasterKellyBlueBook
    {
        private List<KellyBlueBook> m_KellyBlueBookList;

        public void setKBBTrimList(List<KellyBlueBook> KellyBlueBookList)
        {
            m_KellyBlueBookList = KellyBlueBookList;
        }

        public List<KellyBlueBook> GetKBBTrimList()
        {
            return m_KellyBlueBookList;
        }
    }
}
