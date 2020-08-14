using GST_Billing.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace GST_Billing.Controllers
{
    [Authorize]
    public class PurchaseController : Controller
    {
        // GET: Purchase

        public ActionResult Purchaseorder()
        {
            ViewBag.Title = "Purchaseorder";

            PurchaseOrderManagement objPurchase = new PurchaseOrderManagement();

            SupplierManagement objSupplier = new SupplierManagement();
            objSupplier.Sp_Operation = "select_for_dropdown";
            DataTable dt = new DataTable();
            dt = objSupplier.Getsupplier();
            objPurchase.SupplierList = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objPurchase.SupplierList.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Company_Name"].ToString() });
            }

            ItemManagement objItem = new ItemManagement();
            objItem.Sp_Operation = "select";
            dt = new DataTable();
            dt = objItem.Getitems();

            objPurchase.ItemList = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objPurchase.ItemList.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Item_Name"].ToString() });
            }

            return View(objPurchase);
        }
        public ActionResult AddPurchaseorder()
        {
            ViewBag.Title = "Purchaseorder";

            PurchaseOrderManagement objPurchase = new PurchaseOrderManagement();

            DataTable dt = new DataTable();

            if (Request.QueryString["Purchaseid"] != null)
            {
                objPurchase.Sp_Operation = "select_Purchase";
                objPurchase.Id = Convert.ToInt32(Request.QueryString["Purchaseid"]);
                dt = new DataTable();
                dt = objPurchase.Getpurchaseorder();

                List<PurchaseOrderManagement> purchaseOrders = new List<PurchaseOrderManagement>();
                purchaseOrders = CommonMethod.ConvertToList<PurchaseOrderManagement>(dt);
                objPurchase = purchaseOrders[0];

                objPurchase.Sp_Operation = "select_Purchase_Action";
                objPurchase.Id = Convert.ToInt32(Request.QueryString["Purchaseid"]);
                dt = new DataTable();
                dt = objPurchase.Getpurchaseorder();

                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(dt.Rows[0]["ShipAddress"]) != "")
                    {
                        objPurchase.ShipAddressAction = Convert.ToBoolean(dt.Rows[0]["ShipAddress"]);
                    }
                    if (Convert.ToString(dt.Rows[0]["BillAddress"]) != "")
                    {
                        objPurchase.BillAddressAction = Convert.ToBoolean(dt.Rows[0]["BillAddress"]);
                    }
                    if (Convert.ToString(dt.Rows[0]["PaymentTerms"]) != "")
                    {
                        objPurchase.PaymentTermsAction = Convert.ToBoolean(dt.Rows[0]["PaymentTerms"]);
                    }
                    if (Convert.ToString(dt.Rows[0]["ClientNote"]) != "")
                    {
                        objPurchase.ClientNoteAction = Convert.ToBoolean(dt.Rows[0]["ClientNote"]);
                    }
                    if (Convert.ToString(dt.Rows[0]["Transport"]) != "")
                    {
                        objPurchase.TransportAction = Convert.ToBoolean(dt.Rows[0]["Transport"]);
                    }
                    if (Convert.ToString(dt.Rows[0]["TermsCondition"]) != "")
                    {
                        objPurchase.TermsConditionAction = Convert.ToBoolean(dt.Rows[0]["TermsCondition"]);
                    }
                }
            }
            else
            {
                objPurchase.Sp_Operation = "get_last_po_no";
                dt = new DataTable();
                dt = objPurchase.Getpurchaseorder();

                if (dt.Rows.Count > 0)
                {
                    objPurchase.PO_No = Convert.ToString(dt.Rows[0]["PO_No"]);
                }

                objPurchase.ShipAddressAction = true;
                objPurchase.BillAddressAction = true;
                objPurchase.PaymentTermsAction = true;
                objPurchase.ClientNoteAction = true;
                objPurchase.TransportAction = true;
                objPurchase.TermsConditionAction = true;
            }

            SupplierManagement objSupplier = new SupplierManagement();
            objSupplier.Sp_Operation = "select_for_dropdown";
            dt = new DataTable();
            dt = objSupplier.Getsupplier();

            objPurchase.SupplierList = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objPurchase.SupplierList.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Company_Name"].ToString() });
            }

            ClientManagement objClient = new ClientManagement();
            objClient.Sp_Operation = "select_for_dropdown";
            dt = new DataTable();
            dt = objClient.GetClient();

            objPurchase.ClientList = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objPurchase.ClientList.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Company_Name"].ToString() });
            }

            HttpContext.Cache.Remove("ItemList");

            return View(objPurchase);
        }
        [HttpPost]
        public ActionResult AddPurchaseorder(PurchaseOrderManagement ObjPurchase, string Gst_Type)
        {

            DataTable dt = new DataTable();
            //Add Columns
            dt.Columns.Add("Purchaseorder_Product_Id");
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

            for (int i = 0; i < ObjPurchase.Item_Id.Count(); i++)
            {
                if (Convert.ToString(ObjPurchase.Item_Id[i]) != "")
                {
                    dt.Rows.Add(ObjPurchase.PurchaseOrder_Product_Id[i], ObjPurchase.Item_Id[i], ObjPurchase.Rate[i], ObjPurchase.Quantity[i], ObjPurchase.UOM_Unit[i],
                        ObjPurchase.Tax[i], ObjPurchase.Discount_Percentage[i], ObjPurchase.Total[i], ObjPurchase.Item_Note[i], ObjPurchase.HSN_SAC[i], ObjPurchase.Batch_No[i]);
                }
            }

            ObjPurchase.dtItems = dt;

            ObjPurchase.User_ID = (User as GST_Billing.Models.MyPrincipal).User.User_ID;
            ObjPurchase.Gst_Type = Gst_Type;
            if (ObjPurchase.Id == 0)
            {
                ObjPurchase.Sp_Operation = "insert";
            }
            else
            {
                ObjPurchase.Sp_Operation = "update";
            }
            ObjPurchase.Getpurchaseorder();

            TempData["Success"] = "Success";
            return Redirect("Purchaseorder");

        }
        public JsonResult IsPoNoExist(string PO_No, int Id)
        {
            bool isExist = true;

            PurchaseOrderManagement objPurchase = new PurchaseOrderManagement();
            objPurchase.Id = Id;
            objPurchase.PO_No = PO_No;
            objPurchase.Sp_Operation = "check_po_no";
            DataTable dt = new DataTable();
            dt = objPurchase.Getpurchaseorder();

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
        public ActionResult PurchaseItemList(PurchaseOrderProductManagement objProduct, string action_type)
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
            PurchaseOrderProductManagement objProduct = new PurchaseOrderProductManagement();

            if (Request.QueryString["Purchaseid"] != null)
            {
                PurchaseOrderManagement objPurchase = new PurchaseOrderManagement();
                objPurchase.Id = Convert.ToInt32(Request.QueryString["Purchaseid"]);
                objPurchase.Sp_Operation = "select_Purchase_Items";
                objProduct.dt = objPurchase.Getpurchaseorder();

            }

            return View(objProduct);
        }
        public ActionResult PurchaseDataTable()
        {
            ViewBag.Title = "Purchaseorder";
            PurchaseOrderManagement objPurchase = new PurchaseOrderManagement();

            objPurchase.Sp_Operation = "select_report_Pagination";
            objPurchase.dt1 = objPurchase.Getpurchaseorder();

            objPurchase.Sp_Operation = "select_report";
            objPurchase.dt = objPurchase.Getpurchaseorder();

            objPurchase.Page_No = 1;

            return View(objPurchase);
        }
        [HttpPost]
        public ActionResult PurchaseDataTable(PurchaseOrderManagement objPurchase)
        {
            ViewBag.Title = "Purchaseorder";

            objPurchase.Sp_Operation = "select_report_Pagination";
            objPurchase.dt1 = objPurchase.Getpurchaseorder();

            objPurchase.Sp_Operation = "select_report";
            objPurchase.dt = objPurchase.Getpurchaseorder();

            if (objPurchase.Page_No == 0)
                objPurchase.Page_No = 1;

            Thread.Sleep(1000);

            return View(objPurchase);
        }
        public ActionResult Purchaseview()
        {
            ViewBag.Title = "Purchaseorder";

            PurchaseOrderManagement objPurchase = new PurchaseOrderManagement();

            SupplierManagement objSupplier = new SupplierManagement();
            objSupplier.Sp_Operation = "select_for_dropdown";
            DataTable dt = new DataTable();
            dt = objSupplier.Getsupplier();
            objPurchase.SupplierList = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objPurchase.SupplierList.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Company_Name"].ToString() });
            }

            ItemManagement objItem = new ItemManagement();
            objItem.Sp_Operation = "select";
            dt = new DataTable();
            dt = objItem.Getitems();

            objPurchase.ItemList = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objPurchase.ItemList.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Item_Name"].ToString() });
            }

            return View(objPurchase);
        }
        public ActionResult DeletePurchase()
        {
            PurchaseOrderManagement objPurchase = new PurchaseOrderManagement();
            objPurchase.Id = Convert.ToInt32(Request.QueryString["purchaseid"]);
            objPurchase.Sp_Operation = "delete_Purchase";
            objPurchase.Getpurchaseorder();

            return Redirect("Purchaseorder");
        }
        public ActionResult ShortDataTable()
        {
            ViewBag.Title = "Purchaseorder";
            PurchaseOrderManagement objPurchase = new PurchaseOrderManagement();

            objPurchase.Id = Convert.ToInt32(Request.QueryString["purchaseid"]);
            objPurchase.Sp_Operation = "select_report_for_short";
            objPurchase.dt = objPurchase.Getpurchaseorder();


            return View(objPurchase);
        }
        [HttpPost]
        public ActionResult ShortDataTable(PurchaseOrderManagement objPurchase)
        {
            ViewBag.Title = "Purchaseorder";

            objPurchase.Sp_Operation = "select_report_for_short";
            objPurchase.dt = objPurchase.Getpurchaseorder();

            Thread.Sleep(1000);

            return View(objPurchase);
        }
        public JsonResult PurchaseStatusUpdate(int id, string status)
        {
            PurchaseOrderManagement objPurchase = new PurchaseOrderManagement();
            objPurchase.User_ID = (User as GST_Billing.Models.MyPrincipal).User.User_ID;
            objPurchase.Id = id;
            objPurchase.Status = status;
            objPurchase.Sp_Operation = "update_status";
            objPurchase.dt = objPurchase.Getpurchaseorder();

            return Json("Status update successfully...", JsonRequestBehavior.AllowGet);
        }
        public JsonResult Getpurchasestatus(int id)
        {
            PurchaseOrderManagement objPurchase = new PurchaseOrderManagement();
            objPurchase.Sp_Operation = "select_status";
            objPurchase.Id = id;
            DataTable dt = new DataTable();
            dt = objPurchase.Getpurchaseorder();

            List<PurchaseOrderManagement> purchaseOrders = new List<PurchaseOrderManagement>();
            purchaseOrders = CommonMethod.ConvertToList<PurchaseOrderManagement>(dt);
            objPurchase = purchaseOrders[0];

            return Json(objPurchase, JsonRequestBehavior.AllowGet);
        }
    }
}