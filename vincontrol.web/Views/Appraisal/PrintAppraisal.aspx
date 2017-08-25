<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Print Appraisal Options</title>

    <style type="text/css">
        html
        {
            background: #222;
            color: white;
            font-family: "Trebuchet MS", Arial, Helvetica, sans-serif;
        }

        td
        {
            min-width: 100px;
            background: #333;
        }

        table
        {
            background: #333;
            padding: 1em;
        }

        #break td
        {
            background: #ddd;
            border: 1px #ddd solid;
        }

        input[type="submit"]
        {
            background: #680000;
            border: 0;
            color: white;
            font-size: 1.1em;
            font-weight: bold;
            padding: .5em;
        }
    </style>
</head>
<body style="width: 500px; margin: 0 auto;">
    <h1 style="margin-bottom: -.9em;">Print Appraisal</h1>
    <div id="appraisal" style="width: 500px;">
        <form id="appraisalPrint" action="" method="post">
            <span style="float: right;">Appraisal good for
                <input style="width: 30px;" type="text" name="days" />
                days.</span>
            <table>
                <tr>
                    <td>Item</td>
                    <td>Good</td>
                    <td>Fair</td>
                    <td>Poor</td>
                </tr>
                <tr>
                    <td>Front Seats:</td>
                    <td>
                        <input type="checkbox" name="Good" /></td>
                    <td>
                        <input type="checkbox" name="Fair" /></td>
                    <td>
                        <input type="checkbox" name="Poor" /></td>
                </tr>
                <tr>
                    <td>Rear Seats:</td>
                    <td>
                        <input type="checkbox" name="Good" /></td>
                    <td>
                        <input type="checkbox" name="Fair" /></td>
                    <td>
                        <input type="checkbox" name="Poor" /></td>
                </tr>
                <tr>
                    <td>Carpet:</td>
                    <td>
                        <input type="checkbox" name="Good" /></td>
                    <td>
                        <input type="checkbox" name="Fair" /></td>
                    <td>
                        <input type="checkbox" name="Poor" /></td>
                </tr>
                <tr>
                    <td>Transmission:</td>
                    <td>
                        <input type="checkbox" name="Good" /></td>
                    <td>
                        <input type="checkbox" name="Fair" /></td>
                    <td>
                        <input type="checkbox" name="Poor" /></td>
                </tr>
                <tr>
                    <td>Engine:</td>
                    <td>
                        <input type="checkbox" name="Good" /></td>
                    <td>
                        <input type="checkbox" name="Fair" /></td>
                    <td>
                        <input type="checkbox" name="Poor" /></td>
                </tr>
                <tr>
                    <td>AWD:</td>
                    <td>
                        <input type="checkbox" name="Good" /></td>
                    <td>
                        <input type="checkbox" name="Fair" /></td>
                    <td>
                        <input type="checkbox" name="Poor" /></td>
                </tr>
                <tr>
                    <td>Front Tires:</td>
                    <td>
                        <input type="checkbox" name="Good" /></td>
                    <td>
                        <input type="checkbox" name="Fair" /></td>
                    <td>
                        <input type="checkbox" name="Poor" /></td>
                </tr>
                <tr>
                    <td>Rear Tires:</td>
                    <td>
                        <input type="checkbox" name="Good" /></td>
                    <td>
                        <input type="checkbox" name="Fair" /></td>
                    <td>
                        <input type="checkbox" name="Poor" /></td>
                </tr>
                <tr>
                    <td>Wheels:</td>
                    <td>
                        <input type="checkbox" name="Good" /></td>
                    <td>
                        <input type="checkbox" name="Fair" /></td>
                    <td>
                        <input type="checkbox" name="Poor" /></td>
                </tr>
                <tr id="break">
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Frame:</td>
                    <td>
                        <input type="checkbox" name="Good" />
                        No Damage</td>
                    <td>
                        <input type="checkbox" name="Fair" />
                        Damage</td>
                    <td>
                        <input type="submit" name="submit" onclick="print()" value="Print Appraisal" /></td>
                </tr>
            </table>

        </form>
        <script language="javascript" type="text/javascript">
            function print() {
                window.open('appraisalPrint.html');
                parent.close();
            }
</script>
    </div>
</body>
</html>
