/*
Copyright (c) 2003-2010, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.addTemplates('default', { imagesPath: CKEDITOR.getUrl(CKEDITOR.plugins.getPath('templates') + 'templates/images/'),
    templates:
            [
                {
                    title: 'Image and Title',
                    image: 'template1.gif',
                    description: 'One main image with a title and text that surround the image.',
                    html:
                            '<h3><img style="margin-right: 10px" height="100" width="100" align="left"/>Type the title here</h3><p>Type the text here</p>'
                },

                {
                    title: 'Strange Template',
                    image: 'template2.gif',
                    description: 'A template that defines two colums, each one with a title, and some text.',
                    html: '<table cellspacing="0" cellpadding="0" style="width:100%" border="0"><tr><td style="width:50%"><h3>Title 1</h3></td><td></td><td style="width:50%"><h3>Title 2</h3></td></tr><tr><td>Text 1</td><td></td><td>Text 2</td></tr></table><p>More text goes here.</p>'
                },

                {
                    title: 'Text and Table',
                    image: 'template3.gif',
                    description: 'A title with some text and a table.',
                    html: '<div style="width: 80%"><h3>Title goes here</h3><table style="width:150px;float: right" cellspacing="0" cellpadding="0" border="1"><caption style="border:solid 1px black"><strong>Table title</strong></caption></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table><p>Type the text here</p></div>'
                }
            ]
});

            CKEDITOR.addTemplates('custom', { imagesPath: CKEDITOR.getUrl(CKEDITOR.plugins.getPath('templates') + 'templates/images/'),
                templates:
            [
                {
                    title: 'Key Investment Personnel Biographies',
                    image: 'custom_template1.PNG',
                    html:
                        '<table style="width:100%" cellspacing="0" cellpadding="0" >' +
                        '<tr>' +
                        '<th><p style="text-align:left;background-color:#DBDBDB; ">Employee</p></th>' +
                        '<th><p style="text-align:left;background-color:#DBDBDB; ">Biography</p></th>' +
                        '<th><p style="text-align:center;background-color:#DBDBDB; ">Year Hired</p></th>' +
                        '</tr>' +
                        '<tr>' +
                        '<td style="width:22%;vertical-align:top;"><b>(Employee Name)</b><br/>(Job Title)</td>' +
                        '<td style="width:65%;vertical-align:top;">1. Primary responsibilities. 2. All relevant prior work experience starting with the most recent. 3. Education starting with the most recent.</td>' +
                        '<td style="width:13%;vertical-align:top;text-align:center;">(Hire Year)</td>' +
                        '</tr>' +
                        '</table>'
                },

                {
                    title: 'Core Investment Strategies',
                    image: 'custom_template2.PNG',
                    html:
                           '<table style="width:100%" cellspacing="0" cellpadding="0" >' +
                           '<tr>' +
                           '<th colspan="2"><p style="text-align:left;background-color:#DBDBDB; ">Core Investment Strategies</p></th>' +
                           '</tr>' +
                           '<tr>' +
                           '<td style="width:22%;vertical-align:top;">(Investment Strategy)</td>' +
                           '<td style="width:78%;vertical-align:top;">(Overview of the investment strategy)</td>' +
                           '</tr>' +
                           '</table>'
                },

                {
                    title: 'ASC 820 Levels of the Portfolio Fund',
                    image: 'custom_template3.PNG',
                    html:
                           '<table style="width:42%" cellspacing="0" cellpadding="0">' +
                           '<tr>' +
                           '<th colspan="2"><p style="text-align:left;background-color:#DBDBDB; ">ASC 820 Levels of the Portfolio Fund</p></th>' +
                           '</tr>' +
                           '<tr>' +
                           '<td style="width:70%;vertical-align:top;">Level 1</td>' +
                           '<td style="width:30%;vertical-align:top;">(X%)</td>' +
                           '</tr>' +
                           '<tr>' +
                           '<td style="vertical-align:top;">Level 2</td>' +
                           '<td style="vertical-align:top;">(X%)</td>' +
                           '</tr>' +
                           '<tr>' +
                           '<td style="vertical-align:top;">Level 3</td>' +
                           '<td style="vertical-align:top;">(X%)</td>' +
                           '</tr>' +
                           '</table>'
                },

                {
                    title: 'Portfolio Fund Service Providers',
                    image: 'custom_template4.PNG',
                    html:
                            '<table style="width:100%" cellspacing="0" cellpadding="0" >' +
                            '<tr>' +
                            '<th colspan="2"><p style="text-align:left;background-color:#DBDBDB; ">Portfolio Fund Service Providers</p></th>' +
                            '</tr>' +
                            '<tr>' +
                            '<td style="width:35%;vertical-align:top;"><b>Prime Brokers/Custodians</b><br/>Insert list of prime brokers/custodians</td>' +
                            '<td style="width:65%;vertical-align:top;"><ul><li>Details</li></ul></td>' +
                            '</tr>' +
                            '<tr>' +
                            '<td style="vertical-align:top;"><b>Administrator</b><br/> Insert administrator</td>' +
                            '<td style="vertical-align:top;"><ul><li>Details</li></ul></td>' +
                            '</tr>' +
                            '<tr>'+
                            '<td style="vertical-align:top;"><b>Independent Public Accountants</b><br/> Insert independent public accountant</td>' +
                            '<td style="vertical-align:top;"><ul><li>Details</li></ul></td>' +
                            '</tr>' +
                            '<tr>' +
                            '<td style="vertical-align:top;"><b>Legal Counsel</b><br/> Insert list of legal counsel</td>' +
                            '<td style="vertical-align:top;"><ul><li>Details</li></ul></td>' +
                            '</tr>' +
                            '<tr>' +
                            '<td style="vertical-align:top;"><b>Compliance</b><br/> Insert list of compliance advisors</td>' +
                            '<td style="vertical-align:top;"><ul><li>Details</li></ul></td>' +
                            '</tr>' +
                            '<tr>' +
                            '<td style="vertical-align:top;"><b>Risk Management Provider</b><br/> Insert risk management provider</td>' +
                            '<td style="vertical-align:top;"><ul><li>Details</li></ul></td>' +
                            '</tr>' +
                            '<tr>' +
                            '<td style="vertical-align:top;"><b>Business Continuity and Disaster Recovery Enabler</b><br/> Insert BC and DR enabler</td>' +
                            '<td style="vertical-align:top;"><ul><li>Details</li></ul></td>' +
                            '</tr>' +
                            '</table>'
                },

                {
                    title: 'Legal Terms And Conditions',
                    image: 'custom_template5.PNG',
                    html:
                            '<table style="width:100%" cellspacing="0" cellpadding="0" >' +
                            '<tr>' +
                            '<th colspan="2"><p style="text-align:left;background-color:#DBDBDB; ">Legal Terms and Conditions Abstract</p></th>' +
                            '</tr>' +
                            '<tr>' +
                            '<td style="vertical-align:top;width:180px">Portfolio Fund Structure</td>' +
                            '<td style="vertical-align:top;">(Domicile and Structure of the Portfolio Fund)</td>' +
                            '</tr>' +
                            '<tr>' +
                            '<td style="vertical-align:top;">Investment Manager</td>' +
                            '<td style="vertical-align:top;">(Investment Manager of the Portfolio Fund)</td>' +
                            '</tr>' +
                            '<tr>' +
                            '<td style="vertical-align:top;">Capital Structure</td>' +
                            '<td style="vertical-align:top;">(Structure of the Portfolio Fund share class offerings)</td>' +
                            '</tr>' +
                            '<tr>' +
                            '<td style="vertical-align:top;">Eligible Investors</td>' +
                            '<td style="vertical-align:top;">(Eligibility of investors)</td>' +
                            '</tr>' +
                            '<tr>' +
                            '<td style="vertical-align:top;">Subscriptions</td>' +
                            '<td style="vertical-align:top;">(Subscription options and minimum investment details)</td>' +
                            '</tr>' +
                            '<tr>' +
                            '<td style="vertical-align:top;">Management Fee</td>' +
                            '<td style="vertical-align:top;">(Management fee and calculation details)</td>' +
                            '</tr>' +
                            '<tr>' +
                            '<td style="vertical-align:top;">Incentive Allocation</td>' +
                            '<td style="vertical-align:top;">(Incentive allocation and calculation details)</td>' +
                            '</tr>' +
                            '<tr>' +
                            '<td style="vertical-align:top;">Expenses</td>' +
                            '<td style="vertical-align:top;">(Portfolio Fund expenses)</td>' +
                            '</tr>' +
                            '<tr>' +
                            '<td style="vertical-align:top;">ERISA Entities</td>' +
                            '<td style="vertical-align:top;">(ERISA entity investments)</td>' +
                            '</tr>' +
                            '<tr>' +
                            '<td style="vertical-align:top;"><div>Redemption Frequency</div><div>and Notice</div></td>' +
                            '<td style="vertical-align:top;">(Redemption frequency and notice period)</td>' +
                            '</tr>' +
                            '<tr>' +
                            '<td style="vertical-align:top;">Gate</td>' +
                            '<td style="vertical-align:top;">(Redemption gate and application)</td>' +
                            '</tr>' +
                            '<tr>' +
                            '<td style="vertical-align:top;">Side Pockets</td>' +
                            '<td style="vertical-align:top;">(State if side pockets exist and the policy associated with them)</td>' +
                            '</tr>' +
                            '<tr>' +
                            '<td style="vertical-align:top;">Key Man</td>' +
                            '<td style="vertical-align:top;">(Key man policies)</td>' +
                            '</tr>' +
                            '<tr>' +
                            '<td style="vertical-align:top;">Settlement</td>' +
                            '<td style="vertical-align:top;">(Settlement provisions)</td>' +
                            '</tr>' +
                            '</table>'
                },

                {
                    title: 'Portfolio Fund Related Documentation',
                    image: 'custom_template6.PNG',
                    html:
                           '<table style="width:100%" cellspacing="0" cellpadding="0" >' +
                           '<tr>' +
                           '<th><p style="text-align:left;background-color:#DBDBDB; ">Document</p></th>' +
                           '<th><p style="text-align:left;background-color:#DBDBDB; ">Last Updated Date</p></th>' +
                           '</tr>' +
                           '<tr>' +
                           '<td style="vertical-align:top;width:60%;">Document name</td>' +
                           '<td style="vertical-align:top;width:300px;">Document date</td>' +
                           '</tr>' +
                           '</table>'
                },

                {
                    title: 'Systems And Technology Infrastructure',
                    image: 'custom_template7.PNG',
                    html:
                           '<table style="width:60%" cellspacing="0" cellpadding="0" >' +
                           '<tr>' +
                           '<th colspan="2"><p style="text-align:left;background-color:#DBDBDB; ">Systems</p></th>' +
                           '</tr>' +
                           '<tr>' +
                           '<td style="width:40%;vertical-align:top;">System Type</td>' +
                           '<td style="width:60%;vertical-align:top;">System Detail</td>' +
                           '</tr>' +
                           '</table>'
                }
            ]
            });
