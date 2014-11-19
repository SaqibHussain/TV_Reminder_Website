function HideContent(d) {
    if (d.length < 1) { return; }
    document.getElementById(d).style.display = "none";
}
function ShowContent(d) {
    if (d.length < 1) { return; }
    document.getElementById(d).style.display = "block";
}

//Shows and hides the bottom row of links depending on which page you are viewing
function showLinks() {
    if (location.href.indexOf('Default') != -1) { document.getElementById("Home").style.borderBottom = "2px solid #00BCF2"; ShowContent('Home_subnav'); HideContent('Search_subnav'); HideContent('My_Account_subnav'); }
    if (location.href.indexOf('/Home/') != -1) { document.getElementById("Home").style.borderBottom = "2px solid #00BCF2"; ShowContent('Home_subnav'); HideContent('Search_subnav'); HideContent('My_Account_subnav'); }
    if (location.href.indexOf('/Search/') != -1) { document.getElementById("Search").style.borderBottom = "2px solid #FF8C0D"; HideContent('Home_subnav'); ShowContent('Search_subnav'); HideContent('My_Account_subnav'); }
    if (location.href.indexOf('/MyAccount/') != -1) { document.getElementById("My_Account").style.borderBottom = "2px solid #f00"; HideContent('Home_subnav'); HideContent('Search_subnav'); ShowContent('My_Account_subnav'); }
    if (location.href.indexOf('/Login/') != -1) { document.getElementById("My_Account").style.borderBottom = "2px solid #f00"; HideContent('Home_subnav'); HideContent('Search_subnav');  ShowContent('My_Account_subnav'); }
}
//Handles colour underlining for the bottom row of links
function linkColours() {
    if (location.href.indexOf('Default.aspx') != -1) { document.getElementById("home_subnav_home").style.borderBottom = "2px solid #00BCF2"; }
    if (location.href.indexOf('About.aspx') != -1) { document.getElementById("home_subnav_about").style.borderBottom = "2px solid #00BCF2"; }
    if (location.href.indexOf('Help.aspx') != -1) { document.getElementById("home_subnav_help").style.borderBottom = "2px solid #00BCF2"; }
    if (location.href.indexOf('ContactUs.aspx') != -1) { document.getElementById("home_subnav_contactus").style.borderBottom = "2px solid #00BCF2"; }

    if (location.href.indexOf('Search.aspx') != -1) { document.getElementById("XboX_subnav_Search").style.borderBottom = "2px solid #FF8C0D"; }
    if (location.href.indexOf('CurrentShows.aspx') != -1) { document.getElementById("XboX_subnav_CurrentShows").style.borderBottom = "2px solid #FF8C0D"; }
   
    if (location.href.indexOf('MyAccount.aspx') != -1) { document.getElementById("MyAccounts_subnav_MyAccounts").style.borderBottom = "2px solid #f00"; }
    if (location.href.indexOf('MyShows.aspx') != -1) { document.getElementById("MyAccounts_subnav_MyShows").style.borderBottom = "2px solid #f00"; }
}