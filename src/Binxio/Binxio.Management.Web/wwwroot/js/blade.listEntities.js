var bladeListEntities = function (config) {

    var blade = this;
    blade.config = config;
    blade.controls = {};
    blade.fullScreen = true;
    blade.sizes = ['l', 'm'];

    blade.store = new Reef.Store({
        data: {
            config: config,
            model: null,
            entities: [],
            loading: true
        }
    })

    blade.shrink = function () {

        var isLarge = $(config.el).hasClass('full-width');
        if (isLarge)
            $(config.el).removeClass('full-width');

    }

    blade.grow = function () {

        var isLarge = $(config.el).hasClass('full-width');
        if (!isLarge)
            $(config.el).addClass('full-width');

    }

    blade.ui = new Reef(config.el, {
        store: blade.store,
        template: function (data) {
            var o = '';
            o += '<div class="blade-title"><div class="title">List</div><div class="blade-controls"><a href="#" class="blade-close"><i class="fas fa-times"></i></a></div></div>';
            o += '<div class="blade-actions">';
            o += '</div>';
            o += '<div class="blade-content">';

            if (data.model && data.entities.length) {

                var model = data.model[data.entities[0].model];

                o += '<table class="entity-list"><thead>';
                o += ' <tr>';
                model.properties.filter(x => x.showInList && x.urlPart != "model").forEach(x => {
                    o += '<th>' + x.title + '</th>';
                });
                o += ' </tr></thead><tbody>';

                data.entities.forEach(x => {
                    o += '<tr>';
                    model.properties.filter(x => x.showInList && x.urlPart != "model").forEach(y => {
                        o += '<td>';
                        if (y.isEditorLink)
                            o += '<a href="#" data-target="blade" data-blade-class="manageEntity" data-blade-type="' + config.bladeType + '" data-object-id="' + x.urlPart + '">';

                        if (y.referenceModel && data.model[y.referenceModel]) {
                            var el = data.model[y.referenceModel].properties.filter(x => x.isEditorLink);
                            if (el.length) {
                                o += '<a href="#" data-target="blade" data-blade-class="manageEntity" data-blade-type="' + y.referenceModel + '" data-object-id="' + x[y.urlPart].urlPart + '">'+x[y.urlPart][el[0].urlPart]+'</a>';
                            } else {
                                o += '(error)';
                            }
                        } else {
                            o += x[y.urlPart];
                        }

                        if (y.isEditorLink)
                            o += '</a>';

                        o += '</td>';
                    });
                    o += '</tr>';
                });

                o += '</tbody></table>';
            }

            o += '</div>';
            return o;
        }
    });

    document.addEventListener('render', function (e) {
        if (e.target.matches(config.el)) {
            bladeManager.refresh();
            $(config.el).find('.blade-close').off('click').on('click', closeBlade);
        }
    });

    function closeBlade(e) {
        e.preventDefault();
        bladeManager.closeBlade(config.id);
    }

    $.ajax({
        url: '/manage/ui/list/' + config.bladeType,
        type: 'get',
        success: function (h) {
            blade.store.data.model = h.model;
            blade.store.data.entities = h.entities.result;
            blade.store.data.loading = false;
        }
    });

    return blade;

}