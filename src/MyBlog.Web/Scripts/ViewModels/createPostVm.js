var createPostViewModel = function() {
    var self = this;

    self.isCalculationsDone = ko.observable(false);
    self.timeZoneAndLocation = ko.observable("Calculating your location and timezone...");

    self.getCoordinates = function() {
        $.get('/services/getcoordinates').done(function(response) {
            ko.mapping.fromJS(response, {}, self);
            self.getTimeZone();
        });
    };

    self.getTimeZone = function() {
        $.get('/services/gettimezone', { latitude: self.latitude(), longitude: self.longitude() }).done(function(response) {
            self.timeZoneId = ko.observable(response.timeZoneId);
            self.timeZoneName = ko.observable(response.timeZoneName);
            self.timeZoneAndLocation("You are now in " + self.countryName().toLowerCase() + ", " + self.cityName().toLowerCase() + " and in timezone " + self.timeZoneName());
        });
    };
};