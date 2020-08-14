using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using GST_Billing.Models;
using System.Threading;

namespace GST_Billing.Controllers
{
    [Authorize]
    public class QuotationController : Controller
    {
        public ActionResult Quotation()
        {
            ViewBag.Title = "Quotation";

            QuotationManagement objQuote = new QuotationManagement();

            ClientManagement objclient = new ClientManagement();
            objclient.Sp_Operation = "select_for_dropdown";

            DataTable dt = new DataTable();

            dt = objclient.GetClient();
            objQuote.ClientList = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objQuote.ClientList.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Company_Name"].ToString() });
            }

            ItemManagement objItem = new ItemManagement();
            objItem.Sp_Operation = "select";
            dt = new DataTable();
            dt = objItem.Getitems();

            objQuote.ItemList = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objQuote.ItemList.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Item_Name"].ToString() });
            }

            return View(objQuote);
        }
        public ActionResult AddQuotation()
        {
            ViewBag.Title = "Quotation";

            QuotationManagement objQuote = new QuotationManagement();

            DataTable dt = new DataTable();

            if (Request.QueryString["Quote_Type"] != null)
            {
                objQuote.Quote_Type = Request.QueryString["Quote_Type"];


                if (Request.QueryString["Quotationid"] != null)
                {
                    objQuote.Sp_Operation = "select_Quotation";
                    objQuote.Id = Convert.ToInt32(Request.QueryString["Quotationid"]);
                    dt = new DataTable();
                    dt = objQuote.Getquotation();

                    List<QuotationManagement> quotationList = new List<QuotationManagement>();
                    quotationList = CommonMethod.ConvertToList<QuotationManagement>(dt);
                    objQuote = quotationList[0];

                    objQuote.Sp_Operation = "select_Quote_Action";
                    objQuote.Id = Convert.ToInt32(Request.QueryString["Quotationid"]);
                    dt = new DataTable();
                    dt = objQuote.Getquotation();

                    if (dt.Rows.Count > 0)
                    {
                        if (Convert.ToString(dt.Rows[0]["ShipAddress"]) != "")
                        {
                            objQuote.ShipAddressAction = Convert.ToBoolean(dt.Rows[0]["ShipAddress"]);
                        }

                        if (Convert.ToString(dt.Rows[0]["ClientNote"]) != "")
                        {
                            objQuote.ClientNoteAction = Convert.ToBoolean(dt.Rows[0]["ClientNote"]);
                        }
                        if (Convert.ToString(dt.Rows[0]["Transport"]) != "")
                        {
                            objQuote.TransportAction = Convert.ToBoolean(dt.Rows[0]["Transport"]);
                        }
                        if (Convert.ToString(dt.Rows[0]["TermsCondition"]) != "")
                        {
                            objQuote.TermsConditionAction = Convert.ToBoolean(dt.Rows[0]["TermsCondition"]);
                        }
                    }
                }
                else
                {
                    objQuote.Sp_Operation = "get_last_po_no";
                    dt = new DataTable();
                    dt = objQuote.Getquotation();

                    if (dt.Rows.Count > 0)
                    {
                        objQuote.Quotation_No = Convert.ToString(dt.Rows[0]["Quotation_No"]);
                    }

                    objQuote.ShipAddressAction = true;
                    objQuote.BillAddressAction = true;
                    objQuote.ClientNoteAction = true;
                    objQuote.TransportAction = true;
                    objQuote.TermsConditionAction = true;
                }

                ClientManagement objClient = new ClientManagement();
                objClient.Sp_Operation = "select_for_dropdown";
                dt = new DataTable();
                dt = objClient.GetClient();

                objQuote.ClientList = new List<SelectListItem>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objQuote.ClientList.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Company_Name"].ToString() });
                }

                HttpContext.Cache.Remove("ItemList");

                return View(objQuote);
            }
            else
            {
                return Redirect("Quotation");
            }
        }
        [HttpPost]
        public ActionResult AddQuotation(QuotationManagement ObjQuote, string Gst_Type, string Quote_Type)
        {

            DataTable dt = new DataTable();
            //Add Columns
            dt.Columns.Add("Quotation_Product_Id");
            dt.Columns.Add("Item_Id");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("UOM_Unit");
            dt.Columns.Add("Tax");
            dt.Columns.Add("Discount_Percentage");
            dt.Columns.Add("Total");
            dt.Columns.Add("Item_Note");
            dt.Columns.Add("HSN_SAC");
            //Add rows

            for (int i = 0; i < ObjQuote.Item_Id.Count(); i++)
            {
                if (Convert.ToString(ObjQuote.Item_Id[i]) != "")
                {
                    dt.Rows.Add(ObjQuote.Quotation_Product_Id[i], ObjQuote.Item_Id[i], ObjQuote.Rate[i], ObjQuote.Quantity[i], ObjQuote.UOM_Unit[i],
                        ObjQuote.Tax[i], ObjQuote.Discount_Percentage[i], ObjQuote.Total[i], ObjQuote.Item_Note[i], ObjQuote.HSN_SAC[i]);
                }
            }

            ObjQuote.dtItems = dt;

            ObjQuote.User_ID = (User as GST_Billing.Models.MyPrincipal).User.User_ID;
            ObjQuote.Gst_Type = Gst_Type;
            ObjQuote.Quote_Type = Quote_Type;

            if (ObjQuote.Id == 0)
            {
                ObjQuote.Sp_Operation = "insert";
            }
            else
            {
                ObjQuote.Sp_Operation = "update";
            }

            ObjQuote.Getquotation();

            TempData["Success"] = "Success";
            return Redirect("AddQuotation");

        }
        public JsonResult IsQuoteNoExist(string Quotation_No, int Id)
        {
            bool isExist = true;

            QuotationManagement objQuote = new QuotationManagement();
            objQuote.Id = Id;
            objQuote.Quotation_No = Quotation_No;
            objQuote.Sp_Operation = "check_po_no";
            DataTable dt = new DataTable();
            dt = objQuote.Getquotation();

            if (dt.Rows.Count > 0)
            {
                isExist = true;
            }
            else
            {
                isExist = false;
            }

            return Json(!isExist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemDetails(int Item_Id)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)HttpContext.Cache["ItemDetails"];

            DataTable dt1 = new DataTable();
            DataRow[] dr = dt.Select("Id=" + Item_Id.ToString());
            dt1 = dr.CopyToDataTable();

            List<ItemManagement> itemsList = new List<ItemManagement>();
            itemsList = CommonMethod.ConvertToList<ItemManagement>(dt1);

            return Json(itemsList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult QuotationItemList(QuotationProductManagement objProduct, string action_type)
        {
            if (HttpContext.Cache["ItemList"] == null)
            {
                ItemManagement objItem = new ItemManagement();
                objItem.Sp_Operation = "select";
                DataTable dt = new DataTable();
                dt = objItem.Getitems();

                //dt =(DataTable)HttpContext.Cache["ItemList"];
                objProduct.ItemList = new List<SelectListItem>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objProduct.ItemList.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Item_Name"].ToString() });
                }

                HttpContext.Cache["ItemList"] = objProduct.ItemList;
                HttpContext.Cache["ItemDetails"] = dt;

                ItemTaxesManagement objTax = new ItemTaxesManagement();
                objTax.Sp_Operation = "Select";
                dt = new DataTable();
                dt = objTax.GetTaxes();

                objProduct.TaxList = new List<SelectListItem>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objProduct.TaxList.Add(new SelectListItem() { Value = Convert.ToDouble(dt.Rows[i]["GST"]).ToString(), Text = dt.Rows[i]["Tax_Name"].ToString() });
                }

                HttpContext.Cache["TaxList"] = objProduct.TaxList;

                ItemUnitManagement objItemUnit = new ItemUnitManagement();
                objItemUnit.Sp_Operation = "select";
                dt = new DataTable();
                dt = objItemUnit.GetItemUnit();

                objProduct.ItemUnitList = new List<SelectListItem>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objProduct.ItemUnitList.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Unit_Name"].ToString() });
                }

                HttpContext.Cache["ItemUnitList"] = objProduct.ItemUnitList;
            }


            ViewBag.ActionType = action_type;
            return View(objProduct);
        }
        public ActionResult GetItemsForUpdate()
        {
            QuotationProductManagement objProduct = new QuotationProductManagement();

            if (Request.QueryString["Quotationid"] != null)
            {
                QuotationManagement objQuote = new QuotationManagement();
                objQuote.Id = Convert.ToInt32(Request.QueryString["Quotationid"]);
                objQuote.Sp_Operation = "select_Quote_Items";
                objProduct.dt = objQuote.Getquotation();
            }

            return View(objProduct);
        }
        public ActionResult QuotationDataTable()
        {
            ViewBag.Title = "Quotation";
            QuotationManagement objQuote = new QuotationManagement();

            objQuote.Sp_Operation = "select_report_Pagination";
            objQuote.dt1 = objQuote.Getquotation();

            objQuote.Sp_Operation = "select_report";
            objQuote.dt = objQuote.Getquotation();

            objQuote.Page_No = 1;

            return View(objQuote);
        }
        [HttpPost]
        public ActionResult QuotationDataTable(QuotationManagement objQuote)
        {
            ViewBag.Title = "Quotation";

            objQuote.Sp_Operation = "select_report_Pagination";
            objQuote.dt1 = objQuote.Getquotation();

            objQuote.Sp_Operation = "select_report";
            objQuote.dt = objQuote.Getquotation();

            if (objQuote.Page_No == 0)
                objQuote.Page_No = 1;

            Thread.Sleep(1000);

            return View(objQuote);
        }
        public ActionResult Quotationview()
        {
            ViewBag.Title = "Quotation";

            QuotationManagement objQuote = new QuotationManagement();

            ClientManagement objClient = new ClientManagement();
            objClient.Sp_Operation = "select_for_dropdown";
            DataTable dt = new DataTable();
            dt = objClient.GetClient();
            objQuote.ClientList = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objQuote.ClientList.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Company_Name"].ToString() });
            }

            ItemManagement objItem = new ItemManagement();
            objItem.Sp_Operation = "select";
            dt = new DataTable();
            dt = objItem.Getitems();

            objQuote.ItemList = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objQuote.ItemList.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Item_Name"].ToString() });
            }

            return View(objQuote);
        }
        public ActionResult DeleteQuotation()
        {
            QuotationManagement objQuote = new QuotationManagement();
            objQuote.Id = Convert.ToInt32(Request.QueryString["Quotationid"]);
            objQuote.Sp_Operation = "delete_Quote";
            objQuote.Getquotation();

            return Redirect("Quotation");
        }
        public ActionResult ShortDataTable()
        {
            ViewBag.Title = "objQuote";
            QuotationManagement objQuote = new QuotationManagement();

            objQuote.Id = Convert.ToInt32(Request.QueryString["Quotationid"]);
            objQuote.Sp_Operation = "select_report_for_short";
            objQuote.dt = objQuote.Getquotation();


            return View(objQuote);
        }
        [HttpPost]
        public ActionResult ShortDataTable(QuotationManagement objQuote)
        {
            ViewBag.Title = "Quotation";

            objQuote.Sp_Operation = "select_report_for_short";
            objQuote.dt = objQuote.Getquotation();

            Thread.Sleep(1000);

            return View(objQuote);
        }
        public JsonResult QuotationStatusUpdate(int id, string status)
        {
            QuotationManagement objQuote = new QuotationManagement();
            objQuote.User_ID = (User as GST_Billing.Models.MyPrincipal).User.User_ID;
            objQuote.Id = id;
            objQuote.Status = status;
            objQuote.Sp_Operation = "update_status";
            objQuote.dt = objQuote.Getquotation();

            return Json("Status update successfully...", JsonRequestBehavior.AllowGet);
        }
        public JsonResult Getquotationstatus(int id)
        {
            QuotationManagement objQuote = new QuotationManagement();
            objQuote.Sp_Operation = "select_status";
            objQuote.Id = id;
            DataTable dt = new DataTable();
            dt = objQuote.Getquotation();

            List<QuotationManagement> quotation = new List<QuotationManagement>();
            quotation = CommonMethod.ConvertToList<QuotationManagement>(dt);
            objQuote = quotation[0];

            return Json(objQuote, JsonRequestBehavior.AllowGet);
        }
    }
}