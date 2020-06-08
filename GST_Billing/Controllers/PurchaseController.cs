using GST_Billing.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
            ViewBag.Title = "Purchase Order";

            PurchaseOrderManagement objPurchase = new PurchaseOrderManagement();

            CompanyDetailsManagement objCom = new CompanyDetailsManagement();
            objCom.Company_Id = (User as GST_Billing.Models.MyPrincipal).User.Company_Id;
            objCom.Sp_Operation = "select";
            objPurchase.dtCompanyDetails = objCom.Getcompanydetails();

            SupplierManagement objSupplier = new SupplierManagement();
            objSupplier.Sp_Operation = "select_for_dropdown";
            DataTable dt = new DataTable();
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

            return View(objPurchase);
        }
        [HttpGet]
        public JsonResult GetsupplierBillAddress(int Supplier_Id)
        {
            ViewBag.Title = "Purchase Order";

            SupplierManagement objSupplier = new SupplierManagement();
                objSupplier.Id = Supplier_Id;
            
            objSupplier.Sp_Operation = "select";

            DataTable dt = new DataTable();
            dt = objSupplier.Getsupplier();

            string[] data= new string[3];

            if (dt.Rows.Count > 0)
            {
                string Address = "";
                Address += Convert.ToString(dt.Rows[0]["Address"]);
                Address+= Convert.ToString(dt.Rows[0]["City"]) == "" ? "" : ", "+Convert.ToString(dt.Rows[0]["City"])+"";
                Address += Convert.ToString(dt.Rows[0]["State"]) == "" ? "" : ", " + Convert.ToString(dt.Rows[0]["State"]) + "";
                Address += Convert.ToString(dt.Rows[0]["Country"]) == "" ? "" : ", " + Convert.ToString(dt.Rows[0]["Country"]) + "";
                Address += Convert.ToString(dt.Rows[0]["Pin_Code"]) == "" ? "" : "-" + Convert.ToString(dt.Rows[0]["Pin_Code"]) + "";

                data[0] = Address;

                data[1] = Convert.ToString(dt.Rows[0]["GSTIN"]);
                data[2] = Convert.ToString(dt.Rows[0]["Mobile_No"]) + (Convert.ToString(dt.Rows[0]["Phone_No"]) == "" ? "" : "," + Convert.ToString(dt.Rows[0]["Phone_No"]) + "");
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNewAddeddSupplierRecord()
        {
            string[] data = new string[2];

            data[0] = Convert.ToString(Session["Supplier_Id"]);
            data[1] = Convert.ToString(Session["Supplier_Company_Name"]);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNewAddeddClientRecord()
        {
            string[] data = new string[2];

            data[0] = Convert.ToString(Session["Client_Id"]);
            data[1] = Convert.ToString(Session["Client_Company_Name"]);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetClientShipAddress(int Client_Id)
        {
            ClientManagement objClient = new ClientManagement();
            objClient.Id = Client_Id;
            objClient.Sp_Operation = "select_shipping_address";
            objClient.dt = objClient.GetClient();
                
            return View(objClient);
        }
        public ActionResult Addnewentry()
        {
            return View();
        }
    }
}