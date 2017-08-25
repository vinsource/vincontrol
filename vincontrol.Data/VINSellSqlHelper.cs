using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace vincontrol.Data
{
    public class VINSellSqlHelper : DapperSqlHelper
    {
        public override string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["VINSellConnection"].ConnectionString;
            }
        }
    }
}
