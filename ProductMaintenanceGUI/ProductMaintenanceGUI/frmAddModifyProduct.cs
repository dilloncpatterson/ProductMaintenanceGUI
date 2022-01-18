using ProductMaintenanceGUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
/// <summary>
/// Author: Dillon Crowshoe-Patterson
/// Due: July 20,2021
/// SECOND FORM: When user clicks add, the form will display empty tect boxes for user to enter Product Code, Product Name,
/// Product Version, and Product Release Date. When user clicks modify, user will be able to modify Product Name, Product... 
/// Version, and Product Release Date. Buttons OK to confirm, and Cancel to abort add or modify. 
/// </summary>

namespace ProductMaintenanceGUI
{
    public partial class frmAddModifyProduct : Form
    {
        public Products Product { get; set; }
        public bool AddProduct { get; set; }       
        public frmAddModifyProduct()
        {
            InitializeComponent();
        }
            //On form load for either adding or modifying product. 
            private void frmAddModifyProduct_Load(object sender, EventArgs e)
            {
                if (AddProduct)
                {
                    this.Text = "Add Product";
                    txtProductCode.ReadOnly = false;  // allow entry of new product code.
                }               
                else
                {
                    this.Text = "Modify Product";
                    txtProductCode.ReadOnly = true;   // can't change existing product code.
                    this.DisplayProduct();                  
                }         
            }
            //Displays empty form for Data to be entered in the text boxes. 
            private void DisplayProduct()
            {
                txtProductCode.Text = Product.ProductCode;
                txtName.Text = Product.Name;
                txtVersion.Text = Product.Version.ToString("N1");
                txtReleaseDate.Text = Product.ReleaseDate.ToString();
            }


        //Validator class to validate that proper input is entered. 
        private bool IsValidData()
        {
            bool success = true;
            string errorMessage = "";

            errorMessage += Validator.IsPresent(txtProductCode.Text, txtProductCode.Tag.ToString());
            errorMessage += Validator.IsPresent(txtName.Text, txtName.Tag.ToString());
            errorMessage += Validator.IsDecimal(txtVersion.Text, txtVersion.Tag.ToString());
            errorMessage += Validator.IsPresent(txtReleaseDate.Text, txtReleaseDate.Tag.ToString());

            if (errorMessage != "")
            {
                success = false;
                MessageBox.Show(errorMessage, "Entry Error");
            }
            return success;
        }

        //Loads data into the database and converts entered text, decimal and DateTime data. 
        private void LoadProductData()
            {
                Product.ProductCode = txtProductCode.Text;
                Product.Name = txtName.Text;
                Product.Version = Convert.ToDecimal(txtVersion.Text);
                Product.ReleaseDate = Convert.ToDateTime(txtReleaseDate.Text);          
            }

        //Ok button click will add data to database once data entered is valid.
        private void btnOk_Click(object sender, EventArgs e)
        {
            frmProductMaintenance Mainform = new frmProductMaintenance();
            if (IsValidData())
            {
                if (AddProduct)
                {
                    // initialize the Product property with new Products object
                    this.Product = new Products();
                }
                this.LoadProductData();
                this.DialogResult = DialogResult.OK;           
            }         
        }

        //Cancel button to abort data input. 
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }//class
 }//namespace

