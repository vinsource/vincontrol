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
width: 275px !important;
height: 45px;
}
#more-info-form ul li {
list-style: none;
height: 54px !important;
float: left;
width: 50%;
margin-top: 10px;
}
</style>

<div style="width:auto;height:auto;overflow:auto;position:relative;display:none;">
<div id="more-info" style="height: 340px;" class="form-wrap">
    <h4>
        Request More Info</h4>
    <div id="more-info-form">
        <form id="formSubmit_more_info" method="post">
        <ul>
            <li>
                <label class="lb-info">
                    First Name :</label>
                <input id="fname_more" class="input-info" type="text" onfocus="if (this.value == 'First Name') this.value = '';"
                    onblur="if (this.value == '') this.value = 'First Name';" value="First Name" />
                <span class="red">* </span></li>
            <li>
                <label class="lb-info">
                    Last Name :</label>
                <input id="lname_more" class="input-info" type="text" onfocus="if (this.value == 'Last Name') this.value = '';"
                    onblur="if (this.value == '') this.value = 'Last Name';" value="Last Name" />
                <span class="red">* </span></li>
            <li>
                <label class="lb-info">
                    Email :</label>
                <input id="email_more" class="email_form input-info" type="text" onfocus="if (this.value == 'Email Address') this.value = '';"
                    onblur="if (this.value == '') this.value = 'Email Address';" value="Email Address" />
                <br />
            </li>
            <li>
                <label class="lb-info">
                    Home Phone :</label>
                <input id="phone_more" class="input-info mask_phone" type="text" value="999-999-9999" />
                <span class="red">* </span></li>
            <li style="width: 75%; height: 30px !important">
                <label class="lb-info" style="width: 155px; margin-top: 0px;">
                    Preferred Contact :</label>
                <input id="more-email" type="radio" class="test-radio" name="ra-more-contact" checked="checked" />
                <label for="more-email" class="more-radio-lb">
                    Email</label>
                <input id="more-phone" type="radio" class="test-radio" name="ra-more-contact" />
                <label for="more-phone" class="more-radio-lb">
                    Phone</label>
            </li>
            <li>
                <label style="font-size: 20px; width: 150px;" class="lb-info">
                    Comments :</label>
                <textarea name="comments" id="comment_more" onfocus="if (this.value == 'Additional Comments') this.value = '';"
                    onblur="if (this.value == '') this.value = 'Additional Comments';">Additional Comments</textarea>
            </li>
        </ul>
        <a href="javascript:void(0);">
            <div id="button-more-info" style="margin-top: 66px;" class="buttons-info">
                Request
            </div>
        </a>
        </form>
    </div>
</div>
</div>