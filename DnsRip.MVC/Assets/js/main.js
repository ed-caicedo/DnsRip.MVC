// ReSharper disable once InconsistentNaming

(function () {
    var DnsRip = function () {
        this.$query = $("#query");
        this.$panelOptions = $("#panel-options");

        this.duration = 333;
    }

    DnsRip.prototype = {
        init: function () {
            var t = this;

            t.$query.typeWatch({
                callback: function (value) {
                    t.parse(value);
                },
                wait: t.duration,
                highlight: false,
                allowSubmit: false,
                captureLength: 3
            });
        },

        parse: function (text) {
            var t = this;

            if (text)
                $.post("/parse/", {
                    text: text
                }, function (data) {
                    console.log(data);

                    t.$panelOptions.fadeIn(this.duration);
                });
        }
    }

    $(function () {
        new DnsRip().init();
    });
})();