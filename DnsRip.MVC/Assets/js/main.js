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
            hosts: ko.observableArray()
        }

        ko.bindingHandlers.fade = {
            update: function (element, valueAccessor, allBindings, viewModel) {
                if (ko.unwrap(valueAccessor()))
                    $(element).fadeIn(viewModel.duration);
                else
                    $(element).fadeOut(viewModel.duration);
            }
        };
    }

    DnsRip.prototype = {
        init: function () {
            this.initQuery();
            ko.applyBindings(this.viewModel);
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
                            //console.log(data);
                            var vm = t.viewModel;
                            vm.hosts.removeAll();

                            if (data.Type !== "Invalid") {
                                vm.optionsVisible(true);
                                vm.hosts.push({
                                    name: data.Parsed,
                                    selected: true
                                });

                                if (data.Additional)
                                    for (var a = 0; a < data.Additional.length; a++)
                                        vm.hosts.push({
                                            name: data.Additional[a],
                                            selected: false
                                        });
                            }

                            //console.log(ko.toJS(vm.hosts));

                            if (!vm.hosts().length)
                                vm.optionsVisible(false);
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
        }
    }

    $(function () {
        new DnsRip().init();
    });
})();