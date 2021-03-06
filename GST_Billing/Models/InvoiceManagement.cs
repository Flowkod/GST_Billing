﻿using System;
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
    public class InvoiceManagement
    {
        #region "Variable"

        SqlDataAdapter sda;
        SqlCommand cmd;

        #endregion

        #region "Property"

        public int Id { get; set; }
        public int Initial_No { get; set; }
        [Required(ErrorMessage = "Invoice no required.")]
        [Remote("IsInvoiceNoExist", "Invoice", ErrorMessage = "Invoice no already exist", AdditionalFields = "Id")]
        public string Invoice_No { get; set; }
        public string Reference_No { get; set; }
        [Required(ErrorMessage = "Date required.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime? Date { get; set; }
        public DateTime? To_Date { get; set; }
        public DateTime? Due_From_Date { get; set; }
        public DateTime? Due_To_Date { get; set; }

        [Required(ErrorMessage = "Client required.")]
        public int Client_Id { get; set; }
        public string Company_Name { get; set; }
        public string Client_Address { get; set; }
        public string Client_GSTIN { get; set; }
        public string Client_Email { get; set; }
        public string Client_Tel { get; set; }
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
        public decimal Shipping_Amount { get; set; }
        public string Other_Plus_Text { get; set; }
        public decimal Other_Plus_Amount { get; set; }
        public string Other_Minus_Text { get; set; }
        public decimal Other_Minus_Amount { get; set; }
        public decimal Round_Off { get; set; }
        public decimal Gross_Total { get; set; }
        public string Payment_Mode { get; set; }
        public string Due_Date { get; set; }
        public string Payment_Terms { get; set; }
        public string Transport { get; set; }
        public string Note { get; set; }
        public string Gst_Type { get; set; }
        public int Quotation_Id { get; set; }
        public int User_ID { get; set; }
        public int Invoice_Item_Id { get; set; }
        public string Status { get; set; }
        public int Page_No { get; set; }
        public string Created_On { get; set; }
        public string Modified_On { get; set; }
        public string Modified_By { get; set; }
        public string Sp_Operation { get; set; }

        //To get item list
        public int?[] Invoice_Product_Id { get; set; }
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

            if (Invoice_No != string.Empty && Invoice_No != null)
                Command.Parameters.Add(new SqlParameter("@Invoice_No", SqlDbType.VarChar)).Value = Invoice_No;
            else
                Command.Parameters.Add(new SqlParameter("@Invoice_No", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Reference_No != string.Empty && Reference_No != null)
                Command.Parameters.Add(new SqlParameter("@Reference_No", SqlDbType.VarChar)).Value = Reference_No;
            else
                Command.Parameters.Add(new SqlParameter("@Reference_No", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Date != DateTime.MinValue && Date != null)
                Command.Parameters.Add(new SqlParameter("@Date", SqlDbType.DateTime)).Value = Date;
            else
                Command.Parameters.Add(new SqlParameter("@Date", SqlDbType.DateTime)).Value = DBNull.Value;

            if (To_Date != DateTime.MinValue && To_Date != null)
                Command.Parameters.Add(new SqlParameter("@To_Date", SqlDbType.DateTime)).Value = To_Date;
            else
                Command.Parameters.Add(new SqlParameter("@To_Date", SqlDbType.DateTime)).Value = DBNull.Value;

            if (Due_From_Date != DateTime.MinValue && Due_From_Date != null)
                Command.Parameters.Add(new SqlParameter("@Due_From_Date", SqlDbType.DateTime)).Value = Due_From_Date;
            else
                Command.Parameters.Add(new SqlParameter("@Due_From_Date", SqlDbType.DateTime)).Value = DBNull.Value;

            if (Due_To_Date != DateTime.MinValue && Due_To_Date != null)
                Command.Parameters.Add(new SqlParameter("@Due_To_Date", SqlDbType.DateTime)).Value = Due_To_Date;
            else
                Command.Parameters.Add(new SqlParameter("@Due_To_Date", SqlDbType.DateTime)).Value = DBNull.Value;

            if (Client_Id > 0)
                Command.Parameters.Add(new SqlParameter("@Client_Id", SqlDbType.Int)).Value = Client_Id;
            else
                Command.Parameters.Add(new SqlParameter("@Client_Id", SqlDbType.Int)).Value = DBNull.Value;

            if (Company_Name != string.Empty && Company_Name != null)
                Command.Parameters.Add(new SqlParameter("@Company_Name", SqlDbType.VarChar)).Value = Company_Name;
            else
                Command.Parameters.Add(new SqlParameter("@Company_Name", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Client_Address != string.Empty && Client_Address != null)
                Command.Parameters.Add(new SqlParameter("@Client_Address", SqlDbType.VarChar)).Value = Client_Address;
            else
                Command.Parameters.Add(new SqlParameter("@Client_Address", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Client_GSTIN != string.Empty && Client_GSTIN != null)
                Command.Parameters.Add(new SqlParameter("@Client_GSTIN", SqlDbType.VarChar)).Value = Client_GSTIN;
            else
                Command.Parameters.Add(new SqlParameter("@Client_GSTIN", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Client_Email != string.Empty && Client_Email != null)
                Command.Parameters.Add(new SqlParameter("@Client_Email", SqlDbType.VarChar)).Value = Client_Email;
            else
                Command.Parameters.Add(new SqlParameter("@Client_Email", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Client_Tel != string.Empty && Client_Tel != null)
                Command.Parameters.Add(new SqlParameter("@Client_Tel", SqlDbType.VarChar)).Value = Client_Tel;
            else
                Command.Parameters.Add(new SqlParameter("@Client_Tel", SqlDbType.VarChar)).Value = DBNull.Value;

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

            if (Shipping_Amount > 0)
                Command.Parameters.Add(new SqlParameter("@Shipping_Amount", SqlDbType.Decimal)).Value = Shipping_Amount;
            else
                Command.Parameters.Add(new SqlParameter("@Shipping_Amount", SqlDbType.Decimal)).Value = DBNull.Value;

            if (Other_Plus_Text != string.Empty && Other_Plus_Text != null)
                Command.Parameters.Add(new SqlParameter("@Other_Plus_Text", SqlDbType.VarChar)).Value = Other_Plus_Text;
            else
                Command.Parameters.Add(new SqlParameter("@Other_Plus_Text", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Other_Plus_Amount > 0)
                Command.Parameters.Add(new SqlParameter("@Other_Plus_Amount", SqlDbType.Decimal)).Value = Other_Plus_Amount;
            else
                Command.Parameters.Add(new SqlParameter("@Other_Plus_Amount", SqlDbType.Decimal)).Value = DBNull.Value;

            if (Other_Minus_Text != string.Empty && Other_Minus_Text != null)
                Command.Parameters.Add(new SqlParameter("@Other_Minus_Text", SqlDbType.VarChar)).Value = Other_Minus_Text;
            else
                Command.Parameters.Add(new SqlParameter("@Other_Minus_Text", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Other_Minus_Amount > 0)
                Command.Parameters.Add(new SqlParameter("@Other_Minus_Amount", SqlDbType.Decimal)).Value = Other_Minus_Amount;
            else
                Command.Parameters.Add(new SqlParameter("@Other_Minus_Amount", SqlDbType.Decimal)).Value = DBNull.Value;

            if (Round_Off > 0)
                Command.Parameters.Add(new SqlParameter("@Round_Off", SqlDbType.Decimal)).Value = Round_Off;
            else
                Command.Parameters.Add(new SqlParameter("@Round_Off", SqlDbType.Decimal)).Value = DBNull.Value;

            if (Gross_Total > 0)
                Command.Parameters.Add(new SqlParameter("@Gross_Total", SqlDbType.Decimal)).Value = Gross_Total;
            else
                Command.Parameters.Add(new SqlParameter("@Gross_Total", SqlDbType.Decimal)).Value = DBNull.Value;

            if (Payment_Mode != string.Empty && Payment_Mode != null)
                Command.Parameters.Add(new SqlParameter("@Payment_Mode", SqlDbType.VarChar)).Value = Payment_Mode;
            else
                Command.Parameters.Add(new SqlParameter("@Payment_Mode", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Due_Date != string.Empty && Due_Date != null)
                Command.Parameters.Add(new SqlParameter("@Due_Date", SqlDbType.Date)).Value = Due_Date;
            else
                Command.Parameters.Add(new SqlParameter("@Due_Date", SqlDbType.Date)).Value = DBNull.Value;

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

            if (Quotation_Id > 0)
                Command.Parameters.Add(new SqlParameter("@Quotation_Id", SqlDbType.Int)).Value = Quotation_Id;
            else
                Command.Parameters.Add(new SqlParameter("@Quotation_Id", SqlDbType.Int)).Value = DBNull.Value;

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

            if (Invoice_Item_Id > 0)
                Command.Parameters.Add(new SqlParameter("@Item_Id", SqlDbType.Int)).Value = Invoice_Item_Id;
            else
                Command.Parameters.Add(new SqlParameter("@Item_Id", SqlDbType.Int)).Value = DBNull.Value;

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
        public DataTable GetInvoice()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString))
            {
                cmd = new SqlCommand("InvoiceManagement", con);
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