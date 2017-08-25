
function FilterCarModelForMarket(modelWord) {
    modelWord = modelWord.replace("new", "");

    modelWord = modelWord.replace("sdn", "");

    modelWord = modelWord.replace("xl", "");

    modelWord = modelWord.replace("wagon", "");

    modelWord = modelWord.replace("(natl)", "");

    modelWord = modelWord.replace("sedan", "");

    modelWord = modelWord.replace("sdn", "");

    modelWord = modelWord.replace("coupe", "");

    modelWord = modelWord.replace("cpe", "");

    modelWord = modelWord.replace("convertible", "");

    modelWord = modelWord.replace("utility", "");

    modelWord = modelWord.replace("2d", "");

    modelWord = modelWord.replace("4d", "");

    modelWord = modelWord.replace("4matic", "");

    modelWord = modelWord.replace("unlimited", "");

    modelWord = modelWord.replace("sportWagen", "");

    modelWord = modelWord.replace("lwb", "");

    modelWord = modelWord.replace("classic", "");

    modelWord = modelWord.replace("cargo van", "");

    modelWord = modelWord.replace("commercial cutaway", "");

    modelWord = modelWord.replace("commercial chassis", "");

    modelWord = modelWord.replace("passenger", "");

    modelWord = modelWord.replace("super duty", "");

    modelWord = modelWord.replace("drw", "");

    modelWord = modelWord.replace("-", "");

    modelWord = modelWord.replace("4wdtruck", "");

    modelWord = modelWord.replace("2wdtruck", "");

    modelWord = modelWord.replace("awdtruck", "");

    modelWord = modelWord.replace("srw", "");

    modelWord = modelWord.replace("lwb", "");

    modelWord = modelWord.replace("swb", "");

    return modelWord;

}

function ExactvsSimilarMatchlist(list, dcar) {
        var exactMatchList = [];
        var similartMatchList = [];


        var exactTrim = dcar.title.trim.toLowerCase();
        var make = dcar.title.make.toLowerCase();
        var modelWord = dcar.title.model.toLowerCase();

    var model = FilterCarModelForMarket(modelWord);
    
        if (make == "land rover") {
            
            if (model == "range rover sport" || model == "range rover") {
              
                if (exactTrim == 'hse lux') {
                    for (var i = 0; i < list.length; i++) {
                        {
                            var trim = list[i].title.trim.toLowerCase();

                            if (trim == 'hse lux') {
                                exactMatchList.push(list[i]);
                            } else {
                                similartMatchList.push(list[i]);
                            }


                        }
                    }
                }

                if (exactTrim == 'hse') {
                    for (var i = 0; i < list.length; i++) {
                        {
                            var trim = list[i].title.trim.toLowerCase();

                            if (trim == 'hse') {
                                exactMatchList.push(list[i]);
                            } else {
                                similartMatchList.push(list[i]);
                            }


                        }
                    }
                }

                if (exactTrim == 'sc' || exactTrim == 'supercharged') {
                    for (var i = 0; i < list.length; i++) {
                        {
                            var trim = list[i].title.trim.toLowerCase();

                            if (trim == 'sc') {
                                exactMatchList.push(list[i]);
                            } else {
                                similartMatchList.push(list[i]);
                            }


                        }
                    }
                }
          
               
            }
            else if (model == "range rover evoque" ) {

                if (exactTrim == 'pure plus') {
                    for (var i = 0; i < list.length; i++) {
                        {
                            var trim = list[i].title.trim.toLowerCase();

                            if (trim == 'pure plus') {
                                exactMatchList.push(list[i]);
                            } else {
                                similartMatchList.push(list[i]);
                            }


                        }
                    }
                }

                if (exactTrim == 'pure premium') {
                    for (var i = 0; i < list.length; i++) {
                        {
                            var trim = list[i].title.trim.toLowerCase();

                            if (trim == 'pure premium') {
                                exactMatchList.push(list[i]);
                            } else {
                                similartMatchList.push(list[i]);
                            }


                        }
                    }
                }

                if (exactTrim == 'pure' ) {
                    for (var i = 0; i < list.length; i++) {
                        {
                            var trim = list[i].title.trim.toLowerCase();

                            if (trim == 'pure') {
                                exactMatchList.push(list[i]);
                            } else {
                                similartMatchList.push(list[i]);
                            }


                        }
                    }
                }


            }
            else if (model == "lr2" || model=="lr3" || model=="lr4") {

                if (exactTrim == "hse") {
                    for (var i = 0; i < list.length; i++) {
                        {
                            var trim = list[i].title.trim.toLowerCase();

                            if (trim == "hse") {
                                exactMatchList.push(list[i]);
                            } else {
                                similartMatchList.push(list[i]);
                            }


                        }
                    }
                }

                if (exactTrim == "se") {
                    for (var i = 0; i < list.length; i++) {
                        {
                            var trim = list[i].title.trim.toLowerCase();

                            if (trim == "se") {
                                exactMatchList.push(list[i]);
                            } else {
                                similartMatchList.push(list[i]);
                            }


                        }
                    }
                }
                if (exactTrim == "lux") {
                    for (var i = 0; i < list.length; i++) {
                        {
                            var trim = list[i].title.trim.toLowerCase();

                            if (trim == "lux") {
                                exactMatchList.push(list[i]);
                            } else {
                                similartMatchList.push(list[i]);
                            }


                        }
                    }
                }
                if (exactTrim == "luxury") {
                    for (var i = 0; i < list.length; i++) {
                        {
                            var trim = list[i].title.trim.toLowerCase();

                            if (trim == "luxury") {
                                exactMatchList.push(list[i]);
                            } else {
                                similartMatchList.push(list[i]);
                            }


                        }
                    }
                }

             


            }
         
        }

        if (make == "jaguar") {
            
            if (model == "xf") {

                if (exactTrim == "i4 rwd" || exactTrim == "i4 t rwd") {
                    for (var i = 0; i < list.length; i++) {
                        {
                            var trim = list[i].title.trim.toLowerCase();

                            if (trim == "i4 rwd") {
                                exactMatchList.push(list[i]);
                            } else {
                                similartMatchList.push(list[i]);
                            }


                        }
                    }
                }
                if (exactTrim == "v8 xfr-s") {
                    for (var i = 0; i < list.length; i++) {
                        {
                            var trim = list[i].title.trim.toLowerCase();

                            if (trim == "v8 xfr-s") {
                                exactMatchList.push(list[i]);
                            } else {
                                similartMatchList.push(list[i]);
                            }


                        }
                    }
                }
                
                if (exactTrim == "v6 rwd" || exactTrim=='v6 awd') {
                    for (var i = 0; i < list.length; i++) {
                        {
                            var trim = list[i].title.trim.toLowerCase();

                            if (trim.indexOf("v6")!=-1 ) {
                                exactMatchList.push(list[i]);
                            } else {
                                similartMatchList.push(list[i]);
                            }


                        }
                    }
                }

                if (exactTrim == "v6 sc rwd") {
                    for (var i = 0; i < list.length; i++) {
                        {
                            var trim = list[i].title.trim.toLowerCase();

                            if (trim.indexOf("sc") != -1) {
                                exactMatchList.push(list[i]);
                            } else {
                                similartMatchList.push(list[i]);
                            }


                        }
                    }
                }


                if (exactTrim == "supercharged") {
                    for (var i = 0; i < list.length; i++) {
                        {
                            var trim = list[i].title.trim.toLowerCase();

                            if (trim == "supercharged") {
                                exactMatchList.push(list[i]);
                            } else {
                                similartMatchList.push(list[i]);
                            }


                        }
                    }
                }

                if (exactTrim == "portfolio") {
                    for (var i = 0; i < list.length; i++) {
                        {
                            var trim = list[i].title.trim.toLowerCase();

                            if (trim == "portfolio") {
                                exactMatchList.push(list[i]);
                            } else {
                                similartMatchList.push(list[i]);
                            }


                        }
                    }
                }

                if (exactTrim == "premium luxury") {
                    
                    for (var i = 0; i < list.length; i++) {
                        {
                            var trim = list[i].title.trim.toLowerCase();

                            if (trim == "premium") {
                                exactMatchList.push(list[i]);
                            } else {
                                similartMatchList.push(list[i]);
                            }


                        }
                    }
                }

                if (exactTrim == "premium") {
                    for (var i = 0; i < list.length; i++) {
                        {
                            var trim = list[i].title.trim.toLowerCase();

                            if (trim == "premium") {
                                exactMatchList.push(list[i]);
                            } else {
                                similartMatchList.push(list[i]);
                            }


                        }
                    }
                }


            }
            else if (model == "xj" || model == "xjl" ) {
                
                if (exactTrim == "supercharged") {
                    for (var i = 0; i < list.length; i++) {
                        {
                            var trim = list[i].title.trim.toLowerCase();

                            if (trim == "supercharged") {
                                exactMatchList.push(list[i]);
                            } else {
                                similartMatchList.push(list[i]);
                            }


                        }
                    }
                }

                if (exactTrim == "vdp") {
                    for (var i = 0; i < list.length; i++) {
                        {
                            var trim = list[i].title.trim.toLowerCase();

                            if (trim == "vdp") {
                                exactMatchList.push(list[i]);
                            } else {
                                similartMatchList.push(list[i]);
                            }


                        }
                    }
                }

                if (exactTrim == "xjl supercharged") {
                    for (var i = 0; i < list.length; i++) {
                        {
                            var trim = list[i].title.trim.toLowerCase();

                            if (trim == "xjl supercharged") {
                                exactMatchList.push(list[i]);
                            } else {
                                similartMatchList.push(list[i]);
                            }


                        }
                    }
                }

                if (exactTrim == "xjl ultimate") {
                    for (var i = 0; i < list.length; i++) {
                        {
                            var trim = list[i].title.trim.toLowerCase();

                            if (trim == "xjl ultimate") {
                                exactMatchList.push(list[i]);
                            } else {
                                similartMatchList.push(list[i]);
                            }


                        }
                    }
                }
                if (exactTrim == "xjl portfolio") {
                    for (var i = 0; i < list.length; i++) {
                        {
                            var trim = list[i].title.trim.toLowerCase();

                            if (trim == "xjl portfolio") {
                                exactMatchList.push(list[i]);
                            } else {
                                similartMatchList.push(list[i]);
                            }


                        }
                    }
                }

                if (exactTrim == "xj8 lwb") {
                    for (var i = 0; i < list.length; i++) {
                        {
                            var trim = list[i].title.trim.toLowerCase();

                            if (trim == "xj8 l") {
                                exactMatchList.push(list[i]);
                            } else {
                                similartMatchList.push(list[i]);
                            }


                        }
                    }
                }

            }

          else if (model == "xk") {

              if (exactTrim == "xkr" || exactTrim=="2dr conv xkr") {
                    for (var i = 0; i < list.length; i++) {
                        {
                            var trim = list[i].title.trim.toLowerCase();

                            if (trim == "xkr") {
                                exactMatchList.push(list[i]);
                            } else {
                                similartMatchList.push(list[i]);
                            }


                        }
                    }
                }

                if (exactTrim == "touring") {
                    for (var i = 0; i < list.length; i++) {
                        {
                            var trim = list[i].title.trim.toLowerCase();

                            if (trim == "touring") {
                                exactMatchList.push(list[i]);
                            } else {
                                similartMatchList.push(list[i]);
                            }


                        }
                    }
                }


          }

          else if (model == "ftype") {
               
              if (exactTrim == "v8 s") {
                  for (var i = 0; i < list.length; i++) {
                      {
                          var trim = list[i].title.trim.toLowerCase();

                          if (trim == "v8 s") {
                              exactMatchList.push(list[i]);
                          } else {
                              similartMatchList.push(list[i]);
                          }


                      }
                  }
              }

              


          }

        }


        if (make == "mini") {

            if (model == "cooper hardtop" || model == "cooper" || model == "cooper country man" || model == "cooper clubman") {
                if (exactTrim == "s") {
                    for (var i = 0; i < list.length; i++) {
                        {
                            var trim = list[i].title.trim.toLowerCase();

                            if (trim == "s") {
                                exactMatchList.push(list[i]);
                            } else {
                                similartMatchList.push(list[i]);
                            }


                        }
                    }
                }
            }
            
        }
    if (make == "mercedes-benz") {

        for (var i = 0; i < list.length; i++) {

            var trim = list[i].title.model.toLowerCase();

            exactTrim = exactTrim.replace("sport", "");

            exactTrim = exactTrim.replace("luxury", "");

            exactTrim = exactTrim.replace("bluetec", "");

            exactTrim = exactTrim.replace("btc", "");

            exactTrim = exactTrim.replace("cdi", "");

            exactTrim = exactTrim.replace("blk", "");


            if (exactTrim.length > 2) {
                if (exactTrim.substring(exactTrim.length - 2) == 's4')
                    exactTrim = exactTrim.replace("s4", "");
                if (exactTrim.substring(exactTrim.length - 2) == 'v4')
                    exactTrim = exactTrim.replace("v4", "");
                if (exactTrim.substring(exactTrim.length - 2) == 'w4')
                    exactTrim = exactTrim.replace("w4", "");
                if (exactTrim.substring(exactTrim.length - 2) == 's4')
                    exactTrim = exactTrim.replace("s4", "");
                if (exactTrim.substring(exactTrim.length - 2) == 'ae')
                    exactTrim = exactTrim.replace("ae", "");
                if (exactTrim.substring(exactTrim.length - 2) == 'wz')
                    exactTrim = exactTrim.replace("wz", "");
            }

            if (exactTrim.length >= 1) {
                if (exactTrim.substring(exactTrim.length - 1) == 'w')
                    exactTrim = exactTrim.replace("w", "");
                if (exactTrim.substring(exactTrim.length - 1) == 'r')
                    exactTrim = exactTrim.replace("r", "");
                if (exactTrim.substring(exactTrim.length - 1) == 'v')
                    exactTrim = exactTrim.replace("v", "");
                if (exactTrim.substring(exactTrim.length - 1) == 'c')
                    exactTrim = exactTrim.replace("c", "");
                if (exactTrim.substring(exactTrim.length - 1) == 'a')
                    exactTrim = exactTrim.replace("a", "");
                if (exactTrim.substring(exactTrim.length - 1) == 'k')
                    exactTrim = exactTrim.replace("k", "");
            }

            exactTrim = FilterCarModelForMarket(exactTrim);
            
            exactTrim = exactTrim.replace(" ", "");

            if (trim == exactTrim) {
              
                exactMatchList.push(list[i]);
            } else {
              
                similartMatchList.push(list[i]);
            }


        }
    }

    //if (make == "honda") {

    //        if (model == "crv") {
    //            if (exactTrim == "exl" || exactTrim == "ex-l") {
    //                for (var i = 0; i < list.length; i++) {
    //                    {
    //                        var trim = list[i].title.trim.toLowerCase();

    //                        if (trim == "ex-l") {
    //                            exactMatchList.push(list[i]);
    //                        } else {
    //                            similartMatchList.push(list[i]);
    //                        }


    //                    }
    //                }
    //            }
    //            if (exactTrim == "ex") {
    //                for (var i = 0; i < list.length; i++) {
    //                    {
    //                        var trim = list[i].title.trim.toLowerCase();

    //                        if (trim == "ex") {
    //                            exactMatchList.push(list[i]);
    //                        } else {
    //                            similartMatchList.push(list[i]);
    //                        }


    //                    }
    //                }
    //            }
    //            if (exactTrim == "se") {
    //                for (var i = 0; i < list.length; i++) {
    //                    {
    //                        var trim = list[i].title.trim.toLowerCase();

    //                        if (trim == "se") {
    //                            exactMatchList.push(list[i]);
    //                        } else {
    //                            similartMatchList.push(list[i]);
    //                        }


    //                    }
    //                }
    //            }
    //            if (exactTrim == "lx") {
    //                for (var i = 0; i < list.length; i++) {
    //                    {
    //                        var trim = list[i].title.trim.toLowerCase();

    //                        if (trim == "lx") {
    //                            exactMatchList.push(list[i]);
    //                        } else {
    //                            similartMatchList.push(list[i]);
    //                        }


    //                    }
    //                }
    //            }
    //        }
    //        else if (model == "accord") {
    //            if (exactTrim == "exl" || exactTrim == "ex-l") {
    //                for (var i = 0; i < list.length; i++) {
    //                    {
    //                        var trim = list[i].title.trim.toLowerCase();

    //                        if (trim == "ex-l") {
    //                            exactMatchList.push(list[i]);
    //                        } else {
    //                            similartMatchList.push(list[i]);
    //                        }


    //                    }
    //                }
    //            }
    //            if (exactTrim == "se") {
    //                for (var i = 0; i < list.length; i++) {
    //                    {
    //                        var trim = list[i].title.trim.toLowerCase();

    //                        if (trim == "se") {
    //                            exactMatchList.push(list[i]);
    //                        } else {
    //                            similartMatchList.push(list[i]);
    //                        }


    //                    }
    //                }
    //            }
    //            if (exactTrim == "lx") {
    //                for (var i = 0; i < list.length; i++) {
    //                    {
    //                        var trim = list[i].title.trim.toLowerCase();

    //                        if (trim == "lx") {
    //                            exactMatchList.push(list[i]);
    //                        } else {
    //                            similartMatchList.push(list[i]);
    //                        }


    //                    }
    //                }
    //            }
    //            if (exactTrim == "lx-s") {
    //                for (var i = 0; i < list.length; i++) {
    //                    {
    //                        var trim = list[i].title.trim.toLowerCase();

    //                        if (trim == "lx-s") {
    //                            exactMatchList.push(list[i]);
    //                        } else {
    //                            similartMatchList.push(list[i]);
    //                        }


    //                    }
    //                }
    //            }
    //            if (exactTrim == "lx-p") {
    //                for (var i = 0; i < list.length; i++) {
    //                    {
    //                        var trim = list[i].title.trim.toLowerCase();

    //                        if (trim == "lx-p") {
    //                            exactMatchList.push(list[i]);
    //                        } else {
    //                            similartMatchList.push(list[i]);
    //                        }


    //                    }
    //                }
    //            }
    //            if (exactTrim == "ex") {
    //                for (var i = 0; i < list.length; i++) {
    //                    {
    //                        var trim = list[i].title.trim.toLowerCase();

    //                        if (trim == "ex") {
    //                            exactMatchList.push(list[i]);
    //                        } else {
    //                            similartMatchList.push(list[i]);
    //                        }


    //                    }
    //                }
    //            }
              
    //        }

    //    }
   
        return {
            exact: exactMatchList,
            similar: similartMatchList
        };
    }