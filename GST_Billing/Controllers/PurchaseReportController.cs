using GST_Billing.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace GST_Billing.Controllers
{
    public class PurchaseReportController : Controller
    {
        // GET: PurchaseReport
        public ActionResult ItemWise()
        {
            PurchaseReportManagement objReport = new PurchaseReportManagement();

            objReport.From_Date = Convert.ToDateTime("1 " + System.DateTime.Now.ToString("MMM yyyy"));
            objReport.To_Date = System.DateTime.Now;

            ItemManagement objItem = new ItemManagement();
            objItem.Sp_Operation = "select";
            DataTable dt = new DataTable();
            dt = objItem.Getitems();

            objReport.ItemList = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objReport.ItemList.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Item_Name"].ToString() });
            }

            SupplierManagement objSupplier = new SupplierManagement();
            objSupplier.Sp_Operation = "select";
            dt = new DataTable();
            dt = objSupplier.Getsupplier();

            objReport.SupplierList = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objReport.SupplierList.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Company_Name"].ToString() });
            }

            objReport.Sp_Operation = "Item_Wise_Report";
            dt = new DataTable();
            dt = objReport.Getreport();

            var rptviewer = new ReportViewer();
            rptviewer.ProcessingMode = ProcessingMode.Local;
            rptviewer.LocalReport.ReportPath = Server.MapPath("~/PurchaseReport/ItemWise.rdlc");
            ReportDataSource rptdatasource = new ReportDataSource("DataSet1", dt);
            rptviewer.LocalReport.DataSources.Add(rptdatasource);
            rptviewer.SizeToReportContent = true;
            rptviewer.PageCountMode = PageCountMode.Actual;
            ViewBag.ReportViewer = rptviewer;

            return View(objReport);
        }
        [HttpPost]
        public ActionResult ItemWise(PurchaseReportManagement objReport)
        {
            objReport.Sp_Operation = "Item_Wise_Report";
            DataTable dt = new DataTable();
            dt = objReport.Getreport();

            var rptviewer = new ReportViewer();
            rptviewer.ProcessingMode = ProcessingMode.Local;
            rptviewer.LocalReport.ReportPath = Server.MapPath("~/PurchaseReport/ItemWise.rdlc");
            ReportDataSource rptdatasource = new ReportDataSource("DataSet1", dt);
            rptviewer.LocalReport.DataSources.Add(rptdatasource);
            rptviewer.SizeToReportContent = true;
            rptviewer.PageCountMode = PageCountMode.Actual;
            ViewBag.ReportViewer = rptviewer;
            Thread.Sleep(1000);
            return View("ReportView");
        }

        public ActionResult SupplierWise()
        {
            PurchaseReportManagement objReport = new PurchaseReportManagement();

            objReport.From_Date = Convert.ToDateTime("1 " + System.DateTime.Now.ToString("MMM yyyy"));
            objReport.To_Date = System.DateTime.Now;

            ItemManagement objItem = new ItemManagement();
            objItem.Sp_Operation = "select";
            DataTable dt = new DataTable();
            dt = objItem.Getitems();

            objReport.ItemList = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objReport.ItemList.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Item_Name"].ToString() });
            }

            SupplierManagement objSupplier = new SupplierManagement();
            objSupplier.Sp_Operation = "select";
            dt = new DataTable();
            dt = objSupplier.Getsupplier();

            objReport.SupplierList = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objReport.SupplierList.Add(new SelectListItem() { Value = dt.Rows[i]["Id"].ToString(), Text = dt.Rows[i]["Company_Name"].ToString() });
            }

            objReport.Sp_Operation = "Supplier_Wise_Report";
            dt = new DataTable();
            dt = objReport.Getreport();

            var rptviewer = new ReportViewer();
            rptviewer.ProcessingMode = ProcessingMode.Local;
            rptviewer.LocalReport.ReportPath = Server.MapPath("~/PurchaseReport/SupplierWise.rdlc");
            ReportDataSource rptdatasource = new ReportDataSource("DataSet1", dt);
            rptviewer.LocalReport.DataSources.Add(rptdatasource);
            rptviewer.SizeToReportContent = true;
            rptviewer.AsyncRendering = true;
            rptviewer.PageCountMode = PageCountMode.Actual;
           

            ViewBag.ReportViewer = rptviewer;

            return View(objReport);
        }
        [HttpPost]
        public ActionResult SupplierWise(PurchaseReportManagement objReport)
        {
            objReport.Sp_Operation = "Supplier_Wise_Report";
            DataTable dt = new DataTable();
            dt = objReport.Getreport();

            var rptviewer = new ReportViewer();
            rptviewer.ProcessingMode = ProcessingMode.Local;
            rptviewer.LocalReport.ReportPath = Server.MapPath("~/PurchaseReport/SupplierWise.rdlc");
            ReportDataSource rptdatasource = new ReportDataSource("DataSet1", dt);
            rptviewer.LocalReport.DataSources.Add(rptdatasource);
            rptviewer.SizeToReportContent = true;
            rptviewer.AsyncRendering = true;
            rptviewer.PageCountMode = PageCountMode.Actual;
            

            ViewBag.ReportViewer = rptviewer;
            Thread.Sleep(1000);
            return View("ReportView");
        }

        [HttpPost]
        public ActionResult ReportView()
        {
            
            return View();
        }
    }
}
