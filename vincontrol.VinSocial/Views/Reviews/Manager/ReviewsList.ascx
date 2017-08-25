<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div class="reviews-list-tab page-tab hidden">
    <div class="reviews-list-wrap">
        <div class="reviews-listing-filter">
            Filter:
            <select>
                <option>Any Review Site</option>
                <!-- Options Below -->
                <option>Yelp</option>
                <option>Dealer Rater</option>
                <option>Google+</option>
            </select>
            <select>
                <option>Any Rating</option>
                <!-- Options Below -->
                <option class="one-star star-dropdown"></option>
                <option class="two-star star-dropdown"></option>
                <option class="three-star star-dropdown"></option>
                <option class="four-star star-dropdown"></option>
            </select>
        </div>
        <div class="reviews-listing-items">
            <div class="scrollableContainer">
                <div class="scrollingArea">
                    <table class="list" cellspacing="0">
                        <thead>
                            <tr>
                                <th width="60">
                                    Site
                                </th>
                                <th width="175">
                                    Salesperson
                                </th>
                                <th width="250">
                                    Description
                                </th>
                                <th width="110">
                                    Department
                                </th>
                                <th width="85">
                                    Rating
                                </th>
                                <th width="130">
                                    Date
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                            <tr>
                                <td class="review-site-source" width="60">
                                    <span class="ico">ICO</span>
                                </td>
                                <td class="review-customer-name" width="175">
                                    Testname Goesrighthere
                                </td>
                                <td class="review-description" width="250">
                                    Description
                                </td>
                                <td class="review-department" width="100">
                                    <select>
                                        <option>Sales</option>
                                        <option>Service</option>
                                        <option>Finance</option>
                                        <option>Parts</option>
                                    </select>
                                </td>
                                <td class="review-star-rating" width="100">
                                    <div class="stars five-stars">
                                    </div>
                                </td>
                                <td class="review-date" width="100">
                                    Date
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <div class="popup survey-popup hidden">
        <!-- This opens when they click on a survey in a list -->
        <div class="popup-wrap">
            <div class="survey-total-rating stars five-stars">
            </div>
            <h3>
                Customer Survey</h3>
            <div class="survey-popup-customer-info">
                <ul class="customer-info-list">
                    <li>
                        <div class="customer-info-list-label">
                            Name
                        </div>
                        Johnathan T. Hamilton </li>
                    <li>
                        <div class="customer-info-list-label">
                            Email
                        </div>
                        john.hamilton@hotmail.com </li>
                    <li>
                        <div class="customer-info-list-label">
                            Work Phone
                        </div>
                        (123) 456-7890 </li>
                    <li>
                        <div class="customer-info-list-label">
                            Cell/Home Phone
                        </div>
                        (123) 456-7890 </li>
                </ul>
            </div>
            <h3>
                Questions</h3>
            <div class="survey-popup-survey-answers">
                <ul class="survey-answer-list">
                    <li class="survey-question">
                        <div class="question-label">
                            Q.1
                        </div>
                        <div class="question-text">
                            Lorem ipsum dolor sit amet, consectetur adipisicing elit?
                        </div>
                        <div class="question-answer">
                            4
                        </div>
                    </li>
                    <li class="survey-question">
                        <div class="question-label">
                            Q.2
                        </div>
                        <div class="question-text">
                            Lorem ipsum dolor sit amet, consectetur adipisicing elit?
                        </div>
                        <div class="question-answer">
                            1.5
                        </div>
                    </li>
                    <li class="survey-question">
                        <div class="question-label">
                            Q.3
                        </div>
                        <div class="question-text">
                            Lorem ipsum dolor sit amet, consectetur adipisicing elit?
                        </div>
                        <div class="question-answer">
                            0.5
                        </div>
                    </li>
                    <li class="survey-question">
                        <div class="question-label">
                            Q.4
                        </div>
                        <div class="question-text">
                            Lorem ipsum dolor sit amet, consectetur adipisicing elit?
                        </div>
                        <div class="question-answer">
                            4.5
                        </div>
                    </li>
                    <li class="survey-question">
                        <div class="question-label">
                            Q.5
                        </div>
                        <div class="question-text">
                            Lorem ipsum dolor sit amet, consectetur adipisicing elit?
                        </div>
                        <div class="question-answer">
                            2
                        </div>
                    </li>
                </ul>
                <div class="comments">
                    <b>Comments:</b> Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do
                    eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
                    quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.
                    Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu
                    fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa
                    qui officia deserunt mollit anim id est laborum.
                </div>
                <div class="popup-btn popup-cancel-btn">
                    Close
                </div>
            </div>
        </div>
    </div>
    <div class="popup review-popup hidden">
        <!-- this opens when they click on a review in a list -->
        <div class="popup-wrap">
            <h3>
                Customer Review</h3>
            <div class="review-popup-customer-info">
                <ul class="customer-info-list">
                    <li>
                        <div class="customer-info-list-label">
                            Name
                        </div>
                        Johnathan T. Hamilton </li>
                    <li>
                        <div class="customer-info-list-label">
                            Email
                        </div>
                        john.hamilton@hotmail.com </li>
                    <li>
                        <div class="customer-info-list-label">
                            Work Phone
                        </div>
                        (123) 456-7890 </li>
                    <li>
                        <div class="customer-info-list-label">
                            Cell/Home Phone
                        </div>
                        (123) 456-7890 </li>
                </ul>
            </div>
            <div class="review-popup-review-answers">
                <div class="customer-review-rating">
                    <div class="review-website-icon">
                        <img src="/Content/Images/social/yelp-icon.png" alt="">
                    </div>
                    <div class="review-total-rating stars five-stars">
                    </div>
                    <div class="view-on-wrap">
                        <a href="http://yelp.com" target="_blank">View on Yelp</a>
                    </div>
                </div>
                <div class="comments">
                    <b>Comments:</b> Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do
                    eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
                    quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.
                    Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu
                    fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa
                    qui officia deserunt mollit anim id est laborum.
                </div>
                <div class="popup-btn popup-cancel-btn">
                    Close
                </div>
            </div>
        </div>
    </div>
</div>
