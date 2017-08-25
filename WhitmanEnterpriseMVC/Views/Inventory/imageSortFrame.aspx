<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<WhitmanEnterpriseMVC.Models.ImageViewModel>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Image Edit Frame</title>
<link rel="stylesheet" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.13/themes/base/jquery-ui.css" id="theme">

<link rel="stylesheet" href="<%=Url.Content("~/UploadImages/jquery.fileupload-ui.css")%>" >
<link rel="stylesheet" href="<%=Url.Content("~/UploadImages/style.css")%>" >
<link href="<%=Url.Content("~/js/fancybox/jquery.fancybox-1.3.4.css")%>" rel="stylesheet" type="text/css" media="screen" />

<script src="//ajax.googleapis.com/ajax/libs/jquery/1.6.1/jquery.min.js"></script>
<script src="//ajax.googleapis.com/ajax/libs/jqueryui/1.8.13/jquery-ui.min.js"></script>
<script src="//ajax.aspnetcdn.com/ajax/jquery.templates/beta1/jquery.tmpl.min.js"></script>

<style type="text/css">
	#container {width: 615px !important; margin: 0 auto; background: #222222; font-family: "Trebuchet MS", Arial, Helvetica, sans-serif;}
	div {padding: 8px;}
	#barWrap {border: grey solid 1px; padding: 3px; width: 300px;}
	#color {height: 20px; background: #bbb; width: 183px; padding: 0;}
	.imageWrap {display: inline-block; position: relative;}
	.check {position: absolute; top: 5px; right: 5px;}
	#delete {background: #bbb; padding: 2px; width: 250px; display: inline-block;}
	#info {color: white; background: darkred; font-size: .9em;}
	#uploaded {max-height: 500px; overflow: auto; overflow-x: none; background: #111; padding: 4px; margin-bottom: 8px;}
	h1 {margin: 0; margin-bottom: 2px; color: white;}
</style>
</head>

<body>
<%if (Model.SessionTimeOut)
  { %>
  
 <script>
   
     parent.$.fancybox.close();
     alert("Your session is timed out. Please login back again");
     var actionUrl = '<%= Url.Action("LogOff", "Account" ) %>';
     window.parent.location = actionUrl;

</script>
<% }%>
<script>
    $(document).ready(function() {
        $("#btnCancel").click(function() {

            parent.$.fancybox.close();
        });

        $("#uploadImageForm").submit(function(event) {

            event.preventDefault();
            //            var finalNormalImageUploadFileURL = "";
            //            var finalThumbnailImageUploadFileURL = "";
            //            if ($("#ImageUploadFiles").val().substring(0, 1) == ",")
            //                $("#ImageUploadFiles").val($("#ImageUploadFiles").val().substring(1, $("#ImageUploadFiles").val().length));
            //            if ($("#ThumbnailImageUploadFiles").val().substring(0, 1) == ",")
            //                $("#ThumbnailImageUploadFiles").val($("#ThumbnailImageUploadFiles").val().substring(1, $("#ThumbnailImageUploadFiles").val().length));
            //            console.log($("#ThumbnailImageUploadFiles").val());

            //            console.log($("#ImageUploadFiles").val());


            UpdateImageUrl(this);


        });



    });



function UpdateImageUrl(form) {



    $.ajax({

        url: form.action,

        type: form.method,

        dataType: "json",

        data: $(form).serialize(),

        success: UpdateImageSuccessAndClose

    });


}

function UpdateImageSuccessAndClose(result) {

    parent.$.fancybox.close();
    window.parent.location.reload(true);

}




</script>
<div id="container">
	<h1>Image Manager</h1>
	<div id="info">This uploader is limited to images with files sizes <b><em>less than 1024kb</em></b> (1MB). Be sure to resize your images before upload.</div>
	<div id="fileupload">
        
     <%=Html.DynamicHtmlImageForm() %>
        <div class="fileupload-buttonbar">
            
             Overlay: <input type="checkbox" id="cbkoverlay" name="cbkoverlay" checked="checked" onclick="javascript:changeFormURL(this);">
            <label class="fileinput-button">
                <span>Add files...</span>
                <input id="file" type="file" name="files[]" multiple>
            </label>
            <!--<button type="submit" class="start">Start upload</button>-->
            <button type="reset" class="cancel">Cancel upload</button>
            
            <button type="button" class="delete" id="btnDelete">Delete files</button>
            
            
           
            <br />
            
            
            
            <span id="number"></span>
           
         
            
        </div>
     <% Html.EndForm(); %>
    <div class="fileupload-content">
        <table class="files"></table>
        <div class="fileupload-progressbar" ></div>
    </div>
      <div id="uploaded">
    	
    	
    </div>
   	<% Html.BeginForm("UpdateCarImageUrlFromImageSortFrame", "Inventory", FormMethod.Post, new { id = "uploadImageForm", name = "uploadImageForm" }); %>
   	<%=Html.HiddenFor(x=>x.ListingId) %>
   	<%=Html.HiddenFor(x=>x.Vin) %>
   	<%=Html.HiddenFor(x=>x.InventoryStatus) %>
   	<%=Html.HiddenFor(x=>x.ImageUploadFiles) %>
   	<%=Html.HiddenFor(x=>x.ThumbnailImageUploadFiles) %>
   	<button type="button" id="btnCancel">Cancel</button>
   <button type="submit" id="btnClose" >Save and Close</button>
    <% Html.EndForm(); %>
   
</div>
</div>

<script id="template-upload" type="text/x-jquery-tmpl">
    <tr class="template-upload{{if error}} ui-state-error{{/if}}">
        <td class="preview"></td>
        <td class="name">${name}</td>
        <td class="size">${sizef}</td>
        {{if error}}
            <td class="error" colspan="2">Error:
                {{if error === 'maxFileSize'}}File is too big
                {{else error === 'minFileSize'}}File is too small
                {{else error === 'acceptFileTypes'}}Filetype not allowed
                {{else error === 'maxNumberOfFiles'}}Max number of files exceeded
                {{else}}${error}
                {{/if}}
            </td>
        {{else}}
            <td class="progress"><div></div></td>
            <td class="start"><button>Start</button></td>
        {{/if}}
        <td class="cancel"><button>Cancel</button></td>
    </tr>
</script>
<script id="template-download" type="text/x-jquery-tmpl">
    <tr class="template-download{{if error}} ui-state-error{{/if}}">
        {{if error}}
            <td></td>
            <td class="name">${namefdsa}</td>
            <td class="size">${sizef}</td>
            <td class="error" colspan="2">Error:
                {{if error === 1}}File exceeds upload_max_filesize (php.ini directive)
                {{else error === 2}}File exceeds MAX_FILE_SIZE (HTML form directive)
                {{else error === 3}}File was only partially uploaded
                {{else error === 4}}No File was uploaded
                {{else error === 5}}Missing a temporary folder
                {{else error === 6}}Failed to write file to disk
                {{else error === 7}}File upload stopped by extension
                {{else error === 'maxFileSize'}}File is too big
                {{else error === 'minFileSize'}}File is too small
                {{else error === 'acceptFileTypes'}}Filetype not allowed
                {{else error === 'maxNumberOfFiles'}}Max number of files exceeded
                {{else error === 'uploadedBytes'}}Uploaded bytes exceed file size
                {{else error === 'emptyResult'}}Empty file upload result
                {{else}}${error}
                {{/if}}
            </td>
        {{else}}
            <td class="preview">
                {{if Thumbnail_url}}
                    <a href="${url}" target="_blank"><img src="${Thumbnail_url}"></a>
                {{/if}}
            </td>
            <td class="name">
                <a href="${url}"{{if thumbnail_url}} target="_blank"{{/if}}>${Name}</a>
            </td>
            <td class="size">${Length}</td>
            <td colspan="2"></td>
        {{/if}}
        <td class="delete">
            <button data-type="${delete_type}" data-url="${delete_url}">Delete</button>
        </td>
    </tr>
</script>
<script src="<%=Url.Content("~/UploadImages/jquery.iframe-transport.js")%>"></script>
<script src="<%=Url.Content("~/UploadImages/jquery.fileupload.js")%>"></script>
<script src="<%=Url.Content("~/UploadImages/jquery.fileupload-ui.js")%>"></script>
<script src="<%=Url.Content("~/UploadImages/application.js")%>"></script>

</body> 
<script language="javascript">

    $("#btnDelete").click(function() {
        
        $('#uploaded div input[type="checkbox"]').each(function(index) {
            if ($(this).is(':checked')) {
                $(this).parent().remove();
                var tmpNormal = $("#ImageUploadFiles").val();

                tmpNormal = tmpNormal.replace($(this).prev().attr("src") + ",", "");
                tmpNormal = tmpNormal.replace($(this).prev().attr("src"), "");
                var tmpThmubmail = tmpNormal.replace("NormalSizeImages", "ThumbnailSizeImages");
                $("#ImageUploadFiles").val(tmpNormal);
                $("#ThumbnailImageUploadFiles").val(tmpThmubmail);


            }
        });

    });

    function changeFormURL(checkbox) {
     
        if (checkbox.checked) {
            var actionUrl = document.forms[0].action.replace('Overlay=0', 'Overlay=1');
            document.forms[0].action = actionUrl;
        } else {
            actionUrl = document.forms[0].action.replace('Overlay=1', 'Overlay=0');
            document.forms[0].action = actionUrl;
        }
       
    }
  
</script>
</html>