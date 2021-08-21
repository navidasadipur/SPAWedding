/** Setting menu **/

$(document).on('click', '#setting-button',  function(){
  if ($("#setting-button i").hasClass("fa-cogs")){
    $("#options-setting").addClass("move-right");
    $("#setting-button i").removeClass("fa-cogs");
    $("#setting-button i").addClass("fa-times");
  }
  else
  {
    $("#options-setting").removeClass("move-right");
    $("#setting-button i").removeClass("fa-times");
    $("#setting-button i").addClass("fa-cogs");
  }
});

/** Color menu **/

var current_color = "dapper";


$(document).on('click', '#dapper',  function(){
  $('link[rel*=skin]').remove();
  $('head').append('<link rel="stylesheet skin" href="css/color/dapper.css" type="text/css" />');
  current_color = "dapper";
});
 
$(document).on('click', '#magical',  function(){
  $('link[rel*=skin]').remove();
  $('head').append('<link rel="stylesheet skin" href="css/color/magical.css" type="text/css" />');
  current_color = "magical";
});

$(document).on('click', '#breezy',  function(){
  $('link[rel*=skin]').remove();
  $('head').append('<link rel="stylesheet skin" href="css/color/breezy.css" type="text/css" />');
  current_color = "magical";
});

$(document).on('click', '#eclectic',  function(){
  $('link[rel*=skin]').remove();
  $('head').append('<link rel="stylesheet skin" href="css/color/eclectic.css" type="text/css" />');
  current_color = "eclectic";
});

$(document).on('click', '#ashwood',  function(){
  $('link[rel*=skin]').remove();
  $('head').append('<link rel="stylesheet skin" href="css/color/ashwood.css" type="text/css" />');
  current_color = "ashwood";
});

$(document).on('click', '#sassy',  function(){
  $('link[rel*=skin]').remove();
  $('head').append('<link rel="stylesheet skin" href="css/color/sassy.css" type="text/css" />');
  current_color = "sassy";
});

$(document).on('click', '#artistic',  function(){
  $('link[rel*=skin]').remove();
  $('head').append('<link rel="stylesheet skin" href="css/color/artistic.css" type="text/css" />');
  current_color = "artistic";
});

$(document).on('click', '#gentle',  function(){
  $('link[rel*=skin]').remove();
  $('head').append('<link rel="stylesheet skin" href="css/color/gentle.css" type="text/css" />');
  current_color = "gentle";
});

$(document).on('click', '#northern',  function(){
  $('link[rel*=skin]').remove();
  $('head').append('<link rel="stylesheet skin" href="css/color/northern.css" type="text/css" />');
  current_color = "northern";
});

$(document).on('click', '#earthy',  function(){
  $('link[rel*=skin]').remove();
  $('head').append('<link rel="stylesheet skin" href="css/color/earthy.css" type="text/css" />');
  current_color = "earthy";
});

$(document).on('click', '#zesty',  function(){
  $('link[rel*=skin]').remove();
  $('head').append('<link rel="stylesheet skin" href="css/color/zesty.css" type="text/css" />');
  current_color = "zesty";
});

$(document).on('click', '#timeless',  function(){
  $('link[rel*=skin]').remove();
  $('head').append('<link rel="stylesheet skin" href="css/color/timeless.css" type="text/css" />');
  current_color = "timeless";
});


$(document).on('click', '#header-white',  function(){
 $( "body" ).addClass( "header-bg");
});

$(document).on('click', '#header-transparent',  function(){
 $( "body" ).removeClass( "header-bg");
});

$(document).on('click', '#header-top',  function(){
  $( "header" ).removeClass( "bottom-menu");
  $( "header" ).addClass( "top-menu");
});

$(document).on('click', '#header-bottom',  function(){
  $( "header" ).removeClass("top-menu");
  $( "header" ).addClass( "bottom-menu");
});