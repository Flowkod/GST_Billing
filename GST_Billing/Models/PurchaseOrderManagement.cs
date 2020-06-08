using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Mvc;

namespace GST_Billing.Models
{
    public class PurchaseOrderManagement
    {
        #region "Variable"

        SqlDataAdapter sda;
        SqlCommand cmd;

        #endregion

        #region "Property"

        public int Id { get; set; }
		public string Initial_No { get; set; }
		public int PO_No { get; set; }
		public DateTime Date { get; set; }
		public int Supplier_Id { get; set; }
		public int Ship_To { get; set; }
		public double Net_Total { get; set; }
		public double GST_Amount { get; set; }
		public double Round_Off { get; set; }
		public double Gross_Total { get; set; }
		public string Delivery_Period { get; set; }
		public string Payment_Terms { get; set; }
		public string Transport { get; set; }
		public string Note { get; set; }
		public string Gst_Type { get; set; }
        public int User_ID { get; set; }
        public int Ship_To_Client_Id { get; set; }
        public int Client_Ship_Address_Id { get; set; }
        public string Sp_Operation { get; set; }

        public List<SelectListItem> SupplierList { get; set; }
        public List<SelectListItem> ClientList { get; set; }
        public DataTable dtCompanyDetails { get; set; }

        #endregion

        #region "Function"
        public void AddwithParameter(SqlCommand Command)
        {
            if (Id > 0)
                Command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = Id;
            else
                Command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = DBNull.Value;

            if (Initial_No != string.Empty && Initial_No != null)
                Command.Parameters.Add(new SqlParameter("@Initial_No", SqlDbType.VarChar)).Value = Initial_No;
            else
                Command.Parameters.Add(new SqlParameter("@Initial_No", SqlDbType.VarChar)).Value = DBNull.Value;

            if (PO_No > 0)
                Command.Parameters.Add(new SqlParameter("@PO_No", SqlDbType.Int)).Value = PO_No;
            else
                Command.Parameters.Add(new SqlParameter("@PO_No", SqlDbType.Int)).Value = DBNull.Value;

            if (Date != DateTime.MinValue && Date != null)
                Command.Parameters.Add(new SqlParameter("@Date", SqlDbType.DateTime)).Value = Date;
            else
                Command.Parameters.Add(new SqlParameter("@Date", SqlDbType.DateTime)).Value = DBNull.Value;

            if (Supplier_Id > 0)
                Command.Parameters.Add(new SqlParameter("@Supplier_Id", SqlDbType.Int)).Value = Supplier_Id;
            else
                Command.Parameters.Add(new SqlParameter("@Supplier_Id", SqlDbType.Int)).Value = DBNull.Value;

            if (Ship_To > 0)
                Command.Parameters.Add(new SqlParameter("@Ship_To", SqlDbType.Int)).Value = Ship_To;
            else
                Command.Parameters.Add(new SqlParameter("@Ship_To", SqlDbType.Int)).Value = DBNull.Value;

            if (Net_Total > 0)
                Command.Parameters.Add(new SqlParameter("@Net_Total", SqlDbType.Decimal)).Value = Net_Total;
            else
                Command.Parameters.Add(new SqlParameter("@Net_Total", SqlDbType.Decimal)).Value = DBNull.Value;

            if (GST_Amount > 0)
                Command.Parameters.Add(new SqlParameter("@GST_Amount", SqlDbType.Decimal)).Value = GST_Amount;
            else
                Command.Parameters.Add(new SqlParameter("@GST_Amount", SqlDbType.Decimal)).Value = DBNull.Value;

            if (Round_Off > 0)
                Command.Parameters.Add(new SqlParameter("@Round_Off", SqlDbType.Decimal)).Value = Round_Off;
            else
                Command.Parameters.Add(new SqlParameter("@Round_Off", SqlDbType.Decimal)).Value = DBNull.Value;

            if (Gross_Total > 0)
                Command.Parameters.Add(new SqlParameter("@Gross_Total", SqlDbType.Decimal)).Value = Gross_Total;
            else
                Command.Parameters.Add(new SqlParameter("@Gross_Total", SqlDbType.Decimal)).Value = DBNull.Value;

            if (Delivery_Period != string.Empty && Delivery_Period != null)
                Command.Parameters.Add(new SqlParameter("@Delivery_Period", SqlDbType.VarChar)).Value = Delivery_Period;
            else
                Command.Parameters.Add(new SqlParameter("@Delivery_Period", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Payment_Terms != string.Empty && Payment_Terms != null)
                Command.Parameters.Add(new SqlParameter("@Payment_Terms", SqlDbType.VarChar)).Value = Payment_Terms;
            else
                Command.Parameters.Add(new SqlParameter("@Payment_Terms", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Transport != string.Empty && Transport != null)
                Command.Parameters.Add(new SqlParameter("@Transport", SqlDbType.VarChar)).Value = Transport;
            else
                Command.Parameters.Add(new SqlParameter("@Transport", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Note != string.Empty && Note != null)
                Command.Parameters.Add(new SqlParameter("@Note", SqlDbType.VarChar)).Value = Note;
            else
                Command.Parameters.Add(new SqlParameter("@Note", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Gst_Type != string.Empty && Gst_Type != null)
                Command.Parameters.Add(new SqlParameter("@Gst_Type", SqlDbType.VarChar)).Value = Gst_Type;
            else
                Command.Parameters.Add(new SqlParameter("@Gst_Type", SqlDbType.VarChar)).Value = DBNull.Value;

            if (User_ID > 0)
                Command.Parameters.Add(new SqlParameter("@User_ID", SqlDbType.Int)).Value = User_ID;
            else
                Command.Parameters.Add(new SqlParameter("@User_ID", SqlDbType.Int)).Value = DBNull.Value;

            if (Sp_Operation != string.Empty && Sp_Operation != null)
                Command.Parameters.Add(new SqlParameter("@Sp_Operation", SqlDbType.VarChar)).Value = Sp_Operation;
            else
                Command.Parameters.Add(new SqlParameter("@Sp_Operation", SqlDbType.VarChar)).Value = DBNull.Value;

        }
        public DataTable Getpurchaseorder()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString))
            {
                cmd = new SqlCommand("ExpensesMaster", con);
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