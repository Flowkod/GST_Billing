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
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

#endregion

namespace GST_Billing.Models
{
    public class UserManagement
    {
        #region "Variable"

        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString);
        SqlDataAdapter sda;
        SqlCommand cmd;

        #endregion

        #region "Property"
        public int Created_By { get; set; }
        public int User_ID { set; get; }
        public int Company_Id { get; set; }
        [Required (ErrorMessage ="Name required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Mobile no required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please enter a valid Mobile no.")]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "Email required")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Please enter a valid Email.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Old Password required")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Confirm Password required")]
        [Compare("Password", ErrorMessage ="Confirm Password and New Password do not match..")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Designation required")]
        public int Designation_ID { get; set; }
        [Required(ErrorMessage = "Department required")]
        public string Department { get; set; }
        [Required(ErrorMessage = "Department required")]
        public int Department_Id { get; set; }
        [Required(ErrorMessage = "Designation required")]
        public string Designation { get; set; }
        public DateTime Joining_Date { get; set; }
        public string Status { get; set; }
        public int[] UserPermissionId { get; set; }
        public int[] ApplicationModuleId { get; set; }
        public bool[] Read { get; set; }
        public bool[] Modify { get; set; }
        public bool[] Create { get; set; }
        public bool[] Delete { get; set; }
        public int Page_No { get; set; }
        public string Sp_Operation { get; set; }
        public bool RememberMe { get; set; }
        public DataTable dt { get; set; }
        public DataTable dt1 { get; set; }
        public DataTable dtUserPermission { get; set; }
        public List<SelectListItem> DepartmentList { get; set; }
        public List<SelectListItem> DesignationList { get; set; }

        #endregion

        #region "Function"
        public void AddwithParameter(SqlCommand Command)
        {
            if (Created_By > 0)
                Command.Parameters.Add(new SqlParameter("@Created_By", SqlDbType.Int)).Value = Created_By;
            else
                Command.Parameters.Add(new SqlParameter("@Created_By", SqlDbType.Int)).Value = DBNull.Value;

            if (User_ID > 0)
                Command.Parameters.Add(new SqlParameter("@User_ID", SqlDbType.Int)).Value = User_ID;
            else
                Command.Parameters.Add(new SqlParameter("@User_ID", SqlDbType.Int)).Value = DBNull.Value;

            if (Company_Id > 0)
                Command.Parameters.Add(new SqlParameter("@Company_Id", SqlDbType.Int)).Value = Company_Id;
            else
                Command.Parameters.Add(new SqlParameter("@Company_Id", SqlDbType.Int)).Value = DBNull.Value;

            if (Name != String.Empty && Name != null)
                Command.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar)).Value = Name;
            else
                Command.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Mobile != String.Empty && Mobile != null)
                Command.Parameters.Add(new SqlParameter("@Mobile", SqlDbType.VarChar)).Value = Mobile;
            else
                Command.Parameters.Add(new SqlParameter("@Mobile", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Email != String.Empty && Email != null)
                Command.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar)).Value = Email;
            else
                Command.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Password != String.Empty && Password != null)
                Command.Parameters.Add(new SqlParameter("@Password", SqlDbType.VarChar)).Value = Password;
            else
                Command.Parameters.Add(new SqlParameter("@Password", SqlDbType.VarChar)).Value = DBNull.Value;

            if (OldPassword != String.Empty && OldPassword != null)
                Command.Parameters.Add(new SqlParameter("@OldPassword", SqlDbType.VarChar)).Value = OldPassword;
            else
                Command.Parameters.Add(new SqlParameter("@OldPassword", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Designation_ID > 0)
                Command.Parameters.Add(new SqlParameter("@Designation_ID", SqlDbType.Int)).Value = Designation_ID;
            else
                Command.Parameters.Add(new SqlParameter("@Designation_ID", SqlDbType.Int)).Value = DBNull.Value;

            if (Designation != string.Empty && Designation != null)
                Command.Parameters.Add(new SqlParameter("@Designation", SqlDbType.VarChar)).Value = Designation;
            else
                Command.Parameters.Add(new SqlParameter("@Designation", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Department_Id > 0)
                Command.Parameters.Add(new SqlParameter("@Department_Id", SqlDbType.Int)).Value = Department_Id;
            else
                Command.Parameters.Add(new SqlParameter("@Department_Id", SqlDbType.Int)).Value = DBNull.Value;

            if (Department != string.Empty && Department != null)
                Command.Parameters.Add(new SqlParameter("@Department", SqlDbType.VarChar)).Value = Department;
            else
                Command.Parameters.Add(new SqlParameter("@Department", SqlDbType.VarChar)).Value = DBNull.Value;

            if (Joining_Date != null && Joining_Date != DateTime.MinValue)
                Command.Parameters.Add(new SqlParameter("@Joining_Date", SqlDbType.Int)).Value = Joining_Date;
            else
                Command.Parameters.Add(new SqlParameter("@Joining_Date", SqlDbType.Int)).Value = DBNull.Value;

            if (Status != string.Empty && Status != null)
                Command.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar)).Value = Status;
            else
                Command.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar)).Value = DBNull.Value;

            if (dtUserPermission != null)
                Command.Parameters.Add(new SqlParameter("@UserPermissions", SqlDbType.Structured)).Value = dtUserPermission;
            else
                Command.Parameters.Add(new SqlParameter("@UserPermissions", SqlDbType.Structured)).Value = null;

            if (Page_No > 0)
                Command.Parameters.Add(new SqlParameter("@Page_No", SqlDbType.Int)).Value = Page_No;
            else
                Command.Parameters.Add(new SqlParameter("@Page_No", SqlDbType.Int)).Value = 1;

            if (Sp_Operation != string.Empty && Sp_Operation != null)
                Command.Parameters.Add(new SqlParameter("@Sp_Operation", SqlDbType.VarChar)).Value = Sp_Operation;
            else
                Command.Parameters.Add(new SqlParameter("@Sp_Operation", SqlDbType.VarChar)).Value = DBNull.Value;
        }

        public DataTable GetUser()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString))
            {
                cmd = new SqlCommand("User_Management", con);
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
    public class UserData
    {
        public int User_ID { set; get; }
        public int Company_Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Designation_ID { get; set; }
    }

    public class MyPrincipal : IPrincipal
    {
        public MyPrincipal(IIdentity identity)
        {
            Identity = identity;
        }
        public IIdentity Identity
        {
            get;
            private set;
        }
        public UserData User { get; set; }
        public bool IsInRole(string role)
        {
            return true;
        }
    }
}