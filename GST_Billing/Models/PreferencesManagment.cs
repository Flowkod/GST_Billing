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
    public class PreferencesManagment
    {
        #region "Variable"

        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString);
        SqlDataAdapter sda;
        SqlCommand cmd;

        #endregion

        #region "Property"

        public int Id { set; get; }
        public int Company_Id { get; set; }
        public int User_Id { get; set; }
        public string Terms_Condition { get; set; }
        public string Signature { get; set; }
        public bool PrintShipAddress { get; set; }
        public bool PrintBillAddress { get; set; }
        public bool PrintPaymentTerms { get; set; }
        public bool PrintClientNote { get; set; }
        public bool PrintTransport { get; set; }
        public bool PrintTermsCondition { get; set; }
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

            if (Company_Id > 0)
                Command.Parameters.Add(new SqlParameter("@Company_Id", SqlDbType.Int)).Value = Company_Id;
            else
                Command.Parameters.Add(new SqlParameter("@Company_Id", SqlDbType.Int)).Value = DBNull.Value;

            if (User_Id > 0)
                Command.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.Int)).Value = User_Id;
            else
                Command.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.Int)).Value = DBNull.Value;

            if (Terms_Condition != String.Empty && Terms_Condition != null)
                Command.Parameters.Add(new SqlParameter("@Terms_Condition", SqlDbType.VarChar)).Value = Terms_Condition;
            else
                Command.Parameters.Add(new SqlParameter("@Terms_Condition", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Signature != String.Empty && Signature != null)
                Command.Parameters.Add(new SqlParameter("@Signature", SqlDbType.VarChar)).Value = Signature;
            else
                Command.Parameters.Add(new SqlParameter("@Signature", SqlDbType.VarChar)).Value = DBNull.Value;

            if (PrintShipAddress != false)
                Command.Parameters.Add(new SqlParameter("@PrintShipAddress", SqlDbType.VarChar)).Value = PrintShipAddress;
            else
                Command.Parameters.Add(new SqlParameter("@PrintShipAddress", SqlDbType.VarChar)).Value = false;

            if (PrintBillAddress != false)
                Command.Parameters.Add(new SqlParameter("@PrintBillAddress", SqlDbType.VarChar)).Value = PrintBillAddress;
            else
                Command.Parameters.Add(new SqlParameter("@PrintBillAddress", SqlDbType.VarChar)).Value = false;

            if (PrintPaymentTerms != false)
                Command.Parameters.Add(new SqlParameter("@PrintPaymentTerms", SqlDbType.VarChar)).Value = PrintPaymentTerms;
            else
                Command.Parameters.Add(new SqlParameter("@PrintPaymentTerms", SqlDbType.VarChar)).Value = false;

            if (PrintClientNote != false)
                Command.Parameters.Add(new SqlParameter("@PrintClientNote", SqlDbType.VarChar)).Value = PrintClientNote;
            else
                Command.Parameters.Add(new SqlParameter("@PrintClientNote", SqlDbType.VarChar)).Value = false;

            if (PrintTermsCondition != false)
                Command.Parameters.Add(new SqlParameter("@PrintTermsCondition", SqlDbType.VarChar)).Value = PrintTermsCondition;
            else
                Command.Parameters.Add(new SqlParameter("@PrintTermsCondition", SqlDbType.VarChar)).Value = false;

            if (PrintTransport != false)
                Command.Parameters.Add(new SqlParameter("@PrintTransport", SqlDbType.VarChar)).Value = PrintTransport;
            else
                Command.Parameters.Add(new SqlParameter("@PrintTransport", SqlDbType.VarChar)).Value = false;

            if (Sp_Operation != string.Empty && Sp_Operation != null)
                Command.Parameters.Add(new SqlParameter("@Sp_Operation", SqlDbType.VarChar)).Value = Sp_Operation;
            else
                Command.Parameters.Add(new SqlParameter("@Sp_Operation", SqlDbType.VarChar)).Value = DBNull.Value;
        }
        public DataTable GetPreferences()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString))
            {
                cmd = new SqlCommand("PreferencesManagment", con);
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