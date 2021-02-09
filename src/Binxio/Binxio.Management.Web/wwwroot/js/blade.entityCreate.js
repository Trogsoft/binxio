var bladeEntityCreate = function (config) {

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
            o += '<div class="blade-title"><div class="title">Create ' + data.config.bladeType + '</div><div class="blade-controls"><a href="#" class="blade-close"><i class="fas fa-times"></i></a></div></div>';
            o += '<div class="blade-content">';
            if (data.props.length == 0) {
                o += '<i class="fas fa-cog fa-spin"></i>';
            } else {
                for (var i in data.props) {
                    var prop = data.props[i];
                    o += '<div class="control" data-control="' + prop.editor + 'Control" data-prop="' + prop.propertyName + '"></div>';
                }
            }
            o += '</div>';
            o += '<div class="blade-buttons">';
            o += ' <button class="btn btn-primary create-entity">Create</button>';
            o += '</div>';
            return o;
        }
    });

    document.addEventListener('render', function (e) {
        if (e.target.matches(config.el)) {
            $(config.el).find('.create-entity').off('click').on('click', createEntity);
            $(config.el).find('.blade-close').off('click').on('click', closeBlade);
        }
    });

    function closeBlade(e) {
        e.preventDefault();
        bladeManager.closeBlade(config.id);
    }

    function validate() {

        var data = {};
        blade.store.data.props.forEach((o, i) => {
            var control = blade.controls[o.propertyName];
            var el = $(config.el).find('.control[data-prop=' + o.propertyName + ']');
            data[o.propertyName] = control.val(el);
        });

        return {
            package: data,
            success: true
        };

    }

    function createEntity() {
        var validation = validate();
        if (validation.success) {
            console.log(validation.package);
            $.ajax({
                url: '/manage/ui/create/' + config.bladeType,
                data: { objectValues: JSON.stringify(validation.package) },
                type: 'post',
                cache: false,
                success: function (h) {
                    if (h.status >= 0)
                        bladeManager.closeBlade(config.id);
                    else
                        alert(h.message);
                }
            });
        } else {
            alert(validation.message);
        }
    }

    $.ajax({
        url: '/manage/ui/props/' + config.bladeType + '/create',
        type: 'get',
        success: function (h) {
            blade.store.data.controls = {};
            blade.store.data.props = h;
            blade.store.data.props.forEach((v, i) => {
                if (ui.controls[v.editor.toLowerCase()]) {

                    let control = ui.controls[v.editor.toLowerCase()](v);

                    if (control.init)
                        control.init(blade.store);

                    let cc = {
                        attachTo: blade.ui,
                        store: blade.store,
                        template: control.template
                    }

                    let c = new Reef('[data-prop=' + v.propertyName + ']', cc);
                    blade.controls[v.propertyName] = {
                        val: control.val
                    };

                } else {

                    throw new Error('Unable to find editor for ' + v.editor.toLowerCase());

                }
            });
        }
    });

    return blade;

}