using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexShop
{
    class Item
    {
        public string barcode;
        public DateTime date;
        public int product_id;

        public Item(string barcode, DateTime date, int product_id)
        {
            this.barcode    = barcode;
            this.date       = date;
            this.product_id = product_id;
        }

        public Item()
        {
        }
    }
}
