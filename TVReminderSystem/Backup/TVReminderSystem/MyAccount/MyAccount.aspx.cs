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
    public partial class MyAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataAccess info = new DataAccess();

            DataView dv = new DataView();
            dv = info.getUserInfo(HttpContext.Current.User.Identity.Name.ToString());

            lblFirstName.Text = dv[0][0].ToString();
            lblFirstName2.Text = dv[0][0].ToString();
            lblSurname.Text = dv[0][1].ToString();
            lblSurname2.Text = dv[0][1].ToString();
            lblEmail.Text = dv[0][2].ToString();
            lblMobile.Text = dv[0][3].ToString();
        }
        protected void btnEditAccount_Click(object sender, EventArgs e)
        {
            Response.Redirect("/MyAccount/EditAccount.aspx");
        }
    }
}