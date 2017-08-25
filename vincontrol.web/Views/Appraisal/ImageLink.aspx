<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <title>Image Link</title>
    <style type="text/css">
        #listImages li
        {
            float: left;
            clear: right;
            list-style-type: none;
        }

        #listImages
        {
            overflow: auto;
            width: 100%;
            max-height: 210px;
            margin: 0 0 0 2px;
            padding: 0;
        }

        .centerImage input[type="checkbox"]
        {
            display: none;
        }

        #galleria
        {
            width: 1000px;
            height: 650px;
            background: #000;
        }

        .galleria-errors
        {
            display: none;
        }
    </style>
</head>
<body>
    <div id="galleria">
        <% = ViewData["result"] %>
    </div>
    <script src="<%=Url.Content("~/js/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts" + "/galleria-1.2.9.min.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        Galleria.loadTheme('<%=Url.Content("~/Scripts/themes/classic/galleria.classic.min.js")%>');
        Galleria.run('#galleria');
    </script>
</body>
</html>
