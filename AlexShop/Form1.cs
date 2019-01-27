using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;

namespace AlexShop
{
    public partial class Form1 : Form
    {

        //__________Class Variables__________//
        private Database db;
        private System.Media.SoundPlayer soundPlayer;
        private string currDir;
        private Item prevItem;
        private Analysis analysis;
        //__________Class Variables__________//



        public Form1()
        {

            //Initialize GUI.
            InitializeComponent();

            //Get current directory.
            currDir = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

            //Create a sound player.
            soundPlayer = new System.Media.SoundPlayer(currDir+"\\Resources\\sound.wav");

            //Create the database Object.
            db = new Database("localhost", "babaliaris", "123456", "alexshop");

            //Update choosers upon initialization.
            UpdateChoosers();
        }






        //Tab1 Button Clicked.
        private void tab1_button_Click(object sender, EventArgs e)
        {

            //Get data.
            string barcode  = tab1_barcode_entry.Text;
            string name     = tab1_product_name_entry.Text;
            string supp     = tab1_supplier_entry.Text;
            string phone    = tab1_phone_entry.Text;
            string desc     = tab1_description_entry.Text;
            decimal price   = tab1_price_entry.Value;
            string selected = (string)tab1_product_chooser.SelectedItem;

            //Remove the newline character from the barcode.
            barcode = barcode.Replace("\r", "").Replace("\n", "");

            //Create the product.
            Product pd = new Product(-1, barcode, name, price, supp, phone, -1, -1, desc);

            //Insert Product.
            if (insert_product_radio.Checked)
            {

                //Validate the data and insert them into the database.
                if (Miscellaneum.ValidateProductData(barcode, name, phone))
                {
                    db.InsertProduct(pd);
                    UpdateChoosers();
                }
            }



            //Edit Product.
            else if (edit_product_radio.Checked)
            {

                //Validate the data and insert them into the database.
                if (Miscellaneum.ValidateProductData(barcode, name, phone))
                {

                    //If selected index is not empty.
                    if (tab1_product_chooser.SelectedIndex != -1)
                    {

                        //Get the old product from the database.
                        Product old_product = db.GetProduct(selected);

                        //If old product is not NULL.
                        if (old_product != null)
                        {
                            //Update the product.
                            db.UpdateProduct(pd, old_product);

                            //Update the choosers.
                            UpdateChoosers();
                        }
                    }


                    //There was no product selected for edit.
                    else
                        MessageBox.Show("Δεν έχεις επιλέξει κάποιο προϊόν για επεξεργασία.",
                            "Σφάλμα", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }



            //Deletion Radio Checked.
            else
            {

                //Delete the selected product.
                if (tab1_product_chooser.SelectedIndex != -1)
                {

                    //Create a confirmation question.
                    DialogResult result = MessageBox.Show("Είσαι σίγουρος ότι θέλεις να διαγράψεις το προϊόν: " + selected + "?\n\n" +
                        "Όλες οι καταχωρήσεις του συγκεκριμένου προϊόντος και το ιστορικό του θα διαγραφούν " +
                        "μόνιμα από την βαση δεδομένων.", "Αποδοχή Διαγραφής", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    //Delete the product.
                    if (result == DialogResult.Yes)
                    {
                        db.DeleteProduct(selected);
                        UpdateChoosers();
                    }
                }

                //There was no product selected for deletion.
                else
                    MessageBox.Show("Δεν έχεις επιλέξει κάποιο προϊόν για διαγραφή.",
                        "Σφάλμα", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        //Update Choosers.
        private void UpdateChoosers()
        {

            //Clear all the choosers.
            tab1_product_chooser.Items.Clear();
            tab3_product_chooser.Items.Clear();

            //Append each product name to all choosers.
            foreach (string name in db.GetProductNames())
            {
                tab1_product_chooser.Items.Add(name);
                tab3_product_chooser.Items.Add(name);
            }
        }


        //Insert Product Radio Changed.
        private void insert_product_radio_CheckedChanged(object sender, EventArgs e)
        {

            //Insert Radio is Checked.
            if (insert_product_radio.Checked)
            {
                tab1_button.Text             = "Εισαγωγή";
                tab1_product_chooser.Enabled = false;
                tab1_product_chooser_label.Enabled = false;
                DisableProductEntries(false);

                tab1_button.ForeColor = Color.Blue;
            }
        }

        //Edit Product Radio Changed.
        private void edit_product_radio_CheckedChanged(object sender, EventArgs e)
        {
            //Insert Radio is Checked.
            if (edit_product_radio.Checked)
            {
                tab1_button.Text             = "Επεξεργασία";
                tab1_product_chooser.Enabled = true;
                tab1_product_chooser_label.Enabled = true;
                DisableProductEntries(false);

                tab1_button.ForeColor = Color.DarkOrange;
            }
        }


        //Delete Product Radio Changed.
        private void delete_product_radio_CheckedChanged(object sender, EventArgs e)
        {
            //Insert Radio is Checked.
            if (delete_product_radio.Checked)
            {
                tab1_button.Text             = "Διαγραφή";
                tab1_product_chooser.Enabled = true;
                tab1_product_chooser_label.Enabled = true;
                DisableProductEntries(true);

                tab1_button.ForeColor = Color.Red;
            }
        }





        //Diable or Enable the entries.
        private void DisableProductEntries(bool disable)
        {

            //Disable the entries.
            if (disable)
            {
                tab1_barcode_entry.Enabled      = false;
                tab1_product_name_entry.Enabled = false;
                tab1_supplier_entry.Enabled     = false;
                tab1_phone_entry.Enabled        = false;
                tab1_price_entry.Enabled        = false;
                tab1_description_entry.Enabled  = false;
            }

            //Enable the entries.
            else
            {
                tab1_barcode_entry.Enabled      = true;
                tab1_product_name_entry.Enabled = true;
                tab1_supplier_entry.Enabled     = true;
                tab1_phone_entry.Enabled        = true;
                tab1_price_entry.Enabled        = true;
                tab1_description_entry.Enabled  = true;
            }
        }





        //Tab1 Chooser Selection Changed.
        private void tab1_product_chooser_SelectedIndexChanged(object sender, EventArgs e)
        {

            //If the chooser has a valid item selected.
            if (tab1_product_chooser.SelectedIndex != -1)
            {

                //Load product from the database.
                Product pd = db.GetProduct((string)tab1_product_chooser.SelectedItem);

                //Product loaded successfully from the database.
                if (pd != null)
                {

                    //Fill up the product information to the entries.
                    tab1_barcode_entry.Text      = pd.barcode;
                    tab1_product_name_entry.Text = pd.name;
                    tab1_supplier_entry.Text     = pd.supplier;
                    tab1_phone_entry.Text        = pd.phone;
                    tab1_price_entry.Value       = pd.price;
                    tab1_description_entry.Text  = pd.description;
                }
            }
        }





        //Tab2 Barcode Event.
        private void tab2_barcode_entry_KeyDown(object sender, KeyEventArgs e)
        {

            //Enter was pressed.
            if (e.KeyCode == Keys.Enter)
            {

                //Get the barcode and remove space if exists.
                string barcode = tab2_barcode_entry.Text.Replace("\n", "").Replace("\r", "");
                int amount     = (int)tab2_amount_entry.Value;

                //Amount should not be zero.
                if (amount == 0 && tab2_insert_radio.Checked)
                {
                    MessageBox.Show("Έχεις επιλέξη προσθήκη μηδενικής ποσότητας. " +
                        "Δεν έγινε καμία αλλαγή στο σύστημα.", "Προσοχή!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    UpdateRecordsInfo(barcode);
                    tab2_barcode_entry.Clear();
                    return;
                }

                //Insert Barcode.
                if (tab2_insert_radio.Checked)
                {

                    //If amount is negative.
                    if (amount < 0)
                    {
                        Product pd = db.GetProductByBarcode(barcode);

                        //The addition should not lead to a negative value.
                        if (pd.quantity + amount < 0)
                            amount = -pd.quantity;
                    }

                    //Try to add the quanity into the database.
                    if (db.AddQuantity(barcode, amount))
                    {
                        UpdateRecordsInfo(barcode);
                        tab2_amount_entry.Value = 0;
                        tab2_barcode_entry.Clear();
                        soundPlayer.Play();
                    }
                }

                //Export Product.
                else
                {
                    Item item = new Item(barcode, DateTime.Now, -1);

                    if (db.SellItem(item))
                        UpdateRecordsInfo(barcode);
                }


                //Clear the barcode entry.
                tab2_barcode_entry.Clear();
            }


        }




        //Update Record Info Panel.
        private void UpdateRecordsInfo(string barcode)
        {
            Product pd = db.GetProductByBarcode(barcode);

            if (pd != null)
            {
                tab2_barcode_info.Text      = pd.barcode;
                tab2_product_name_info.Text = pd.name;
                tab2_supplier_info.Text     = pd.supplier;
                tab2_quantity_info.Text     = pd.quantity.ToString();
            }
        }





        //Tab3 Product Chooser Selected Index Changed.
        private void tab3_product_chooser_SelectedIndexChanged(object sender, EventArgs e)
        {

            //Get the selected product name.
            string pd_name = (string)tab3_product_chooser.SelectedItem;

            //Get the product from the database.
            Product pd = db.GetProduct(pd_name);

            //Update the informations.
            if (pd != null)
            {
                tab3_barode_info.Text             = pd.barcode;
                tab3_product_name_info.Text       = pd.name;
                tab3_supplier_info.Text           = pd.supplier;
                tab3_phone_info.Text              = pd.phone;
                tab3_price_info.Text              = pd.price.ToString();
                tab3_description_info.Text        = pd.description;
                tab3_available_quantity_info.Text = pd.quantity.ToString();
                tab3_sold_quantity_info.Text      = pd.sold_quantity.ToString();
                tab3_price_sold_info.Text         = (pd.sold_quantity * pd.price).ToString();
            }
        }





        //Analysis Year Selector Changed.
        private void analysis_year_selector_SelectedIndexChanged(object sender, EventArgs e)
        {

            //Update the chart only if Year selector is Checked.
            if (analysis_perYear_radio.Checked)
            {
                //Load analysis from the database.
                analysis = db.GetAnalysis((int)analysis_year_selector.SelectedItem, -1);

                //Load the first page into the chart.
                if (analysis != null)
                {
                    UpdateAnalysisChart(analysis.NextPage());
                }
            }


            //Analysis Per Month Selected.
            else
            {
                //If a Year AND a Month has been selected.
                if (analysis_month_selector.SelectedIndex > -1)
                {

                    //Get month number.
                    int month = DateTime.ParseExact((string)analysis_month_selector.SelectedItem, "MMMM", CultureInfo.CurrentCulture).Month;

                    //Load analysis from the database.
                    analysis = db.GetAnalysis((int)analysis_year_selector.SelectedItem, month);

                    //Load the first page into the chart.
                    if (analysis != null)
                    {
                        UpdateAnalysisChart(analysis.NextPage());
                    }
                }
            }
        }


        //Analysis Month Selector Changed.
        private void analysis_month_selector_SelectedIndexChanged(object sender, EventArgs e)
        {

            //If a year has been selected.
            if (analysis_year_selector.SelectedIndex > -1)
            {

                //Get month number.
                int month = DateTime.ParseExact((string)analysis_month_selector.SelectedItem, "MMMM", CultureInfo.CurrentCulture).Month;

                //Load analysis from the database.
                analysis = db.GetAnalysis((int)analysis_year_selector.SelectedItem, month);

                //Load the first page into the chart.
                if (analysis != null)
                {
                    UpdateAnalysisChart(analysis.NextPage());
                }
            }
        }


        //Analysis Previous Button Clicked.
        private void analysis_prev_button_Click(object sender, EventArgs e)
        {

            if (analysis != null)
                UpdateAnalysisChart(analysis.PrevPage());
        }


        //Analysis Next Button Clicked.
        private void analysis_next_button_Click(object sender, EventArgs e)
        {
            if (analysis != null)
                UpdateAnalysisChart(analysis.NextPage());
        }



        //Update analysis chart.
        private void UpdateAnalysisChart(List<AnalysisItem> page)
        {

            //First clear the chart.
            analysis_chart.Series["Ποσότητα"].Points.Clear();
            analysis_chart.Series["Συνολική Τιμή"].Points.Clear();

            //Fill the chart.
            foreach (AnalysisItem item in page)
            {
                int index1 = analysis_chart.Series["Ποσότητα"].Points.AddY(item.amount);
                int index2 = analysis_chart.Series["Συνολική Τιμή"].Points.AddY(item.amount * item.price);

                analysis_chart.Series["Ποσότητα"].Points[index1].Label     = item.amount.ToString();
                analysis_chart.Series["Ποσότητα"].Points[index1].AxisLabel = item.name;

                analysis_chart.Series["Συνολική Τιμή"].Points[index2].Label     = (item.amount * item.price).ToString();
                analysis_chart.Series["Συνολική Τιμή"].Points[index2].AxisLabel = item.name;
            }
        }




        //Tab Control 2 Selected Index Changed.
        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {


            //If analysis tab is selected, fill out the Year AND Month Selectors.
            if (tabControl2.SelectedIndex == 1)
            {

                //Get Analysis Dates From the database.
                List<DateTime> dates = db.GetAnalysisDates();

                //Get date time format calture info.
                DateTimeFormatInfo info = CultureInfo.CurrentCulture.DateTimeFormat;

                //Go through each date and fill the year and month selectors.
                foreach (DateTime date in dates)
                {

                    //Fill the year selector.
                    if ( !analysis_year_selector.Items.Contains(date.Year) )
                        analysis_year_selector.Items.Add(date.Year);

                    //Fill the month selector.
                    if ( !analysis_month_selector.Items.Contains(info.GetMonthName(date.Month)) )
                        analysis_month_selector.Items.Add(info.GetMonthName(date.Month));
                }
            }
        }




        private void analysis_perMonth_radio_CheckedChanged(object sender, EventArgs e)
        {

            //Analysis per month selected.
            if (analysis_perMonth_radio.Checked)
            {

                //Enable month selector.
                analysis_month_selector.Enabled = true;

                //If a year has been selected and a month.
                if (analysis_year_selector.SelectedIndex > -1 && analysis_month_selector.SelectedIndex > -1)
                {

                    //Get month number.
                    int month = DateTime.ParseExact((string)analysis_month_selector.SelectedItem, "MMMM", CultureInfo.CurrentCulture).Month;

                    //Load analysis from the database.
                    analysis = db.GetAnalysis((int)analysis_year_selector.SelectedItem, month);

                    //Load the first page into the chart.
                    if (analysis != null)
                    {
                        UpdateAnalysisChart(analysis.NextPage());
                    }
                }
            }

            //Analysis per year selected.
            else
            {

                //Disable month selector.
                analysis_month_selector.Enabled = false;

                //If a year is selected, update the chart.
                if (analysis_year_selector.SelectedIndex > -1)
                {
                    //Load analysis from the database.
                    analysis = db.GetAnalysis((int)analysis_year_selector.SelectedItem, -1);

                    //Load the first page into the chart.
                    if (analysis != null)
                    {
                        UpdateAnalysisChart(analysis.NextPage());
                    }
                }
            }
        }






        //Export Products To Excel.
        private void προϊόνταToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //Create Excel Handler.
            ExcelHandler h = new ExcelHandler();

            //Create the excel file.
            h.WriteProducts(db, currDir);

            //Close the handler.
            h.Close();
        }



        //Create Backup.
        private void backupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(currDir + "\\backup.bat");
        }


        //Restore Backup.
        private void backupToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Process.Start(currDir + "\\restore.bat");
        }


        //Enable or disable Tab2 Amount Entry.
        private void tab2_insert_radio_CheckedChanged(object sender, EventArgs e)
        {

            if (tab2_insert_radio.Checked)
            {
                tab2_amount_entry.Enabled = true;
                groupBox4.ForeColor = Color.Blue;
            }

            else
            {
                tab2_amount_entry.Enabled = false;
                groupBox4.ForeColor = Color.Red;
            }
        }
    }
}
