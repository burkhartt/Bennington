﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Routes.aspx.cs" Inherits="WEBMODEL.Routes" %>
<%@ Import Namespace="System.Web.Routing" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <%foreach (Route route in RouteTable.Routes)%>
        <%{%>
            <%=route.Url %><br />
        <%}%>
    </div>
    </form>
</body>
</html>
