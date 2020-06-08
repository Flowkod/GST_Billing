using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

#region "Additional Name space"

using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Web.Mvc;

#endregion

namespace GST_Billing.Models
{
    public class ClientManagement
    {
        #region "Variable"

        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString);
        SqlDataAdapter sda;
        SqlCommand cmd;

        #endregion

        #region "Property"
        public int Id { get; set; }
        [Required(ErrorMessage = "Client code required..")]
        [Remote("IsClientCodeExist", "Master", ErrorMessage = "Client code already exist", AdditionalFields = "Id")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Company Name required..")]
        public string Company_Name { get; set; }
        public string Contact_Person_Name { get; set; }
        
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please enter a valid Mobile no.")]
        public string Mobile_No { get; set; }
        public string Phone_No { get; set; }
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Please enter a valid Email.")]
        public string Email { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Pin_Code { get; set; }
        //Fot Shipping Address
        public int[] ShippingAddressId { get; set; }
        public string[] Ship_Address { get; set; }
        public string[] Ship_Country { get; set; }
        public string[] Ship_State { get; set; }
        public string[] Ship_City { get; set; }
        public string[] Ship_Pin_Code { get; set; }
        public string[] Ship_Company_Name { get; set; }
        public string[] Ship_GSTIN { get; set; }
        public string[] Ship_Contact_Person { get; set; }
        public string[] Ship_Mobile_No { get; set; }

        public string Website { get; set; }
        public string GSTIN { get; set; }
        public string PAN { get; set; }
        public string LST { get; set; }
        public string TPT { get; set; }
        public string VAT { get; set; }
        public string Note { get; set; }
        public decimal? Opening_Balance { get; set; }
        public string Opening_Type { get; set; }
        public int User_ID { get; set; }
        public int Page_No { get; set; }
        public Uri PreviousPage { get; set; }
        public string Sp_Operation { get; set; }
        public DataTable dt { get; set; }
        public DataTable dt1 { get; set; }
        public DataTable ShippingAddress { get; set; }

        #endregion

        #region "Function"
        public void AddwithParameter(SqlCommand Command)
        {
            if (Id > 0)
                Command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = Id;
            else
                Command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = DBNull.Value;

            if (Code != string.Empty && Code != null)
                Command.Parameters.Add(new SqlParameter("@Code", SqlDbType.VarChar)).Value = Code;
            else
                Command.Parameters.Add(new SqlParameter("@Code", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Company_Name != string.Empty && Company_Name != null)
                Command.Parameters.Add(new SqlParameter("@Company_Name", SqlDbType.VarChar)).Value = Company_Name;
            else
                Command.Parameters.Add(new SqlParameter("@Company_Name", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Contact_Person_Name != string.Empty && Contact_Person_Name != null)
                Command.Parameters.Add(new SqlParameter("@Contact_Person_Name", SqlDbType.VarChar)).Value = Contact_Person_Name;
            else
                Command.Parameters.Add(new SqlParameter("@Contact_Person_Name", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Mobile_No != string.Empty && Mobile_No != null)
                Command.Parameters.Add(new SqlParameter("@Mobile_No", SqlDbType.VarChar)).Value = Mobile_No;
            else
                Command.Parameters.Add(new SqlParameter("@Mobile_No", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Phone_No != string.Empty && Phone_No != null)
                Command.Parameters.Add(new SqlParameter("@Phone_No", SqlDbType.VarChar)).Value = Phone_No;
            else
                Command.Parameters.Add(new SqlParameter("@Phone_No", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Email != string.Empty && Email != null)
                Command.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar)).Value = Email;
            else
                Command.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Address != string.Empty && Address != null)
                Command.Parameters.Add(new SqlParameter("@Address", SqlDbType.VarChar)).Value = Address;
            else
                Command.Parameters.Add(new SqlParameter("@Address", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Country != string.Empty && Country != null)
                Command.Parameters.Add(new SqlParameter("@Country", SqlDbType.VarChar)).Value = Country;
            else
                Command.Parameters.Add(new SqlParameter("@Country", SqlDbType.VarChar)).Value = DBNull.Value;

            if (State != string.Empty && State != null)
                Command.Parameters.Add(new SqlParameter("@State", SqlDbType.VarChar)).Value = State;
            else
                Command.Parameters.Add(new SqlParameter("@State", SqlDbType.VarChar)).Value = DBNull.Value;

            if (City != string.Empty && City != null)
                Command.Parameters.Add(new SqlParameter("@City", SqlDbType.VarChar)).Value = City;
            else
                Command.Parameters.Add(new SqlParameter("@City", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Pin_Code != string.Empty && Pin_Code != null)
                Command.Parameters.Add(new SqlParameter("@Pin_Code", SqlDbType.VarChar)).Value = Pin_Code;
            else
                Command.Parameters.Add(new SqlParameter("@Pin_Code", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Website != string.Empty && Website != null)
                Command.Parameters.Add(new SqlParameter("@Website", SqlDbType.VarChar)).Value = Website;
            else
                Command.Parameters.Add(new SqlParameter("@Website", SqlDbType.VarChar)).Value = DBNull.Value;

            if (GSTIN != string.Empty && GSTIN != null)
                Command.Parameters.Add(new SqlParameter("@GSTIN", SqlDbType.VarChar)).Value = GSTIN;
            else
                Command.Parameters.Add(new SqlParameter("@GSTIN", SqlDbType.VarChar)).Value = DBNull.Value;

            if (PAN != string.Empty && PAN != null)
                Command.Parameters.Add(new SqlParameter("@PAN", SqlDbType.VarChar)).Value = PAN;
            else
                Command.Parameters.Add(new SqlParameter("@PAN", SqlDbType.VarChar)).Value = DBNull.Value;

            if (LST != string.Empty && LST != null)
                Command.Parameters.Add(new SqlParameter("@LST", SqlDbType.VarChar)).Value = LST;
            else
                Command.Parameters.Add(new SqlParameter("@LST", SqlDbType.VarChar)).Value = DBNull.Value;

            if (TPT != string.Empty && TPT != null)
                Command.Parameters.Add(new SqlParameter("@TPT", SqlDbType.VarChar)).Value = TPT;
            else
                Command.Parameters.Add(new SqlParameter("@TPT", SqlDbType.VarChar)).Value = DBNull.Value;

            if (VAT != string.Empty && VAT != null)
                Command.Parameters.Add(new SqlParameter("@VAT", SqlDbType.VarChar)).Value = VAT;
            else
                Command.Parameters.Add(new SqlParameter("@VAT", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Note != string.Empty && Note != null)
                Command.Parameters.Add(new SqlParameter("@Note", SqlDbType.VarChar)).Value = Note;
            else
                Command.Parameters.Add(new SqlParameter("@Note", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Opening_Balance > 0)
                Command.Parameters.Add(new SqlParameter("@Opening_Balance", SqlDbType.Int)).Value = Opening_Balance;
            else
                Command.Parameters.Add(new SqlParameter("@Opening_Balance", SqlDbType.Int)).Value = DBNull.Value;

            if (Opening_Type != string.Empty && Opening_Type != null)
                Command.Parameters.Add(new SqlParameter("@Opening_Type", SqlDbType.VarChar)).Value = Opening_Type;
            else
                Command.Parameters.Add(new SqlParameter("@Opening_Type", SqlDbType.VarChar)).Value = DBNull.Value;

            if (User_ID > 0)
                Command.Parameters.Add(new SqlParameter("@User_ID", SqlDbType.Int)).Value = User_ID;
            else
                Command.Parameters.Add(new SqlParameter("@User_ID", SqlDbType.Int)).Value = DBNull.Value;

            if (ShippingAddress != null)
                Command.Parameters.Add(new SqlParameter("@ShippingAddress", SqlDbType.Structured)).Value = ShippingAddress;
            else
                Command.Parameters.Add(new SqlParameter("@ShippingAddress", SqlDbType.Structured)).Value = null;

            if (Page_No > 0)
                Command.Parameters.Add(new SqlParameter("@Page_No", SqlDbType.Int)).Value = Page_No;
            else
                Command.Parameters.Add(new SqlParameter("@Page_No", SqlDbType.Int)).Value = 1;

            if (Sp_Operation != string.Empty && Sp_Operation != null)
                Command.Parameters.Add(new SqlParameter("@Sp_Operation", SqlDbType.VarChar)).Value = Sp_Operation;
            else
                Command.Parameters.Add(new SqlParameter("@Sp_Operation", SqlDbType.VarChar)).Value = DBNull.Value;

        }
        public DataTable GetClient()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString))
            {
                cmd = new SqlCommand("ClientManagement", con);
                AddwithParameter(cmd);
                DataTable DtUser = new DataTable();

                cmd.CommandType = CommandType.StoredProcedure;

                sda = new SqlDataAdapter(cmd);
                con.Open();
                sda.Fill(DtUser);

                return DtUser;
            }
        }

        #endregion
    }
}