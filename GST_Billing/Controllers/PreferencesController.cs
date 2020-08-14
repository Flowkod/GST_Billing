using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GST_Billing.Models;
using System.Data;

namespace GST_Billing.Controllers
{
    [Authorize]
    public class PreferencesController : Controller
    {
        // GET: Preferences
        public ActionResult Setting()
        {
            return View();
        }
        public ActionResult PurchaseOrderPreferences()
        {
            PreferencesManagment objPreference = new PreferencesManagment();
            objPreference.Company_Id = (User as GST_Billing.Models.MyPrincipal).User.Company_Id;
            objPreference.Sp_Operation = "select_PreferencesPurchaseOrder";
            DataTable dt = new DataTable();
            dt = objPreference.GetPreferences();

            if (dt.Rows.Count > 0)
            {
                List<PreferencesManagment> PreferencesList = new List<PreferencesManagment>();
                PreferencesList = CommonMethod.ConvertToList<PreferencesManagment>(dt);
                objPreference = PreferencesList[0];
            }
            else
            {
                objPreference.PrintShipAddress = true;
                objPreference.PrintBillAddress = true;
                objPreference.PrintPaymentTerms = true;
                objPreference.PrintClientNote = true;
                objPreference.PrintTransport = true;
                objPreference.PrintTermsCondition = true;
            }

            return View(objPreference);
        }
        [HttpPost]
        public ActionResult PurchaseOrderPreferences(PreferencesManagment objPreference, HttpPostedFileBase signature)
        {
            objPreference.Company_Id = (User as GST_Billing.Models.MyPrincipal).User.Company_Id;

            string logofilePath = string.Empty;

            if (signature != null)
            {
                string path = Server.MapPath("~/app-assets/images/logo/");

                logofilePath = path + objPreference.Company_Id + "_signature.png";
                signature.SaveAs(logofilePath);
                logofilePath = "../app-assets/images/logo/" + objPreference.Company_Id + "_signature.png";
            }

            objPreference.Signature = logofilePath;
            objPreference.Sp_Operation = "Insert_PreferencesPurchaseOrder";
            objPreference.GetPreferences();

            return Redirect("Setting");
        }
    }
}