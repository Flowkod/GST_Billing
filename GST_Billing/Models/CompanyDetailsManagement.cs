using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace GST_Billing.Models
{
    public class CompanyDetailsManagement
    {
		#region "Variable"

		SqlDataAdapter sda;
		SqlCommand cmd;

		#endregion

		public int Company_Id { get; set; }
		public string Company_Name { get; set; }
		public string Contact_Person { get; set; }
		public string Email { get; set; }
		public string Mobile_No { get; set; }
		public string GSTIN { get; set; }
		public string Website { get; set; }
		public string Address { get; set; }
		public string State { get; set; }
		public string City { get; set; }
		public string Pin_Code { get; set; }
		public string Shipping_Address { get; set; }
		public string Company_Logo { get; set; }
		public string Favicon { get; set; }
		public int User_ID { get; set; }
        public string Sp_Operation { get; set; }

        #region "Function"
        public void AddwithParameter(SqlCommand Command)
        {
            if (Company_Id > 0)
                Command.Parameters.Add(new SqlParameter("@Company_Id", SqlDbType.Int)).Value = Company_Id;
            else
                Command.Parameters.Add(new SqlParameter("@Company_Id", SqlDbType.Int)).Value = DBNull.Value;

            if (Company_Name != string.Empty && Company_Name != null)
                Command.Parameters.Add(new SqlParameter("@Company_Name", SqlDbType.VarChar)).Value = Company_Name;
            else
                Command.Parameters.Add(new SqlParameter("@Company_Name", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Contact_Person != string.Empty && Contact_Person != null)
                Command.Parameters.Add(new SqlParameter("@Contact_Person", SqlDbType.VarChar)).Value = Contact_Person;
            else
                Command.Parameters.Add(new SqlParameter("@Contact_Person", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Email != string.Empty && Email != null)
                Command.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar)).Value = Email;
            else
                Command.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Mobile_No != string.Empty && Mobile_No != null)
                Command.Parameters.Add(new SqlParameter("@Mobile_No", SqlDbType.VarChar)).Value = Mobile_No;
            else
                Command.Parameters.Add(new SqlParameter("@Mobile_No", SqlDbType.VarChar)).Value = DBNull.Value;

            if (GSTIN != string.Empty && GSTIN != null)
                Command.Parameters.Add(new SqlParameter("@GSTIN", SqlDbType.VarChar)).Value = GSTIN;
            else
                Command.Parameters.Add(new SqlParameter("@GSTIN", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Website != string.Empty && Website != null)
                Command.Parameters.Add(new SqlParameter("@Website", SqlDbType.VarChar)).Value = Website;
            else
                Command.Parameters.Add(new SqlParameter("@Website", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Address != string.Empty && Address != null)
                Command.Parameters.Add(new SqlParameter("@Address", SqlDbType.VarChar)).Value = Address;
            else
                Command.Parameters.Add(new SqlParameter("@Address", SqlDbType.VarChar)).Value = DBNull.Value;

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

            if (Shipping_Address != string.Empty && Shipping_Address != null)
                Command.Parameters.Add(new SqlParameter("@Shipping_Address", SqlDbType.VarChar)).Value = Shipping_Address;
            else
                Command.Parameters.Add(new SqlParameter("@Shipping_Address", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Company_Logo != string.Empty && Company_Logo != null)
                Command.Parameters.Add(new SqlParameter("@Company_Logo", SqlDbType.VarChar)).Value = Company_Logo;
            else
                Command.Parameters.Add(new SqlParameter("@Company_Logo", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Favicon != string.Empty && Favicon != null)
                Command.Parameters.Add(new SqlParameter("@Favicon", SqlDbType.VarChar)).Value = Favicon;
            else
                Command.Parameters.Add(new SqlParameter("@Favicon", SqlDbType.VarChar)).Value = DBNull.Value;

            if (User_ID > 0)
                Command.Parameters.Add(new SqlParameter("@User_ID", SqlDbType.Int)).Value = User_ID;
            else
                Command.Parameters.Add(new SqlParameter("@User_ID", SqlDbType.Int)).Value = DBNull.Value;

            if (Sp_Operation != string.Empty && Sp_Operation != null)
                Command.Parameters.Add(new SqlParameter("@Sp_Operation", SqlDbType.VarChar)).Value = Sp_Operation;
            else
                Command.Parameters.Add(new SqlParameter("@Sp_Operation", SqlDbType.VarChar)).Value = DBNull.Value;

        }
        public DataTable Getcompanydetails()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString))
            {
                cmd = new SqlCommand("CompanyDetailsManagement", con);
                AddwithParameter(cmd);
                DataTable dt = new DataTable();

                cmd.CommandType = CommandType.StoredProcedure;

                sda = new SqlDataAdapter(cmd);
                con.Open();
                sda.Fill(dt);

                return dt;
            }
        }

        #endregion
    }
}