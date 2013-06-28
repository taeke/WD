//@author = Taeke van der Veen

$(function () {
    var changeVisualCountryHub = $.connection.changeVisualCountryHub;
    changeVisualCountryHub.client.setChangeCountry = function (country, color, number) {
      wdNS.draw(country, color, number);;
    };

    $.connection.hub.start();
});
