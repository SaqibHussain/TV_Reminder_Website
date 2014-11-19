using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccessLayer;
using System.Web.Security;

namespace TVReminderSystem
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                DataAccess info = new DataAccess();
               lblLoginStatus.Text = "Welcome, " + info.getUserName(HttpContext.Current.User.Identity.Name.ToString());
            }
            else
            {
                lblLoginStatus.Text = "Not logged in";
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string s = tbSearch.Text;
            Response.Redirect("/Search/Search.aspx?searchterm=" + s);
        }
    }
}