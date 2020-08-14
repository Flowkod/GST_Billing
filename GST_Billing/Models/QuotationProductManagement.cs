using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GST_Billing.Models
{
    public class QuotationProductManagement
    {
        #region "Property"

        public int Quotation_Product_Id { get; set; }
        public int Quotation_Id { get; set; }
        public int? Item_Id { get; set; }
        public string Item_Name { get; set; }
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
    }
}