using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccessLayer;
using System.Web.Security;

namespace TVReminderSystem.Login
{
    public partial class Signup : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblSignUpCheck.Text = "";
        }

        protected void btnSignup_Click(object sender, EventArgs e)
        {
            //Check if they have entered a mobile number
            if (validNumber(txtSignUpMobile.Text))
            {
                //Continue with Sign up
                DataAccess signup = new DataAccess();
                //If the email already exists output error 
                if (signup.emailExists(txtSignUpEmail.Text))
                {
                    lblSignUpCheck.Text = "This email already exists in the database";
                }
                //otherwise carry on with signup
                else
                {
                    //If the mobile number already exists, output error message
                    if (txtSignUpMobile.Text != string.Empty && signup.mobileExists(txtSignUpMobile.Text))
                    {
                        lblSignUpCheck.Text = "This mobile number already exists in the database";
                    }
                    //Otherwsie mobile number doesn't exist to carry on with sign up
                    else
                    {
                        //If signed up successfully, login automatically and redirect
                        if (signup.signUp(txtSignUpFirstName.Text, txtSignUpSurname.Text, txtSignUpEmail.Text, txtSignUpMobile.Text, txtSignUpPass.Text))
                        {
                            FormsAuthentication.RedirectFromLoginPage(signup.getID(txtSignUpEmail.Text), true);
                        }

                    }

                }

            }
            else
            {
                //Change label to show error in mobile number format
                lblSignUpCheck.Text = "Invalid Mobile number format. Must be a valid length and start with a + symbol";
            }
        }
        protected bool validNumber(string number)
        {
            //Check if they have entered anything
            if (number != string.Empty)
            {
                //If they have eneter a value then chick it starts with a +
                if (number.StartsWith("+"))
                {
                    //Check that it is a numerical value
                    string Str = number.Substring(1).Trim();
                    double d;
                    bool isNum = double.TryParse(Str, out d);
                    //Check that it was able to parse into a number and is longer than or equal to 11 characters
                    if (isNum && Str.Length >= 11) { return true; }
                    else { return false; }
                }
                //Doesn't start with a +
                else { return false; }
            }
            //They haven't entered a number so continue
            else { return true; }
        }
    }
}