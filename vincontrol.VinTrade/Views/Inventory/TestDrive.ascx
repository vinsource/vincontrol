<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<style type="text/css">
#fancybox-content {
border: 10px solid #3366cc !important;
}
.form-wrap {
background: white !important;
padding: 10px;
width: 700px;
overflow: auto;
}
#fancybox-content {
width: 0;
height: 0;
padding: 0;
outline: none;
position: relative;
overflow: hidden;
z-index: 1102;
border: 0px solid #000;
}   
.form-wrap div input[type="text"] {
height: 30px;
width: 170px !important;
}
input[type="text"] {
width: 150px;
background: #111517;
color: white;
border-radius: 4px 4px 4px 4px;
border: 1px solid rgba(26, 37, 40, 1);
border-top: 1px solid #2F2F2F;
}
input[type="text"] {
display: inline-block;
width: 210px;
height: 16px;
padding: 4px;
margin-bottom: 9px;
font-size: 13px;
line-height: 16px;
/*color: #555;*/
border: 1px solid #CCC;
-webkit-border-radius: 3px;
-moz-border-radius: 3px;
border-radius: 3px;
resize: none;
}
#offer-form .buttons-info {
margin-top: 65px;
}
.buttons-info:hover {
background-color: #0394FC;
}
.buttons-info {
font-weight: bold;
float: right;
background-color: #3366cc;
padding-top: 6px;
width: 100px;
color: white;
height: 23px;
border: 1px solid gray;
text-align: center;
}
#offer-form ul {
margin: 0px;
padding: 0px;
}
ul {
padding: 0 !important;
}
ul {
list-style-type: none;
}
user agent stylesheetul, menu, dir {
display: block;
list-style-type: disc;
-webkit-margin-before: 1em;
-webkit-margin-after: 1em;
-webkit-margin-start: 0px;
-webkit-margin-end: 0px;
-webkit-padding-start: 40px;
}
#offer-form ul li {
list-style: none;
height: 54px !important;
float: left;
width: 50%;
margin-top: 10px;
}
#offer-form ul li {
list-style: none;
}
.lb-info {
font-size: 15px;
margin-bottom: 5px;
font-weight: bold;
margin-top: 6px;
float: left;
width: 100px;
color: #333;
display: block;
}
.red { color: Red; }
.form-wrap h4 {
color: #333;
/*padding: 9px;*/
font-size: 1.5em;
margin-bottom: 10px;
margin-top: 10px;
}
.form-wrap textarea {
resize: none;
width: 445px !important;
height: 45px;
}
.control-group {
height: 54px;
margin-top: 10px;
margin-bottom: 8px;
}
</style>

<div style="width:auto;height:auto;overflow:auto;position:relative;display:none;">
<div id="test_drive" class="form-wrap">
    <div id="test-drive-listing">
        <!-- AD CELLS -->
        <h4>
            Take a Test Drive!</h4>
        <div id="test-drive" class="validate form-section">
            <legend style="border: none; margin-top: 10px;"></legend>
            <label style="font-size: 20px; width: 100%;" class="lb-info">
                Appointment Date
            </label>
            <div style="width:100%;display:inline-block;">
            <div class="input-prepend control-group" style="width: 320px; float: left;">
                <label class="lb-info">
                    First Name :</label>
                <input id="fname_test" name="firstname" class="required" type="text" onfocus="if (this.value == 'First Name') this.value = '';"
                    onblur="if (this.value == '') this.value = 'First Name';" value="First Name" />
                <span class="red">* </span>
            </div>
            <div class="input-prepend control-group" style="width: 300px; float: left;">
                <label class="lb-info">
                    Last Name :</label>
                <input id="lname_test" name="lastname" class="required" type="text" onfocus="if (this.value == 'Last Name') this.value = '';"
                    onblur="if (this.value == '') this.value = 'Last Name';" value="Last Name" />
                <span class="red">* </span>
            </div>
            <div class="input-prepend control-group" style="width: 320px; float: left;">
                <label class="lb-info">
                    Email :</label>
                <input name="email" class="email_form required" id="email_test" type="text" onfocus="if (this.value == 'Email Address') this.value = '';"
                    onblur="if (this.value == '') this.value = 'Email Address';" value="Email Address" />
                <span class="red">* </span>
            </div>
            <div class="input-prepend control-group" style="width: 300px; float: left;">
                <label class="lb-info">
                    Home Phone :</label>
                <input name="phone" class="number required mask_phone" id="phone_test" type="text"
                    value="999-999-9999" />
                <span class="red">* </span>
            </div>
            <div class="input-prepend control-group" id="input-prepend-date" style="width: 320px; float: left;">
                <label class="lb-info">
                    Select a Date :</label>
                <input type="text" name="date" id="test-date" class="datepicker required" onfocus="if (this.value == 'Date (Ex: mm/dd/yyyy)') this.value = '';"
                    onblur="if (this.value == '') this.value = 'Date (Ex: mm/dd/yyyy)';" value="Date (Ex: mm/dd/yyyy)" />
                <span class="red">* </span>
            </div>
            <div class="input-prepend control-group" style="width: 300px; float: left;">
                <label class="lb-info">
                    Select a Time :</label>
                <input type="text" name="time" id="time" class="required" onfocus="if (this.value == 'Enter a Time (ie. 12:00pm)') this.value = '';"
                    onblur="if (this.value == '') this.value = 'Enter a Time (ie. 12:00pm)';" value="Enter a Time (ie. 12:00pm)" />
            </div>
            <div class="input-prepend control-group" style="height: 30px; float: left;">
                <label class="lb-info" style="width: 155px; margin-top: 0px;">
                    Preferred Contact :</label>
                <input id="contact_email_test" type="radio" class="test-radio" name="ra-test-contact"
                    checked="checked" />
                <label for="contact_email_test" class="more-radio-lb">
                    Email</label>
                <input id="contact_phone_test" type="radio" class="test-radio" name="ra-test-contact" />
                <label for="contact_phone_test" class="more-radio-lb">
                    Phone</label>                
            </div>
            </div>
            <div style="width:100%;display:inline-block;">
            <label style="font-size: 20px; width: 150px;" class="lb-info">
                Comments
            </label>
            <div class="input-prepend control-group" style="height: 30px;">
                <textarea name="comments" id="comment_test" onfocus="if (this.value == 'Additional Comments') this.value = '';"
                    onblur="if (this.value == '') this.value = 'Additional Comments';">Additional Comments</textarea>
            </div>
            <a href="javascript:void(0);">
                <div id="button-test-info" style="margin-top: 37px;" class="buttons-info">
                    TAKE TEST
                </div>
            </a>
            </div>
        </div>
    </div>
</div>
</div>