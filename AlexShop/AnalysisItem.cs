using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexShop
{
    class AnalysisItem
    {
        public string name;
        public decimal price;
        public DateTime date;
        public long amount;

        public AnalysisItem(string name, decimal price, DateTime date, long amount)
        {
            this.name   = name;
            this.price  = price;
            this.date   = date;
            this.amount = amount;
        }

        public AnalysisItem()
        {
        }
    }
}
