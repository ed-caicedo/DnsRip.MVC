/// <reference path="../lib/jquery-vsdoc.js" />

(function ($) {
    $.extend({
        getAFToken: function () {
            return $("input[name='__RequestVerificationToken']").val();
        },

        appendAFToken: function () {
            return "&__RequestVerificationToken=" + $("input[name='__RequestVerificationToken']").val();
        },

        removeWhitespace: function(string) {
            if (!string)
                return "";

            string = string.replace(/[\r\n\t]/g, "");

            while (/  /i.test(string)) {
                string = string.replace(/  /g, " ");
            }

            return string;
        },

        scrollToTop: function(speed) {
            $("html, body").animate({ scrollTop: 0 }, speed);
        },

        getTimestamp: function() {
            var time = Date.now();
            while (time === Date.now());
            return Date.now();
        }
    });
})(jQuery);