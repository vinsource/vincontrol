<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<string>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<script src="http://code.jquery.com/jquery-latest.js"></script>

<head id="Head1" runat="server">
    <title>Access Denied</title>
    <style type="text/css">
        html
        {
            background: #111111;
            color: #eeeeee;
            font-family: 'Trebuchet MS' , Arial, sans-serif;
        }
        body
        {
            margin: 0 auto;
            width: 800px;
        }
        h1
        {
            margin-bottom: 0;
            color: #c80000;
            margin-bottom: 0;
        }
        h3
        {
            margin: 0;
            padding: 20px;
            font-weight: normal;
        }
        .wrapper
        {
            background: #333333;
            position: relative;
        }
        a
        {
            color: #ffffff;
            text-decoration: none;
            font-style: italic;
            font-weight: bold;
        }
        a:hover
        {
            color: #C80000;
        }
        #ticket
        {
            margin-left: 50%;
            position: relative;
            left: -250px;
        }
    </style>
</head>
<body>
    <div>
        <h1>
            Access Denied</h1>
        <div class="wrapper">
            <h3>
                You don’t have permission to access this page or perform this action.</h3>
        </div>
    </div>
</body>
</html>
