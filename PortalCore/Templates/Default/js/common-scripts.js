var Script = function () {

//    sidebar toggle


    $(function() {
        function responsiveView() {
            var wSize = $(window).width();
            if (wSize <= 768) {
                $('#container').addClass('sidebar-close');
                $('#sidebar > ul').hide();
            }

            if (wSize > 768) {
                $('#container').removeClass('sidebar-close');
                $('#sidebar > ul').show();
            }
        }
        $(window).on('load', responsiveView);
        $(window).on('resize', responsiveView);
    });

    $('.icon-reorder').click(function () {
        if ($('#sidebar > ul').is(":visible") === true) {
            $('#main-content').css({
                'margin-right': '0px'
            });
            $('#sidebar').css({
                'margin-right': '-180px'
            });
            $('#sidebar > ul').hide();
            $("#container").addClass("sidebar-closed");
        } else {
            $('#main-content').css({
                'margin-right': '180px'
            });
            $('#sidebar > ul').show();
            $('#sidebar').css({
                'margin-right': '0'
            });
            $("#container").removeClass("sidebar-closed");
        }
    });


//    tool tips

    $('.tooltips').tooltip();

//    popovers

    $('.popovers').popover();


}();