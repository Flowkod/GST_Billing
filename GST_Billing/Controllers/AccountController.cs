using GST_Billing.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace GST_Billing.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(UserManagement objUser)
        {
            objUser.Sp_Operation = "GetLogin";
            DataTable dt = new DataTable();
            dt = objUser.GetUser();

            if (dt.Rows.Count > 0)
            {
                if (Convert.ToString(dt.Rows[0]["Error_Msg"]) == "")
                {

                    UserData user = new UserData();
                    user.Name = Convert.ToString(dt.Rows[0]["Name"]);
                    user.Email = Convert.ToString(dt.Rows[0]["Email"]);
                    user.Designation_ID = Convert.ToInt32(dt.Rows[0]["Designation_ID"]);
                    user.User_ID = Convert.ToInt32(dt.Rows[0]["User_ID"]);
                    user.Company_Id = Convert.ToInt32(dt.Rows[0]["Company_Id"]);

                    var serializer = new JavaScriptSerializer();
                    string userData = serializer.Serialize(user);
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                            objUser.Email,
                            DateTime.Now,
                            DateTime.Now.AddDays(30),
                            true,
                            userData,
                            FormsAuthentication.FormsCookiePath);
                    // Encrypt the ticket.
                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    // Create the cookie.
                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));


                    return Redirect("~/Home/Index");
                }
                else
                {
                    ViewBag.ErrorMsg = Convert.ToString(dt.Rows[0]["Error_Msg"]);
                }
            }

            return View(objUser);
        }
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        public ActionResult Error()
        {
            return View();
        }
        [Authorize]
        public ActionResult Userprofile()
        {
            UserManagement objUser = new UserManagement();
            objUser.Sp_Operation = "select_user";
            objUser.User_ID = (User as GST_Billing.Models.MyPrincipal).User.User_ID;
            objUser.dt = objUser.GetUser();

            return View(objUser);
        }
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Userprofile(UserManagement objUser)
        {
            objUser.Sp_Operation = "update_password";
            objUser.User_ID = (User as GST_Billing.Models.MyPrincipal).User.User_ID;
            DataTable dt = new DataTable();
            dt = objUser.GetUser();

            if (dt.Rows.Count > 0)
            {
                if (Convert.ToString(dt.Rows[0]["Msg"]) == "Success")
                {
                    TempData["Success"] = "Success";
                }
                else
                {
                    TempData["Error"] = Convert.ToString(dt.Rows[0]["Msg"]);
                }
            }

            return Redirect("Userprofile");
        }
        [Authorize]
        public ActionResult CompanyProfile()
        {
            ViewBag.Title = "CompanyProfile";

            CompanyDetailsManagement objCom = new CompanyDetailsManagement();
            objCom.Company_Id= (User as GST_Billing.Models.MyPrincipal).User.Company_Id;
            objCom.Sp_Operation = "select";
            DataTable dt = new DataTable();
            dt = new DataTable();
            dt = objCom.Getcompanydetails();

            if (dt.Rows.Count > 0)
            {
                List<CompanyDetailsManagement> CompanyDetails = new List<CompanyDetailsManagement>();
                CompanyDetails = CommonMethod.ConvertToList<CompanyDetailsManagement>(dt);
                objCom = CompanyDetails[0];
            }

            return View(objCom);
        }
        [Authorize]
        [HttpPost]
        public ActionResult CompanyProfile(CompanyDetailsManagement objCom, HttpPostedFileBase logo, HttpPostedFileBase favicon)
        {
            ViewBag.Title = "CompanyProfile";

            string logofilePath = string.Empty;
            string faviconfilePath = string.Empty;

            objCom.User_ID = (User as GST_Billing.Models.MyPrincipal).User.User_ID;
            objCom.Sp_Operation = "insert";
            DataTable dt = new DataTable();
            dt = objCom.Getcompanydetails();

            objCom.Company_Id = Convert.ToInt32(dt.Rows[0]["Company_Id"]);

            if (logo != null)
            {
                string path = Server.MapPath("~/app-assets/images/logo/");

                logofilePath = path + objCom.Company_Id+"_"+Path.GetFileName(logo.FileName);
                logo.SaveAs(logofilePath);
                logofilePath = "../app-assets/images/logo/" + objCom.Company_Id + "_" + logo.FileName;
            }

            if (favicon != null)
            {
                string path = Server.MapPath("~/app-assets/images/logo/");

                faviconfilePath = path + objCom.Company_Id + "_" + Path.GetFileName(favicon.FileName);
                favicon.SaveAs(faviconfilePath);
                faviconfilePath = "../app-assets/images/logo/" + objCom.Company_Id + "_" + favicon.FileName;
            }

            if (logofilePath != "" || faviconfilePath != "")
            {
                objCom.Company_Logo = logofilePath;
                objCom.Favicon = faviconfilePath;
                objCom.User_ID = (User as GST_Billing.Models.MyPrincipal).User.User_ID;
                objCom.Sp_Operation = "update_img_path";
                objCom.Getcompanydetails();
            }

            return Redirect("CompanyProfile");
        }
    }
}