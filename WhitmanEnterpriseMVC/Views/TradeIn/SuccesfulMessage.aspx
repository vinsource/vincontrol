<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.SuccesfulMessageViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Succesful Message</title>
</head>
<body>
    <div style="text-align:center;color: black" >
    A trade in email has been successfully sent to <%=Model.UserEmail %>.
    </div>
</body>
</html>
