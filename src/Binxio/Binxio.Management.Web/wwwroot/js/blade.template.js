var bladeTemplate = function (config) {

    var blade = this;
    blade.config = config;
    blade.controls = {};

    blade.store = new Reef.Store({
        data: {
            config: config,
            props: []
        }
    })

    blade.ui = new Reef(config.el, {
        store: blade.store,
        template: function (data) {
            var o = '';
            o += '<div class="blade-title"><div class="title">List</div><div class="blade-controls"><a href="#" class="blade-close"><i class="fas fa-times"></i></a></div></div>';
            o += '<div class="blade-content">';
            o += '</div>';
            o += '<div class="blade-buttons">';
            o += '</div>';
            return o;
        }
    });

    document.addEventListener('render', function (e) {
        if (e.target.matches(config.el)) {
        }
    });

    function closeBlade(e) {
        e.preventDefault();
        bladeManager.closeBlade(config.id);
    }

    $.ajax({
        url: '',
        type: 'get',
        success: function (h) {
        }
    });

    return blade;

}