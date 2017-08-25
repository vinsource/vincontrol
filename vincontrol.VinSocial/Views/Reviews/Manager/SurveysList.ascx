<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div class="survey-list-tab page-tab hidden">
    <div class="survey-list-wrap">
        <div class="survey-list-filter">
            Filter:
            <input type="text" value="Enter Customer Name">
            <select>
                <option>All Stars</option>
                <option>5 Stars</option>
                <option>4+ Stars</option>
                <option>3+ Stars</option>
                <option>2+ Stars</option>
                <option>1+ Stars</option>
            </select>
            <select>
                <option>All Departments</option>
                <option>Sales</option>
                <option>Service</option>
                <option>Finance</option>
                <option>Parts</option>
            </select>
            <select>
                <option>Any Status</option>
                <option>Resolved</option>
                <option>Closed</option>
                <option>In Progress</option>
                <option>Pending</option>
            </select>
        </div>
        <div class="scrollableContainer">
            <div class="scrollingArea">
                <table class="list" cellspacing="0">
                    <thead>
                        <tr>
                            <th width="130">
                                Customer
                            </th>
                            <th width="75">
                                Rating
                            </th>
                            <th width="70">
                                Salesperson
                            </th>
                            <th width="90">
                                Department
                            </th>
                            <th width="300">
                                Description
                            </th>
                            <th width="42">
                                Date
                            </th>
                            <th width="75">
                                Resolved?
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                        <tr>
                            <td class="review-customer-name" width="125">
                                Testname Goesrighthere
                            </td>
                            <td class="review-star-rating" width="75">
                                <div class="stars three-half-stars">
                                </div>
                            </td>
                            <td class="review-salespeople" width="75">
                                Salesperson
                            </td>
                            <td class="review-departments" width="95">
                                Department
                            </td>
                            <td class="review-description" width="300">
                                Description
                            </td>
                            <td class="review-date" width="42">
                                10/22/13
                            </td>
                            <td class="review-date" width="55">
                                Unresolved
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</div>
