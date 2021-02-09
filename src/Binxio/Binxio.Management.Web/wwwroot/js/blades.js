const bladeManager = new function () {

    var bm = this;
    bm.bladeClasses = {
        'createEntity': function (params) { return new bladeEntityCreate(params) },
        'listEntities': function (params) { return new bladeListEntities(params) },
        'manageEntity': function (params) { return new entityBlade(params) }
    }

    bm.blades = [];

    let bladeIndex = 0;

    $(function () {

        bm.refresh();

    });

    bm.openBladeFromLink = function (e) {

        if (e) e.preventDefault();
        var bladeClass = $(this).attr('data-blade-class');
        var objectId = $(this).attr('data-id');

        var bladeId = (bladeIndex++);
        var el = $('<div class="blade" id="blade-' +  bladeId + '"></div>');
        $('.content').append(el);
        var params = { el: '#blade-' + bladeId, id: bladeId };

        if (objectId)
            params.objectId = objectId;

        $.each($(this).data(), function (i, v) {
            params[i] = v;
        });

        var newBlade = bm.bladeClasses[bladeClass](params);
        if (newBlade.fullScreen)
            el.addClass('full-width');

        el.show();
        newBlade.ui.render();

        bm.blades.push({ id: bladeId, blade: newBlade });

        $('.blade-default').hide();

        if (bm.blades.length > 1) {
            bm.blades.filter(x => x.id != bladeId).forEach(x => {
                if (x.blade.shrink) x.blade.shrink();
            });
        }

        $('.content').animate({ scrollLeft: $('#blade-'+bladeId).position().left }, 500);

    }

    function getBlade(id) {
        var o = bm.blades.filter(x => x.id == id);
        if (o.length == 1)
            return o[0];

        return undefined;
    }

    bm.closeBlade = function (id) {

        var blade = getBlade(id);
        if (blade) {
            $('#blade-' + id).remove();
            var index = bm.blades.indexOf(blade);
            if (index > -1)
                bm.blades.splice(index, 1);
        }

        if (bm.blades.length == 0) {
            $('.blade-default').show();
        } else {
            var blade = bm.blades[bm.blades.length - 1];
            if (blade.blade.grow) blade.blade.grow();
        }

    }

    bm.refresh = function () {

        $('[data-target=blade]').off('click').on('click', bm.openBladeFromLink);

    }

    return bm;

}

