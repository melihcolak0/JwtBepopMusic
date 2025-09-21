(function (global, $) {
  "use strict";

  var init = function(){
    var plyrist = false;

    $(document).on('click', '.btn-play' ,function(e){

      if(!plyrist){
        plyrist = new Plyrist(
          {
            playlist: '#plyrist', 
            player: 'audio'
          },
          [],
          {
            theme: 2
          }
        );
        plyrist.player.on('play', event => {
          updateDisplay();
        });

        plyrist.player.on('pause', event => {
          updateDisplay();
        });
      }

      var self = $(this).closest('[data-id]'),
          id = self.attr('data-id');
      if(plyrist.getIndex(id) > 0){
        plyrist.play({id: id});
      }else{
        plyrist.play({
          id: id,
          title: self.find('.title').text(),
          uri: self.find('.title').attr('href'),
          author: self.find('.subtitle').text(),
          poster: self.find('.media-content').css('background-image').replace(/^url|[\(\)]/g, ''),
          type: 'audio',
          source: self.attr('data-source')
        });
      }
    });

    $(document).on('pjaxEnd', function(){
      updateDisplay();
    });

    function updateDisplay(){
      $('[data-id]').find('.list-item').removeClass('active')
      $('[data-id]').find('.btn-play').removeClass('active');
      if(!plyrist) return;
      var item = plyrist.getCurrent();
      var el = $('[data-id="'+item.id+'"]');
      if( plyrist.player.paused ){
        el.find('.list-item').removeClass('active');
        el.find('.btn-play').removeClass('active');
      }else{
        el.find('.list-item').addClass('active');
        el.find('.btn-play').addClass('active');
      }
    }

    $(document).on('show.bs.dropdown', '.list-action > div, .media-action', function () {
      $(this).closest('.list-item').addClass('pos-rlt z-index-1');
    });

    $(document).on('hide.bs.dropdown', '.list-action > div, .media-action', function () {
      $(this).closest('.list-item').removeClass('pos-rlt z-index-1');
    });

    $(document).on('click', '.btn-more', function (e) {
      e.preventDefault();
      var $dp = $(this).next('.dropdown-menu');
      $dp.append('<a class="dropdown-item" href="#">Play</a>'+
        '<a class="dropdown-item" href="#">Next to play</a>'+
        '<a class="dropdown-item" href="#">Add to queue</a>'+
        '<a class="dropdown-item" href="#">Add to playlist</a>'+
        '<div class="dropdown-divider"></div>'+
        '<a class="dropdown-item" href="#">Share</a>'
      );
    });

  }

  // for ajax to init again
  global.plyr = {init: init};

})(this, jQuery);
