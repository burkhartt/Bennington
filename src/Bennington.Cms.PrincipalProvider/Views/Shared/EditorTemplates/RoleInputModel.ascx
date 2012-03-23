<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Bennington.Cms.PrincipalProvider.Models.RoleInputModel>" %>


<link rel="stylesheet" type="text/css" media="screen" href="<%=Url.Content("~/Content/ui.multiselect.css")%>" />
<script type="text/javascript" src="<%=Url.Content("~/Scripts/ui.multiselect.js")%>"></script>

<script type="text/javascript">
    $(function () {
        $(".multiselect").each(function () { $(this).multiselect({ sortable: false, searchable: false }) });
    });
</script>


        <%: Html.ValidationSummary(true) %>
        
        <fieldset>
           <%: Html.HiddenFor(model => model.Id) %>            
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Inactive) %>
            </div>
            <div class="editor-field">
                <%: Html.CheckBoxFor(model => model.Inactive)%>
                <%: Html.ValidationMessageFor(model => model.Inactive)%>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Name) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Name)%>
                <%: Html.ValidationMessageFor(model => model.Name)%>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.AllContent) %>
            </div>
            <div class="editor-field">
                <%: Html.CheckBoxFor(model => model.AllContent)%>
                <%: Html.ValidationMessageFor(model => model.AllContent)%>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.AvailableContentSections) %>
            </div>
            <div class="editor-field">
                <%: Html.ListBoxFor(m => m.AvailableContentSections, new MultiSelectList(Model.ContentSections, "Value", "Text"), new { @class = "multiselect" })%>
                <%: Html.ValidationMessageFor(model => model.AvailableContentSections)%>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.AvailableModules) %>
            </div>
            <div class="editor-field">
                <%: Html.ListBoxFor(m => m.AvailableModules, new MultiSelectList(Model.Modules, "Value", "Text"), new {@class="multiselect"}) %>
                <%: Html.ValidationMessageFor(model => model.AvailableModules)%>
            </div>
        </fieldset>
