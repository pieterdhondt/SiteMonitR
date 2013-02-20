$(function () {

    $('#noSitesToMonitorMessage').hide();

    function SiteStatusItem(u, s, t) {
        var self = this;
        self.url = u;
        self.cssClass = ko.observable(s);
        self.siteStatus = ko.observable(t);
    }

    function GridViewModel(sites) {
        var self = this;
        self.items = ko.observableArray(sites);
    }

    function controller() {
        var self = this;
        self.model = new GridViewModel([]);
        self.connection = $.hubConnection();
        self.connection.logging = true;
        self.siteMonitorHub = self.connection.createHubProxy("SiteMonitR");

        self.updateSite = function (url, cssClass, siteStatus) {
            console.info('updateSite,' + url + ':' + 'Status: ' + siteStatus + ', CSS Class:' + cssClass);
            self.model.items().forEach(function (n) {
                if (n.url == url) {
                    n.cssClass(cssClass);
                    n.siteStatus(siteStatus);
                }
            });
        };

        self.addSite = function (url) {
            if ($('.site[data-url="' + url + '"]').length == 0) {
                var site = new SiteStatusItem(url, 'btn-warning', 'Waiting');
                self.model.items.push(site);
            }
        };

        self.updateSiteStatus = function (monitorUpdate) {
            if (monitorUpdate.Status == 'Up') {
                self.updateSite(monitorUpdate.Url, 'btn-success', 'Up');
            }
            if (monitorUpdate.Status == 'Down') {
                self.updateSite(monitorUpdate.Url, 'btn-danger', 'Down');
            }
            if (monitorUpdate.Status == 'Checking') {
                self.updateSite(monitorUpdate.Url, 'btn-warning', 'Checking');
            }
        };

        self.toggleSpinner = function (isVisible) {
            if (isVisible == true)
                $('#spin').show();
            if (isVisible != true)
                $('#spin').hide();
        };

        self.toggleGrid = function () {
            if ($('.site').length == 0) {
                $('#noSitesToMonitorMessage').show();
                $('#sites').hide();
            }
            else {
                $('#noSitesToMonitorMessage').hide();
                $('#sites').show();
            }
        }
    }

    var c = new controller();

    c.siteMonitorHub
        .on('siteListObtained', function (sites) {
            $(sites).each(function (i, site) {
                c.addSite(site);
            });
            c.toggleSpinner(false);
            c.toggleGrid();

            $('.removeSite').on('click', function () {
                c.toggleSpinner(true);
                var url = $(this).data('url');

                $('.site[data-url="' + url + '"]').fadeOut('fast', function () {
                    $('.site[data-url="' + url + '"]').remove();
                });

                c.siteMonitorHub.invoke('removeSite', url);
            });
        })
        .on('siteStatusUpdated', function (monitorUpdate) {
            c.updateSiteStatus(monitorUpdate);
            c.toggleSpinner(false);
        })
        .on('siteAddedToGui', function (url) {
            $('#siteUrl').val('http://');
            $('#siteUrl').focus();
            c.toggleSpinner(false);
            c.toggleGrid();
        })
        .on('siteRemovedFromGui', function (url) {
            $('.site[data-url="' + url + '"]').remove();
            c.toggleGrid();
            c.toggleSpinner(false);
        })
        .on('checkingSite', function (url) {
            c.toggleSpinner(false);
            c.updateSite(url, 'btn-info', 'Checking');
        });

    $('#addSite').click(function () {
        var u = $('#siteUrl').val();
        c.addSite(u);
        c.toggleSpinner(true);
        c.siteMonitorHub.invoke('addSite', u);
    });

    c.connection.start().done(function () {
        c.toggleSpinner(true);
        c.siteMonitorHub.invoke('getSiteList');
    });

    ko.applyBindings(c.model);

    $('#siteUrl').val('http://');
});