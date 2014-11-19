<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="TVReminderSystem.Login.Login1" %>
<div id="login_container">
    <table class="control_holder">
        <tr>
            <td class="login_text">
                Email:
            </td>
            <td>
                <asp:TextBox ID="txtEmail" runat="server" class="txtEmail"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="login_text">
                Password:
            </td>
            <td>
                <asp:TextBox ID="txtPass" runat="server" TextMode="password"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="login_text">
            </td>
            <td>
                <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" ValidationGroup="LoginGroup" />
            </td>
        </tr>
    </table>
    <asp:Label ID="lblLoginCheck" runat="server"></asp:Label>
</div>
<asp:RequiredFieldValidator ForeColor="Red" 
    ID="txtEmailValidator" 
    runat="server"
    ErrorMessage="Must Provide an Email Address" 
    ControlToValidate="txtEmail"
    Display="None" 
    ValidationGroup="LoginGroup"/>
<asp:RegularExpressionValidator ID="regexEmailValid" 
    runat="server" 
    ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
    ControlToValidate="txtEmail" 
    ForeColor="Red" 
    ErrorMessage="Invalid Email Format" 
    Text="*"
    Display="None" 
    ValidationGroup="LoginGroup"/>
<asp:RequiredFieldValidator ForeColor="Red" 
    ID="txtPassValidator" 
    runat="server"
    ErrorMessage="Must Provide a Password" 
    ControlToValidate="txtPass"
    Display="None"
    ValidationGroup="LoginGroup" />

<asp:ValidationSummary ID="LoginSummary" 
    runat="server" 
    DisplayMode="BulletList"
    EnableClientScript="true"
    ValidationGroup="LoginGroup"
    HeaderText="Please correct the following errors:"/>
