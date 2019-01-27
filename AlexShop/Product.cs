using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexShop
{
    class Product
    {
        //__________Class Variables__________//
        public int       id;
        public string  barcode;
        public string  name;
        public decimal price;
        public string  supplier;
        public string  phone;
        public int     quantity;
        public int     sold_quantity;
        public string  description;
        //__________Class Variables__________//


        //Constructor.
        public Product
            (
            int id,
            string barcode,
            string name, 
            decimal price, 
            string supplier, 
            string phone, 
            int quantity, 
            int sold_quantity, 
            string description
            )
        {
            this.id            = id;
            this.barcode       = barcode;
            this.name          = name;
            this.price         = price;
            this.supplier      = supplier;
            this.phone         = phone;
            this.quantity      = quantity;
            this.sold_quantity = sold_quantity;
            this.description   = description;
        }



        //To String Method.
        public override string ToString()
        {
            return "Όνομα: "+name+"\nΤιμή: "+price+"\nΠρομηθευτής: "+supplier+"\nΤηλέφωνο: "+phone;
        }
    }
}
