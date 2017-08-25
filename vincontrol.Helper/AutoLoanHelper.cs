using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.Helper
{
    public class AutoLoanHelper
    {
        public static double MonthlyPayment(double carPrice, double taxRate, double interest, double downPayment,double tradeinValue, int months)
        {
            var rate = interest/1200;

            var principal = (carPrice + carPrice * taxRate / 100) - downPayment - tradeinValue;

            var monthly = interest > 0 ? ((rate + rate / (Math.Pow(1 + rate, months) - 1)) * principal) : principal / months;

            return monthly;
            
        }

    }
}
