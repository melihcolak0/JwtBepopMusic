(function ($) {
    'use strict';
    $(function () {
        var body = $('body');
        var sidebar = $('.sidebar');

        // Geçerli URL
        var current = window.location.pathname.toLowerCase();

        // Sidebar nav-link aktif et
        $('.nav li a', sidebar).each(function () {
            var element = $(this);
            var href = element.attr('href')?.toLowerCase();

            if (href && current === href) {
                element.parents('.nav-item').last().addClass('active');
                element.addClass('active');

                // Eðer submenu içindeyse aç
                if (element.parents('.sub-menu').length) {
                    element.closest('.collapse').addClass('show');
                }
            }
        });

        // Horizontal menu nav-link aktif et
        $('.horizontal-menu .nav li a').each(function () {
            var element = $(this);
            var href = element.attr('href')?.toLowerCase();

            if (href && current === href) {
                element.parents('.nav-item').last().addClass('active');
                element.addClass('active');

                if (element.parents('.sub-menu').length) {
                    element.closest('.collapse').addClass('show');
                }
            }
        });

        // Sidebar collapse yönetimi: bir submenu açýlýrsa diðerini kapat
        sidebar.on('show.bs.collapse', '.collapse', function () {
            sidebar.find('.collapse.show').collapse('hide');
        });

        // Sidebar toggle
        $('[data-bs-toggle="minimize"]').on("click", function () {
            if ((body.hasClass('sidebar-toggle-display')) || (body.hasClass('sidebar-absolute'))) {
                body.toggleClass('sidebar-hidden');
            } else {
                body.toggleClass('sidebar-icon-only');
            }
        });

        // Perfect scrollbar (opsiyonel, varsa kullan)
        if (body.hasClass("sidebar-fixed")) {
            if ($('#sidebar').length) {
                new PerfectScrollbar('#sidebar .nav');
            }
        }

    });
})(jQuery);
