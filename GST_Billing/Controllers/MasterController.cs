using GST_Billing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Threading;
using System.IO;
using ClosedXML.Excel;
using System.Data.OleDb;
using System.Configuration;
using System.Data.SqlClient;

namespace GST_Billing.Controllers
{
    [Authorize]
    public class MasterController : Controller
    {
        // Item Master Management
        public ActionResult item()
        {
            ViewBag.Title = "item";

            ItemManagement objItem = new ItemManagement();
            //objItem.Sp_Operation = "select_report";
            //objItem.dt = objItem.Getitems();

            ItemUnitManagement objUnit = new ItemUnitManagement();
            objUnit.Sp_Operation = "select";
            DataTable dt = new DataTable();
            dt = objUnit.GetItemUnit();

            objItem.Item_Units_List = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objItem.Item_Units_List.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Unit_Name"].ToString() });
            }

            ItemTaxesManagement objTax = new ItemTaxesManagement();
            objTax.Sp_Operation = "select";
            dt = new DataTable();
            dt = objTax.GetTaxes();

            objItem.Tax_List = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objItem.Tax_List.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Tax_Name"].ToString() });
            }

            return View(objItem);
        }
        public ActionResult ItemDataTable()
        {
            ViewBag.Title = "item";
            ItemManagement objItem = new ItemManagement();

            objItem.Sp_Operation = "select_report_Pagination";
            objItem.dt1 = objItem.Getitems();

            objItem.Sp_Operation = "select_report";
            objItem.dt = objItem.Getitems();

            objItem.Page_No = 1;

            return View(objItem);
        }
        [HttpPost]
        public ActionResult ItemDataTable(ItemManagement objItem)
        {
            ViewBag.Title = "item";

            objItem.Sp_Operation = "select_report_Pagination";
            objItem.dt1 = objItem.Getitems();

            objItem.Sp_Operation = "select_report";
            objItem.dt = objItem.Getitems();

            if (objItem.Page_No == 0)
                objItem.Page_No = 1;

            Thread.Sleep(1000);

            return View(objItem);
        }
        [HttpPost]
        public ActionResult ItemTableExcelExport(ItemManagement objItem)
        {
            objItem.Sp_Operation = "select_report_export";
            DataTable dt = new DataTable();
            dt = objItem.Getitems();

            dt.TableName = "ItemsReport";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= ItemsReport.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }

            return View("item", objItem);
        }
        public ActionResult Additem()
        {
            ViewBag.Title = "item";

            ItemManagement objItem = new ItemManagement();
            DataTable dt = new DataTable();

            if (Request.QueryString["itemid"] != null)
            {
                objItem.Sp_Operation = "select";
                objItem.Id = Convert.ToInt32(Request.QueryString["itemid"]);
                dt = new DataTable();
                dt = objItem.Getitems();

                List<ItemManagement> itemsList = new List<ItemManagement>();
                itemsList = CommonMethod.ConvertToList<ItemManagement>(dt);
                objItem = itemsList[0];
            }
            else
            {
                objItem.Sp_Operation = "get_last_item_code";
                dt = new DataTable();
                dt = objItem.Getitems();

                if (dt.Rows.Count > 0)
                {
                    objItem.Item_Code = Convert.ToString(dt.Rows[0]["Item_Code"]);
                }
            }

            ItemUnitManagement objUnit = new ItemUnitManagement();
            objUnit.Sp_Operation = "select";
            dt = new DataTable();
            dt = objUnit.GetItemUnit();

            objItem.Item_Units_List = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objItem.Item_Units_List.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Unit_Name"].ToString() });
            }

            ItemTaxesManagement objTax = new ItemTaxesManagement();
            objTax.Sp_Operation = "select";
            dt = new DataTable();
            dt = objTax.GetTaxes();

            objItem.Tax_List = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objItem.Tax_List.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Tax_Name"].ToString() });
            }

            return View(objItem);
        }
        [HttpPost]
        public ActionResult Additem(ItemManagement objItem)
        {
            ViewBag.Title = "item";

            objItem.User_ID = (User as GST_Billing.Models.MyPrincipal).User.User_ID;

            if (objItem.Id == 0)
            {
                objItem.Sp_Operation = "insert";
            }
            else
            {
                objItem.Sp_Operation = "update";
            }
            objItem.Getitems();
            TempData["Success"] = "Success";

            return Redirect("item");
        }
        public ActionResult ImportItems()
        {
            ViewBag.Title = "item";
            return View();
        }
        [HttpPost]
        public ActionResult ImportItems(HttpPostedFileBase importdata)
        {
            ViewBag.Title = "item";
            ItemManagement objItem = new ItemManagement();
            objItem.Sp_Operation = "Truncate_Temp";
            objItem.Getitems();

            try
            {
                DataTable dt = new DataTable();

                string filePath = string.Empty;

                if (importdata != null)
                {
                    string path = Server.MapPath("~/Uploads/");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(importdata.FileName);
                    importdata.SaveAs(filePath);
                }

                string extension = Path.GetExtension(importdata.FileName);
                string conStr = "";

                switch (extension)
                {
                    case ".xls": //Excel 97-03
                                 //conStr = "metadata=res://*/Models.Model1.csdl|res://*/Models.Model1.ssdl|res://*/Models.Model1.msl;Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\\Spark 2018\\CRM\\CRM\\Uploads\\;Extended Properties='Text;HDR=NO;IMEX=1';";
                        conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1';";
                        break;
                    case ".xlsx": //Excel 07
                        conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1';";
                        break;
                }

                OleDbConnection connExcel = new OleDbConnection(conStr);
                OleDbCommand cmdExcel = new OleDbCommand();
                OleDbDataAdapter oda = new OleDbDataAdapter();
                dt = new DataTable();
                cmdExcel.Connection = connExcel;

                //Get the name of First Sheet
                connExcel.Open();
                DataTable dtExcelSchema;
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                ///int index_row = CheckHiddenFile(filePath);

                string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();

                connExcel.Close();

                //Read Data from First Sheet
                connExcel.Open();
                cmdExcel.CommandText = "Select * from [" + SheetName + "] ";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dt);
                connExcel.Close();

                string sqlconn = ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString;

                dt.Columns.Add("CREATED_BY", typeof(System.Int32));

                foreach (DataRow row in dt.Rows)
                {
                    row["CREATED_BY"] = (User as GST_Billing.Models.MyPrincipal).User.User_ID;
                }

                dt.Columns[0].ColumnName = "Item_Type";
                dt.Columns[1].ColumnName = "Item_Name";
                dt.Columns[2].ColumnName = "Item_Code";
                dt.Columns[3].ColumnName = "Item_Short_Name";
                dt.Columns[4].ColumnName = "Item_Group";
                dt.Columns[5].ColumnName = "Description";
                dt.Rows[0].Delete();
                dt.AcceptChanges();

                using (SqlConnection con = new SqlConnection(sqlconn))
                {
                    using (SqlBulkCopy objbulk = new SqlBulkCopy(con))
                    {
                        objbulk.DestinationTableName = "Items_Temp";
                        objbulk.ColumnMappings.Add("Item_Type", "items_type");
                        objbulk.ColumnMappings.Add("Item_Name", "Item_Name");
                        objbulk.ColumnMappings.Add("Item_Code", "Item_Code");
                        objbulk.ColumnMappings.Add("Item_Short_Name", "Item_Short_Name");
                        objbulk.ColumnMappings.Add("Item_Group", "Item_Group");
                        objbulk.ColumnMappings.Add("Description", "Description");
                        objbulk.ColumnMappings.Add("CREATED_BY", "Created_By");

                        con.Open();
                        objbulk.WriteToServer(dt);
                        con.Close();

                        objItem = new ItemManagement();
                        objItem.User_ID = (User as GST_Billing.Models.MyPrincipal).User.User_ID;
                        objItem.Sp_Operation = "Item_Import_Excel";
                        dt = new DataTable();
                        dt = objItem.Getitems();

                        if (Convert.ToString(dt.Rows[0]["Message"]) == "Success")
                        {
                            TempData["Success"] = "Success";
                        }
                        else
                        {
                            TempData["Error"] = Convert.ToString(dt.Rows[0]["Message"]);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "File content not in format...Please check and try again... " + ex.Message;

                objItem.Sp_Operation = "Truncate_Temp";
                objItem.Getitems();
            }


            return View();
        }
        public ActionResult DeleteItem()
        {
            ViewBag.Title = "item";

            ItemManagement objItem = new ItemManagement();
            objItem.Sp_Operation = "delete";
            objItem.User_ID = (User as GST_Billing.Models.MyPrincipal).User.User_ID;
            objItem.Id = Convert.ToInt32(Request.QueryString["itemid"]);
            objItem.Getitems();

            return Redirect("item");
        }
        [HttpGet]
        public JsonResult IsItemCodeExist(string Item_Code, int Id)
        {
            bool isExist = true;

            ItemManagement objItem = new ItemManagement();
            objItem.Id = Id;
            objItem.Item_Code = Item_Code;
            objItem.Sp_Operation = "check_item_code";
            DataTable dt = new DataTable();
            dt = objItem.Getitems();

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
        public ActionResult AddItemUnit()
        {
            ViewBag.Title = "item";
            return View();
        }
        [HttpPost]
        public JsonResult AddItemUnit(ItemUnitManagement objUnit)
        {
            ViewBag.Title = "item";

            objUnit.Sp_Operation = "insert";
            objUnit.User_ID = (User as GST_Billing.Models.MyPrincipal).User.User_ID;
            DataTable dt = new DataTable();
            dt = objUnit.GetItemUnit();

            if (dt.Rows.Count > 0)
            {
                objUnit.UOM_Unit = Convert.ToInt32(dt.Rows[0]["Id"]);
            }

            return Json(objUnit.UOM_Unit);
        }
        public ActionResult AddNewTax()
        {
            ViewBag.Title = "item";
            return View();
        }
        [HttpPost]
        public JsonResult AddNewTax(ItemTaxesManagement objTax)
        {
            ViewBag.Title = "item";

            objTax.Sp_Operation = "insert";
            objTax.User_ID = (User as GST_Billing.Models.MyPrincipal).User.User_ID;
            DataTable dt = new DataTable();
            dt = objTax.GetTaxes();

            if (dt.Rows.Count > 0)
            {
                objTax.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
            }


            return Json(objTax.Id);
        }
        //End Item Master

        //Start Supplier Master
        public ActionResult AddSupplier()
        {
            ViewBag.Title = "Supplier";

            SupplierManagement objSupplier = new SupplierManagement();
            DataTable dt = new DataTable();

            if (Request.QueryString["supplierid"] != null)
            {
                objSupplier.Sp_Operation = "select";
                objSupplier.Id = Convert.ToInt32(Request.QueryString["supplierid"]);
                dt = objSupplier.Getsupplier();

                List<SupplierManagement> supplierList = new List<SupplierManagement>();
                supplierList = CommonMethod.ConvertToList<SupplierManagement>(dt);
                objSupplier = supplierList[0];

                objSupplier.Sp_Operation = "select_bank";
                objSupplier.Id = Convert.ToInt32(Request.QueryString["supplierid"]);
                objSupplier.dt = objSupplier.Getsupplier();
            }
            else
            {
                objSupplier.Sp_Operation = "get_last_Supplier_code";
                dt = new DataTable();
                dt = objSupplier.Getsupplier();

                if (dt.Rows.Count > 0)
                {
                    objSupplier.Code = Convert.ToString(dt.Rows[0]["Code"]);
                }
                objSupplier.dt = new DataTable();
            }
            
            return View(objSupplier);
        }
        [HttpPost]
        public ActionResult AddSupplier(SupplierManagement objSupplier, string Addedfrom)
        {
            ViewBag.Title = "Supplier";

            objSupplier.User_ID = (User as GST_Billing.Models.MyPrincipal).User.User_ID;

            DataTable dt = new DataTable();
            //Add Columns
            dt.Columns.Add("Id");
            dt.Columns.Add("Bank_Acc_No");
            dt.Columns.Add("Bank_IFSC");
            dt.Columns.Add("Bank_Name");
            dt.Columns.Add("Bank_Branch");
            //Add rows
            for (int i = 0; i < objSupplier.Bank_Name.Count(); i++)
            {
                dt.Rows.Add(objSupplier.SupplierBankDetailsId[i], objSupplier.Bank_Acc_No[i], objSupplier.Bank_IFSC[i], objSupplier.Bank_Name[i], objSupplier.Bank_Branch[i]);
            }

            objSupplier.BankDetails = dt;
            if (objSupplier.Id == 0)
            {
                objSupplier.Sp_Operation = "insert";
            }
            else
            {
                objSupplier.Sp_Operation = "update";
            }
            dt = new DataTable();
            dt = objSupplier.Getsupplier();

            if (Convert.ToString(dt.Rows[0]["Msg"]) == "Success")
            {

                //if entry add for purchase order 
                if (Addedfrom != null)
                {
                    if (Addedfrom == "purchase supplier")
                    {
                        TempData["AddedFrom"] = "Purchase";
                        Session["Supplier_Id"] = Convert.ToString(dt.Rows[0]["Supplier_Id"]);
                        Session["Supplier_Company_Name"] = objSupplier.Company_Name + " (" + objSupplier.Code + ")";
                    }

                    return Redirect("AddSupplier");
                }
                else
                {
                    TempData["Success"] = "Success";
                    return Redirect("Supplier");
                }

            }
            else
            {
                TempData["Error"] = "Internal server issue please try again";
                return View(objSupplier);
            }

        }
        [HttpGet]
        public JsonResult IsSupplierCodeExist(string Code, int Id)
        {
            bool isExist = true;

            SupplierManagement objSupplier = new SupplierManagement();
            objSupplier.Id = Id;
            objSupplier.Code = Code;
            objSupplier.Sp_Operation = "check_item_code";
            DataTable dt = new DataTable();
            dt = objSupplier.Getsupplier();

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
        public ActionResult Supplier()
        {
            ViewBag.Title = "Supplier";

            return View();
        }
        public ActionResult SupplierDataTable()
        {
            ViewBag.Title = "Supplier";
            SupplierManagement objSupplier = new SupplierManagement();

            objSupplier.Sp_Operation = "select_report_Pagination";
            objSupplier.dt1 = objSupplier.Getsupplier();

            objSupplier.Sp_Operation = "select_report";
            objSupplier.dt = objSupplier.Getsupplier();

            objSupplier.Page_No = 1;

            return View(objSupplier);
        }
        [HttpPost]
        public ActionResult SupplierDataTable(SupplierManagement objSupplier)
        {
            ViewBag.Title = "Supplier";

            objSupplier.Sp_Operation = "select_report_Pagination";
            objSupplier.dt1 = objSupplier.Getsupplier();

            objSupplier.Sp_Operation = "select_report";
            objSupplier.dt = objSupplier.Getsupplier();

            if (objSupplier.Page_No == 0)
                objSupplier.Page_No = 1;

            Thread.Sleep(1000);

            return View(objSupplier);
        }
        public ActionResult DeleteSupplier()
        {
            ViewBag.Title = "Supplier";

            SupplierManagement objSupplier = new SupplierManagement();
            objSupplier.User_ID = (User as GST_Billing.Models.MyPrincipal).User.User_ID;
            objSupplier.Sp_Operation = "delete";
            objSupplier.Id = Convert.ToInt32(Request.QueryString["supplierid"]);
            objSupplier.Getsupplier();

            return Redirect("Supplier");
        }
        [HttpPost]
        public ActionResult SupplierTableExcelExport(SupplierManagement objSupplier)
        {
            objSupplier.Sp_Operation = "select_report_export";
            DataTable dt = new DataTable();
            dt = objSupplier.Getsupplier();

            dt.TableName = "SupplierReport";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=SupplierReport.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }

            return View("Supplier", objSupplier);
        }
        public ActionResult ImportSupplier()
        {
            ViewBag.Title = "Supplier";
            return View();
        }
        [HttpPost]
        public ActionResult ImportSupplier(HttpPostedFileBase importdata)
        {
            ViewBag.Title = "Supplier";
            SupplierManagement objSupplier = new SupplierManagement();
            objSupplier.Sp_Operation = "Truncate_Temp";
            objSupplier.Getsupplier();

            try
            {
                DataTable dt = new DataTable();

                string filePath = string.Empty;

                if (importdata != null)
                {
                    string path = Server.MapPath("~/Uploads/");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(importdata.FileName);
                    importdata.SaveAs(filePath);
                }

                string extension = Path.GetExtension(importdata.FileName);
                string conStr = "";

                switch (extension)
                {
                    case ".xls": //Excel 97-03
                                 //conStr = "metadata=res://*/Models.Model1.csdl|res://*/Models.Model1.ssdl|res://*/Models.Model1.msl;Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\\Spark 2018\\CRM\\CRM\\Uploads\\;Extended Properties='Text;HDR=NO;IMEX=1';";
                        conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1';";
                        break;
                    case ".xlsx": //Excel 07
                        conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1';";
                        break;
                }

                OleDbConnection connExcel = new OleDbConnection(conStr);
                OleDbCommand cmdExcel = new OleDbCommand();
                OleDbDataAdapter oda = new OleDbDataAdapter();
                dt = new DataTable();
                cmdExcel.Connection = connExcel;

                //Get the name of First Sheet
                connExcel.Open();
                DataTable dtExcelSchema;
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                ///int index_row = CheckHiddenFile(filePath);

                string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();

                connExcel.Close();

                //Read Data from First Sheet
                connExcel.Open();
                cmdExcel.CommandText = "Select * from [" + SheetName + "] ";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dt);
                connExcel.Close();

                string sqlconn = ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString;

                dt.Columns.Add("CREATED_BY", typeof(System.Int32));

                foreach (DataRow row in dt.Rows)
                {
                    row["CREATED_BY"] = (User as GST_Billing.Models.MyPrincipal).User.User_ID;
                }

                dt.Columns[0].ColumnName = "Code";
                dt.Columns[1].ColumnName = "Company_Name";
                dt.Columns[2].ColumnName = "Contact_Person_Name";
                dt.Columns[3].ColumnName = "Mobile_No";
                dt.Columns[4].ColumnName = "Phone_No";
                dt.Columns[5].ColumnName = "Email";
                dt.Columns[6].ColumnName = "Address";
                dt.Columns[7].ColumnName = "Country";
                dt.Columns[8].ColumnName = "State";
                dt.Columns[9].ColumnName = "City";
                dt.Columns[10].ColumnName = "Pin_Code";
                dt.Columns[11].ColumnName = "Website";
                dt.Columns[12].ColumnName = "GSTIN";
                dt.Columns[13].ColumnName = "PAN";
                dt.Columns[14].ColumnName = "LST";
                dt.Columns[15].ColumnName = "TPT";
                dt.Columns[16].ColumnName = "VAT";
                dt.Columns[17].ColumnName = "Opening_Balance";
                dt.Columns[18].ColumnName = "Opening_Type";
                dt.Rows[0].Delete();
                dt.AcceptChanges();

                using (SqlConnection con = new SqlConnection(sqlconn))
                {
                    using (SqlBulkCopy objbulk = new SqlBulkCopy(con))
                    {
                        objbulk.DestinationTableName = "Supplier_Temp";
                        objbulk.ColumnMappings.Add("Code", "Code");
                        objbulk.ColumnMappings.Add("Company_Name", "Company_Name");
                        objbulk.ColumnMappings.Add("Contact_Person_Name", "Contact_Person_Name");
                        objbulk.ColumnMappings.Add("Mobile_No", "Mobile_No");
                        objbulk.ColumnMappings.Add("Phone_No", "Phone_No");
                        objbulk.ColumnMappings.Add("Email", "Email");
                        objbulk.ColumnMappings.Add("Address", "Address");
                        objbulk.ColumnMappings.Add("Country", "Country");
                        objbulk.ColumnMappings.Add("State", "State");
                        objbulk.ColumnMappings.Add("City", "City");
                        objbulk.ColumnMappings.Add("Pin_Code", "Pin_Code");
                        objbulk.ColumnMappings.Add("Website", "Website");
                        objbulk.ColumnMappings.Add("GSTIN", "GSTIN");
                        objbulk.ColumnMappings.Add("PAN", "PAN");
                        objbulk.ColumnMappings.Add("LST", "LST");
                        objbulk.ColumnMappings.Add("TPT", "TPT");
                        objbulk.ColumnMappings.Add("VAT", "VAT");
                        objbulk.ColumnMappings.Add("Opening_Balance", "Opening_Balance");
                        objbulk.ColumnMappings.Add("Opening_Type", "Opening_Type");
                        objbulk.ColumnMappings.Add("CREATED_BY", "Created_By");
                        con.Open();
                        objbulk.WriteToServer(dt);
                        con.Close();

                        objSupplier = new SupplierManagement();
                        objSupplier.User_ID = (User as GST_Billing.Models.MyPrincipal).User.User_ID;
                        objSupplier.Sp_Operation = "Supplier_Import_Excel";
                        dt = new DataTable();
                        dt = objSupplier.Getsupplier();

                        if (Convert.ToString(dt.Rows[0]["Message"]) == "Success")
                        {
                            TempData["Success"] = "Success";
                        }
                        else
                        {
                            TempData["Error"] = Convert.ToString(dt.Rows[0]["Message"]);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "File content not in format...Please check and try again... " + ex.Message;

                objSupplier.Sp_Operation = "Truncate_Temp";
                objSupplier.Getsupplier();
            }

            return View();
        }
        public ActionResult SupplierDetails()
        {
            ViewBag.Title = "Supplier";

            SupplierManagement objSupplier = new SupplierManagement();
            objSupplier.Id = Convert.ToInt32(Request.QueryString["supplierid"]);
            objSupplier.Sp_Operation = "select_report";
            objSupplier.dt = objSupplier.Getsupplier();

            objSupplier.Id = Convert.ToInt32(Request.QueryString["supplierid"]);
            objSupplier.Sp_Operation = "select_bank";
            objSupplier.dt1 = objSupplier.Getsupplier();

            return View(objSupplier);
        }

        //End Supplier Master

        //Start client master

        public ActionResult AddClient()
        {
            ViewBag.Title = "Client";
            ClientManagement objClient = new ClientManagement();

            DataTable dt = new DataTable();

            if (Request.QueryString["clientid"] != null)
            {
                objClient.Sp_Operation = "select";
                objClient.Id = Convert.ToInt32(Request.QueryString["clientid"]);
                dt = objClient.GetClient();

                List<ClientManagement> clientList = new List<ClientManagement>();
                clientList = CommonMethod.ConvertToList<ClientManagement>(dt);
                objClient = clientList[0];

                objClient.Sp_Operation = "select_shipping_address";
                objClient.Id = Convert.ToInt32(Request.QueryString["clientid"]);
                objClient.dt = objClient.GetClient();

            }
            else
            {
                objClient.Sp_Operation = "get_last_Client_code";
                dt = new DataTable();
                dt = objClient.GetClient();

                if (dt.Rows.Count > 0)
                {
                    objClient.Code = Convert.ToString(dt.Rows[0]["Code"]);
                }
                objClient.dt = new DataTable();
            }


            return View(objClient);
        }
        [HttpPost]
        public ActionResult AddClient(ClientManagement objClient,string Addedfrom)
        {
            ViewBag.Title = "Supplier";

            objClient.User_ID = (User as GST_Billing.Models.MyPrincipal).User.User_ID;

            DataTable dt = new DataTable();
            //Add Columns
            dt.Columns.Add("Id");
            dt.Columns.Add("Address");
            dt.Columns.Add("Country");
            dt.Columns.Add("State");
            dt.Columns.Add("City");
            dt.Columns.Add("Pin_Code");
            dt.Columns.Add("Company_Name");
            dt.Columns.Add("GSTIN");
            dt.Columns.Add("Contact_Person");
            dt.Columns.Add("Mobile_No");
            //Add rows
            for (int i = 0; i < objClient.Ship_Address.Count(); i++)
            {
                dt.Rows.Add(objClient.ShippingAddressId[i], objClient.Ship_Address[i], objClient.Ship_Country[i], objClient.Ship_State[i], objClient.Ship_City[i], objClient.Ship_Pin_Code[i], objClient.Ship_Company_Name[i], objClient.Ship_GSTIN[i], objClient.Ship_Contact_Person[i], objClient.Ship_Mobile_No[i]);
            }

            objClient.ShippingAddress = dt;

            if (objClient.Id == 0)
            {
                objClient.Sp_Operation = "insert";
            }
            else
            {
                objClient.Sp_Operation = "update";
            }
            dt = new DataTable();
            dt = objClient.GetClient();

            if (Convert.ToString(dt.Rows[0]["Msg"]) == "Success")
            {

                if (Addedfrom != null)
                {
                    if (Addedfrom == "purchase client")
                    {
                        TempData["AddedFrom"] = "Purchase";
                        Session["Client_Id"] = Convert.ToString(dt.Rows[0]["Client_Id"]);
                        Session["Client_Company_Name"] = objClient.Company_Name + " (" + objClient.Code + ")";
                    }

                    return Redirect("AddSupplier");
                }
                else
                {
                    TempData["Success"] = "Success";
                    return Redirect("Client");
                }
            }
            else
            {
                TempData["Error"] = "Internal server issue please try again";
                return View(objClient);
            }

        }
        [HttpGet]
        public JsonResult IsClientCodeExist(string Code, int Id)
        {
            bool isExist = true;

            ClientManagement objClient = new ClientManagement();
            objClient.Id = Id;
            objClient.Code = Code;
            objClient.Sp_Operation = "check_Client_code";
            DataTable dt = new DataTable();
            dt = objClient.GetClient();

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
        public ActionResult Client()
        {
            ViewBag.Title = "Client";

            return View();
        }
        public ActionResult ClientDataTable()
        {
            ViewBag.Title = "Client";
            ClientManagement objClient = new ClientManagement();

            objClient.Sp_Operation = "select_report_Pagination";
            objClient.dt1 = objClient.GetClient();

            objClient.Sp_Operation = "select_report";
            objClient.dt = objClient.GetClient();

            objClient.Page_No = 1;

            return View(objClient);
        }
        [HttpPost]
        public ActionResult ClientDataTable(ClientManagement objClient)
        {
            ViewBag.Title = "Client";

            objClient.Sp_Operation = "select_report_Pagination";
            objClient.dt1 = objClient.GetClient();

            objClient.Sp_Operation = "select_report";
            objClient.dt = objClient.GetClient();

            if (objClient.Page_No == 0)
                objClient.Page_No = 1;

            Thread.Sleep(1000);

            return View(objClient);
        }
        public ActionResult DeleteClient()
        {
            ViewBag.Title = "Client";

            ClientManagement objClient = new ClientManagement();
            objClient.User_ID = (User as GST_Billing.Models.MyPrincipal).User.User_ID;
            objClient.Sp_Operation = "delete";
            objClient.Id = Convert.ToInt32(Request.QueryString["clientid"]);
            objClient.GetClient();

            return Redirect("Client");
        }
        [HttpPost]
        public ActionResult ClientTableExcelExport(ClientManagement objClient)
        {
            objClient.Sp_Operation = "select_report_export";
            DataTable dt = new DataTable();
            dt = objClient.GetClient();

            dt.TableName = "ClientReport";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=ClientReport.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }

            return View("Client", objClient);
        }
        public ActionResult ImportClient()
        {
            ViewBag.Title = "Client";
            return View();
        }
        [HttpPost]
        public ActionResult ImportClient(HttpPostedFileBase importdata)
        {
            ViewBag.Title = "Client";
            ClientManagement objClient = new ClientManagement();
            objClient.Sp_Operation = "Truncate_Temp";
            objClient.GetClient();

            try
            {
                DataTable dt = new DataTable();

                string filePath = string.Empty;

                if (importdata != null)
                {
                    string path = Server.MapPath("~/Uploads/");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(importdata.FileName);
                    importdata.SaveAs(filePath);
                }

                string extension = Path.GetExtension(importdata.FileName);
                string conStr = "";

                switch (extension)
                {
                    case ".xls": //Excel 97-03
                                 //conStr = "metadata=res://*/Models.Model1.csdl|res://*/Models.Model1.ssdl|res://*/Models.Model1.msl;Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\\Spark 2018\\CRM\\CRM\\Uploads\\;Extended Properties='Text;HDR=NO;IMEX=1';";
                        conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1';";
                        break;
                    case ".xlsx": //Excel 07
                        conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1';";
                        break;
                }

                OleDbConnection connExcel = new OleDbConnection(conStr);
                OleDbCommand cmdExcel = new OleDbCommand();
                OleDbDataAdapter oda = new OleDbDataAdapter();
                dt = new DataTable();
                cmdExcel.Connection = connExcel;

                //Get the name of First Sheet
                connExcel.Open();
                DataTable dtExcelSchema;
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                ///int index_row = CheckHiddenFile(filePath);

                string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();

                connExcel.Close();

                //Read Data from First Sheet
                connExcel.Open();
                cmdExcel.CommandText = "Select * from [" + SheetName + "] ";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dt);
                connExcel.Close();

                string sqlconn = ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString;

                dt.Columns.Add("CREATED_BY", typeof(System.Int32));

                foreach (DataRow row in dt.Rows)
                {
                    row["CREATED_BY"] = (User as GST_Billing.Models.MyPrincipal).User.User_ID;
                }

                dt.Columns[0].ColumnName = "Code";
                dt.Columns[1].ColumnName = "Company_Name";
                dt.Columns[2].ColumnName = "Contact_Person_Name";
                dt.Columns[3].ColumnName = "Mobile_No";
                dt.Columns[4].ColumnName = "Phone_No";
                dt.Columns[5].ColumnName = "Email";
                dt.Columns[6].ColumnName = "Address";
                dt.Columns[7].ColumnName = "Country";
                dt.Columns[8].ColumnName = "State";
                dt.Columns[9].ColumnName = "City";
                dt.Columns[10].ColumnName = "Pin_Code";
                dt.Columns[11].ColumnName = "Website";
                dt.Columns[12].ColumnName = "GSTIN";
                dt.Columns[13].ColumnName = "PAN";
                dt.Columns[14].ColumnName = "LST";
                dt.Columns[15].ColumnName = "TPT";
                dt.Columns[16].ColumnName = "VAT";
                dt.Columns[17].ColumnName = "Opening_Balance";
                dt.Columns[18].ColumnName = "Opening_Type";
                dt.Rows[0].Delete();
                dt.AcceptChanges();

                using (SqlConnection con = new SqlConnection(sqlconn))
                {
                    using (SqlBulkCopy objbulk = new SqlBulkCopy(con))
                    {
                        objbulk.DestinationTableName = "Client_Temp";
                        objbulk.ColumnMappings.Add("Code", "Code");
                        objbulk.ColumnMappings.Add("Company_Name", "Company_Name");
                        objbulk.ColumnMappings.Add("Contact_Person_Name", "Contact_Person_Name");
                        objbulk.ColumnMappings.Add("Mobile_No", "Mobile_No");
                        objbulk.ColumnMappings.Add("Phone_No", "Phone_No");
                        objbulk.ColumnMappings.Add("Email", "Email");
                        objbulk.ColumnMappings.Add("Address", "Address");
                        objbulk.ColumnMappings.Add("Country", "Country");
                        objbulk.ColumnMappings.Add("State", "State");
                        objbulk.ColumnMappings.Add("City", "City");
                        objbulk.ColumnMappings.Add("Pin_Code", "Pin_Code");
                        objbulk.ColumnMappings.Add("Website", "Website");
                        objbulk.ColumnMappings.Add("GSTIN", "GSTIN");
                        objbulk.ColumnMappings.Add("PAN", "PAN");
                        objbulk.ColumnMappings.Add("LST", "LST");
                        objbulk.ColumnMappings.Add("TPT", "TPT");
                        objbulk.ColumnMappings.Add("VAT", "VAT");
                        objbulk.ColumnMappings.Add("Opening_Balance", "Opening_Balance");
                        objbulk.ColumnMappings.Add("Opening_Type", "Opening_Type");
                        objbulk.ColumnMappings.Add("CREATED_BY", "Created_By");
                        con.Open();
                        objbulk.WriteToServer(dt);
                        con.Close();

                        objClient = new ClientManagement();
                        objClient.User_ID = (User as GST_Billing.Models.MyPrincipal).User.User_ID;
                        objClient.Sp_Operation = "Supplier_Import_Excel";
                        dt = new DataTable();
                        dt = objClient.GetClient();

                        if (Convert.ToString(dt.Rows[0]["Message"]) == "Success")
                        {
                            TempData["Success"] = "Success";
                        }
                        else
                        {
                            TempData["Error"] = Convert.ToString(dt.Rows[0]["Message"]);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "File content not in format...Please check and try again... " + ex.Message;

                objClient.Sp_Operation = "Truncate_Temp";
                objClient.GetClient();
            }

            return View();
        }
        public ActionResult ClientDetails()
        {
            ViewBag.Title = "Client";

            ClientManagement objClient = new ClientManagement();
            objClient.Id = Convert.ToInt32(Request.QueryString["clientid"]);
            objClient.Sp_Operation = "select_report";
            objClient.dt = objClient.GetClient();

            objClient.Id = Convert.ToInt32(Request.QueryString["clientid"]);
            objClient.Sp_Operation = "select_shipping_address";
            objClient.dt1 = objClient.GetClient();

            return View(objClient);
        }
        public ActionResult ExpensesType()
        {
            ViewBag.Title = "Expenses";

            ExpensesManagement objExpenses = new ExpensesManagement();
            if (Request.QueryString["etypeid"] != null)
            {
                objExpenses.Id = Convert.ToInt32(Request.QueryString["etypeid"]);
                objExpenses.Sp_Operation = "select_type";
                DataTable dt = new DataTable();
                dt = objExpenses.GetExpenses();

                if (dt.Rows.Count > 0)
                {
                    objExpenses.Type = Convert.ToString(dt.Rows[0]["Type"]);
                    objExpenses.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                }
            }

            return View(objExpenses);
        }
        [HttpPost]
        public ActionResult ExpensesType(ExpensesManagement objExpenses)
        {
            ViewBag.Title = "Expenses";
            objExpenses.User_ID = (User as GST_Billing.Models.MyPrincipal).User.User_ID;

            if (objExpenses.Id == 0)
            {
                objExpenses.Sp_Operation = "insert_type";
            }
            else
            {
                objExpenses.Sp_Operation = "update_type";
            }
            objExpenses.GetExpenses();

            return Redirect("ExpensesType");
        }
        public ActionResult ExpensesTypeDataTable()
        {
            ViewBag.Title = "Expenses";
            ExpensesManagement objExpenses = new ExpensesManagement();

            objExpenses.Sp_Operation = "select_report_Pagination";
            objExpenses.dt1 = objExpenses.GetExpenses();

            objExpenses.Sp_Operation = "select_report";
            objExpenses.dt = objExpenses.GetExpenses();

            objExpenses.Page_No = 1;

            return View(objExpenses);
        }
        [HttpPost]
        public ActionResult ExpensesTypeDataTable(ExpensesManagement objExpenses)
        {
            ViewBag.Title = "Expenses";

            objExpenses.Sp_Operation = "select_report_Pagination";
            objExpenses.dt1 = objExpenses.GetExpenses();

            objExpenses.Sp_Operation = "select_report";
            objExpenses.dt = objExpenses.GetExpenses();

            if (objExpenses.Page_No == 0)
                objExpenses.Page_No = 1;

            Thread.Sleep(1000);

            return View(objExpenses);
        }
        public ActionResult DeleteExpensesType()
        {
            ViewBag.Title = "Expenses";

            ExpensesManagement objExpenses = new ExpensesManagement();
            objExpenses.User_ID = (User as GST_Billing.Models.MyPrincipal).User.User_ID;
            objExpenses.Sp_Operation = "delete_type";
            objExpenses.Id = Convert.ToInt32(Request.QueryString["etypeid"]);
            objExpenses.GetExpenses();

            return Redirect("ExpensesType");
        }

        //User Management

        public ActionResult AddEmployee()
        {
            ViewBag.Title = "Employee";

            UserManagement objUser = new UserManagement();
            DataTable dt = new DataTable();

            if (Request.QueryString["empid"] != null)
            {
                objUser.Sp_Operation = "select_user";
                objUser.User_ID = Convert.ToInt32(Request.QueryString["empid"]);
                dt = new DataTable();
                dt = objUser.GetUser();

                List<UserManagement> userList = new List<UserManagement>();
                userList = CommonMethod.ConvertToList<UserManagement>(dt);
                objUser = userList[0];

                objUser.Sp_Operation = "select_user_permission";
                objUser.User_ID = Convert.ToInt32(Request.QueryString["empid"]);
                objUser.dt = objUser.GetUser();
            }
            else
            {
                objUser.Sp_Operation = "select_user_permission";
                objUser.dt = objUser.GetUser();
            }

            objUser.Sp_Operation = "select_department";
            dt = new DataTable();
            dt = objUser.GetUser();

            objUser.DepartmentList = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objUser.DepartmentList.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Department"].ToString() });
            }

            objUser.Sp_Operation = "select_designation";
            dt = new DataTable();
            dt = objUser.GetUser();

            objUser.DesignationList = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objUser.DesignationList.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Designation"].ToString() });
            }

            return View(objUser);
        }
        [HttpPost]
        public ActionResult AddEmployee(UserManagement ObjUser)
        {
            ViewBag.Title = "Employee";

            ObjUser.Created_By = (User as GST_Billing.Models.MyPrincipal).User.User_ID;

            DataTable dt = new DataTable();
            //Add Columns
            dt.Columns.Add("Id");
            dt.Columns.Add("Application_Module_Id");
            dt.Columns.Add("Read");
            dt.Columns.Add("Create");
            dt.Columns.Add("Modify");
            dt.Columns.Add("Delete");
            //Add rows
            for (int i = 0; i < ObjUser.Read.Count(); i++)
            {
                dt.Rows.Add(ObjUser.UserPermissionId[i], ObjUser.ApplicationModuleId[i], ObjUser.Read[i], ObjUser.Create[i], ObjUser.Modify[i], ObjUser.Delete[i]);
            }
            ObjUser.dtUserPermission = dt;

            if (ObjUser.User_ID == 0)
            {
                ObjUser.Sp_Operation = "insert_user";
            }
            else
            {
                ObjUser.Sp_Operation = "update_user";
            }

            ObjUser.GetUser();
            TempData["Success"] = "Success";

            return Redirect("AddEmployee");
        }
        [HttpGet]
        public ActionResult AddDepartment()
        {
            ViewBag.Title = "Employee";
            Thread.Sleep(500);
            return View();
        }
        [HttpPost]
        public JsonResult AddDepartment(UserManagement ObjUser)
        {
            ViewBag.Title = "Employee";

            ObjUser.Sp_Operation = "insert_department";
            ObjUser.Created_By = (User as GST_Billing.Models.MyPrincipal).User.User_ID;
            DataTable dt = new DataTable();
            dt = ObjUser.GetUser();

            if (dt.Rows.Count > 0)
            {
                ObjUser.Department_Id = Convert.ToInt32(dt.Rows[0]["Id"]);
            }
            Thread.Sleep(500);
            return Json(ObjUser.Department_Id);
        }
        public ActionResult AddDesignation()
        {
            ViewBag.Title = "Employee";
            Thread.Sleep(500);
            return View();
        }
        [HttpPost]
        public JsonResult AddDesignation(UserManagement ObjUser)
        {
            ViewBag.Title = "Employee";

            ObjUser.Sp_Operation = "insert_designation";
            ObjUser.Created_By = (User as GST_Billing.Models.MyPrincipal).User.User_ID;
            DataTable dt = new DataTable();
            dt = ObjUser.GetUser();

            if (dt.Rows.Count > 0)
            {
                ObjUser.Designation_ID = Convert.ToInt32(dt.Rows[0]["Id"]);
            }
            Thread.Sleep(500);
            return Json(ObjUser.Designation_ID);
        }
        public ActionResult Employee()
        {
            ViewBag.Title = "Employee";

            UserManagement ObjUser = new UserManagement();

            ObjUser.Sp_Operation = "select_department";
            DataTable dt = new DataTable();
            dt = ObjUser.GetUser();

            ObjUser.DepartmentList = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ObjUser.DepartmentList.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Department"].ToString() });
            }


            ObjUser.Sp_Operation = "select_designation";
            dt = new DataTable();
            dt = ObjUser.GetUser();

            ObjUser.DesignationList = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ObjUser.DesignationList.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Designation"].ToString() });
            }

            return View(ObjUser);
        }
        public ActionResult EmployeeDataTable()
        {
            ViewBag.Title = "Employee";
            UserManagement ObjUser = new UserManagement();

            ObjUser.Sp_Operation = "select_report_Pagination";
            ObjUser.dt1 = ObjUser.GetUser();

            ObjUser.Sp_Operation = "select_report";
            ObjUser.dt = ObjUser.GetUser();

            ObjUser.Page_No = 1;

            return View(ObjUser);
        }
        [HttpPost]
        public ActionResult EmployeeDataTable(UserManagement ObjUser)
        {
            ViewBag.Title = "Employee";

            ObjUser.Sp_Operation = "select_report_Pagination";
            ObjUser.dt1 = ObjUser.GetUser();

            ObjUser.Sp_Operation = "select_report";
            ObjUser.dt = ObjUser.GetUser();

            if (ObjUser.Page_No == 0)
                ObjUser.Page_No = 1;

            Thread.Sleep(1000);

            return View(ObjUser);
        }
        public ActionResult DeleteEmployee()
        {
            ViewBag.Title = "Employee";

            UserManagement objUser = new UserManagement();
            objUser.Created_By = (User as GST_Billing.Models.MyPrincipal).User.User_ID;
            objUser.Sp_Operation = "delete_user";
            objUser.User_ID = Convert.ToInt32(Request.QueryString["empid"]);
            objUser.GetUser();

            return Redirect("Employee");
        }
    }
}