<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.CarDescriptionModel>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Edit</title>
<script src="http://code.jquery.com/jquery-latest.js" type="text/javascript" language="javascript"></script>
<script type="text/javascript" language="javascript" src="<%=Url.Content("~/js/jquery.js")%>"> </script>

<!-- latest jQuery direct from google's CDN -->
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
<style type="text/css">
    html {font-family: 'Trebuchet MS', Arial, sans-serif; color: #eeeeee;}
    #desc-container {
        width: 700px;
        margin: 0 auto;
        padding: 1em;
        position: relative;
    }
    #desc-inner h1 {margin-bottom: 4px; color:; }
    #desc-inner h3 {margin-bottom: 0;}
    #desc-inner ul {list-style-type: none; padding: 0; margin:0;}
    #desc-inner li {margin-left: 0; background: #222222; margin-bottom: 4px; padding: .2em;font-size: .7em}
    #desc-wrap {
        background: #333333;
        width: 100%;
        padding: .5em;
    }
    #desc-inner p {
        background: #880000;
        padding: .5em;
        margin: 0;
        width: 100%;
        margin-bottom: 10px;
    }
    
    .hideLoader {display: none;}
</style>
</head>

<body>

 <div id="elementID" class="hideLoader" style="position: absolute; z-index: 500; top: 0; left: 0; text-align:center; bottom: 0; right: 0; opacity: .7; background: #111; margin: 0 auto; " >
<img id="loading" style="display: inline; margin: 0 auto; margin-top: 420px;" src="<%=Url.Content("~/images/ajax-loader1.gif")%>" alt="" />
</div>
<div id="desc-container">
        <div id="desc-inner">
            <h1>Edit Description</h1>
            <p style="font-size: .8em">Here you can choose pre-generated sentences to construct your desired description. Once you have selected all your desired sentences, hit the save button and it will be populated in the description box on your vehicle's edit page.</p>
            
            
           
             
            <div id="desc-wrap">
                
                  <h3>Starting/Ending Sentence</h3>
                   <input type="checkbox" name="StartEndSentence" value="Start" />Start
                   <input type="checkbox" name="StartEndSentence" value="End" />End
                   
                <%foreach (var tmp in Model.DescriptionList)
                  {%>
                
                <h3><%=tmp.Title%></h3>
                <%if (tmp.Sentences.Any(x => x.YesNo) && tmp.Sentences.Count(x => !x.YesNo) > 0)
                  {%>
                <input type="radio" value="Yes" name="Option<%= tmp.Title.Replace(" ", "") %>" checked="checked" onclick="javascript:flipDescription('<%= tmp.Title %>',true)"  title="Click to Change Description"   />Yes
                <input type="radio" value="No" name="Option<%= tmp.Title.Replace(" ", "") %>"  title="Click to Change Description" onclick="javascript:flipDescription('<%= tmp.Title %>',false)"  />No
                <% } %>
                <ul id="<%=tmp.Title.Replace(" ","") %>">
                  <%foreach (var des in tmp.Sentences.Where(x => x.YesNo).OrderBy(t => Guid.NewGuid()).ToList().Take(3))
                       {%>
                    <li><input type="radio" name="<%= tmp.Title %>" value="<%=des.DescriptionSentence %>" class="" /><%=des.DescriptionSentence %></li>
                   
                    <% } %>
                
                
                 <%if (!tmp.Sentences.Any(x => x.YesNo))
                  {%>
                    <%foreach (var des in tmp.Sentences.Where(x => !x.YesNo).OrderBy(t => Guid.NewGuid()).ToList().Take(3))
                       {%>
                    <li><input type="radio" name="<%= tmp.Title %>" value="<%=des.DescriptionSentence %>" class="" /><%=des.DescriptionSentence %></li>
                   
                    <% } %>
                  <% } %>
                </ul>
                
                <% }  %>
                
               <h3>Options</h3>
               <table style="font-size: .6em">
                    <tr>
                        <td><input type="checkbox" name="SelectOptions" value="Navigation System" class="" />Navigation System</td>
                        <td><input type="checkbox" name="SelectOptions" value="Panorama Sunroof" class="" />Panorama Sunroof</td>
                        <td><input type="checkbox" name="SelectOptions" value="Dual Power Seats" class="" />Dual Power Seats</td>
                        <td><input type="checkbox" name="SelectOptions" value="Rear View Camera" class="" />Rear View Camera</td>
                      
                     
                    </tr>
                    <tr>
                      
                        <td><input type="checkbox" name="SelectOptions" value="Rear View Camera" class="" />Rear View Camera</td>
                        <td><input type="checkbox" name="SelectOptions" value="AMG Performance package" class="" />AMG Performance package</td>
                        <td><input type="checkbox" name="SelectOptions" value="Panorama Moonroof" class="" />Panorama Moonroof</td>
                  
                        <td><input type="checkbox" name="SelectOptions" value=">Electronic Trunk Close" class="" />Electronic Trunk Close</td>
                      
                     
                    </tr>
                    <tr>
                      
                        <td><input type="checkbox" name="SelectOptions" value="Blind Spot Assist" class="" />Blind Spot Assist</td>
                        <td><input type="checkbox" name="SelectOptions" value="Parktronic With Parking Guidance" class="" />Parktronic With Parking Guidance</td>
                        <td><input type="checkbox" name="SelectOptions" value="Bi-xenon Active Headlamps" class="" />Bi-xenon Active Headlamps</td>
                        <td><input type="checkbox" name="SelectOptions" value="Heated Front Seats" class="" />Heated Front Seats</td>
                      
                     
                    </tr>
                     <tr>
                      
                           <td><input type="checkbox" name="SelectOptions" value="Keyless" class="" />Keyless</td>
                        <td><input type="checkbox" name="SelectOptions" value="Power Rear Sunshade" class="" />Power Rear Sunshade</td>
                        <td><input type="checkbox" name="SelectOptions" value="Sirius" class="" />Sirius</td>
                        <td><input type="checkbox" name="SelectOptions" value="iPod/Mp3 Media Interface" class="" />iPod/Mp3 Media Interface</td>
                     
                    </tr>
               </table>
            </div>
            <%=Html.HiddenFor(x=>x.ListingId) %>
           <input class="pad" type="button" name="SaveDesc" value=" Save Description " id="btnSaveDesc"  />
        </div>
</div>
<script type="text/javascript">
//$(document).ready(function(){
//var radio = $('input[type="radio"]');

//    radio.click(function(){
//        var radios = document.getElementsByTagName('input');
//        for (i in radios) {
//            var parent = radios[i].parentNode;
//            if (radios[i].checked) {
//                parent.className = 'selected';
//            } else if (!radios[i].checked) {
//                parent.className = '';
//            }
//        }
//    });

//});

    $("#btnSaveDesc").click(function(event) {


        var radios = $('input[type="radio"]');
        var item = "";
        for (i in radios) {
            if (radios[i].checked && radios[i].value != "Yes" && radios[i].value != "No") {
                item += radios[i].value;
            }
        }

        var checks = $('input[type="checkbox"]');
        var itemoption = "";
        var startend = "";
        for (i in checks) {
            if (checks[i].checked && checks[i].name == "SelectOptions") {
                itemoption += checks[i].value + ", ";
            }
            else if (checks[i].checked && checks[i].name == "StartEndSentence") {
                startend += checks[i].value;

            }
        }

        
        $.post('<%= Url.Content("~/Inventory/SaveDescription") %>', { ListingId: $("#ListingId").val(), Description: item, OptionSelect: itemoption,StartEnd: startend }, function(data) {
            if (data == "SessionTimeOut") {

                alert("Your session has timed out. Please login back again");
                var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                window.parent.location = actionUrl;
            } else {
                parent.document.editiProfileForm.Description.value = data;

                parent.$.fancybox.close();

            }
        });



    });






        function flipDescription(Title, YesNo) {
            $('#elementID').removeClass('hideLoader');


            $.post('<%= Url.Content("~/Ajax/FlipDescription") %>', { Title: Title, YesNo: YesNo }, function(data) {
                if (data == "SessionTimeOut") {

                    alert("Your session has timed out. Please login back again");
                    var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
                    window.parent.location = actionUrl;
                } else {
                    var items = "";
                    $.each(data, function(i, data) {

                    items += "<li><input type='radio' value='" + data + "' name='" + Title + "'>" + data + "</option>";
                    });
                    var newString = Title.split(' ').join('');
                    $("#" + newString).html(items);
                    $('#elementID').addClass('hideLoader');
                }

            });

            $('#elementID').addClass('hideLoader');

        }

</script>
</body>
</html>
