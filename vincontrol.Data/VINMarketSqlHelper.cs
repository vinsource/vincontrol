using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace vincontrol.Data
{
    public class VINMarketSqlHelper : DapperSqlHelper
    {
        public override string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["VINMarketConnection"].ConnectionString;
            }
        }
    }
}
