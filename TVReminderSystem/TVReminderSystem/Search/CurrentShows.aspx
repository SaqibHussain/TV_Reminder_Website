<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="CurrentShows.aspx.cs" Inherits="TVReminderSystem.Search.CurrentShows" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="middle_content">
Here you can view shows that other users already subcribe to:
<asp:GridView ID="GridView1" runat="server" BorderColor="#FF8C0D" BorderStyle="None"
                ShowHeader="true" BorderWidth="1px" CellPadding="3" 
        GridLines="Horizontal" OnPageIndexChanging="GridView1_PageIndexChanging"
                Width="900" >
                <AlternatingRowStyle BackColor="#F7F7F7" />
                <PagerStyle BackColor="#FFFFFF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                <RowStyle BackColor="#FFFFFF" ForeColor="#575757" BorderColor="#f5e74d" />
                <PagerSettings FirstPageText="First" Visible="true" />
                <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                <SortedAscendingCellStyle BackColor="#F4F4FD" />
                <SortedAscendingHeaderStyle BackColor="#5A4C9D" />
                <SortedDescendingCellStyle BackColor="#D8D8F0" />
                <SortedDescendingHeaderStyle BackColor="#3E3277" />
                <Columns>
                    <asp:TemplateField SortExpression="Sort" ControlStyle-Width="150" HeaderText="Show Name">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("ShowName") %>'></asp:Label>

                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Sort" ControlStyle-Width="150" HeaderText="Show Status">
                        <ItemTemplate>
                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Sort" ControlStyle-Width="150" HeaderText="Number of Subscribers">
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" CssClass="test" Text='<%# Eval("No Of Subscriptions") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Sort" ControlStyle-Width="150">
                        <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" Text="Subscribe to this show" OnCommand="LinkButton1_Command"
                                CommandName="Details" CommandArgument='<%#Eval("ShowID") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
</div>

</asp:Content>
