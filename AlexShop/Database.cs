using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Windows.Forms;

namespace AlexShop
{
    class Database
    {

        //__________Class Variables__________//
        private MySqlConnection connection;
        //__________Class Variables__________//



        //Constructor.
        public Database(string ip, string username, string pass, string database)
        {
            string connString = "SERVER=" + ip + ";" + "DATABASE=" + database + ";UID=" + username + ";PASSWORD=" + pass + ";";
            connection        = new MySqlConnection(connString);
        }





        //Establish Connection.
        private bool Connect()
        {

            //Try creating the connection.
            try
            {
                connection.Open();
                return true;
            }


            //Connection Failed.
            catch (MySqlException)
            {
                MessageBox.Show("Το πρόγραμμα δεν μπόρεσε να συνδεθεί στην Βάση Δεδομένων. Παρακαλώ ελέγξτε αν η Βάση Δεδομένων" +
                    " είναι ανοιχτεί.", "Πρόβλημα Σύνδεσης Στην Βάση Δεδομένων", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }




        //Close the connection.
        private void Close()
        {
            connection.Close();
        }




        //Show Uknown Error.
        private void UknownError(string func, int number, int error_code, uint code, string message)
        {
            MessageBox.Show("Ένα απροσδιόριστο πρόβλημα συνέβει στην βάση δεδομένων. Δοκίμασε ξανά αργότερα." +
                        "\n\nΣυνάρτηση: "+func + "\nΑριθμός Σφάλματος: " + number + "\nΚώδικας Σφάλματος: "
                        + error_code + "\nΓενικός Κώδικας: " + code + "" + "\nΜήνυμα: " + message,
                        "Απροσδιόριστο Πρόβλημα", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }





        //Insert a New product into the database.
        public void InsertProduct(Product pd)
        {

            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection   = connection;

                //Create the procedure.
                cmd.CommandText = "add_product";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //Add the parameters.
                cmd.Parameters.AddWithValue("price", pd.price);
                cmd.Parameters.AddWithValue("pr_name", pd.name);
                cmd.Parameters.AddWithValue("sup", pd.supplier);
                cmd.Parameters.AddWithValue("phon", pd.phone);
                cmd.Parameters.AddWithValue("des", pd.description);
                cmd.Parameters.AddWithValue("barc", pd.barcode);


                //Try executing the command.
                try
                {

                    //Execute the commant.
                    cmd.ExecuteNonQuery();

                    //Show success message.
                    MessageBox.Show("Το προϊόν:\n\n" + pd.ToString() + "\n\n δημιουργήθηκε με επιτυχία!",
                        "Επιτυχής Εισαγωγή", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {

                    //Duplicate entry of product name.
                    if (e.Number == 1062)
                        MessageBox.Show("Το προϊόν με Όνομα: " + pd.name + " και Barcode: "+pd.barcode+", υπάρχει ήδη.", "Υπάρχει",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //Uknown Error.
                    else
                        this.UknownError("InsertProduct", e.Number, e.ErrorCode, e.Code, e.Message);

                }

                //Close the connection.
                this.Close();
            }
        }






        //Update a product from the database.
        public void UpdateProduct(Product new_pd, Product old_pd)
        {
            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;

                //Create the procedure.
                cmd.CommandText = "update_product";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //Add the parameters.
                cmd.Parameters.AddWithValue("pr_id", old_pd.id);
                cmd.Parameters.AddWithValue("pri", new_pd.price);
                cmd.Parameters.AddWithValue("pr_name", new_pd.name);
                cmd.Parameters.AddWithValue("sup", new_pd.supplier);
                cmd.Parameters.AddWithValue("phon", new_pd.phone);
                cmd.Parameters.AddWithValue("des", new_pd.description);
                cmd.Parameters.AddWithValue("barc", new_pd.barcode);


                //Try executing the command.
                try
                {

                    //Execute the commant.
                    cmd.ExecuteNonQuery();

                    //Show success message.
                    MessageBox.Show("Παλιό προϊόν:\n\n"+old_pd.ToString()+"\n\nΝέο προϊόν:\n\n"+new_pd.ToString(),
                        "Επιτυχής Επεξεργασία", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {

                    //Duplicate entry of product name.
                    if (e.Number == 1062)
                        MessageBox.Show("Το προϊόν με Όνομα: " + new_pd.name + ", υπάρχει ήδη.", "Αποτυχία Επεξεργασίας",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //Uknown Error.
                    else
                        this.UknownError("UpdateProduct", e.Number, e.ErrorCode, e.Code, e.Message);

                }

                //Close the connection.
                this.Close();
            }
        }





        //Get Product.
        public Product GetProduct(string name)
        {

            //Create the command.
            string q         = "SELECT * FROM products WHERE product_name='" + name + "';";
            MySqlCommand cmd = new MySqlCommand(q, connection);

            //Connect to the database.
            if (this.Connect())
            {

                //Try executing the command.
                try
                {

                    //Execute the command.
                    MySqlDataReader data = cmd.ExecuteReader();

                    //Read the first row of the data set.
                    data.Read();

                    //Create the product.
                    Product pd = new Product
                        (
                        (int)data["id"],
                        (string)data["barcode"],
                        (string)data["product_name"], 
                        (decimal)data["price"], 
                        (string)data["supplier"], 
                        (string)data["phone"],
                        (int)data["quantity"], 
                        (int)data["sold_quantity"], 
                        (string)data["descrip"]
                        );


                    //Close the data set.
                    data.Close();

                    //Close the connection.
                    this.Close();

                    //Return the product.
                    return pd;
                }

                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    this.UknownError("GetProduct", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }



            return null;
        }




        //Get Product.
        public Product GetProductByBarcode(string barcode)
        {

            //Create the command.
            string q = "SELECT * FROM products WHERE barcode='" + barcode + "';";
            MySqlCommand cmd = new MySqlCommand(q, connection);

            //Connect to the database.
            if (this.Connect())
            {

                //Try executing the command.
                try
                {

                    //Execute the command.
                    MySqlDataReader data = cmd.ExecuteReader();

                    //Read the first row of the data set.
                    data.Read();

                    //Create the product.
                    Product pd = new Product
                        (
                        (int)data["id"],
                        (string)data["barcode"],
                        (string)data["product_name"],
                        (decimal)data["price"],
                        (string)data["supplier"],
                        (string)data["phone"],
                        (int)data["quantity"],
                        (int)data["sold_quantity"],
                        (string)data["descrip"]
                        );


                    //Close the data set.
                    data.Close();

                    //Close the connection.
                    this.Close();

                    //Return the product.
                    return pd;
                }

                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    this.UknownError("GetProductByBarcode", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }



            return null;
        }





        //Get Product Names.
        public List<string> GetProductNames()
        {

            //Names List.
            List<string> names = new List<string>();

            //Create the command.
            string q         = "SELECT product_name FROM products;";
            MySqlCommand cmd = new MySqlCommand(q, connection);

            //Connect to the database.
            if (this.Connect())
            {

                //Try executing the command.
                try
                {

                    //Execute the command.
                    MySqlDataReader data = cmd.ExecuteReader();

                    //Get all the product names from the data set.
                    while (data.Read())
                        names.Add((string)data["product_name"]);

                    //Close the data set.
                    data.Close();
                }


                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    this.UknownError("GetProductNames", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }


            //Return the names.
            return names;

        }






        //Update a product from the database.
        public void DeleteProduct(string name)
        {
            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection   = connection;

                //Create the procedure.
                cmd.CommandText = "delete_product";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //Add the parameters.
                cmd.Parameters.AddWithValue("prod_name", name);


                //Try executing the command.
                try
                {

                    //Execute the commant.
                    cmd.ExecuteNonQuery();

                    //Show success message.
                    MessageBox.Show("Το προϊόν: "+name+ ", διαγράφηκε με επιτυχία!",
                        "Επιτυχής Διαγραφή", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    this.UknownError("DeleteProduct", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }
        }





        //Add Quantity.
        public bool AddQuantity(string barcode, int amount)
        {

            bool ok = true;

            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection   = connection;

                //Create the procedure.
                cmd.CommandText = "add_quantity";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //Add the parameters.
                cmd.Parameters.AddWithValue("barc", barcode);
                cmd.Parameters.AddWithValue("amount", amount);


                //Try executing the command.
                try
                {
                    //Execute the commant.
                    cmd.ExecuteNonQuery();
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    ok = false;
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {

                    ok = false;
                    //Primary key duplicate.
                    if (e.Number == 1062)
                    {
                        MessageBox.Show("To barcode: " + barcode + ", είναι ήδη καταχωρημένο.",
                            "Υπάρχει Ηδη", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    //Uknown error.
                    else
                    {
                        //The barcode is not registered into the database.
                        if (e.Number == 1644 && e.Message == "Not Found")
                            MessageBox.Show("Το προϊόν με Barcode: " + barcode + ", δεν βρέθηκε στο σύστημα.", "Δεν Υπάρχει",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                        else
                            this.UknownError("AddQuantity", e.Number, e.ErrorCode, e.Code, e.Message);
                    }
                }

                //Close the connection.
                this.Close();
            }

            return ok;
        }





        //Sell Item.
        public bool SellItem(Item item)
        {
            bool ok = true;

            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection   = connection;

                //Create the procedure.
                cmd.CommandText = "add_sold_item";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //Parameters.
                cmd.Parameters.AddWithValue("barc", item.barcode);


                //Try executing the command.
                try
                {
                    //Execute the commant.
                    cmd.ExecuteNonQuery();

                    //Show success message.
                    MessageBox.Show("To αντικείμενο: "+item.barcode+", εξάχθηκε με επιτυχία!",
                        "Επιτυχής Εξαγωγή", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {

                    ok = false;

                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {

                    ok = false;

                    //The barcode is not registered into the database.
                    if (e.Number == 1644 && e.Message == "Not Found")
                        MessageBox.Show("Το προϊόν με Barcode: " + item.barcode + ", δεν βρέθηκε στο σύστημα.", "Δεν Υπάρχει",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //The barcode is not registered into the database.
                    else if (e.Number == 1644 && e.Message == "Zero Quantity")
                    {
                        ok = true;
                        MessageBox.Show("Το προϊόν με Barcode: " + item.barcode + ", έχει 0 διαθέσιμη ποσότητα.",
                            "Μηδέν Ποσότητα", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    else
                        this.UknownError("SellItem", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }

            return ok;

        }





        //Get Item.
        public Item GetItem(int id, string barcode)
        {
            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection   = connection;

                //Create the procedure.
                cmd.CommandText = "SELECT * FROM records WHERE product_id="+id.ToString()+" AND barcode='"+barcode+"';";


                //Try executing the command.
                try
                {
                    //Execute the commant.
                    MySqlDataReader data = cmd.ExecuteReader();

                    //Read the data.
                    data.Read();

                    //Create th new item from the sql data.
                    Item new_item = new Item((string)data["barcode"], (DateTime)data["date_in"], (int)data["product_id"]);

                    //Close the data.
                    data.Close();

                    //Close the connection.
                    this.Close();

                    //return the item.
                    return new_item;
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {

                    //Item does not exists.
                    if (e.ErrorCode == -2147467259)
                    {
                        MessageBox.Show("Δεν υπάρχει καταχωρημένο αντικείμενο με barcode: " + barcode, "Δεν Υπάρχει", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                    //Uknown error.
                    else
                        this.UknownError("GetItem", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }

            return null;
        }





        //Delete Record.
        public void DeleteRecord(Item item)
        {
            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection   = connection;

                //Create the procedure.
                cmd.CommandText = "delete_record";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //Add the parameters.
                cmd.Parameters.AddWithValue("pr_id", item.product_id);
                cmd.Parameters.AddWithValue("barc", item.barcode);


                //Try executing the command.
                try
                {
                    //Execute the commant.
                    cmd.ExecuteNonQuery();

                    //Show success message.
                    MessageBox.Show("To αντικείμενο: " + item.barcode + ", διαγράφηκε με επιτυχία!",
                        "Διαγραφή Επιτυχής", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    this.UknownError("DeleteRecord", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }
        }





        //Get Analysis.
        public Analysis GetAnalysis(int year_out, int month_out)
        {

            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection   = connection;

                //Create the procedure.
                cmd.CommandText = "get_analysis";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                if (month_out != -1)
                {
                    cmd.Parameters.AddWithValue("year_out", year_out);
                    cmd.Parameters.AddWithValue("month_out", month_out);
                }

                else
                {
                    cmd.Parameters.AddWithValue("year_out", year_out);
                    cmd.Parameters.AddWithValue("month_out", null);
                }


                //Try executing the command.
                try
                {

                    //Create a new Analysis object.
                    Analysis analysis = new Analysis();

                    //Just a counter.
                    int counter = 0;

                    //Execute the commant.
                    MySqlDataReader data = cmd.ExecuteReader();

                    //Create a new page.
                    List<AnalysisItem> page = new List<AnalysisItem>();

                    //Get the data.
                    while (data.Read())
                    {

                        //Add the page into the analysis.
                        if (counter == 5)
                        {
                            analysis.m_pages.Add(page);
                            page    = new List<AnalysisItem>();
                            counter = 0;
                        }

                        //Add next item into the current page.
                        else
                        {
                            page.Add(new AnalysisItem((string)data["product_name"],
                                                           (decimal)data["price"],
                                                           (DateTime)data["date_out"],
                                                           (long)data["amount"]
                                                        ) 
                                    );

                            //Increase the counter.
                            counter++;
                        }
                    }

                    //Add the last page into the analysis object.
                    if (counter != 0)
                        analysis.m_pages.Add(page);


                    //Close the data and the connection.
                    data.Close();
                    this.Close();

                    //Return the Analysis object.
                    return analysis;
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    this.UknownError("GetAnalysis(year, month)", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }

            return null;
        }





        //Get Analysis Dates.
        public List<DateTime> GetAnalysisDates()
        {
            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection   = connection;

                //Create the procedure.
                cmd.CommandText = "get_analysis_dates";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;


                //Try executing the command.
                try
                {

                    //Execute the commant.
                    MySqlDataReader data = cmd.ExecuteReader();

                    //Create a list of dates..
                    List<DateTime> dates = new List<DateTime>();

                    //Get the data.
                    while (data.Read())
                    {
                        dates.Add((DateTime)data["date_out"]);
                    }


                    //Close the data and the connection.
                    data.Close();
                    this.Close();

                    //Return the dates.
                    return dates;
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    this.UknownError("GetAnalysisDates", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }


            //Return empty list.
            return new List<DateTime>();
        }





        //Get Analysis.
        public List<ExcelItem> GetAnalysis()
        {

            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection   = connection;

                //Create the procedure.
                cmd.CommandText = "get_analysis";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;


                //Add parameters.
                cmd.Parameters.AddWithValue("year_out", null);
                cmd.Parameters.AddWithValue("month_out", null);



                //Try executing the command.
                try
                {

                    //Execute the commant.
                    MySqlDataReader data = cmd.ExecuteReader();

                    //Create a new page.
                    List<ExcelItem> items = new List<ExcelItem>();

                    //Add all the items into the list.
                    while (data.Read())
                    {
                        items.Add(new ExcelItem(           (int)data["id"],
                                                           (string)data["barcode"],
                                                           (string)data["product_name"],
                                                           (decimal)data["price"],
                                                           (string)data["supplier"],
                                                           (string)data["phone"],
                                                           (int)data["quantity"],
                                                           (int)data["sold_quantity"],
                                                           (string)data["descrip"],
                                                           (DateTime)data["date_out"]
                                                   )
                                 );
                    }

                    //Close the data and the connection.
                    data.Close();
                    this.Close();

                    //Return the items.
                    return items;
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    this.UknownError("GetAnalysis()", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }

            return null;
        }





        //Get All Products.
        public List<Product> GetAllProducts()
        {
            //Connect to execute command.
            if (this.Connect())
            {

                //Create the Command.
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection   = connection;

                //Create the procedure.
                cmd.CommandText = "SELECT * FROM products;";



                //Try executing the command.
                try
                {

                    //Execute the commant.
                    MySqlDataReader data = cmd.ExecuteReader();

                    //Create a new page.
                    List<Product> products = new List<Product> ();

                    //Add all the items into the list.
                    while (data.Read())
                    {
                        products.Add(new Product(          (int)data["id"],
                                                           (string)data["barcode"],
                                                           (string)data["product_name"],
                                                           (decimal)data["price"],
                                                           (string)data["supplier"],
                                                           (string)data["phone"],
                                                           (int)data["quantity"],
                                                           (int)data["sold_quantity"],
                                                           (string)data["descrip"]
                                                   )
                                 );
                    }

                    //Close the data and the connection.
                    data.Close();
                    this.Close();

                    //Return the items.
                    return products;
                }



                //Something wrong happened.
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Η σύνδεση στην βάση δεδομένων απέτυχε. Δοκίμασε ξάνα αργότερα.", "Σφάλμα Σύνδεσης",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                //Something wrong happened.
                catch (MySqlException e)
                {
                    this.UknownError("GetAllProducts", e.Number, e.ErrorCode, e.Code, e.Message);
                }

                //Close the connection.
                this.Close();
            }


            //Else return an empty list.
            return new List<Product>();
        }
    }
}
