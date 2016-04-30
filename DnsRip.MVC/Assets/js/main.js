/// <reference path="lib/jquery-vsdoc.js" />
/// <reference path="lib/knockout.debug.js" />
/// <reference path="lib/typewatch.js" />

// ReSharper disable once InconsistentNaming

(function () {
    var DnsRip = function () {
        this.$query = $("#query");

        this.duration = 333;

        this.viewModel = {
            duration: this.duration,
            optionsVisible: ko.observable(false),
            hosts: ko.observableArray(),
            queued: ko.observableArray()
        }

        ko.bindingHandlers.fade = {
            update: function (element, valueAccessor, allBindings, viewModel) {
                var elem = $(element);
                var val = ko.unwrap(valueAccessor());

                if (val && elem.not(":visible"))
                    $(element).fadeIn(viewModel.duration);
                else if (!val && elem.is(":visible"))
                    $(element).fadeOut(viewModel.duration);
            }
        };
    }

    DnsRip.prototype = {
        init: function () {
            this.initQuery();
            ko.applyBindings(this.viewModel);

            console.log(ko.toJS(this.viewModel));
        },

        initQuery: function () {
            var t = this;

            t.$query.typeWatch({
                callback: function (value) {
                    var parsed;

                    if (value)
                        parsed = t.parse(value);

                    if (parsed)
                        parsed.done(function (data) {
                            t.reset();
                            t.addOptions(data);
                        });
                },
                wait: t.duration,
                highlight: false,
                allowSubmit: false,
                captureLength: 3
            });
        },

        parse: function (text) {
            return $.post("/parse/", {
                text: text
            });
        },

        reset: function() {
            var vm = this.viewModel;
            vm.hosts.removeAll();
            vm.queued.removeAll();
        },

        addOptions: function (parseResult) {
            var vm = this.viewModel;

            if (parseResult.Type !== "Invalid") {
                vm.hosts.push({
                    name: parseResult.Parsed,
                    selected: true
                });

                if (parseResult.Additional)
                    for (var a = 0; a < parseResult.Additional.length; a++)
                        vm.hosts.push({
                            name: parseResult.Additional[a],
                            selected: false
                        });
            }

            var hosts = vm.hosts();

            if (hosts.length) {
                vm.optionsVisible(true);

                for (var i = 0; i < hosts.length; i++) {
                    if (hosts[i].selected === true)
                        vm.queued.push({
                            name: hosts[i].name,
                            type: "A"
                        });
                }
            } else {
                vm.optionsVisible(false);
            }
        }
    }

    $(function () {
        new DnsRip().init();
    });
})();