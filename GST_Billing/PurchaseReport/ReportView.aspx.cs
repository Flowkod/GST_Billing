using GST_Billing.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GST_Billing.PurchaseReport
{
    public partial class ReportView : System.Web.UI.Page
    {
        public void ItemWise()
        {
            PurchaseReportManagement objPurchase = new PurchaseReportManagement();


            objPurchase.Item_Id = (Request.QueryString["Item_Id"] == null) ? 0 : int.Parse(Request.QueryString["Item_Id"]);
            objPurchase.Supplier_Id = (Request.QueryString["Supplier_Id"] == null) ? 0 : int.Parse(Request.QueryString["Supplier_Id"]);
            objPurchase.From_Date = (Request.QueryString["From_Date"] == null) ? new DateTime(0) : Convert.ToDateTime(Request.QueryString["From_Date"]);
            objPurchase.From_Date = (Request.QueryString["To_Date"] == null) ? new DateTime(0) : Convert.ToDateTime(Request.QueryString["To_Date"]);
            objPurchase.Page_No = (Request.QueryString["Page_no"] == null) ? 0 : int.Parse(Request.QueryString["Page_no"]);
            objPurchase.Sp_Operation = "Item_Wise_Report";
            DataTable dt = new DataTable();
            dt = objPurchase.Getreport();

            ReportViewer1.SizeToReportContent = true;
            ReportViewer1.LocalReport.EnableExternalImages = true;

            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/PurchaseReport/ItemWise.rdlc");

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource _rsource = new ReportDataSource("DataSet1", dt);
            ReportViewer1.LocalReport.DataSources.Add(_rsource);

            ReportViewer1.AsyncRendering = false;
            ReportViewer1.SizeToReportContent = true;
            ReportViewer1.ZoomMode = ZoomMode.FullPage;

            ReportViewer1.LocalReport.Refresh();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                ItemWise();
            }
        }
    }
}