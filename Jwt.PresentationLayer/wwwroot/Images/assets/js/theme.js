(function ($) {
	'use strict';

    // ie
    if ( !!navigator.userAgent.match(/MSIE/i) || !!navigator.userAgent.match(/Trident.*rv:11\./) ){
      $('body').addClass('ie');
    }

    // iOs, Android, Blackberry, Opera Mini, and Windows mobile devices
    var ua = window['navigator']['userAgent'] || window['navigator']['vendor'] || window['opera'];
    if( (/iPhone|iPod|iPad|Silk|Android|BlackBerry|Opera Mini|IEMobile/).test(ua) ){
        $('body').addClass('touch');
    }

    // fix z-index on ios safari
    if( (/iPhone|iPod|iPad/).test(ua) ){
      $(document, '.modal, .aside').on('shown.bs.modal', function(e) {
        var backDrop = $('.modal-backdrop');
        $(e.target).after($(backDrop));
      });
    }

    //resize
    $(window).on('resize', function () {
      var $w = $(window).width()
          ,$lg = 1200
          ,$md = 991
          ,$sm = 768
          ;
      if($w > $lg){
        $('.aside-lg').modal('hide');
      }
      if($w > $md){
        $('#aside').modal('hide');
        $('.aside-md, .aside-sm').modal('hide');
      }
      if($w > $sm){
        $('.aside-sm').modal('hide');
      }
    });

    // mousewheel
    $(document).on( 'DOMMouseScroll mousewheel', function ( event ) {
      var $header = $('.scroll-header');
      if( event.originalEvent.detail > 0 || event.originalEvent.wheelDelta < 0 ) {
        $header.removeClass('scroll-up').addClass('scroll-down');
      } else {
        $header.removeClass('scroll-down').addClass('scroll-up');
      }
      $header.addClass('scrolled');
      if($(window).scrollTop() == 0){
        $header.removeClass('scroll-up scroll-down');
        $header.removeClass('scrolled');
      }
    });

    // nav
    $(document).on('click', '[data-nav] a', function (e) {
      var $this = $(this), $active, $li, $li_li;

      $li = $this.parent();
      $li_li = $li.parents('li');

      $active = $li.closest( "[data-nav]" ).find('.active');

      $li_li.addClass('active');
      ( $this.next().is('ul') && $li.toggleClass('active') && e.preventDefault() ) || $li.addClass('active');
      
      $active.not($li_li).not($li).removeClass('active');

      if($this.attr('href') && $this.attr('href') !='#'){
        $(document).trigger('Nav:changed');
      }
    });

    // collapse
    $('#navbarCollapse').on('show.bs.collapse', function () {
      $('.scroll-header').addClass('bg-dark');
    });

    $('#navbarCollapse').on('hide.bs.collapse', function () {
      $('.scroll-header').removeClass('bg-dark');
    });

    // toggleClass
    $(document).on('click', '[data-toggle-class]', function (e) {
      var $self = $(this);
      var attr = $self.attr('data-toggle-class');
      var target = $self.attr('data-toggle-class-target') || $self.attr('data-target');
      var closest = $self.attr('data-target-closest');
      var classes = ( attr && attr.split(',')) || '',
        targets = (target && target.split(',')) || Array($self),
        key = 0;
      $.each(classes, function( index, value ) {
        var target = closest ? $self.closest(targets[(targets.length == 1 ? 0 : key)]) : $( targets[(targets.length == 1 ? 0 : key)] ),
                  current = target.attr('data-class'),
                  _class = classes[index];
              (current != _class) && target.removeClass( target.attr('data-class') );
        target.toggleClass(classes[index]);
        target.attr('data-class', _class);
        key++;
      });
      $self.toggleClass('active');
      $self.attr('href') == "#" ? e.preventDefault() : '';
    });

    var init = function () {

      $('[data-toggle="popover"]').popover();
      $('[data-toggle="tooltip"]').tooltip();

      // fix scrollreveal in slick
      $('.slick').on('beforeChange', function(event, slick, currentSlide, nextSlide) {
        var el = $(slick.$slides.get(nextSlide));
        $(this).prev().find('.media-content').css('background-image', 'url('+el.attr('data-bg')+')');
      });

      // init the plugin
      $(document).find('[data-plugin]').plugin();

      sr.reveal('.sr .card, .sr .list-item:not(.slick-item), .sr .item, .sr .sr-item',{ 
        container: $('.is-fullscreen').length ? ($('.is-fullscreen.layout-column').length ? 'body' : '#content') : document.documentElement,
        duration: 1000,
        beforeReveal:function (el) { $(el).addClass('is-show') },
        afterReveal:function (el) { $(el).addClass('is-shown') }
      });

      (typeof feather !== 'undefined') && feather.replace({ 'width': 16, 'height': 16 });

      $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        sr.sync();
      })

      // active nav item
      var url = window.location.pathname.split( '/' );
      if(url.length > 0) url = url[url.length - 1];
      $('[data-nav]:not(.auto-nav) li.active').removeClass('active');
      $('[data-nav]:not(.auto-nav) a').filter( function() {
        return url == $(this).attr('href') && $(this).attr('href') !=='#';
      }).parents('li').addClass( 'active' );

      $('[data-toggle="tab"]').filter( function() {
         return location.href.indexOf( $(this).attr('data-target') || $(this).attr('href') ) != -1;
      }).trigger('click');

    }
    
    window.sr = ScrollReveal();

    init();

    $(document).on('pjaxEnd fullscreen', function(){
      $('#navbarCollapse').collapse('hide');
      init();
    });

    $(document).on('pjaxAddEl', function(){
      $('.sr .card, .sr .list-item:not(.slick-item), .sr .item, .sr .sr-item').css('visibility', 'hidden');
      (typeof feather !== 'undefined') && feather.replace({ 'width': 16, 'height': 16 });
    });

})(jQuery);
