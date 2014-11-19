using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccessLayer;
using System.Data;

namespace TVReminderSystem.MyAccount
{
    public partial class EditAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblUpdate.Text = "";
            if (!IsPostBack)
            {
                DataAccess info = new DataAccess();

                DataView dv = new DataView();
                dv = info.getUserInfo(HttpContext.Current.User.Identity.Name.ToString());

                tbFirstName.Text = dv[0][0].ToString();
                tbSurname.Text = dv[0][1].ToString();
                tbEmail.Text = dv[0][2].ToString();
                tbMobile.Text = dv[0][3].ToString();
                
            }
        }

        protected void btnEditAccount_Click(object sender, EventArgs e)
        {
            DataAccess update = new DataAccess();

            //If the email already exists in the database
            if (update.emailExists(update.getEmail(HttpContext.Current.User.Identity.Name.ToString()), tbEmail.Text))
            {
                lblUpdate.Text = "This email address already exists in the database";
            }
            //Otherwise the email doesn't already exist so continue with the update
            else
            {
                //If the mobile number already exists in the database
                if (update.mobileExists(update.getMobile(HttpContext.Current.User.Identity.Name.ToString()), tbMobile.Text))
                {
                    lblUpdate.Text = "This mobile number already exists in the database";
                }
                //otherwise the mobile number doesn't exist in the database so continue with the update
                else
                {
                    update.updateUser(HttpContext.Current.User.Identity.Name.ToString(), tbFirstName.Text, tbSurname.Text, tbEmail.Text, tbMobile.Text);
                    Response.Redirect("/MyAccount/MyAccount.aspx");
                }
            }
            //info.updateUser(HttpContext.Current.User.Identity.Name.ToString(), ddlTitle.SelectedValue,
                //tbFirstName.Text, tbSurname.Text, tbEmail.Text, tbPhone.Text, tbMobile.Text, tbDateOfBirth.Text, Convert.ToInt32(ddlCountry.SelectedItem.Value));
           // Response.Redirect("/MyAccount/MyAccount.aspx");
        }
    }

}