using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccessLayer;
using System.Data;
using System.Web.Security;

namespace TVReminderSystem.Search
{
    public partial class Search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Setting up the grid view
                GridView1.PageSize = 25;
                GridView1.AutoGenerateColumns = false;
                GridView1.AllowPaging = true;
            }

            string searchterm = Request.QueryString["searchterm"];

            if (searchterm != null)
            {
                DataAccess search = new DataAccess();
                DataTable dt = search.searchShows(searchterm);

                GridView1.DataSource = dt;
                GridView1.DataBind();

                if (dt.Rows.Count == 0)
                {
                    lblResults.Text = "No results found!";
                }
            }
            else
            {
                lblResults.Text = "Please enter a search term in the search box above and hit search";
            }




        }

        //Method to handle page index change
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Set the page of the grid view to the one selected
            GridView1.PageIndex = e.NewPageIndex;
            //Bind to show changes
            GridView1.DataBind();
        }

        protected void LinkButton1_Command(object sender, CommandEventArgs e)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                LinkButton lnk = (LinkButton)sender;
                DataAccess search = new DataAccess();
                lnk.Text = search.trySubscribe(e.CommandArgument.ToString(), HttpContext.Current.User.Identity.Name.ToString()).ToString();
            }
            else
            {
                Response.Redirect("/Login/Login.aspx");
            }
        }
    }
}