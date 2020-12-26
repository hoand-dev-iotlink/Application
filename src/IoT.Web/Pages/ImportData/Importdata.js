$(document).ready(function () {
    //$('#Input_CityCode').select2({
    //    placeholder: 'Chọn thành phố',
    //    width: '100%',
    //    allowClear: true,
    //    ajax:
    //    {
    //        //url: document.location.origin + "/api/BTS/Country/search-city",
    //        url: function (params) {
    //            if (params.term != undefined) {
    //                return document.location.origin + "/api/BTS/Country/search-city?q=" + params.term;
    //            }
    //            else {
    //                return document.location.origin + "/api/BTS/Country/search-city";
    //            }
    //        },
    //        type: 'GET',
    //        delay: 250,
    //        async: true,
    //        processResults: function (data, params) {
    //            console.log(data);
    //            let results = data.data.map((item, index) => {
    //                return {
    //                    id: item.id,
    //                    text: item.text
    //                }
    //            });
    //            return {
    //                results: results
    //            };
    //        },
    //        cache: true,
    //    }
    //})

    GetCountry("", 1);

    $('#Input_CityCode').select2({
        language: Host + "/libs/select2/js/i18n/vi.js",
        placeholder: `Chọn tỉnh/thành phố`,
        allowClear: true,
        width: '100%'
    })

});

function GetCountry(code, level) {
    $.ajax({
        url: document.location.origin + "/api/BTS/Country/getlist-country",
        method: "GET",
        async: false,
        data:
        {
            code: code,
            level: level
        },
        success: function (result) {
            var $option = "<option value = ''>-- Chọn tỉnh/thành phố --</option>";
            $.each(result, function (index, item) {
                $option += '<option value="' + item.code + '">' + item.name + '</option>';
            });

            $('#Input_CityCode').html($option)
        }
    });
}

//function Submit() {
//    $('form').submit(function (e) {
//        e.preventDefault();
//        var check = $(this).valid();
//        var fd = new FormData();
//        var files = $('#Input_Files')[0].files;
//        if (files.length > 0) {
//            //fd.append('Input.Files', files[0]);
//            //fd.append('Input.CityCode', $('#Input_CityCode').val());
//            //fd.append('Input.EndNumber', $('#Input_EndNumber').val());
//            //fd.append('Input.StartNumber', $('#Input_StartNumber').val());


//            fd.append('files', files[0]);
//            fd.append('cityCode', $('#Input_CityCode').val());
//            fd.append('endNumber', $('#Input_EndNumber').val());
//            fd.append('startNumber', $('#Input_StartNumber').val());

//            if (check) {
//                $.ajax({
//                    url: '/api/BTS/InforTram/importdata',
//                    type: 'post',
//                    data: fd,
//                    //contentType: 'application/json',
//                    contentType: false,
//                    processData: false,
//                    success: function (response) {
//                        if (!response.status) {
//                            swal({
//                                title: 'Thêm mới dữ liệu thất bại',
//                                text: "Cảnh báo: " + response.message,
//                                icon: "error",
//                                showCancelButton: false,
//                                confirmButtonColor: '#DD6B55',
//                                confirmButtonText: 'OK',
//                            }).then((willDelete) => {
//                                if (willDelete) {
//                                    document.location.reload();
//                                }
//                            });

//                        }
//                        else {
//                            if (response.message != "") {
//                                swal({
//                                    title: 'Thêm mới dữ liệu thành công',
//                                    text: "Cảnh báo: " + response.message,
//                                    icon: "warning",
//                                    showCancelButton: false,
//                                    confirmButtonColor: '#DD6B55',
//                                    confirmButtonText: 'OK',
//                                }).then((willDelete) => {
//                                    if (willDelete) {
//                                        document.location.reload();
//                                    }
//                                });
//                            }
//                            else {
//                                swal({
//                                    title: 'Thêm mới dữ liệu thành công',
//                                    icon: "success",
//                                    showCancelButton: false,
//                                    confirmButtonColor: '#DD6B55',
//                                    confirmButtonText: 'OK',
//                                }).then((willDelete) => {
//                                    if (willDelete) {
//                                        document.location.reload();
//                                    }
//                                });
//                            }
//                        }

//                        //$('form').find("input, textarea, select").val("");
//                    },
//                });
//            }
//        }
//    })
//}