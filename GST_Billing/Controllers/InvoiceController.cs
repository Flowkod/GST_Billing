using GST_Billing.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace GST_Billing.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        // GET: Invoice
        public ActionResult Invoice()
        {
            ViewBag.Title = "Invoice";

            InvoiceManagement objInvoice = new InvoiceManagement();

            ClientManagement objClient = new ClientManagement();
            objClient.Sp_Operation = "select_for_dropdown";
            DataTable dt = new DataTable();
            dt = objClient.GetClient();

            objInvoice.ClientList = new List<SelectListItem>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objInvoice.ClientList.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Company_Name"].ToString() });
            }

            ItemManagement objItem = new ItemManagement();
            objItem.Sp_Operation = "select";
            dt = new DataTable();
            dt = objItem.Getitems();

            objInvoice.ItemList = new List<SelectListItem>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objInvoice.ItemList.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Item_Name"].ToString() });
            }

            return View(objInvoice);
        }

        public ActionResult AddInvoice()
        {
            ViewBag.Title = "Invoice";

            InvoiceManagement objInvoice = new InvoiceManagement();

            DataTable dt = new DataTable();

            if (Request.QueryString["Invoiceid"] != null)
            {
                objInvoice.Sp_Operation = "select_Invoice";
                objInvoice.Id = Convert.ToInt32(Request.QueryString["Invoiceid"]);
                dt = new DataTable();
                dt = objInvoice.GetInvoice();

                List<InvoiceManagement> Invoice = new List<InvoiceManagement>();
                Invoice = CommonMethod.ConvertToList<InvoiceManagement>(dt);
                objInvoice = Invoice[0];

                objInvoice.Sp_Operation = "select_Invoice_Preferences";
                objInvoice.Id = Convert.ToInt32(Request.QueryString["Invoiceid"]);
                dt = new DataTable();
                dt = objInvoice.GetInvoice();

                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(dt.Rows[0]["ShipAddress"]) != "")
                    {
                        objInvoice.ShipAddressAction = Convert.ToBoolean(dt.Rows[0]["ShipAddress"]);
                    }
                    if (Convert.ToString(dt.Rows[0]["BillAddress"]) != "")
                    {
                        objInvoice.BillAddressAction = Convert.ToBoolean(dt.Rows[0]["BillAddress"]);
                    }
                    if (Convert.ToString(dt.Rows[0]["PaymentTerms"]) != "")
                    {
                        objInvoice.PaymentTermsAction = Convert.ToBoolean(dt.Rows[0]["PaymentTerms"]);
                    }
                    if (Convert.ToString(dt.Rows[0]["ClientNote"]) != "")
                    {
                        objInvoice.ClientNoteAction = Convert.ToBoolean(dt.Rows[0]["ClientNote"]);
                    }
                    if (Convert.ToString(dt.Rows[0]["Transport"]) != "")
                    {
                        objInvoice.TransportAction = Convert.ToBoolean(dt.Rows[0]["Transport"]);
                    }
                    if (Convert.ToString(dt.Rows[0]["TermsCondition"]) != "")
                    {
                        objInvoice.TermsConditionAction = Convert.ToBoolean(dt.Rows[0]["TermsCondition"]);
                    }
                }
            }
            else
            {
                objInvoice.Sp_Operation = "get_last_invoice_no";
                dt = new DataTable();
                dt = objInvoice.GetInvoice();

                if (dt.Rows.Count > 0)
                {
                    objInvoice.Invoice_No= Convert.ToString(dt.Rows[0]["Invoice_No"]);
                }

                objInvoice.ShipAddressAction = true;
                objInvoice.BillAddressAction = true;
                objInvoice.PaymentTermsAction = true;
                objInvoice.ClientNoteAction = true;
                objInvoice.TransportAction = true;
                objInvoice.TermsConditionAction = true;
            }

            ClientManagement objClient = new ClientManagement();
            objClient.Sp_Operation = "select_for_dropdown";
            dt = new DataTable();
            dt = objClient.GetClient();

            objInvoice.ClientList = new List<SelectListItem>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objInvoice.ClientList.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Company_Name"].ToString() });
            }            

            HttpContext.Cache.Remove("ItemList");

            return View(objInvoice);
        }
        [HttpPost]
        public ActionResult AddInvoice(InvoiceManagement ObjInvoice, string Gst_Type)
        {

            DataTable dt = new DataTable();
            //Add Columns
            dt.Columns.Add("Invoice_Product_Id");
            dt.Columns.Add("Item_Id");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("UOM_Unit");
            dt.Columns.Add("Tax");
            dt.Columns.Add("Discount_Percentage");
            dt.Columns.Add("Total");
            dt.Columns.Add("Item_Note");
            dt.Columns.Add("HSN_SAC");
            dt.Columns.Add("Batch_No");
            //Add rows

            for (int i = 0; i < ObjInvoice.Item_Id.Count(); i++)
            {
                if (Convert.ToString(ObjInvoice.Item_Id[i]) != "")
                {
                    dt.Rows.Add(ObjInvoice.Invoice_Product_Id[i], ObjInvoice.Item_Id[i], ObjInvoice.Rate[i], ObjInvoice.Quantity[i], ObjInvoice.UOM_Unit[i],
                        ObjInvoice.Tax[i], ObjInvoice.Discount_Percentage[i], ObjInvoice.Total[i], ObjInvoice.Item_Note[i], ObjInvoice.HSN_SAC[i], ObjInvoice.Batch_No[i]);
                }
            }

            ObjInvoice.dtItems = dt;

            ObjInvoice.User_ID = (User as GST_Billing.Models.MyPrincipal).User.User_ID;
            ObjInvoice.Gst_Type = Gst_Type;
            if (ObjInvoice.Id == 0)
            {
                ObjInvoice.Sp_Operation = "insert";
            }
            else
            {
                ObjInvoice.Sp_Operation = "update";
            }
            ObjInvoice.GetInvoice();

            TempData["Success"] = "Success";
            return Redirect("Invoice");

        }
        public JsonResult IsInvoiceNoExist(string Invoice_No, int Id)
        {
            bool isExist = true;

            InvoiceManagement objInvoice = new InvoiceManagement();
            objInvoice.Id = Id;
            objInvoice.Invoice_No = Invoice_No;
            objInvoice.Sp_Operation = "check_invoice_no";
            DataTable dt = new DataTable();
            dt = objInvoice.GetInvoice();

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
        public ActionResult InvoiceItemList(InvoiceProductManagement objProduct, string action_type)
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
            InvoiceProductManagement objProduct = new InvoiceProductManagement();

            if (Request.QueryString["Invoiceid"] != null)
            {
                InvoiceManagement objInvoice = new InvoiceManagement();
                objInvoice.Id = Convert.ToInt32(Request.QueryString["Invoiceid"]);
                objInvoice.Sp_Operation = "select_Invoice_Items";
                objProduct.dt = objInvoice.GetInvoice();
            }

            return View(objProduct);
        }
        public ActionResult InvoiceDataTable()
        {
            ViewBag.Title = "Invoice";
            InvoiceManagement objInvoice = new InvoiceManagement();

            objInvoice.Sp_Operation = "select_report_Pagination";
            objInvoice.dt1 = objInvoice.GetInvoice();

            objInvoice.Sp_Operation = "select_report";
            objInvoice.dt = objInvoice.GetInvoice();

            objInvoice.Page_No = 1;

            return View(objInvoice);
        }
        [HttpPost]
        public ActionResult InvoiceDataTable(InvoiceManagement objInvoice)
        {
            ViewBag.Title = "Invoice";

            objInvoice.Sp_Operation = "select_report_Pagination";
            objInvoice.dt1 = objInvoice.GetInvoice();

            objInvoice.Sp_Operation = "select_report";
            objInvoice.dt = objInvoice.GetInvoice();

            if (objInvoice.Page_No == 0)
                objInvoice.Page_No = 1;

            Thread.Sleep(1000);

            return View(objInvoice);
        }
        public ActionResult Invoiceview()
        {
            ViewBag.Title = "Invoice";

            InvoiceManagement objInvoice = new InvoiceManagement();

            ClientManagement objClient = new ClientManagement();
            objClient.Sp_Operation = "select_for_dropdown";
            DataTable dt = new DataTable();
            dt = objClient.GetClient();
            objInvoice.ClientList = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objInvoice.ClientList.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Company_Name"].ToString() });
            }

            ItemManagement objItem = new ItemManagement();
            objItem.Sp_Operation = "select";
            dt = new DataTable();
            dt = objItem.Getitems();

            objInvoice.ItemList = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objInvoice.ItemList.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Item_Name"].ToString() });
            }

            return View(objInvoice);
        }
        public ActionResult DeleteInvoice()
        {
            InvoiceManagement objInvoice = new InvoiceManagement();
            objInvoice.Id = Convert.ToInt32(Request.QueryString["Invoiceid"]);
            objInvoice.Sp_Operation = "delete_Invoice";
            objInvoice.GetInvoice();

            return Redirect("Invoice");
        }
        public ActionResult ShortDataTable()
        {
            ViewBag.Title = "Invoice";
            InvoiceManagement objInvoice = new InvoiceManagement();

            objInvoice.Id = Convert.ToInt32(Request.QueryString["Invoiceid"]);
            objInvoice.Sp_Operation = "select_report_for_short";
            objInvoice.dt = objInvoice.GetInvoice();


            return View(objInvoice);
        }
        [HttpPost]
        public ActionResult ShortDataTable(InvoiceManagement objInvoice)
        {
            ViewBag.Title = "Invoice";

            objInvoice.Sp_Operation = "select_report_for_short";
            objInvoice.dt = objInvoice.GetInvoice();

            Thread.Sleep(1000);

            return View(objInvoice);
        }
        public JsonResult InvoiceStatusUpdate(int id, string status)
        {
            InvoiceManagement objInv = new InvoiceManagement();
            objInv.User_ID = (User as GST_Billing.Models.MyPrincipal).User.User_ID;
            objInv.Id = id;
            objInv.Status = status;
            objInv.Sp_Operation = "update_status";
            objInv.dt = objInv.GetInvoice();

            return Json("Status update successfully...", JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetInvoicestatus(int id)
        {
            InvoiceManagement objInvoice = new InvoiceManagement();
            objInvoice.Sp_Operation = "select_status";
            objInvoice.Id = id;
            DataTable dt = new DataTable();
            dt = objInvoice.GetInvoice();

            List<InvoiceManagement> invoice = new List<InvoiceManagement>();
            invoice = CommonMethod.ConvertToList<InvoiceManagement>(dt);
            objInvoice = invoice[0];

            return Json(objInvoice, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EmailInvoice(int id )
        {
            ViewBag.Title = "Invoice";
            ComposeEmail objEmail = new ComposeEmail();

            CompanyDetailsManagement objCom = new CompanyDetailsManagement();
            objCom.Company_Id = (User as GST_Billing.Models.MyPrincipal).User.Company_Id;
            objCom.Sp_Operation = "select";
            DataTable dt = new DataTable();
            dt = objCom.Getcompanydetails();

            objEmail.ReplyTo = Convert.ToString(dt.Rows[0]["Email"]);
            objEmail.Subject = "Invoice from "+ Convert.ToString(dt.Rows[0]["Company_Name"]);

            InvoiceManagement objInv = new InvoiceManagement();
            objInv.Id = id;
            objInv.Sp_Operation = "select_report";
            DataTable dt1 = new DataTable();
            dt1 = objInv.GetInvoice();

            objEmail.Body = "Hi " + Convert.ToString(dt1.Rows[0]["Company_Name"]) + ",\n\nThank you for your business.\n\n";
            objEmail.Body +="Your invoice 1 - 2 can be viewed, printed or downloaded as a PDF from the link below.\n\nRegards,\n"+ Convert.ToString(dt.Rows[0]["Contact_Person"]) + "\n"+ Convert.ToString(dt.Rows[0]["Company_Name"]) + "";

            return View(objEmail);
        }
    }
}