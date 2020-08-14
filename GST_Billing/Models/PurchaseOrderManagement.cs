using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

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
        public int Initial_No { get; set; }
        [Required(ErrorMessage = "Po no required..")]
        [Remote("IsPoNoExist", "Purchase", ErrorMessage = "Po no already exist", AdditionalFields = "Id")]
        public string PO_No { get; set; }
        [Required(ErrorMessage = "Date required..")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime? Date { get; set; }
        public DateTime? To_Date { get; set; }
        public DateTime? Valid_From_Date { get; set; }
        public DateTime? Valid_To_Date { get; set; }

        [Required(ErrorMessage = "Supplier required..")]
        public int Supplier_Id { get; set; }
        public string Supplier_Name { get; set; }
        public string Supplier_Address { get; set; }
        public string Supplier_GSTIN { get; set; }
        public string Supplier_Tel { get; set; }
        public string Ship_To { get; set; }
        public int? Ship_To_Client_Id { get; set; }
        public int? Client_Ship_Address_Id { get; set; }
        public string Ship_Address { get; set; }
        public string Ship_GSTIN { get; set; }
        public string Ship_Tel { get; set; }
        public string Bill_To { get; set; }
        public int? Bill_To_Client_Id { get; set; }
        public string Bill_Address { get; set; }
        public string Bill_GSTIN { get; set; }
        public string Bill_Tel { get; set; }
        public decimal Net_Total { get; set; }
        public decimal GST_Amount { get; set; }
        public decimal Round_Off { get; set; }
        public decimal Gross_Total { get; set; }
        public string Delivery_Period { get; set; }
        public string Payment_Terms { get; set; }
        public string Transport { get; set; }
        public string Note { get; set; }
        public string Gst_Type { get; set; }
        public int User_ID { get; set; }
        public int PO_Item_Id { get; set; }
        public string Status { get; set; }
        public int Page_No { get; set; }
        public string Created_On { get; set; }
        public string Modified_On { get; set; }
        public string Modified_By { get; set; }
        public string Sp_Operation { get; set; }

        //To get item list
        public int?[] PurchaseOrder_Product_Id { get; set; }
        public int?[] Item_Id { get; set; }
        public string[] HSN_SAC { get; set; }
        public string[] Batch_No { get; set; }
        public decimal?[] Rate { get; set; }
        public decimal?[] Quantity { get; set; }
        public int?[] UOM_Unit { get; set; }
        public double?[] Tax { get; set; }
        public double?[] Discount_Percentage { get; set; }
        public decimal?[] Total { get; set; }
        public string[] Item_Note { get; set; }
        //-----------TO Po Action
        public bool ShipAddressAction { get; set; }
        public bool BillAddressAction { get; set; }
        public bool PaymentTermsAction { get; set; }
        public bool ClientNoteAction { get; set; }
        public bool TransportAction { get; set; }
        public bool TermsConditionAction { get; set; }

        public List<SelectListItem> SupplierList { get; set; }
        public List<SelectListItem> ClientList { get; set; }
        public List<SelectListItem> ItemList { get; set; }
        public DataTable dtItems { get; set; }

        public DataTable dt { get; set; }
        public DataTable dt1 { get; set; }

        #endregion

        #region "Function"
        public void AddwithParameter(SqlCommand Command)
        {
            if (Id > 0)
                Command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = Id;
            else
                Command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = DBNull.Value;

            if (Initial_No > 0)
                Command.Parameters.Add(new SqlParameter("@Initial_No", SqlDbType.VarChar)).Value = Initial_No;
            else
                Command.Parameters.Add(new SqlParameter("@Initial_No", SqlDbType.VarChar)).Value = DBNull.Value;

            if (PO_No != string.Empty && PO_No != null)
                Command.Parameters.Add(new SqlParameter("@PO_No", SqlDbType.VarChar)).Value = PO_No;
            else
                Command.Parameters.Add(new SqlParameter("@PO_No", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Date != DateTime.MinValue && Date != null)
                Command.Parameters.Add(new SqlParameter("@Date", SqlDbType.DateTime)).Value = Date;
            else
                Command.Parameters.Add(new SqlParameter("@Date", SqlDbType.DateTime)).Value = DBNull.Value;

            if (To_Date != DateTime.MinValue && To_Date != null)
                Command.Parameters.Add(new SqlParameter("@To_Date", SqlDbType.DateTime)).Value = To_Date;
            else
                Command.Parameters.Add(new SqlParameter("@To_Date", SqlDbType.DateTime)).Value = DBNull.Value;

            if (Valid_From_Date != DateTime.MinValue && Valid_From_Date != null)
                Command.Parameters.Add(new SqlParameter("@Valid_From_Date", SqlDbType.DateTime)).Value = Valid_From_Date;
            else
                Command.Parameters.Add(new SqlParameter("@Valid_From_Date", SqlDbType.DateTime)).Value = DBNull.Value;

            if (Valid_To_Date != DateTime.MinValue && Valid_To_Date != null)
                Command.Parameters.Add(new SqlParameter("@Valid_To_Date", SqlDbType.DateTime)).Value = Valid_To_Date;
            else
                Command.Parameters.Add(new SqlParameter("@Valid_To_Date", SqlDbType.DateTime)).Value = DBNull.Value;

            if (Supplier_Id > 0)
                Command.Parameters.Add(new SqlParameter("@Supplier_Id", SqlDbType.Int)).Value = Supplier_Id;
            else
                Command.Parameters.Add(new SqlParameter("@Supplier_Id", SqlDbType.Int)).Value = DBNull.Value;

            if (Supplier_Name != string.Empty && Supplier_Name != null)
                Command.Parameters.Add(new SqlParameter("@Supplier_Name", SqlDbType.VarChar)).Value = Supplier_Name;
            else
                Command.Parameters.Add(new SqlParameter("@Supplier_Name", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Supplier_Address != string.Empty && Supplier_Address != null)
                Command.Parameters.Add(new SqlParameter("@Supplier_Address", SqlDbType.VarChar)).Value = Supplier_Address;
            else
                Command.Parameters.Add(new SqlParameter("@Supplier_Address", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Supplier_GSTIN != string.Empty && Supplier_GSTIN != null)
                Command.Parameters.Add(new SqlParameter("@Supplier_GSTIN", SqlDbType.VarChar)).Value = Supplier_GSTIN;
            else
                Command.Parameters.Add(new SqlParameter("@Supplier_GSTIN", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Supplier_Tel != string.Empty && Supplier_Tel != null)
                Command.Parameters.Add(new SqlParameter("@Supplier_Tel", SqlDbType.VarChar)).Value = Supplier_Tel;
            else
                Command.Parameters.Add(new SqlParameter("@Supplier_Tel", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Ship_To != string.Empty && Ship_To != null)
                Command.Parameters.Add(new SqlParameter("@Ship_To", SqlDbType.VarChar)).Value = Ship_To;
            else
                Command.Parameters.Add(new SqlParameter("@Ship_To", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Client_Ship_Address_Id > 0)
                Command.Parameters.Add(new SqlParameter("@Client_Ship_Address_Id", SqlDbType.Int)).Value = Client_Ship_Address_Id;
            else
                Command.Parameters.Add(new SqlParameter("@Client_Ship_Address_Id", SqlDbType.Int)).Value = DBNull.Value;

            if (Ship_To_Client_Id > 0)
                Command.Parameters.Add(new SqlParameter("@Ship_To_Client_Id", SqlDbType.Int)).Value = Ship_To_Client_Id;
            else
                Command.Parameters.Add(new SqlParameter("@Ship_To_Client_Id", SqlDbType.Int)).Value = DBNull.Value;

            if (Ship_Address != string.Empty && Ship_Address != null)
                Command.Parameters.Add(new SqlParameter("@Ship_Address", SqlDbType.VarChar)).Value = Ship_Address;
            else
                Command.Parameters.Add(new SqlParameter("@Ship_Address", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Ship_GSTIN != string.Empty && Ship_GSTIN != null)
                Command.Parameters.Add(new SqlParameter("@Ship_GSTIN", SqlDbType.VarChar)).Value = Ship_GSTIN;
            else
                Command.Parameters.Add(new SqlParameter("@Ship_GSTIN", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Ship_Tel != string.Empty && Ship_Tel != null)
                Command.Parameters.Add(new SqlParameter("@Ship_Tel", SqlDbType.VarChar)).Value = Ship_Tel;
            else
                Command.Parameters.Add(new SqlParameter("@Ship_Tel", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Bill_To != string.Empty && Bill_To != null)
                Command.Parameters.Add(new SqlParameter("@Bill_To", SqlDbType.VarChar)).Value = Bill_To;
            else
                Command.Parameters.Add(new SqlParameter("@Bill_To", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Bill_To_Client_Id > 0)
                Command.Parameters.Add(new SqlParameter("@Bill_To_Client_Id", SqlDbType.Int)).Value = Bill_To_Client_Id;
            else
                Command.Parameters.Add(new SqlParameter("@Bill_To_Client_Id", SqlDbType.Int)).Value = DBNull.Value;

            if (Bill_Address != string.Empty && Bill_Address != null)
                Command.Parameters.Add(new SqlParameter("@Bill_Address", SqlDbType.VarChar)).Value = Bill_Address;
            else
                Command.Parameters.Add(new SqlParameter("@Bill_Address", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Bill_GSTIN != string.Empty && Bill_GSTIN != null)
                Command.Parameters.Add(new SqlParameter("@Bill_GSTIN", SqlDbType.VarChar)).Value = Bill_GSTIN;
            else
                Command.Parameters.Add(new SqlParameter("@Bill_GSTIN", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Bill_Tel != string.Empty && Bill_Tel != null)
                Command.Parameters.Add(new SqlParameter("@Bill_Tel", SqlDbType.VarChar)).Value = Bill_Tel;
            else
                Command.Parameters.Add(new SqlParameter("@Bill_Tel", SqlDbType.VarChar)).Value = DBNull.Value;

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

            if (Status != string.Empty && Status != null)
                Command.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar)).Value = Status;
            else
                Command.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar)).Value = DBNull.Value;

            if (ShipAddressAction != false)
                Command.Parameters.Add(new SqlParameter("@ShipAddressAction", SqlDbType.Bit)).Value = ShipAddressAction;
            else
                Command.Parameters.Add(new SqlParameter("@ShipAddressAction", SqlDbType.Bit)).Value = false;

            if (BillAddressAction != false)
                Command.Parameters.Add(new SqlParameter("@BillAddressAction", SqlDbType.Bit)).Value = BillAddressAction;
            else
                Command.Parameters.Add(new SqlParameter("@BillAddressAction", SqlDbType.Bit)).Value = false;

            if (PaymentTermsAction != false)
                Command.Parameters.Add(new SqlParameter("@PaymentTermsAction", SqlDbType.Bit)).Value = PaymentTermsAction;
            else
                Command.Parameters.Add(new SqlParameter("@PaymentTermsAction", SqlDbType.Bit)).Value = false;

            if (ClientNoteAction != false)
                Command.Parameters.Add(new SqlParameter("@ClientNoteAction", SqlDbType.Bit)).Value = ClientNoteAction;
            else
                Command.Parameters.Add(new SqlParameter("@ClientNoteAction", SqlDbType.Bit)).Value = false;

            if (TransportAction != false)
                Command.Parameters.Add(new SqlParameter("@TransportAction", SqlDbType.Bit)).Value = TransportAction;
            else
                Command.Parameters.Add(new SqlParameter("@TransportAction", SqlDbType.Bit)).Value = false;

            if (TermsConditionAction != false)
                Command.Parameters.Add(new SqlParameter("@TermsConditionAction", SqlDbType.Bit)).Value = TermsConditionAction;
            else
                Command.Parameters.Add(new SqlParameter("@TermsConditionAction", SqlDbType.Bit)).Value = false;

            if (dtItems != null)
                Command.Parameters.Add(new SqlParameter("@ProductsDetails", SqlDbType.Structured)).Value = dtItems;
            else
                Command.Parameters.Add(new SqlParameter("@ProductsDetails", SqlDbType.Structured)).Value = null;

            if (PO_Item_Id > 0)
                Command.Parameters.Add(new SqlParameter("@Item_Id", SqlDbType.Int)).Value = PO_Item_Id;
            else
                Command.Parameters.Add(new SqlParameter("@Item_Id", SqlDbType.Int)).Value = DBNull.Value;

            if (User_ID > 0)
                Command.Parameters.Add(new SqlParameter("@User_ID", SqlDbType.Int)).Value = User_ID;
            else
                Command.Parameters.Add(new SqlParameter("@User_ID", SqlDbType.Int)).Value = DBNull.Value;

            if (Page_No > 0)
                Command.Parameters.Add(new SqlParameter("@Page_No", SqlDbType.Int)).Value = Page_No;
            else
                Command.Parameters.Add(new SqlParameter("@Page_No", SqlDbType.Int)).Value =1;

            if (Sp_Operation != string.Empty && Sp_Operation != null)
                Command.Parameters.Add(new SqlParameter("@Sp_Operation", SqlDbType.VarChar)).Value = Sp_Operation;
            else
                Command.Parameters.Add(new SqlParameter("@Sp_Operation", SqlDbType.VarChar)).Value = DBNull.Value;

        }
        public DataTable Getpurchaseorder()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString))
            {
                cmd = new SqlCommand("PurchaseOrderManagement", con);
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