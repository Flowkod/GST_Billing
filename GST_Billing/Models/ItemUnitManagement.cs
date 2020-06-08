using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

#region "Additional Name space"

using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Web.Mvc;

#endregion

namespace GST_Billing.Models
{
    public class ItemUnitManagement
    {
        #region "Variable"

        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString);
        SqlDataAdapter sda;
        SqlCommand cmd;

        #endregion

        #region "Property"
        public int Id { set; get; }
        public int UOM_Unit { get; set; }
        public int User_ID { set; get; }
        [Required (ErrorMessage ="Unit Name required")]
        public string Unit_Name { get; set; }
        public string Sp_Operation { get; set; }
        public DataTable dt { get; set; }
        public List<SelectListItem> Item_Units_List { get; set; }
        #endregion

        #region "Function"
        public void AddwithParameter(SqlCommand Command)
        {
            if (Id > 0)
                Command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = Id;
            else
                Command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = DBNull.Value;

            if (User_ID > 0)
                Command.Parameters.Add(new SqlParameter("@User_ID", SqlDbType.Int)).Value = User_ID;
            else
                Command.Parameters.Add(new SqlParameter("@User_ID", SqlDbType.Int)).Value = DBNull.Value;

            if (Unit_Name != String.Empty && Unit_Name != null)
                Command.Parameters.Add(new SqlParameter("@Unit_Name", SqlDbType.VarChar)).Value = Unit_Name;
            else
                Command.Parameters.Add(new SqlParameter("@Unit_Name", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Sp_Operation != string.Empty && Sp_Operation != null)
                Command.Parameters.Add(new SqlParameter("@Sp_Operation", SqlDbType.VarChar)).Value = Sp_Operation;
            else
                Command.Parameters.Add(new SqlParameter("@Sp_Operation", SqlDbType.VarChar)).Value = DBNull.Value;
        }
        public DataTable GetItemUnit()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString))
            {
                cmd = new SqlCommand("ItemUnitManagement", con);
                AddwithParameter(cmd);
                DataTable DtUser = new DataTable();

                cmd.CommandType = CommandType.StoredProcedure;
                sda = new SqlDataAdapter(cmd);
                con.Open();
                sda.Fill(DtUser);

                return DtUser;
            }
        }
        #endregion       
    }
}