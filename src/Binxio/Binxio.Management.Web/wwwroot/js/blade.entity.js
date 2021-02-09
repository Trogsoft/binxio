var entityBlade = function (config) {

    var blade = this;
    blade.config = config;
    blade.controls = {};
    blade.fullScreen = true;

    blade.store = new Reef.Store({
        data: {
            config: config,
            entity: {
                title: 'Loading...'
            },
            actions: []
        }
    })

    blade.ui = new Reef(config.el, {
        store: blade.store,
        template: function (data) {
            var o = '';
            o += '<div class="blade-title"><div class="title">' + data.entity.title + '</div><div class="blade-controls"><a href="#" class="blade-close"><i class="fas fa-times"></i></a></div></div>';

            if (data.actions.length > 0) {
                o += '<div class="blade-menu">';
                data.actions.forEach(x => {
                    o += '<a class="blade-menu-item">';
                    o += ' <div class="icon"><i class="fas ' + x.icon + '"></i></div>';
                    o += ' <div class="title">' + x.title + '</div>';
                    o += '</a>';
                });
                o += '</div>';
                o += '<div class="blade-main">';
            }

            o += '<div class="blade-content">';
            o += '</div>';
            o += '<div class="blade-buttons">';
            o += '</div>';

            if (data.actions.lenth > 0) {
                o += '</div>';
            }
            return o;
        }
    });

    document.addEventListener('render', function (e) {
        if (e.target.matches(config.el)) {
            $(config.el).find('.blade-close').off('click').on('click', closeBlade);
        }
    });

    function closeBlade(e) {
        e.preventDefault();
        bladeManager.closeBlade(config.id);
    }

    $.ajax({
        url: '/manage/ui/entity/'+config.bladeType+'/'+config.objectId,
        type: 'get',
        success: function (h) {
            if (h.actions && h.actions.isSuccessful)  
                blade.store.data.actions = h.actions.result;

            blade.store.data.model = h.model;
            blade.store.data.entity = h.result.result;
        }
    });

    return blade;

}