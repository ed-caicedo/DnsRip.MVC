﻿/// <reference path="lib/jquery-vsdoc.js" />
/// <reference path="lib/knockout.debug.js" />
/// <reference path="lib/typewatch.js" />
/// <reference path="utl/utilities.js" />

(function ($, ko) {
    var DnsRip = function (opts) {
        this.viewModel = {
            duration: opts.duration,
            optionsVisible: ko.observable(false),
            optionsTimestamp: null,
            definedServer: ko.observable(opts.defaultServer),
            customServer: ko.observable(),
            hosts: ko.observableArray(),
            queued: ko.observableArray(),
            types: [
                ko.observableArray([
                {
                    type: "A",
                    state: null,
                    use: "Hostname"
                }, {
                    type: "AAAA",
                    state: null,
                    use: "Hostname"
                }, {
                    type: "CNAME",
                    state: null,
                    use: "Hostname"
                }]),
                ko.observableArray([
                {
                    type: "MX",
                    state: null,
                    use: "Hostname"
                }, {
                    type: "NS",
                    state: null,
                    use: "Hostname"
                }, {
                    type: "SOA",
                    state: null,
                    use: "Hostname"
                }]),
                ko.observableArray([
                {
                    type: "TXT",
                    state: null,
                    use: "Hostname"
                }, {
                    type: "PTR",
                    state: null,
                    use: "Ip"
                }, {
                    type: "ANY",
                    state: null,
                    use: null
                }])
            ]
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
            var fld = $(opts.queryFld);

            fld.typeWatch({
                callback: function (value) {
                    if (value)
                        t.parse(value);
                },
                wait: t.duration,
                highlight: false,
                allowSubmit: false,
                captureLength: 0
            });

            var loaded = fld.val();

            if (loaded)
                t.parse(loaded);
        }

        this.initServers = function () {
            var $all = $(opts.dnsBtns);
            var t = this;
            var vm = t.viewModel;

            $all.on("click", function () {
                var $t = $(this);

                $all.removeClass("active");
                $t.addClass("active");

                var server = $t.data("value");
                var cnt = $(opts.serverCnt);

                if (!server)
                    cnt.slideDown(t.duration);
                else
                    cnt.slideUp(t.duration);

                vm.definedServer(server);
            });
        }

        this.initOptionTabs = function () {
            var $allTabs = $(opts.optionTabs);
            var $allPanes = $(opts.optionPanes);

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

        this.initHostOptions = function () {
            var t = this;
            var vm = t.viewModel;

            $(opts.optionPanel).on("click", opts.hostOptions, null, function () {
                var $t = $(this);
                var value = $t.data("value");

                if ($t.hasClass("active")) {
                    t.changeHostSelection(value, false);
                    t.removeFromQueue(value, vm.optionsTimestamp);
                } else {
                    t.changeHostSelection(value, true);
                    t.addToQueue(value, vm.optionsTimestamp);
                }
            });
        }

        this.parse = function (value) {
            var t = this;
            var parsed = $.post("/parse/", {
                value: value
            });

            if (parsed)
                parsed.done(function (data) {
                    t.reset();
                    t.addOptions(data);
                    t.selectFirstHost();
                });
        }

        this.selectFirstHost = function () {
            var t = this;
            var hosts = t.viewModel.hosts();

            if (hosts.length)
                t.addToQueue(hosts[0].name, hosts[0].timestamp);
        }

        this.changeHostSelection = function (name, isActive) {
            var vm = this.viewModel;
            var hosts = vm.hosts.removeAll();

            for (var i = 0; i < hosts.length; i++) {
                var active = hosts[i].active;

                if (hosts[i].name === name)
                    active = isActive;

                vm.hosts.push({
                    timestamp: hosts[i].timestamp,
                    name: hosts[i].name,
                    active: active
                });
            }
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

                if (!this.validTypesSelected(parseResult.Type))
                    this.selectDefaultType(parseResult.Type);
            }

            if (vm.hosts().length)
                vm.optionsVisible(true);
            else
                vm.optionsVisible(false);
        }

        this.validTypesSelected = function (parseResultType) {
            var typeGroups = this.viewModel.types;
            var selectCount = 0;

            for (var i = 0; i < typeGroups.length; i++) {
                var types = typeGroups[i]();

                for (var j = 0; j < types.length; j++) {
                    if (types[j].state === "active") {
                        selectCount++;

                        if (types[j].use && parseResultType !== types[j].use)
                            return false;
                    }
                }
            }

            if (!selectCount)
                return false;

            return true;
        }

        this.selectDefaultType = function (parseResultType) {
            var typeGroups = this.viewModel.types;

            for (var i = 0; i < typeGroups.length; i++) {
                var types = typeGroups[i].removeAll();

                for (var j = 0; j < types.length; j++) {
                    var state = null;

                    switch (parseResultType) {
                        case "Hostname":
                            switch (types[j].type) {
                                case "A":
                                    state = "active";
                                    break;
                                case "PTR":
                                    state = "disabled";
                                    break;
                                default:
                                    state = null;
                                    break;
                            }
                            break;
                        case "Ip":
                            switch (types[j].type) {
                                case "PTR":
                                    state = "active";
                                    break;
                                case "ANY":
                                    state = null;
                                    break;
                                default:
                                    state = "disabled";
                                    break;
                            }
                            break;
                    }

                    typeGroups[i].push({
                        type: types[j].type,
                        state: state,
                        use: types[j].use
                    });
                }
            }
        }

        this.getSelectedTypes = function () {
            var typeGroups = this.viewModel.types;
            var selected = [];

            for (var i = 0; i < typeGroups.length; i++) {
                var types = typeGroups[i]();

                for (var j = 0; j < types.length; j++) {
                    if (types[j].state === "active")
                        selected.push(types[j].type);
                }
            }

            return selected;
        }

        this.addToQueue = function (name, timestamp) {
            var types = this.getSelectedTypes();

            for (var i = 0; i < types.length; i++)
                this.viewModel.queued.push({
                    timestamp: timestamp,
                    name: name,
                    type: types[i]
                });
        }

        this.removeFromQueue = function (name, timestamp) {
            this.viewModel.queued.remove(function (item) {
                return item.name === name && item.timestamp === timestamp;
            });
        }
    }

    DnsRip.prototype = {
        init: function () {
            this.initQuery();
            this.initServers();
            this.initOptionTabs();
            this.initHostOptions();

            ko.applyBindings(this.viewModel);
        }
    }

    $(function () {
        var dnsRip = new DnsRip({
            queryFld: "#query",
            dnsBtns: ".dns",
            serverCnt: "#server-container",
            optionTabs: ".option-tab",
            optionPanel: "#option-panel",
            optionPanes: ".option-pane",
            hostOptions: ".host-option",
            typeOptions: ".type-option",
            defaultServer: "8.8.8.8",
            duration: 200
        });

        dnsRip.init();
    });
})(jQuery, ko);