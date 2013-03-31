var createPostViewModel = function (coordinatesUrl, timeZoneUrl, slugUrl, inProgressMessage, timeZoneTemplate) {
    var self = this;
    self.timeZoneAndLocation = ko.observable(inProgressMessage);
    self.timeZoneLoaded = ko.observable(false);
    self.title = ko.observable('');
    self.slug = ko.observable('');
    self.slugLoaded = ko.observable(false);
    self.startedAt = ko.observable();
    self.finishedAt = ko.observable();

    self.title.subscribe(function(newValue) {
        self.generateSlug();
    });

    self.generateSlug = function() {
        $.get(slugUrl, { title: self.title() }).done(function(response) {
            self.slug(response.slug);
            self.slugLoaded(true);
        });
    };
    self.urlPreview = ko.computed(function() {
       return '/blog/' + self.slug();
    });

    self.getCoordinates = function () {
        $.get(coordinatesUrl).done(function (response) {
            ko.mapping.fromJS(response, {}, self);
            self.getTimeZone(timeZoneUrl);
        });
    };

    self.getTimeZone = function () {
        var stamp = new Date() / 1000;
        $.get(timeZoneUrl, { latitude: self.latitude(), longitude: self.longitude(), timestamp: stamp, sensor: false }).done(function (response) {
            self.timeZoneId = ko.observable(response.timeZoneId);
            self.timeZoneName = ko.observable(response.timeZoneName);
            self.timeZoneAndLocation(timeZoneTemplate.format(self.countryName().toTitleCase(), self.cityName().toTitleCase(), self.timeZoneName().toTitleCase()));
            self.timeZoneLoaded(true);
        });
    };

    self.finishPost = function() {
        var date = new Date();
        self.finishedAt(date.getDate() + '.' + (date.getMonth() + 1) + '.' + date.getFullYear() + '//' +
                    date.getHours() + ':' + date.getMinutes() + ':' + date.getSeconds());
    };
};