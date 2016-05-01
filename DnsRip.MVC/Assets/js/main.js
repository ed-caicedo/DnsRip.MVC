/// <reference path="lib/jquery-vsdoc.js" />
/// <reference path="lib/knockout.debug.js" />
/// <reference path="lib/typewatch.js" />
/// <reference path="utl/utilities.js" />

(function ($, ko) {
    var DnsRip = function (opts) {
        this.viewModel = {
            duration: opts.duration,
            optionsVisible: ko.observable(false),
            optionsTimestamp: null,
            definedServer: ko.observable("8.8.8.8"),
            customServer: ko.observable(),
            hosts: ko.observableArray(),
            queued: ko.observableArray()
        }

        this.viewModel.server = ko.pureComputed(function () {
            var defined = this.definedServer();

            if (!defined)
                return this.customServer();

            return defined;
        }, this.viewModel);

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

        this.initQuery = function () {
            var t = this;

            opts.$queryFld.typeWatch({
                callback: function (value) {
                    if (value)
                        t.parse(value);
                },
                wait: t.duration,
                highlight: false,
                allowSubmit: false,
                captureLength: 3
            });

            var loaded = opts.$queryFld.val();

            if (loaded)
                t.parse(loaded);
        }

        this.initServers = function () {
            var $all = opts.$dnsBtns;
            var t = this;
            var vm = t.viewModel;

            $all.on("click", function () {
                var $t = $(this);

                $all.removeClass("active");
                $t.addClass("active");

                var server = $t.data("value");

                if (!server)
                    opts.$serverCnt.slideDown(t.duration);
                else
                    opts.$serverCnt.slideUp(t.duration);

                vm.definedServer(server);
            });
        }

        this.initOptionTabs = function () {
            var $allTabs = opts.$optionTabs;
            var $allPanes = opts.$optionPanes;

            $allTabs.find("a").on("click", function (e) {
                e.preventDefault();
                var $t = $(this);

                $allPanes.hide();
                $allPanes.filter($t.attr("href")).css("display", "inline-block");
            });

            $allTabs.on("click", function () {
                var $t = $(this);

                $allTabs.removeClass("active");
                $t.addClass("active");
            });
        }

        this.parse = function (value) {
            var t = this;
            var vm = t.viewModel;

            var parsed = $.post("/parse/", {
                value: value
            });

            if (parsed)
                parsed.done(function (data) {
                    t.reset();
                    t.addOptions(data);

                    var hosts = vm.hosts();

                    if (hosts.length)
                        t.addToQueue(hosts[0].name, "A", hosts[0].timestamp);
                });
        }

        this.reset = function () {
            var vm = this.viewModel;
            vm.hosts.removeAll();
            vm.queued.removeAll();
        }

        this.addOptions = function (parseResult) {
            var vm = this.viewModel;
            var ts = $.getTimestamp();

            if (parseResult.Type !== "Invalid") {
                vm.optionsTimestamp = ts;

                vm.hosts.push({
                    timestamp: ts,
                    name: parseResult.Parsed,
                    active: true
                });

                if (parseResult.Additional)
                    for (var a = 0; a < parseResult.Additional.length; a++)
                        vm.hosts.push({
                            timestamp: ts,
                            name: parseResult.Additional[a],
                            active: false
                        });
            }

            if (vm.hosts().length)
                vm.optionsVisible(true);
            else
                vm.optionsVisible(false);
        }

        this.addToQueue = function (name, type, timestamp) {
            this.viewModel.queued.push({
                timestamp: timestamp,
                name: name,
                type: type
            });
        }
    }

    DnsRip.prototype = {
        init: function () {
            this.initQuery();
            this.initServers();
            this.initOptionTabs();

            ko.applyBindings(this.viewModel);

            console.log(ko.toJS(this.viewModel));
        }
    }

    $(function () {
        var dnsRip = new DnsRip({
            $queryFld: $("#query"),
            $dnsBtns: $(".dns"),
            $serverCnt: $("#server-container"),
            $serverFld: $("#server"),
            $optionTabs: $(".option-tab"),
            $optionPanes: $(".option-pane"),
            duration: 200
        });

        dnsRip.init();
    });
})(jQuery, ko);