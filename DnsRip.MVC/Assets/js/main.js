// ReSharper disable once InconsistentNaming

(function () {
    var DnsRip = function () {
        this.$query = $("#query");
    }

    DnsRip.prototype = {
        init: function () {
            var t = this;

            t.$query.typeWatch({
                callback: function (value) {
                    t.parse(value);
                },
                wait: 500,
                highlight: false,
                allowSubmit: false,
                captureLength: 3
            });
        },

        parse: function (text) {
            if (text)
                $.post("/parse/", {
                    text: text
                }, function (data) {
                    console.log(data);
                });
        }
    }

    $(function () {
        new DnsRip().init();
    });
})();