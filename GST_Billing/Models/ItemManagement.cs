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
    public class ItemManagement
    {
        #region "Variable"

        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString);
        SqlDataAdapter sda;
        SqlCommand cmd;

        #endregion

        #region "Property"
        public int Id { get; set; }
        public int Item_Type_Id { get; set; }
        [Required(ErrorMessage = "Item Code required")]
        [Remote("IsItemCodeExist", "Master", ErrorMessage = "Item code already exist", AdditionalFields = "Id")]
        public string Item_Code { get; set; }
        [Required(ErrorMessage = "Item Name required")]
        public string Item_Name { get; set; }
        public string Item_Short_Name { get; set; }
        public string Description { get; set; }
        public string Item_Group { get; set; }
        public int? UOM_Qty { get; set; }
        public int? UOM_Unit { get; set; }
        public string Unit_Name { get; set; }
        public decimal? Tax { get; set; }
        public string HSN_SAC { get; set; }
        public decimal? Purchase_Price { get; set; }
        public decimal? Sale_Price { get; set; }
        public decimal? MRP { get; set; }
        public decimal? To_MRP { get; set; }
        public int GST { get; set; }
        public decimal? Discount { get; set; }
        public decimal? To_Discount { get; set; }
        public decimal? Gross_Weight { get; set; }
        public decimal? Net_Weight { get; set; }
        public int? Minimum_Stock { get; set; }
        public int User_ID { set; get; }
        public string Sp_Operation { get; set; }
        public int Page_No { get; set; }
        public DataTable dt { get; set; }
        public DataTable dt1 { get; set; }
        public List<SelectListItem> Item_Units_List { get; set; }
        public List<SelectListItem> Tax_List { get; set; }
        #endregion

        #region "Function"
        public void AddwithParameter(SqlCommand Command)
        {
            if (Id > 0)
                Command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = Id;
            else
                Command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = DBNull.Value;

            if (Item_Type_Id > 0)
                Command.Parameters.Add(new SqlParameter("@items_type_id", SqlDbType.Int)).Value = Item_Type_Id;
            else
                Command.Parameters.Add(new SqlParameter("@items_type_id", SqlDbType.Int)).Value = DBNull.Value;
            if (Item_Code != string.Empty && Item_Code != null)
                Command.Parameters.Add(new SqlParameter("@Item_Code", SqlDbType.VarChar)).Value = Item_Code;
            else
                Command.Parameters.Add(new SqlParameter("@Item_Code", SqlDbType.VarChar)).Value = DBNull.Value;
            if (Item_Name != string.Empty && Item_Name != null)
                Command.Parameters.Add(new SqlParameter("@Item_Name", SqlDbType.VarChar)).Value = Item_Name;
            else
                Command.Parameters.Add(new SqlParameter("@Item_Name", SqlDbType.VarChar)).Value = DBNull.Value;
            if (Item_Short_Name != string.Empty && Item_Short_Name != null)
                Command.Parameters.Add(new SqlParameter("@Item_Short_Name", SqlDbType.VarChar)).Value = Item_Short_Name;
            else
                Command.Parameters.Add(new SqlParameter("@Item_Short_Name", SqlDbType.VarChar)).Value = DBNull.Value;
            if (Description != string.Empty && Description != null)
                Command.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar)).Value = Description;
            else
                Command.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar)).Value = DBNull.Value;
            if (Item_Group != string.Empty && Item_Group != null)
                Command.Parameters.Add(new SqlParameter("@Item_Group", SqlDbType.VarChar)).Value = Item_Group;
            else
                Command.Parameters.Add(new SqlParameter("@Item_Group", SqlDbType.VarChar)).Value = DBNull.Value;
            if (UOM_Qty > 0)
                Command.Parameters.Add(new SqlParameter("@UOM_Qty", SqlDbType.Int)).Value = UOM_Qty;
            else
                Command.Parameters.Add(new SqlParameter("@UOM_Qty", SqlDbType.Int)).Value = DBNull.Value;
            if (UOM_Unit > 0)
                Command.Parameters.Add(new SqlParameter("@UOM_Unit", SqlDbType.Int)).Value = UOM_Unit;
            else
                Command.Parameters.Add(new SqlParameter("@UOM_Unit", SqlDbType.Int)).Value = DBNull.Value;
            if (Tax > 0)
                Command.Parameters.Add(new SqlParameter("@Tax", SqlDbType.Decimal)).Value = Tax;
            else
                Command.Parameters.Add(new SqlParameter("@Tax", SqlDbType.Decimal)).Value = DBNull.Value;
            if (HSN_SAC != string.Empty && HSN_SAC != null)
                Command.Parameters.Add(new SqlParameter("@HSN_SAC", SqlDbType.VarChar)).Value = HSN_SAC;
            else
                Command.Parameters.Add(new SqlParameter("@HSN_SAC", SqlDbType.VarChar)).Value = DBNull.Value;
            if (Purchase_Price > 0)
                Command.Parameters.Add(new SqlParameter("@Purchase_Price", SqlDbType.Decimal)).Value = Purchase_Price;
            else
                Command.Parameters.Add(new SqlParameter("@Purchase_Price", SqlDbType.Decimal)).Value = DBNull.Value;
            if (Sale_Price > 0)
                Command.Parameters.Add(new SqlParameter("@Sale_Price", SqlDbType.Decimal)).Value = Sale_Price;
            else
                Command.Parameters.Add(new SqlParameter("@Sale_Price", SqlDbType.Decimal)).Value = DBNull.Value;
            if (MRP > 0)
                Command.Parameters.Add(new SqlParameter("@MRP", SqlDbType.Decimal)).Value = MRP;
            else
                Command.Parameters.Add(new SqlParameter("@MRP", SqlDbType.Decimal)).Value = DBNull.Value;
            if (To_MRP > 0)
                Command.Parameters.Add(new SqlParameter("@To_MRP", SqlDbType.Decimal)).Value = To_MRP;
            else
                Command.Parameters.Add(new SqlParameter("@To_MRP", SqlDbType.Decimal)).Value = DBNull.Value;
            if (GST > 0)
                Command.Parameters.Add(new SqlParameter("@GST", SqlDbType.Int)).Value = GST;
            else
                Command.Parameters.Add(new SqlParameter("@GST", SqlDbType.Int)).Value = DBNull.Value;
            if (Discount > 0)
                Command.Parameters.Add(new SqlParameter("@Discount", SqlDbType.Decimal)).Value = Discount;
            else
                Command.Parameters.Add(new SqlParameter("@Discount", SqlDbType.Decimal)).Value = DBNull.Value;
            if (To_Discount > 0)
                Command.Parameters.Add(new SqlParameter("@To_Discount", SqlDbType.Decimal)).Value = To_Discount;
            else
                Command.Parameters.Add(new SqlParameter("@To_Discount", SqlDbType.Decimal)).Value = DBNull.Value;
            if (Gross_Weight > 0)
                Command.Parameters.Add(new SqlParameter("@Gross_Weight", SqlDbType.Decimal)).Value = Gross_Weight;
            else
                Command.Parameters.Add(new SqlParameter("@Gross_Weight", SqlDbType.Decimal)).Value = DBNull.Value;
            if (Net_Weight > 0)
                Command.Parameters.Add(new SqlParameter("@Net_Weight", SqlDbType.Decimal)).Value = Net_Weight;
            else
                Command.Parameters.Add(new SqlParameter("@Net_Weight", SqlDbType.Decimal)).Value = DBNull.Value;
            if (Minimum_Stock > 0)
                Command.Parameters.Add(new SqlParameter("@Minimum_Stock", SqlDbType.Int)).Value = Minimum_Stock;
            else
                Command.Parameters.Add(new SqlParameter("@Minimum_Stock", SqlDbType.Int)).Value = DBNull.Value;

            if (User_ID > 0)
                Command.Parameters.Add(new SqlParameter("@User_ID", SqlDbType.Int)).Value = User_ID;
            else
                Command.Parameters.Add(new SqlParameter("@User_ID", SqlDbType.Int)).Value = DBNull.Value;

            if (Page_No > 0)
                Command.Parameters.Add(new SqlParameter("@Page_No", SqlDbType.Int)).Value = Page_No;
            else
                Command.Parameters.Add(new SqlParameter("@Page_No", SqlDbType.Int)).Value = 1;

            if (Sp_Operation != string.Empty && Sp_Operation != null)
                Command.Parameters.Add(new SqlParameter("@Sp_Operation", SqlDbType.VarChar)).Value = Sp_Operation;
            else
                Command.Parameters.Add(new SqlParameter("@Sp_Operation", SqlDbType.VarChar)).Value = DBNull.Value;
        }
        public DataTable Getitems()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString))
            {
                cmd = new SqlCommand("ItemsManagement", con);
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