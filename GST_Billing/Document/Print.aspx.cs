using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GST_Billing.Models;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.Drawing;

namespace GST_Billing.rdlc
{
    public partial class ViewRdlc : System.Web.UI.Page
    {
        string DocumentName = string.Empty;
        public void Getpurchaseorder()
        {
            PurchaseOrderManagement objPurchase = new PurchaseOrderManagement();
            objPurchase.Id = Convert.ToInt32(Request.QueryString["purchaseid"]);

            DataTable dt = new DataTable();
            objPurchase.Sp_Operation = "select_Purchase_Action";
            dt = objPurchase.Getpurchaseorder();


            DataTable dt1 = new DataTable();
            objPurchase.Sp_Operation = "select_PO_Items_for_PO";
            dt1 = objPurchase.Getpurchaseorder();

            DataTable dt2 = new DataTable();
            objPurchase.Sp_Operation = "select_for_po";
            dt2 = objPurchase.Getpurchaseorder();

            DocumentName = Convert.ToString(dt2.Rows[0]["Company_Name"])+" PO"+ Convert.ToString(dt2.Rows[0]["PO_No"]);

            NoInWord obj = new NoInWord();
            string amountinworld = obj.NumWordsWrapper(Convert.ToInt32(dt2.Rows[0]["Gross_Total"]));
            dt2.Columns.Add("Total_In_Word", typeof(System.String));
            dt2.Rows[0]["Total_In_Word"] = amountinworld;

            CompanyDetailsManagement objCom = new CompanyDetailsManagement();
            objCom.Company_Id = (User as GST_Billing.Models.MyPrincipal).User.Company_Id;
            objCom.Sp_Operation = "select_for_print";
            DataTable dt3 = new DataTable();           
            dt3 = objCom.Getcompanydetails();

            //dt3.Rows[0]["Company_Logo"] ="http://"+ HttpContext.Current.Request.Url.Authority+ Convert.ToString(dt3.Rows[0]["Company_Logo"]).Replace("..","");


            ReportViewer1.SizeToReportContent = true;
            ReportViewer1.LocalReport.EnableExternalImages = true;

            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Document/PurchaseOrder.rdlc");           

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource _rsource = new ReportDataSource("DataSet1", dt);
            ReportViewer1.LocalReport.DataSources.Add(_rsource);

            ReportDataSource _rsource1 = new ReportDataSource("DataSet2", dt1);
            ReportViewer1.LocalReport.DataSources.Add(_rsource1);

            ReportDataSource _rsource2 = new ReportDataSource("DataSet3", dt2);
            ReportViewer1.LocalReport.DataSources.Add(_rsource2);

            ReportDataSource _rsource3 = new ReportDataSource("DataSet4", dt3);
            ReportViewer1.LocalReport.DataSources.Add(_rsource3);

            ReportViewer1.LocalReport.Refresh();
        }
        public void Getquotation()
        {
            QuotationManagement objQuote = new QuotationManagement();
            objQuote.Id = Convert.ToInt32(Request.QueryString["Quotationid"]);

            DataTable dt = new DataTable();
            objQuote.Sp_Operation = "select_Quote_Action";
            dt = objQuote.Getquotation();


            DataTable dt1 = new DataTable();
            objQuote.Sp_Operation = "select_Quote_Items_for_quote";
            dt1 = objQuote.Getquotation();

            DataTable dt2 = new DataTable();
            objQuote.Sp_Operation = "select_for_Quote";
            dt2 = objQuote.Getquotation();

            DocumentName = Convert.ToString(dt2.Rows[0]["Company_Name"]) + " Quotation" + Convert.ToString(dt2.Rows[0]["Quotation_No"]);

            NoInWord obj = new NoInWord();
            string amountinworld = obj.NumWordsWrapper(Convert.ToInt32(dt2.Rows[0]["Gross_Total"]));
            dt2.Columns.Add("Total_In_Word", typeof(System.String));
            dt2.Rows[0]["Total_In_Word"] = amountinworld;

            CompanyDetailsManagement objCom = new CompanyDetailsManagement();
            objCom.Company_Id = (User as GST_Billing.Models.MyPrincipal).User.Company_Id;
            objCom.Sp_Operation = "select_for_print";
            DataTable dt3 = new DataTable();
            dt3 = objCom.Getcompanydetails();

            //dt3.Rows[0]["Company_Logo"] ="http://"+ HttpContext.Current.Request.Url.Authority+ Convert.ToString(dt3.Rows[0]["Company_Logo"]).Replace("..","");


            ReportViewer1.SizeToReportContent = true;
            ReportViewer1.LocalReport.EnableExternalImages = true;

            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Document/Quotation.rdlc");

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource _rsource = new ReportDataSource("DataSet1", dt);
            ReportViewer1.LocalReport.DataSources.Add(_rsource);

            ReportDataSource _rsource1 = new ReportDataSource("DataSet2", dt1);
            ReportViewer1.LocalReport.DataSources.Add(_rsource1);

            ReportDataSource _rsource2 = new ReportDataSource("DataSet3", dt2);
            ReportViewer1.LocalReport.DataSources.Add(_rsource2);

            ReportDataSource _rsource3 = new ReportDataSource("DataSet4", dt3);
            ReportViewer1.LocalReport.DataSources.Add(_rsource3);

            ReportViewer1.LocalReport.Refresh();
        }
        public void Getinvoice()
        {
            InvoiceManagement objInv = new InvoiceManagement();
            objInv.Id = Convert.ToInt32(Request.QueryString["Invoiceid"]);

            DataTable dt = new DataTable();
            objInv.Sp_Operation = "select_Invoice_Preferences";
            dt = objInv.GetInvoice();


            DataTable dt1 = new DataTable();
            objInv.Sp_Operation = "select_Invoice_Items_for_Invoice";
            dt1 = objInv.GetInvoice();

            DataTable dt2 = new DataTable();
            objInv.Sp_Operation = "select_for_invoice";
            dt2 = objInv.GetInvoice();

            DocumentName = Convert.ToString(dt2.Rows[0]["Company_Name"]) + " Invoice" + Convert.ToString(dt2.Rows[0]["Invoice_No"]);

            NoInWord obj = new NoInWord();
            string amountinworld = obj.NumWordsWrapper(Convert.ToInt32(dt2.Rows[0]["Gross_Total"]));
            dt2.Columns.Add("Total_In_Word", typeof(System.String));
            dt2.Rows[0]["Total_In_Word"] = amountinworld;

            CompanyDetailsManagement objCom = new CompanyDetailsManagement();
            objCom.Company_Id = (User as GST_Billing.Models.MyPrincipal).User.Company_Id;
            objCom.Sp_Operation = "select_for_print";
            DataTable dt3 = new DataTable();
            dt3 = objCom.Getcompanydetails();

            //dt3.Rows[0]["Company_Logo"] ="http://"+ HttpContext.Current.Request.Url.Authority+ Convert.ToString(dt3.Rows[0]["Company_Logo"]).Replace("..","");


            ReportViewer1.SizeToReportContent = true;
            ReportViewer1.LocalReport.EnableExternalImages = true;

            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Document/Invoice.rdlc");

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource _rsource = new ReportDataSource("DataSet1", dt);
            ReportViewer1.LocalReport.DataSources.Add(_rsource);

            ReportDataSource _rsource1 = new ReportDataSource("DataSet2", dt1);
            ReportViewer1.LocalReport.DataSources.Add(_rsource1);

            ReportDataSource _rsource2 = new ReportDataSource("DataSet3", dt2);
            ReportViewer1.LocalReport.DataSources.Add(_rsource2);

            ReportDataSource _rsource3 = new ReportDataSource("DataSet4", dt3);
            ReportViewer1.LocalReport.DataSources.Add(_rsource3);

            ReportViewer1.LocalReport.Refresh();
        }
        public void ExportPdf()
        {
            Warning[] warnings;
            string[] streamIds;
            string mimeType = "application/pdf";
            string encoding = string.Empty;
            string extension = string.Empty;

            byte[] bytes = ReportViewer1.LocalReport.Render(
                "PDF",
                null,
                out mimeType,
                out encoding,
                out extension,
                out streamIds,
                out warnings);

            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;

            Response.AppendHeader("Content-Disposition", "inline; filename="+ DocumentName + ".pdf");

            //using (var fs = new System.IO.FileStream(Server.MapPath("~/Invoices/" + Convert.ToInt32(Request.QueryString["invoiceno"]) + ".pdf"), System.IO.FileMode.Create))
            //{
            //    fs.Write(bytes, 0, bytes.Length);
            //    fs.Close();
            //}

            Response.OutputStream.Write(bytes, 0, bytes.Length);
            Response.Flush();
            Response.End();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (Request.QueryString["purchaseid"] != null)
                {
                    Getpurchaseorder();
                }

                if (Request.QueryString["Quotationid"] != null)
                {
                    Getquotation();
                }

                if (Request.QueryString["Invoiceid"] != null)
                {
                    Getinvoice();
                }
            }

            if (Request.QueryString["DocumentType"] == "pdf")
            {
                ExportPdf();
            }
        }
    }
}