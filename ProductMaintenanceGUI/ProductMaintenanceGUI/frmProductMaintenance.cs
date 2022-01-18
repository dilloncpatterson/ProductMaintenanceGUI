using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProductMaintenanceGUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
/// <summary>
/// Author: Dillon Crowshoe-Patterson
/// Due: July 20, 2021
/// MAIN FORM, displays data of all products and allows user to select one to either modify or delete the product.  
/// </summary>

namespace ProductMaintenanceGUI
{
    public partial class frmProductMaintenance : Form
    {
        private Products selectedProduct = null; // current Product(s).     

        //database context object.
        private TechSupportContext db = new TechSupportContext();

        //initialize Product Maintenance form >>>(MAIN FORM)<<<.
        public frmProductMaintenance()
        {
            InitializeComponent();
        }

        //Load Product Maintenace form. 
        //Required some pair-programming help from Alishea Armand in regards to placing formatting inside of Form Load...
        // and creating a DisplayProducts method.
        //
        private void frmProductMaintenance_Load(object sender, EventArgs e)
        {
            DisplayProducts();
        }
            
        //Received some pair-programming help from group during our first meeting for starting stages of formatting the...
        //Data Grid View layout. 
        private void DisplayProducts()
        {
            //Clear the columns after add, modify or delete. 
            dgvProducts.Columns.Clear();

            // Create db context object using TechSupport Database in Microsoft SQL Server Managment Studio 18. 
            using (TechSupportContext db = new TechSupportContext())
            {
                //Get products data from the database.
                var products = db.Products.OrderBy(p => p.ProductCode).
                   Select(p => new
                   {
                       p.ProductCode,
                       p.Name,
                       p.Version,
                       p.ReleaseDate
                   }).ToList();

                //Bind grid view to Products from database.
                dgvProducts.DataSource = products;

                //formatting 1st column.
                dgvProducts.Columns[0].HeaderText = "Product Code";
                dgvProducts.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvProducts.Columns[0].Width = 80;
                dgvProducts.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

                //formatting 2nd column.
                dgvProducts.Columns[1].HeaderText = "Name";
                dgvProducts.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvProducts.Columns[1].Width = 235;
                dgvProducts.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

                //formatting 3rd column.
                dgvProducts.Columns[2].HeaderText = "Version";
                dgvProducts.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvProducts.Columns[2].Width = 80;
                dgvProducts.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                
                //formatting 4th column.
                dgvProducts.Columns[3].HeaderText = "Release Date";
                dgvProducts.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvProducts.Columns[3].Width = 110;
                dgvProducts.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

                
                //Add column "Modify" button. 
                var modifyColumn = new DataGridViewButtonColumn() {               
                UseColumnTextForButtonValue = true,
                HeaderText = "",
                Text = "Modify" };
                dgvProducts.Columns.Add(modifyColumn);


                // Add column for "Delete" button.
                var deleteColumn = new DataGridViewButtonColumn()
                {
                    UseColumnTextForButtonValue = true,
                    HeaderText = "",
                    Text = "Delete"
                };
                dgvProducts.Columns.Add(deleteColumn);


                //format the grid
                // make column headers bold
                dgvProducts.ColumnHeadersDefaultCellStyle.Font =
                    new Font("Segoe", 9, FontStyle.Bold);
                // change background colour on alternating rows
                dgvProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            }
        }
        //received some pair-programming help from Micheal Lipski in regards to creating an event handler... 
        // for dgvProducts_CellContentClick. 
        private void dgvProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // store the index values for Modify and Delete columns
            const int ModifyIndex = 4;
            const int DeleteIndex = 5;

            if (e.ColumnIndex == ModifyIndex || e.ColumnIndex == DeleteIndex)
            {
                string productCode = dgvProducts.Rows[e.RowIndex].Cells[0].Value.ToString().Trim();
                selectedProduct = db.Products.Find(productCode);
            }

            if (e.ColumnIndex == ModifyIndex)
            {
                ModifyProduct();
            }

            else if (e.ColumnIndex == DeleteIndex)
            {
                DeleteProduct();
            }
        }

        //Add button calls to second form (frmAddModifyProduct) so user can add more data. 
        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddModifyProduct addProductForm = new frmAddModifyProduct();
            addProductForm.AddProduct = true;
            addProductForm.Product = null; // not Product yet
           

            //display the second form model and process result whem it closes
            DialogResult result = addProductForm.ShowDialog();

            if (result == DialogResult.OK)// second form has a customer object with data
            {
                selectedProduct = addProductForm.Product;
                try
                {
                    db.Products.Add(selectedProduct);
                    db.SaveChanges();                 
                    DisplayProducts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error when adding Product: " + ex.Message,
                               ex.GetType().ToString());
                }
            }         
        }
        //Modify button in dgv calls to secondform (frmAddModifyProduct) and allows user to modify product.   
        private void ModifyProduct()
        {
              frmAddModifyProduct modifyProductForm = new frmAddModifyProduct()
            {
                AddProduct = false,
                Product = selectedProduct
            };
            DialogResult result = modifyProductForm.ShowDialog();
            
            if (result == DialogResult.OK)
            {
                selectedProduct = modifyProductForm.Product;
                try
                {
                    db.SaveChanges();                   
                    DisplayProducts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error when modifying Product: " + ex.Message,
                               ex.GetType().ToString());
                }
            }
        }

        //method for Deleting product with message asking user to confirm the deletion of product. 
        private void DeleteProduct()
        {           
            if (selectedProduct != null)
            {
                // get confirmation from the user
                DialogResult answer = MessageBox.Show($"Do you want to delete {selectedProduct.Name}?",
                                                        "Conform Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (answer == DialogResult.Yes)// user confirmed 
                {
                    try
                    {
                        db.Products.Remove(selectedProduct);                        
                        db.SaveChanges();
                        DisplayProducts();                      
                    }
                    catch (DbUpdateException ex)//database error
                    {
                        //get inner exception with potential multiple errors
                        SqlException innerException = (SqlException)ex.InnerException;
                        string message = "";
                        foreach (SqlError err in innerException.Errors)
                        {
                            message += $"ERROR code: {err.Number} - {err.Message } \n";

                        }
                        MessageBox.Show("Other error when deleting product: " + ex.Message,
                            ex.GetType().ToString());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error when deleting product: " + ex.Message,
                            ex.GetType().ToString());
                    }
                }             
            }
        }
        //Exit the Application
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }//class
}//namespece
    

