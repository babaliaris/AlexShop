using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace AlexShop
{
    class ExcelHandler
    {

        //Class Variables.
        private Excel.Application app;
        private Excel.Workbook wbook;
        private Excel.Worksheet wsheet;


        //Constructor.
        public ExcelHandler()
        {

            //Create the Excel app.
            app    = new Excel.Application();

            //Create an empty workbook.
            wbook  = app.Workbooks.Add();

            //Get the workbook's sheet.
            wsheet = (Excel.Worksheet)wbook.ActiveSheet;

        }



        //Write Products
        public void WriteProducts(Database db, string currDir)
        {

            //Get all the products.
            List<Product> products = db.GetAllProducts();

            //Save path.
            string save_path = currDir + "\\Προϊόντα.xlsx";

            //Create the columms.
            wsheet.Cells[1, 1] = "Barcode";
            wsheet.Cells[1, 2] = "Όνομα";
            wsheet.Cells[1, 3] = "Τιμή";
            wsheet.Cells[1, 4] = "Προμηθευτής";
            wsheet.Cells[1, 5] = "Τηλέφωνο";
            wsheet.Cells[1, 6] = "Ποσότητα";
            wsheet.Cells[1, 7] = "Πωλήσεις";
            wsheet.Cells[1, 8] = "Σύνολο";
            wsheet.Cells[1, 9] = "Περιγραφή";

            //Rows counter.
            int row = 2;

            //Go through each product.
            foreach (Product pd in products)
            {
                wsheet.Cells[row, 1] = pd.barcode;
                wsheet.Cells[row, 2] = pd.name;
                wsheet.Cells[row, 3] = pd.price;
                wsheet.Cells[row, 4] = pd.supplier;
                wsheet.Cells[row, 5] = pd.phone;
                wsheet.Cells[row, 6] = pd.quantity;
                wsheet.Cells[row, 7] = pd.sold_quantity;
                wsheet.Cells[row, 8] = pd.price * pd.sold_quantity;
                wsheet.Cells[row, 9] = pd.description;

                row++;
            }

            //If there was indeed products to write to the excel file.
            if (products.Count > 0)
            {
                //Save the workbook.
                wbook.SaveAs(save_path);

                //Show a message.
                MessageBox.Show("Επιτυχής εξαγωγή των προϊόντων στην τοποθεσία: " + save_path, "Επιτυχής Εξαγωγή", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            //No products available for export.
            else
            {
                //Show a message.
                MessageBox.Show("Δεν υπάρχουν προϊόντα για εξαγωγή.", "Δεν Υπάρχουν Προϊόντα", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }




        //Close the Excel handler.
        public void Close()
        {
            wbook.Close();
            app.Quit();

            app    = null;
            wbook  = null;
            wsheet = null;
        }
    }
}
