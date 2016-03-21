/* ===== Google Map for Contact Us page ===== */

function initialize()
{
    var map_canvas = document.getElementById('map_canvas');

  var map_options = {
      center: new google.maps.LatLng(37.6692492, -77.573838),
    zoom: 12,
    mapTypeId: google.maps.MapTypeId.ROADMAP
  }
  var map = new google.maps.Map(map_canvas, map_options)

  //alert('hello');
}

google.maps.event.addDomListener(window, 'load', initialize);

