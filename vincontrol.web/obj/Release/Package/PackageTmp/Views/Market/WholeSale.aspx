<%@ Page Language="C#" MasterPageFile="~/Views/Shared/NewSite.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Wholesale</title>
    <link href="<%=Url.Content("~/Css/VINstyle.css")%>" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        #c2
        {
            width: 784px;
        }

        h4
        {
            margin-bottom: 0;
            color: white;
        }

        table
        {
            width: 784px;
            font-size: .8em;
            overflow: visible;
        }

        tr.l
        {
            background: #222;
            margin: 0;
        }

        tr.d
        {
            padding-top: .2em;
            padding-bottom: .3em;
            margin: 0;
        }

        .listHeader
        {
            background: black;
            text-align: center;
        }

        .none
        {
            float: right;
            color: #999;
            margin-right: .3em;
        }

        .hider
        {
            display: none;
        }

        .fit
        {
            width: 100px;
        }

        #purchases input
        {
            width: 100%;
            min-width: 67px;
        }

        #name
        {
            width: 200px;
        }

        #wholesaleList input[type="text"]
        {
            width: 50px;
        }

        #Username
        {
            width: 100px;
        }

        tr.space td
        {
            height: 10px;
            border-top: #ddd solid 5px;
        }

        input.save
        {
            padding: .5em;
            font-weight: bold;
        }

        body
        {
            background: url('../images/cBgRepeatW.png') top center repeat-y;
        }

        #c2
        {
            border-right: none;
        }

        #listed
        {
            height: 400px;
            overflow: scroll;
            overflow-x: hidden;
        }

        .delete
        {
            width: 50px;
        }

        .num
        {
            width: 40px;
        }

        .color
        {
            width: 100px;
            background: #C33;
            font-weight: bold;
            text-align: center;
            color: black;
        }

        #search
        {
            float: right;
        }

            #search input[name="search"]
            {
                width: 200px;
            }

        #topNav input[type="button"]
        {
            border-bottom: 5px solid black;
            background: #000;
            padding: .4em;
            padding-bottom: 0;
            font-weight: bold;
            font-size: 1em;
        }

            #topNav input[type="button"]:hover
            {
                background: #860000;
            }

        input.on
        {
            border-bottom: 5px solid #860000 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h3>Wholesale</h3>
    <div id="topNav">
        <input class="btn on" type="button" name="inventory" value=" Your Inventory " />
        <input class="btn" type="button" name="purchases" value=" Purchases " />
        <input class="btn" type="button" name="wholesaleList" value=" Wholesale Listings " />
    </div>
    <div id="contentBTN" class="">

        <h4>Inventory</h4>
        Sort By:
        <select name="sortSet">
            <option value="">Make (Default) </option>
            <option value="">Model </option>
            <option value="">Year  </option>
            <option value="">Market </option>
        </select>
        <table id="header">
            <tr class="listHeader">
                <td class="listHeader">Year</td>
                <td class="listHeader">Make</td>
                <td class="listHeader">Model</td>
                <td class="listHeader">Trim</td>
                <td class="listHeader">Color</td>
                <td class="listHeader">VIN</td>
                <td class="listHeader"># Bids</td>
                <td class="listHeader">Highest Bid</td>
                <td class="delete">Remove</td>
            </tr>
            <tr class="l">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="l">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="l">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="l">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="l">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="l">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="l">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="l">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="l">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="l">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="l">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="l">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="l">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="l">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="l">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
            <tr class="">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td class="delete">
                    <input type="submit" nam="delete" value="Remove" />
                </td>
            </tr>
        </table>

    </div>
    <div id="wholesaleList" class="hider">
        <h4>Wholesale Listings</h4>
        Sort:
        <select name="sortSet">
            <option value="">Make (Default) </option>
            <option value="">Model </option>
            <option value="">Year  </option>
            <option value="">Market </option>
        </select>
        <a href="">View Favorites</a>
        <div id="search">Search:
            <input type="text" name="search" /></div>

        <br />
        <table id="header">
            <tr class="listHeader">
                <td class="listHeader">Year</td>
                <td class="listHeader">Make</td>
                <td class="listHeader">Model</td>
                <td class="listHeader">Trim</td>
                <td class="listHeader">Color</td>
                <td class="listHeader">VIN</td>
                <td class="listHeader">Avg Market</td>
                <td class="listHeader">Highest Bid</td>
                <td class="listHeader" colspan="2">Place Bid  ($)</td>
                <td class="listHeader">Fav</td>
            </tr>
            <tr class="l">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td>100</td>
                <td>$15,000</td>
                <td>
                    <input type="text" name="bidAmount" value="550,000" /></td>
                <td>
                    <input type="submit" name="bid" value="Bid" /></td>
                <td>
                    <center>
                        <input type="checkbox" name="track" /></center>
                </td>
            </tr>
        </table>
    </div>
    <div id="purchases" class="hider">
        <h4>Purchases</h4>
        Sort By:
        <select name="sortSet">
            <option value="">Status (Default) </option>
            <option value="">Make </option>
            <option value="">Model  </option>
            <option value="">Year </option>
        </select>
        <table id="header">
            <tr class="listHeader">
                <td class="listHeader">Year</td>
                <td class="listHeader">Make</td>
                <td class="listHeader">Model</td>
                <td class="listHeader">Trim</td>
                <td class="listHeader">Color</td>
                <td class="listHeader">VIN</td>
                <td class="listHeader">Status</td>
                <td class="listHeader">Location</td>
            </tr>
            <tr class="l">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td class="color">Processing</td>
                <td>San Bernardino, CA</td>
            </tr>
            <tr class="">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td class="color">Processing</td>
                <td>San Bernardino, CA</td>
            </tr>
            <tr class="l">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td class="color">Processing</td>
                <td>San Bernardino, CA</td>
            </tr>
            <tr class="">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td class="color">Processing</td>
                <td>San Bernardino, CA</td>
            </tr>
            <tr class="l">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td class="color">Processing</td>
                <td>San Bernardino, CA</td>
            </tr>
            <tr class="">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td class="color">Processing</td>
                <td>San Bernardino, CA</td>
            </tr>
            <tr class="l">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td class="color">Processing</td>
                <td>San Bernardino, CA</td>
            </tr>
            <tr class="">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td class="color">Processing</td>
                <td>San Bernardino, CA</td>
            </tr>
            <tr class="l">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td class="color">Processing</td>
                <td>San Bernardino, CA</td>
            </tr>
            <tr class="">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td class="color">Processing</td>
                <td>San Bernardino, CA</td>
            </tr>
            <tr class="l">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td class="color">Processing</td>
                <td>San Bernardino, CA</td>
            </tr>
            <tr class="">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td class="color">Processing</td>
                <td>San Bernardino, CA</td>
            </tr>
            <tr class="l">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td class="color">Processing</td>
                <td>San Bernardino, CA</td>
            </tr>
            <tr class="">
                <td>2011</td>
                <td>Mercedes-Benz</td>
                <td>CLS550</td>
                <td>Four Door Coupe</td>
                <td>Sienna Black Metallic</td>
                <td>12345678978912345</td>
                <td class="color">Processing</td>
                <td>San Bernardino, CA</td>
            </tr>

        </table>
    </div>
    
    <script src="<%=Url.Content("~/Scripts/jquery-1.6.4.min.js")%>" type="text/javascript"></script>
    <!-- JAVASCRIPT FOR BUTTONS -->
    <script language="javascript" type="text/javascript">

        $('input[name="inventory"]').click(function () {
            $("#contentBTN").slideToggle("slow");
            $("#wholesaleList").hide("slow");
            $("#purchases").hide("slow");
            $(this).toggleClass("on");
            $('#topNav input[type="button"]').not(this).removeClass('on');
        });
        $('input[name="wholesaleList"]').click(function () {
            $("#wholesaleList").slideToggle("slow");
            $("#contentBTN").hide("slow");
            $("#purchases").hide("slow");
            $(this).toggleClass("on");
            $('#topNav input[type="button"]').not(this).removeClass('on');
        });
        $('input[name="purchases"]').click(function () {
            $("#purchases").slideToggle("slow");
            $("#wholesaleList").hide("slow");
            $("#contentBTN").hide("slow");
            $('#topNav input[type="button"]').not(this).removeClass('on');
            $(this).toggleClass("on");
        });


        $('input[name="aUserList"]').click(function () {
            $("#aUserList").slideToggle("slow");

            $("#pUserList").hide("slow");
            $("#nUserList").hide("slow");
            $("#c24hUserList").hide("slow");
            $("#iUserList").hide("slow");
            $("#wUserList").hide("slow");
        });

        $('input[name="pUserList"]').click(function () {
            $("#pUserList").slideToggle("slow");

            $("#aUserList").hide("slow");
            $("#nUserList").hide("slow");
            $("#c24hUserList").hide("slow");
            $("#iUserList").hide("slow");
            $("#wUserList").hide("slow");
        });

        $('input[name="nUserList"]').click(function () {
            $("#nUserList").slideToggle("slow");

            $("#pUserList").hide("slow");
            $("#aUserList").hide("slow");
            $("#c24hUserList").hide("slow");
            $("#iUserList").hide("slow");
            $("#wUserList").hide("slow");
        });

        $('input[name="c24hUserList"]').click(function () {
            $("#c24hUserList").slideToggle("slow");

            $("#pUserList").hide("slow");
            $("#nUserList").hide("slow");
            $("#aUserList").hide("slow");
            $("#iUserList").hide("slow");
            $("#wUserList").hide("slow");
        });

        $('input[name="iUserList"]').click(function () {
            $("#iUserList").slideToggle("slow");

            $("#pUserList").hide("slow");
            $("#nUserList").hide("slow");
            $("#c24hUserList").hide("slow");
            $("#aUserList").hide("slow");
            $("#wUserList").hide("slow");
        });

        $('input[name="wUserList"]').click(function () {
            $("#wUserList").slideToggle("slow");

            $("#pUserList").hide("slow");
            $("#nUserList").hide("slow");
            $("#c24hUserList").hide("slow");
            $("#iUserList").hide("slow");
            $("#aUserList").hide("slow");
        });

    </script>
</asp:Content>
