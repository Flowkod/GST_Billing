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
    public class PurchaseOrderProductManagement
    {
        #region "Variable"

        SqlDataAdapter sda;
        SqlCommand cmd;

        #endregion

        #region "Property"

        public int PurchaseOrder_Product_Id { get; set; }
        public int Purchase_Order_Id { get; set; }
        public int? Item_Id { get; set; }
        public string Item_Name { get; set; }
        public string Batch_No { get; set; }
        public string HSN_SAC { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Quantity { get; set; }
        public string Unit_Name { get; set; }
        public int? UOM_Unit { get; set; }
        public string Item_Unit_Name { get; set; }               
        public decimal? Tax { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal GST_Amount { get; set; }
        public decimal Discount_Percentage { get; set; }
        public decimal Discount_Amount { get; set; }
        public decimal Total { get; set; }
        public string Note { get; set; }
        public string Item_Note { get; set; }
        public int User_ID { get; set; }       
        public string Sp_Operation { get; set; }        
        public DataTable dt { get; set; }

        public List<PurchaseOrderProductManagement> Poitemlist { get; set; }
        public List<SelectListItem> ItemList { get; set; }
        public List<SelectListItem> TaxList { get; set; }
        public List<SelectListItem> ItemUnitList { get; set; }

        #endregion

        //#region "Function"
        //public void AddwithParameter(SqlCommand Command)
        //{
        //    if (Id > 0)
        //        Command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = Id;
        //    else
        //        Command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = DBNull.Value;

        //    if (Purchase_Order_Id > 0)
        //        Command.Parameters.Add(new SqlParameter("@Purchase_Order_Id", SqlDbType.Int)).Value = Purchase_Order_Id;
        //    else
        //        Command.Parameters.Add(new SqlParameter("@Purchase_Order_Id", SqlDbType.Int)).Value = DBNull.Value;

        //    if (Item_Id > 0)
        //        Command.Parameters.Add(new SqlParameter("@Item_Id", SqlDbType.Int)).Value = Item_Id;
        //    else
        //        Command.Parameters.Add(new SqlParameter("@Item_Id", SqlDbType.Int)).Value = DBNull.Value;

        //    if (Serial_No != string.Empty && Serial_No != null)
        //        Command.Parameters.Add(new SqlParameter("@Serial_No", SqlDbType.VarChar)).Value = Serial_No;
        //    else
        //        Command.Parameters.Add(new SqlParameter("@Serial_No", SqlDbType.VarChar)).Value = DBNull.Value;

        //    if (Rate > 0)
        //        Command.Parameters.Add(new SqlParameter("@Rate", SqlDbType.Decimal)).Value = Rate;
        //    else
        //        Command.Parameters.Add(new SqlParameter("@Rate", SqlDbType.Decimal)).Value = DBNull.Value;

        //    if (Quantity > 0)
        //        Command.Parameters.Add(new SqlParameter("@Quantity", SqlDbType.Decimal)).Value = Quantity;
        //    else
        //        Command.Parameters.Add(new SqlParameter("@Quantity", SqlDbType.Decimal)).Value = DBNull.Value;

        //    if (UOM_Unit > 0)
        //        Command.Parameters.Add(new SqlParameter("@UOM_Unit", SqlDbType.Int)).Value = UOM_Unit;
        //    else
        //        Command.Parameters.Add(new SqlParameter("@UOM_Unit", SqlDbType.Int)).Value = DBNull.Value;

        //    if (Tax > 0)
        //        Command.Parameters.Add(new SqlParameter("@Tax", SqlDbType.Int)).Value = Tax;
        //    else
        //        Command.Parameters.Add(new SqlParameter("@Tax", SqlDbType.Int)).Value = DBNull.Value;

        //    if (GST_Amount > 0)
        //        Command.Parameters.Add(new SqlParameter("@GST_Amount", SqlDbType.Decimal)).Value = GST_Amount;
        //    else
        //        Command.Parameters.Add(new SqlParameter("@GST_Amount", SqlDbType.Decimal)).Value = DBNull.Value;

        //    if (Discount_Percentage > 0)
        //        Command.Parameters.Add(new SqlParameter("@Discount_Percentage", SqlDbType.Decimal)).Value = Discount_Percentage;
        //    else
        //        Command.Parameters.Add(new SqlParameter("@Discount_Percentage", SqlDbType.Decimal)).Value = DBNull.Value;

        //    if (Discount_Amount > 0)
        //        Command.Parameters.Add(new SqlParameter("@Discount_Amount", SqlDbType.Decimal)).Value = Discount_Amount;
        //    else
        //        Command.Parameters.Add(new SqlParameter("@Discount_Amount", SqlDbType.Decimal)).Value = DBNull.Value;

        //    if (Item_Note != string.Empty && Item_Note != null)
        //        Command.Parameters.Add(new SqlParameter("@Note", SqlDbType.VarChar)).Value = Item_Note;
        //    else
        //        Command.Parameters.Add(new SqlParameter("@Note", SqlDbType.VarChar)).Value = DBNull.Value;

        //    if (User_ID > 0)
        //        Command.Parameters.Add(new SqlParameter("@User_ID", SqlDbType.Int)).Value = User_ID;
        //    else
        //        Command.Parameters.Add(new SqlParameter("@User_ID", SqlDbType.Int)).Value = DBNull.Value;

        //    if (Sp_Operation != string.Empty && Sp_Operation != null)
        //        Command.Parameters.Add(new SqlParameter("@Sp_Operation", SqlDbType.VarChar)).Value = Sp_Operation;
        //    else
        //        Command.Parameters.Add(new SqlParameter("@Sp_Operation", SqlDbType.VarChar)).Value = DBNull.Value;

        //}
        //public DataTable Getpurchaseorderproduct()
        //{
        //    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString))
        //    {
        //        cmd = new SqlCommand("PurchaseOrderProductManagement", con);
        //        AddwithParameter(cmd);
        //        DataTable DtUser = new DataTable();

        //        cmd.CommandType = CommandType.StoredProcedure;

        //        sda = new SqlDataAdapter(cmd);
        //        con.Open();
        //        sda.Fill(DtUser);

        //        return DtUser;
        //    }
        //}

        //#endregion
    }
}