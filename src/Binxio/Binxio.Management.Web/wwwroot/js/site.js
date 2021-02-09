// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const ui = {
    controls: {
        'int64': (v) => new numericControl(v),
        'string': (v) => new stringControl(v),
        'client': (v) => new clientEditor(v),
    }
};


const numericControl = function (v) {
    return {
        template: function (p) {
            var o = `
                <label>${v.displayName}</label>
                <input class="control-input" type="number" />
            `;
            return o;
        }
    }
};

const clientEditor = function (v) {
    return {
        init: function (dataStore) {
            $.ajax({
                url: '/manage/ui/entities/client',
                success: function (h) {
                    dataStore.data.clients = h;
                }
            });
        },
        val: function (parent) {
            return parent.find('.control-input').val();
        },
        template: function (p) {
            if (p.clients) {
                var o = '<label>' + v.displayName + '</label>';
                o += '<select class="control-input">';
                if (p.clients) {
                    p.clients.forEach((client, ix) => {
                        o += '<option value="' + client.id + '">' + client.title + '</option>';
                    });
                }
                o += '</select>';
                return o;
            } else {
                return '';
            }
        }
    }
}

const stringControl = function (v) {
    return {
        val: function (parent) {
            return parent.find('.control-input').val();
        },
        template: function (p) {
            var o = `
                <label>${v.displayName}</label>
                <input class="control-input" type="text" />
            `;
            return o;
        }
    }
};
