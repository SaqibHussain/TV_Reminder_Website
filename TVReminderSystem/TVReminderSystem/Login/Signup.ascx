<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Signup.ascx.cs" Inherits="TVReminderSystem.Login.Signup" %>
<table class="control_holder">
        <tr>
            <td class="login_text">
                First Name:
            </td>
            <td>
                <asp:TextBox ID="txtSignUpFirstName" runat="server" CssClass="txtEmail"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="login_text">
                Surname:
            </td>
            <td>
                <asp:TextBox ID="txtSignUpSurname" runat="server" CssClass="txtEmail"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td class="login_text">
               Email:
            </td>
            <td>
                <asp:TextBox ID="txtSignUpEmail" runat="server" CssClass="txtEmail"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td class="login_text">
               Mobile number with Country Code:
               <br />
               (e.g. +4478146904058)
            </td>
            <td>
                <asp:TextBox ID="txtSignUpMobile" runat="server" CssClass="txtEmail"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td class="login_text">
               Password:
            </td>
            <td>
                <asp:TextBox ID="txtSignUpPass" runat="server" CssClass="txtEmail" TextMode="password"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="login_text">
            </td>
            <td>
                <asp:Button ID="btnSignup" runat="server" Text="Sign up" OnClick="btnSignup_Click" ValidationGroup="SignUpGroup"/>
            </td>
        </tr>
    </table>
       <asp:Label ID="lblSignUpCheck" runat="server"></asp:Label>

        <asp:RequiredFieldValidator ForeColor="Red" 
    ID="RFVFirstName" 
    runat="server"
    ErrorMessage="Must Provide a First Name" 
    ControlToValidate="txtSignUpFirstName"
    Display="None" 
    ValidationGroup="SignUpGroup"/>

        <asp:RequiredFieldValidator ForeColor="Red" 
    ID="RFVSurname" 
    runat="server"
    ErrorMessage="Must Provide a Surname" 
    ControlToValidate="txtSignUpSurname"
    Display="None" 
    ValidationGroup="SignUpGroup"/>

    <asp:RequiredFieldValidator ForeColor="Red" 
    ID="RFVEmail" 
    runat="server"
    ErrorMessage="Must Provide an Email Address" 
    ControlToValidate="txtSignUpEmail"
    Display="None" 
    ValidationGroup="SignUpGroup"/>
   
    <asp:RegularExpressionValidator ID="RegexEmail" 
    runat="server" 
    ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
    ControlToValidate="txtSignUpEmail" 
    ForeColor="Red" 
    ErrorMessage="Invalid Email Format" 
    Text="*"
    Display="None" 
    ValidationGroup="SignUpGroup"/>

<%--    <asp:RegularExpressionValidator ID="RegexMobile" 
    runat="server" 
    ValidationExpression="^[\s\S]{11,11}$"
    ControlToValidate="txtSignUpMobile" 
    ForeColor="Red" 
    ErrorMessage="Invalid Mobile Number Length" 
    Text="*"
    Display="None" 
    ValidationGroup="SignUpGroup"/>--%>

<asp:RequiredFieldValidator ForeColor="Red" 
    ID="RFVPassword" 
    runat="server"
    ErrorMessage="Must Provide a Password" 
    ControlToValidate="txtSignUpPass"
    Display="None"
    ValidationGroup="SignUpGroup" />

<asp:ValidationSummary ID="SignUpSummary" 
 ValidationGroup="SignUpGroup"
    runat="server" 
    DisplayMode="BulletList"
    EnableClientScript="true"
    HeaderText="Please correct the following errors:"/>
