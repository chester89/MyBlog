var createPostViewModel = function (coordinatesUrl, timeZoneUrl, inProgressMessage, timeZoneTemplate) {
    var self = this;
    self.timeZoneAndLocation = ko.observable(inProgressMessage);
    self.timeZoneLoaded = ko.observable(false);

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
};