using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

#region "Additional Name space"

using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;

#endregion

namespace GST_Billing.Models
{
    public class PurchaseReportManagement
    {
        #region "Variable"

        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString);
        SqlDataAdapter sda;
        SqlCommand cmd;

        #endregion

        #region "Property"

        public int Item_Id { get; set; }
        public int Supplier_Id { get; set; }
        public DateTime? From_Date { get; set; }
        public DateTime? To_Date { get; set; }
        public int Page_No { get; set; }
        public string Sp_Operation { get; set; }
        public DataTable dt { get; set; }
        public List<SelectListItem> ItemList { get; set; }
        public List<SelectListItem> SupplierList { get; set; }

        #endregion

        #region "Function"
        public void AddwithParameter(SqlCommand Command)
        {
            if (Item_Id > 0)
                Command.Parameters.Add(new SqlParameter("@Item_Id", SqlDbType.Int)).Value = Item_Id;
            else
                Command.Parameters.Add(new SqlParameter("@Item_Id", SqlDbType.Int)).Value = DBNull.Value;

            if (Supplier_Id > 0)
                Command.Parameters.Add(new SqlParameter("@Supplier_Id", SqlDbType.Int)).Value = Supplier_Id;
            else
                Command.Parameters.Add(new SqlParameter("@Supplier_Id", SqlDbType.Int)).Value = DBNull.Value;

            if (From_Date != DateTime.MinValue && From_Date != null)
                Command.Parameters.Add(new SqlParameter("@From_Date", SqlDbType.Date)).Value = From_Date;
            else
                Command.Parameters.Add(new SqlParameter("@From_Date", SqlDbType.Date)).Value = DBNull.Value;

            if (To_Date != DateTime.MinValue && To_Date != null)
                Command.Parameters.Add(new SqlParameter("@To_Date", SqlDbType.Date)).Value = To_Date;
            else
                Command.Parameters.Add(new SqlParameter("@To_Date", SqlDbType.Date)).Value = DBNull.Value;

            if (Page_No > 0)
                Command.Parameters.Add(new SqlParameter("@Page_No", SqlDbType.Int)).Value = Page_No;
            else
                Command.Parameters.Add(new SqlParameter("@Page_No", SqlDbType.Int)).Value = 1;

            if (Sp_Operation != string.Empty && Sp_Operation != null)
                Command.Parameters.Add(new SqlParameter("@Sp_Operation", SqlDbType.VarChar)).Value = Sp_Operation;
            else
                Command.Parameters.Add(new SqlParameter("@Sp_Operation", SqlDbType.VarChar)).Value = DBNull.Value;
        }
        public DataTable Getreport()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString))
            {
                cmd = new SqlCommand("PurchaseReportManagement", con);
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