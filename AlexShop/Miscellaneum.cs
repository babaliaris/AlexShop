using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlexShop
{
    class Miscellaneum
    {


        //Validate Product Insert Data.
        public static bool ValidateProductData(string barcode, string name, string phone)
        {
            if (barcode.Length == 0)
            {
                MessageBox.Show("Το πεδίο 'Barcode' δεν πρέπει να είναι κενό.", "Λάθος Δεδομένα", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            //The name field is empty.
            if (name.Length == 0)
            {
                MessageBox.Show("Το πεδίο 'Όνομα' δεν πρέπει να είναι κενό.", "Λάθος Δεδομένα", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            //Phone length is not equal to 10.
            if (phone.Length != 10)
            {
                MessageBox.Show("Το πεδίο 'Τηλέφωνο' πρέπει να έχει ακριβώς 10 αριθμούς.", 
                    "Λάθος Δεδομένα", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            //To to convert the phon into a number.
            try
            {
                long.Parse(phone);
            }


            //The phone string was not a number format.
            catch (FormatException)
            {
                MessageBox.Show("Το πεδίο 'Τηλέφωνο' πρέπει να έχει μόνο αριθμούς, όχι σύμβολα.",
                    "Λάθος Δεδομένα", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            //Validate succeeded.
            return true;
        }
    }
}
