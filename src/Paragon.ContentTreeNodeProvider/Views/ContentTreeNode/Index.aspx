﻿<%@ Import Namespace="Paragon.ContentTreeNodeProvider.Models" %>
<%--<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Paragon.ContentTreeNodeProvider.Models.ContentTreeNodeDisplayViewModel>" %>--%>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Security.Policy" %>

<%--<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">--%>

<h1><%=Model.Header %></h1>

<div><%=Model.Body %></div>

<%--</asp:Content>--%>
