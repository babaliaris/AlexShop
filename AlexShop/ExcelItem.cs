using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexShop
{
    class ExcelItem : Product
    {

        public DateTime date;

        public ExcelItem(
            int id,
            string barcode,
            string name,
            decimal price,
            string supplier,
            string phone,
            int quantity,
            int sold_quantity,
            string description,
            DateTime date
            )
            : base(id, barcode, name, price, supplier, phone, quantity, sold_quantity, description)
        {
            this.date = date;
        }
    }
}
