<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Vincontrol.Web.Models.EmailContentViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Email Content</title>
</head>
<body>
    <fieldset style="height: 350px">
        <legend>Trade In Email</legend>
        <p>
            <div style="font-weight: bold">
                Receivers:</div>
            <%= Html.Encode(Model.Receivers) %>
        </p>
        <p>
            <div style="font-weight: bold">
                Text Content:</div>
            <%= Html.Encode(Model.TextContent) %>
        </p>
        <p>
            <div style="font-weight: bold">
                ADF Content:</div>
            <%= Html.Encode(Model.ADFContent) %>
        </p>
    </fieldset>
</body>
</html>
