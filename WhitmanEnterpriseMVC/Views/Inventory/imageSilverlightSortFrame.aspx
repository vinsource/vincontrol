<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.SilverlightImageViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Image Edit Frame</title>
    <style type="text/css">
        #container
        {
            width: 615px !important;
            margin: 0 auto;
            background: #222222;
            font-family: "Trebuchet MS" , Arial, Helvetica, sans-serif;
        }
        div
        {
            padding: 8px;
        }
        #barWrap
        {
            border: grey solid 1px;
            padding: 3px;
            width: 300px;
        }
        #color
        {
            height: 20px;
            background: #bbb;
            width: 183px;
            padding: 0;
        }
        .imageWrap
        {
            display: inline-block;
            position: relative;
        }
        .check
        {
            position: absolute;
            top: 5px;
            right: 5px;
        }
        #delete
        {
            background: #bbb;
            padding: 2px;
            width: 250px;
            display: inline-block;
        }
        #info
        {
            color: white;
            background: darkred;
            font-size: .9em;
        }
        #uploaded
        {
            max-height: 500px;
            overflow: auto;
            overflow-x: none;
            background: #111;
            padding: 4px;
            margin-bottom: 8px;
        }
        h1
        {
            margin: 0;
            margin-bottom: 2px;
            color: white;
        }
        
        html, body
        {
            height: 100%;
            overflow: auto;
        }
        body
        {
            padding: 0;
            margin: 0;
        }
        #silverlightControlHost
        {
            height: 683px;
            text-align: center;
        }
    </style>
</head>
<body>
    <%if (Model.SessionTimeOut)
      { %>
    <script type="text/javascript">

        parent.$.fancybox.close();
        alert("Your session is timed out. Please login back again");
        var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
        window.parent.location = actionUrl;

</script>
    <% }%>
    <script type="text/javascript">
        function closeForm() {
            parent.$.fancybox.close();
        }      
    </script>
    <div id="silverlightControlHost">
        <object data="data:application/x-silverlight-2," type="application/x-silverlight-2"
            width="100%" height="100%">
           
           
            <param name="source" value="/../../ClientBin/VINCONTROL.Silverlight.xap?id=4" />
            <param name="onError" value="onSilverlightError" />
            <param name="background" value="white" />
            <param name="minRuntimeVersion" value="5.0.61118.0" />
            <param name="autoUpgrade" value="true" />
            <param name="initParams" value='DealerID=<% = Model.DealerId %>,ListingId=<% = Model.ListingId%>,Vin=<% =Model.Vin %>,ImageServiceURL=<% = Model.ImageServiceURL %>,InventoryStatus=<% =Model.InventoryStatus %>' />
            <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=5.0.61118.0" style="text-decoration: none">
                <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="Get Microsoft Silverlight"
                    style="border-style: none" />
            </a>
        </object>
        <iframe id="_sl_historyFrame" style="visibility: hidden; height: 0px; width: 0px;
            border: 0px"></iframe>
    </div>
</body>
</html>
