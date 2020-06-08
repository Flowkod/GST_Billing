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

#endregion

namespace GST_Billing.Models
{
    public class ItemTaxesManagement
    {
        #region "Variable"

        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString);
        SqlDataAdapter sda;
        SqlCommand cmd;

        #endregion

        #region "Property"
        public int Id { set; get; }
        public int User_ID { set; get; }
        public string Tax_Name { get; set; }
        public double GST { get; set; }
        public string Sp_Operation { get; set; }
        public DataTable dt { get; set; }
        #endregion

        #region "Function"
        public void AddwithParameter(SqlCommand Command)
        {
            if (Id > 0)
                Command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = Id;
            else
                Command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = DBNull.Value;

            if (User_ID > 0)
                Command.Parameters.Add(new SqlParameter("@User_ID", SqlDbType.Int)).Value = User_ID;
            else
                Command.Parameters.Add(new SqlParameter("@User_ID", SqlDbType.Int)).Value = DBNull.Value;

            if (Tax_Name != String.Empty && Tax_Name != null)
                Command.Parameters.Add(new SqlParameter("@Tax_Name", SqlDbType.VarChar)).Value = Tax_Name;
            else
                Command.Parameters.Add(new SqlParameter("@Tax_Name", SqlDbType.VarChar)).Value = DBNull.Value;

            if (GST > 0)
                Command.Parameters.Add(new SqlParameter("@GST", SqlDbType.Decimal)).Value = GST;
            else
                Command.Parameters.Add(new SqlParameter("@GST", SqlDbType.Decimal)).Value = DBNull.Value;

            if (Sp_Operation != string.Empty && Sp_Operation != null)
                Command.Parameters.Add(new SqlParameter("@Sp_Operation", SqlDbType.VarChar)).Value = Sp_Operation;
            else
                Command.Parameters.Add(new SqlParameter("@Sp_Operation", SqlDbType.VarChar)).Value = DBNull.Value;
        }
        public DataTable GetTaxes()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString))
            {
                cmd = new SqlCommand("ItemTaxesManagement", con);
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