(function ($) {
	"use strict";

  var init = function(){
    var engine = new Bloodhound({
      datumTokenizer: Bloodhound.tokenizers.obj.whitespace('title'),
      queryTokenizer: Bloodhound.tokenizers.whitespace,
      prefetch: '../assets/api/music.json'
    });
    $('.typeahead').removeAttr('data-plugin').typeahead('destroy').typeahead({
        classNames: {
          menu: 'dropdown-menu mt-3'
        }
      }, {
        display: 'title',
        source: engine,
        templates: {
          suggestion: function(data) {
              return '<a class="dropdown-item" href="../html/item.detail.html"><span class="d-block font-weight-500">' + data.title + '</span><small class="text-muted">'+data.subtitle+'</small></a>';
          }
        }
      }
    ).on('typeahead:rendered', function(obj, datum) {
        // for ajax
        $(document).trigger('refresh');
    });
  }
  // for ajax to init again
  $.fn.typeahead.init = init;

})(jQuery);
