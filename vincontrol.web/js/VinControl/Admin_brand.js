var dataUrl = '/StockingGuide/StockingGuideAdminBrandJson';
function LoadBrandData() {
    //$.blockUI({ message: '<div><img src=\"' + loadingImg + '\" /></div>', css: { width: '600px', backgroundColor: 'none', border: 'none' } });
    $.ajax({
        type: "POST",
        //                contentType: "text/JSON; charset=utf-8",
        dataType: "JSON",
        url: dataUrl,
        data: {},
        cache: false,
        traditional: true,
        success: function (result) {
            $('#DDLFilterModel').html($("#AdminBrandsTemplate").render(result.listBrand));
            var myArray = result.selectedBrands;
            $("#DDLFilterModel").multipleSelect({
                multiple: true,
                multipleWidth: 200,
                placeholder: "Click here to filter your brand"
            });
            $("#DDLFilterModel").multipleSelect("setSelects", myArray);
            $('.ms-parent li.multiple').addClass("subOpt");
            $.unblockUI();
        },
        error: function (err) {
            console.log(err.status + " - " + err.statusText);
        }
    });
}