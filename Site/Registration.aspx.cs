using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Registration : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnRegister_Click(object sender, EventArgs e)
    {
        if (Authorization.User.RegisterUser(txtEmailLogin.Text, txtPassword.Text, txtPhone.Text))
            Response.Redirect("~/success-registration");
        else Response.Redirect("~/failed-registration");
    }
}