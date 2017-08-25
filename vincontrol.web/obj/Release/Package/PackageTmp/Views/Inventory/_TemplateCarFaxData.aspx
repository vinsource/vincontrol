<script id="CarFaxTemplate" type="text/x-jsrender">
      {{if ListItems.length > 0}}
      <ul>
                     {{for ListItems}}
            
                            {{if Text == 'Prior Rental'}} 
                                      <li style="background-color: red">
                                     {{:Text}}
                                    <img class="c-fax-img" src= {{:Image}} height="10px" width="10px" />
                                     </li>
                            {{else Text == 'Accident(s) / Damage Reported to CARFAX'}}
                                   <li style="background-color: red">
                                     {{:Text}}
                                    <img class="c-fax-img" src= {{:Image}} height="10px" width="10px" />
                                     </li>
                                {{else}}
                                    <li>
                                     {{:Text}}
                                    <img class="c-fax-img" src= {{:Image}} height="10px" width="10px" />
                                     </li>
                            {{/if}}

                                                 
               
                        {{/for}}
     </ul>
    <a style="margin-left: 25px; color: black; font-weight: bold" href="JavaScript:newPopup('http://www.carfaxonline.com/cfm/Display_Dealer_Report.cfm?partner=VIT_0&UID={{:CarfaxdealerId}}&vin={{:Vin}}')">View Full Report</a>
      {{/if}}
  
</script>
