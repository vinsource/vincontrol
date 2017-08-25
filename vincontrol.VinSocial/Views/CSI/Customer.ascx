<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div class="customer-profile-tab page-tab hidden">
    <div class="customer-profile-info">
        <h3 class="customer-profile-reviews-header">
            Client Info</h3>
        <div class="client-status">
            <img src="/Content/images/social/Orange-Circle.png" alt="">
        </div>
        <div class="client-info-container">
            <ul>
                <li><b>Name:</b> Jeff Hakimi </li>
                <li><b>Home Phone:</b> (123) 456-7890 </li>
                <li><b>Cell Phone</b> (123) 456-7890 </li>
                <li><b>Email:</b> jeff@vincontrol.com </li>
            </ul>
        </div>
    </div>
    <div class="customer-profile-vehicle">
        <h3 class="customer-profile-reviews-header">
            Vehicle</h3>
        <div class="client-vehicle">
            <ul>
                <li><b>Year:</b> 2011 </li>
                <li><b>Make:</b> Mercedes </li>
                <li><b>Model:</b> S-Class </li>
                <li><b>Trim:</b> S500 </li>
            </ul>
        </div>
    </div>
    <div class="customer-profile-salesinfo">
        <h3 class="customer-profile-reviews-header">
            Sales Info</h3>
        <ul>
            <li><b>Salesperson</b> John Wilson </li>
            <li><b>Manager</b> Bob Williams </li>
            <li><b>Client Rep:</b> Sandy Adams </li>
            <li><b>Level:</b> Urgent </li>
        </ul>
    </div>
    <div class="customer-profile-notes">
        <h3 class="customer-profile-reviews-header">
            Notes</h3>
        <div class="client-notes-box">
            <textarea name="notes">Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</textarea>
        </div>
    </div>
    <div class="customer-profile-survey">
        <h3 class="customer-profile-reviews-header">
            Survey - Finance</h3>
        <div class="client-survey">
            <ul>
                <li><b>1.</b><span>How was your overall experience at our dealership ipsum dolar sit
                    amet non sequitor?</span><div class="stars five-stars">
                    </div>
                </li>
                <li><b>2.</b><span>How was your overall experience at our dealership?</span><div
                    class="stars five-stars">
                </div>
                </li>
                <li><b>3.</b><span>How was your overall experience at our dealership?</span><div
                    class="stars five-stars">
                </div>
                </li>
                <li><b>4.</b><span>How was your overall experience at our dealership?</span><div
                    class="stars five-stars">
                </div>
                </li>
                <li><b>5.</b><span>How was your overall experience at our dealership?</span><div
                    class="stars five-stars">
                </div>
                </li>
                <li><b>6.</b><span>How was your overall experience at our dealership?</span><div
                    class="stars five-stars">
                </div>
                </li>
                <li><b>7.</b><span>How was your overall experience at our dealership?</span><div
                    class="stars five-stars">
                </div>
                </li>
                <li><b>8.</b><span>How was your overall experience at our dealership?</span><div
                    class="stars five-stars">
                </div>
                </li>
                <li><b>9.</b><span>How was your overall experience at our dealership?</span><div
                    class="stars five-stars">
                </div>
                </li>
                <li><b>10.</b><span>How was your overall experience at our dealership?</span><div
                    class="stars five-stars">
                </div>
                </li>
            </ul>
        </div>
        <div class="client-survey-box-controls">
            <a href="#csi_popup_takingsurvey" id="csi_popup_takingsurvey_btn"><button type="" name="take-new-survey">
                Take New Survey
            </button></a>
            <button type="" name="send-new-survey">
                Send New Survey
            </button>
        </div>
    </div>
    <div class="customer-profile-communication">
        <div class="communication-controls">
            <a href="#csi_popup_newcustomer" id="csi_newcustomer_btn"><button type="" name="">
                New
            </button></a>
        </div>
        <h3 class="customer-profile-reviews-header">
            Communication</h3>
        <div class="communication-table">
            <div class="scrollableContainer">
                <div class="scrollingArea">
                    <table class="list" cellspacing="0">
                        <thead>
                            <tr>
                                <th width="100">
                                    Date
                                </th>
                                <th width="175">
                                    Type
                                </th>
                                <th width="600">
                                    Notes
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                            <tr>
                                <td width="100">
                                    10/24/2013
                                </td>
                                <td width="175">
                                    Inbound Call
                                </td>
                                <td width="600">
                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                                    incididunt ut labore et dolore magna aliqua.
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
</div>
<div class="popup take-survey-popup hidden" id="csi_popup_takingsurvey">
    <div class="popup-wrap">
        <h3>
            Taking Survey</h3>
        <div class="controls">
            <button name="cancel-survey">
                Cancel Survey
            </button>
            <button name="save-survey">
                Save Survey
            </button>
        </div>
        <div class="questions-wrapper">
            <div class="client-survey">
                <ul>
                    <li><b>1.</b><span>How was your overall experience at our dealership ipsum dolar sit
                        amet non sequitor?</span>
                        <div class="rate-box-choice">
                            Horrid
                            <br>
                            <input type="radio" name="q01" />
                        </div>
                        <div class="rate-box-choice">
                            Bad
                            <br>
                            <input type="radio" name="q01" />
                        </div>
                        <div class="rate-box-choice">
                            Average
                            <br>
                            <input type="radio" name="q01" />
                        </div>
                        <div class="rate-box-choice">
                            Good
                            <br>
                            <input type="radio" name="q01" />
                        </div>
                        <div class="rate-box-choice">
                            Great
                            <br>
                            <input type="radio" name="q01" />
                        </div>
                    </li>
                    <li><b>2.</b><span>How was your overall experience at our dealership?</span>
                        <div class="rate-box-choice">
                            Horrid
                            <br>
                            <input type="radio" name="q02" />
                        </div>
                        <div class="rate-box-choice">
                            Bad
                            <br>
                            <input type="radio" name="q02" />
                        </div>
                        <div class="rate-box-choice">
                            Average
                            <br>
                            <input type="radio" name="q02" />
                        </div>
                        <div class="rate-box-choice">
                            Good
                            <br>
                            <input type="radio" name="q02" />
                        </div>
                        <div class="rate-box-choice">
                            Great
                            <br>
                            <input type="radio" name="q02" />
                        </div>
                    </li>
                    <li><b>3.</b><span>How was your overall experience at our dealership?</span>
                        <div class="rate-box-choice">
                            Horrid
                            <br>
                            <input type="radio" name="q03" />
                        </div>
                        <div class="rate-box-choice">
                            Bad
                            <br>
                            <input type="radio" name="q03" />
                        </div>
                        <div class="rate-box-choice">
                            Average
                            <br>
                            <input type="radio" name="q03" />
                        </div>
                        <div class="rate-box-choice">
                            Good
                            <br>
                            <input type="radio" name="q03" />
                        </div>
                        <div class="rate-box-choice">
                            Great
                            <br>
                            <input type="radio" name="q03" />
                        </div>
                    </li>
                    <li><b>4.</b><span>How was your overall experience at our dealership?</span>
                        <div class="rate-box-choice">
                            Horrid
                            <br>
                            <input type="radio" name="q04" />
                        </div>
                        <div class="rate-box-choice">
                            Bad
                            <br>
                            <input type="radio" name="q04" />
                        </div>
                        <div class="rate-box-choice">
                            Average
                            <br>
                            <input type="radio" name="q04" />
                        </div>
                        <div class="rate-box-choice">
                            Good
                            <br>
                            <input type="radio" name="q04" />
                        </div>
                        <div class="rate-box-choice">
                            Great
                            <br>
                            <input type="radio" name="q04" />
                        </div>
                    </li>
                    <li><b>5.</b><span>How was your overall experience at our dealership?</span>
                        <div class="rate-box-choice">
                            Horrid
                            <br>
                            <input type="radio" name="q05" />
                        </div>
                        <div class="rate-box-choice">
                            Bad
                            <br>
                            <input type="radio" name="q05" />
                        </div>
                        <div class="rate-box-choice">
                            Average
                            <br>
                            <input type="radio" name="q05" />
                        </div>
                        <div class="rate-box-choice">
                            Good
                            <br>
                            <input type="radio" name="q05" />
                        </div>
                        <div class="rate-box-choice">
                            Great
                            <br>
                            <input type="radio" name="q05" />
                        </div>
                    </li>
                    <li><b>6.</b><span>How was your overall experience at our dealership?</span>
                        <div class="rate-box-choice">
                            Horrid
                            <br>
                            <input type="radio" name="q06" />
                        </div>
                        <div class="rate-box-choice">
                            Bad
                            <br>
                            <input type="radio" name="q06" />
                        </div>
                        <div class="rate-box-choice">
                            Average
                            <br>
                            <input type="radio" name="q06" />
                        </div>
                        <div class="rate-box-choice">
                            Good
                            <br>
                            <input type="radio" name="q06" />
                        </div>
                        <div class="rate-box-choice">
                            Great
                            <br>
                            <input type="radio" name="q06" />
                        </div>
                    </li>
                    <li><b>7.</b><span>How was your overall experience at our dealership?</span>
                        <div class="rate-box-choice">
                            Horrid
                            <br>
                            <input type="radio" name="q07" />
                        </div>
                        <div class="rate-box-choice">
                            Bad
                            <br>
                            <input type="radio" name="q07" />
                        </div>
                        <div class="rate-box-choice">
                            Average
                            <br>
                            <input type="radio" name="q07" />
                        </div>
                        <div class="rate-box-choice">
                            Good
                            <br>
                            <input type="radio" name="q07" />
                        </div>
                        <div class="rate-box-choice">
                            Great
                            <br>
                            <input type="radio" name="q07" />
                        </div>
                    </li>
                    <li><b>8.</b><span>How was your overall experience at our dealership?</span>
                        <div class="rate-box-choice">
                            Horrid
                            <br>
                            <input type="radio" name="q08" />
                        </div>
                        <div class="rate-box-choice">
                            Bad
                            <br>
                            <input type="radio" name="q08" />
                        </div>
                        <div class="rate-box-choice">
                            Average
                            <br>
                            <input type="radio" name="q08" />
                        </div>
                        <div class="rate-box-choice">
                            Good
                            <br>
                            <input type="radio" name="q08" />
                        </div>
                        <div class="rate-box-choice">
                            Great
                            <br>
                            <input type="radio" name="q08" />
                        </div>
                    </li>
                    <li><b>9.</b><span>How was your overall experience at our dealership?</span>
                        <div class="rate-box-choice">
                            Horrid
                            <br>
                            <input type="radio" name="q09" />
                        </div>
                        <div class="rate-box-choice">
                            Bad
                            <br>
                            <input type="radio" name="q09" />
                        </div>
                        <div class="rate-box-choice">
                            Average
                            <br>
                            <input type="radio" name="q09" />
                        </div>
                        <div class="rate-box-choice">
                            Good
                            <br>
                            <input type="radio" name="q09" />
                        </div>
                        <div class="rate-box-choice">
                            Great
                            <br>
                            <input type="radio" name="q09" />
                        </div>
                    </li>
                    <li><b>10.</b><span>How was your overall experience at our dealership?</span>
                        <div class="rate-box-choice">
                            Horrid
                            <br>
                            <input type="radio" name="q10" />
                        </div>
                        <div class="rate-box-choice">
                            Bad
                            <br>
                            <input type="radio" name="q10" />
                        </div>
                        <div class="rate-box-choice">
                            Average
                            <br>
                            <input type="radio" name="q10" />
                        </div>
                        <div class="rate-box-choice">
                            Good
                            <br>
                            <input type="radio" name="q10" />
                        </div>
                        <div class="rate-box-choice">
                            Great
                            <br>
                            <input type="radio" name="q10" />
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</div>
<div class="popup customer-information-popup hidden" id="csi_popup_newcustomer">
    <div class="popup-wrap">
        <div class="customer-info-slider">
            <h3 class="customer-profile-reviews-header">
                Customer Info</h3>
            <div class="popup-section-wrapper">
                <div class="client-status">
                    <img src="/Content/Images/social/Orange-Circle.png" alt="">
                </div>
                <div class="show-client-info">
                    <img src="/Content/Images/social/triangle.png" />
                </div>
                <div class="popup-client-info hidden">
                    <ul>
                        <li class="popup-client-info-header">Client </li>
                        <li><b>Name:</b> Jeff Hakimi </li>
                        <li><b>Home Phone:</b> (123) 456-9870 </li>
                        <li><b>Cell Phone:</b> (123) 456-9870 </li>
                        <li><b>Email:</b> jeff@vincontrol.com </li>
                    </ul>
                    <ul>
                        <li class="popup-client-info-header">Vehicle </li>
                        <li><b>Year:</b> 2013 </li>
                        <li><b>Make:</b> Mercedes-Benz </li>
                        <li><b>Model:</b> S-Class </li>
                        <li><b>Trim:</b> S500 </li>
                    </ul>
                    <ul>
                        <li class="popup-client-info-header">Sales </li>
                        <li><b>Salesperson:</b> John Wilson </li>
                        <li><b>Manager:</b> Bob Williams </li>
                        <li><b>Client Rep:</b> Sandy Adams </li>
                    </ul>
                </div>
                <div class="popup-client-form">
                    <div class="form-container">
                        <b>Type</b>
                        <select>
                            <option value="">Inbound Call</option>
                            <option value="">Outbound Call</option>
                            <option value="">Inbound Email</option>
                            <option value="">Outbound Email</option>
                        </select>
                    </div>
                    <div class="form-container">
                        <b>Action</b>
                        <select>
                            <option value="">Status Update</option>
                        </select>
                    </div>
                </div>
            </div>
            <h3 class="customer-profile-reviews-header">
                Script</h3>
            <div class="popup-section-wrapper">
                <div class="script">
                    Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                    incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud
                    exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute
                    irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla
                    pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia
                    deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipisicing
                    elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim
                    ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea
                    commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse
                    cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident,
                    sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor
                    sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore
                    et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco
                    laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit
                    in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat
                    cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
                </div>
            </div>
            <h3 class="customer-profile-reviews-header">
                Notes</h3>
            <div class="popup-section-wrapper">
                <div class="popup-notes-controls">
                    <button type="">
                        No Answer
                    </button>
                    <button type="">
                        Left a Message
                    </button>
                    <button type="">
                        Wrong Number
                    </button>
                </div>
                <div class="notes">
                    <p>
                        Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                        incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud
                        exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute
                        irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla
                        pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia
                        deserunt mollit anim id est laborum.
                    </p>
                    <p>
                        Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
                        incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud
                        exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute
                        irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla
                        pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia
                        deserunt mollit anim id est laborum.
                    </p>
                </div>
            </div>
            <h3 class="customer-profile-reviews-header">
                Status Update</h3>
            <div class="popup-section-wrapper">
                <div class="status-update-form">
                    <select name="status-update-dropdown">
                        <option value="resolved">Resolved</option>
                        <option>Follow-Up</option>
                    </select>
                    <button class="client-popup-save">
                        Save
                    </button>
                    <button class="client-popup-cancel">
                        Cancel
                    </button>
                </div>
                <div class="status-update-form-wrap">
                    <div class="form-container">
                        <b>Date</b>
                        <input type="text" name="" class="datepicker" value="" placeholder="">
                    </div>
                    <div class="form-container">
                        <b>Time</b>
                        <input type="text" name="" value="" placeholder="">
                    </div>
                    <div class="form-container">
                        <b>Call</b>
                        <input type="text" name="" value="" placeholder="">
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
