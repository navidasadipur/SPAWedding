'use strict';

/*====================================
Project:     WeddingTime
Author:      AbsharBY
Version:     1.0
Website:     http://abshar.by/wedding-time/
======================================*/

/*---------------------- 
  JS Guide
------------------------
 
 1. LOADER
 2. MENU
 3. MENU STICKY
 4. SLIDER
 5. COUNTDOWN
 6. SCROLL TOP 
 7. ACCORDION ELEMENT
 8. TAB ELEMENT
 9. CONTACT FORM
10. COUNTER
11. MASONRY GALLERY
12. GALLERY WITH FILTERS
13. INSTAGRAM FEED
14. IMAGE POPUP
15. SCROLL ANIMATE
16. MAPS
17. ANIMATION ELEMENTS

*/





// 1. LOADER
//================================================================

window.addEventListener('load', function () {
    $('#loader').fadeOut('300');
})





// 2. MENU
//================================================================

var $menu = $('#menu'),
    $menulink = $('.menu-link'),
    $parentlink = $('.scroll-nav a.parent'),
    $menuTrigger = $('.has-sub-menu > a');

$menulink.on('click', function (e) {
    e.preventDefault();
    $menulink.toggleClass('active');
    $menu.toggleClass('active');
});

$parentlink.on('click', function (e) {
    e.preventDefault();
    $menulink.toggleClass('active');
    $menu.toggleClass('active');
});

$menuTrigger.on('click', function (e) {
    e.preventDefault();
    var $this = $(this);
    $this.toggleClass('active').next('ul').toggleClass('active');
});





// 3. MENU STICKY
//================================================================

var hdr = $(window).height() - 1;

var hm = $(".header-menu");
$(window).scroll(function () {
    if ($(this).scrollTop() > hdr) {
        hm.addClass("header-menu-sticky");
        $("body").addClass("header-bg-sticky");
        $(".logo").addClass("logo-hide");
    } else {
        hm.removeClass("header-menu-sticky");
        $("body").removeClass("header-bg-sticky");
        $(".logo").removeClass("logo-hide");
    }
});






// 4. SLIDER
//================================================================

/** fullscrean slider **/

var owl1 = $("#fullscreen-slider");
owl1.owlCarousel({
    loop: true,
    margin: 0,
    items: 1,
    autoplay: true,
    autoplayTimeout: 7000,
    nav: true,
    navSpeed: 1000,
    singleItem: true,
    navText: [
        "<i class='fa fa-angle-left'></i>",
        "<i class='fa fa-angle-right'></i>"
    ],
    animateIn: 'fadeIn',
    animateOut: 'fadeOut'
});

owl1.on('changed.owl.carousel', function (event) {
    var $currentItem = $('.owl-item', owl1).eq(event.item.index);
    var $elemsToanim = $currentItem.find("[data-animation]");

    setAnimation($elemsToanim);
})

function setAnimation(_elem) {
    var animationEndEvent = 'webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend';

    _elem.each(function () {
        var $elem = $(this);
        $elem.removeClass('animated');
        $elem.removeClass($elem.data('animation'));

        var $animationType = 'animated ' + $elem.data('animation');
        var $animationTimeOut = $elem.data('timeout');

        if ($animationTimeOut) {
            window.setTimeout(function () {
                $elem.addClass($animationType);
            }, parseInt($animationTimeOut, 10));
        } else {

            $elem.addClass($animationType);
        }
    });
}





/** halfscrean slider **/

var owl2 = $("#halfscreen-slider");
owl2.owlCarousel({
    loop: true,
    margin: 0,
    items: 1,
    autoplay: true,
    autoplayTimeout: 8000,
    nav: true,
    navSpeed: 1000,
    dots: false,
    singleItem: true,
    navText: [
        "<i class='fa fa-angle-left'></i>",
        "<i class='fa fa-angle-right'></i>"
    ],
    animateIn: 'fadeIn',
    animateOut: 'fadeOut'
});

/** boxed slider **/

var owl3 = $("#boxed-slider");
owl3.owlCarousel({
    loop: true,
    margin: 0,
    items: 1,
    autoplay: true,
    autoplayTimeout: 4000,
    nav: true,
    navSpeed: 1000,
    dots: false,
    singleItem: true,
    navText: [
        "<i class='fa fa-angle-left'></i>",
        "<i class='fa fa-angle-right'></i>"
    ],
    animateIn: 'fadeIn',
    animateOut: 'fadeOut'
});

/** gallery carousel **/

var owl4 = $("#gallery-carousel");
owl4.owlCarousel({
    loop: true,
    margin: 20,
    items: 3,
    autoplay: true,
    autoplayTimeout: 4000,
    nav: false,
    navSpeed: 1000,
    dots: true,
    singleItem: true,
    navText: [
        "<i class='fa fa-angle-left'></i>",
        "<i class='fa fa-angle-right'></i>"
    ],
    responsiveClass: true,
    responsive: {
        0: {
            items: 1,
            nav: true
        },
        900: {
            items: 2,
            nav: false
        },
        1200: {
            items: 3,
            nav: false
        }
    }
});
var owl44 = $("#gallery2-carousel");
owl44.owlCarousel({
    loop: true,
    margin: 20,
    items: 3,
    autoplay: true,
    autoplayTimeout: 4000,
    nav: false,
    navSpeed: 1000,
    dots: true,
    singleItem: true,
    navText: [
        "<i class='fa fa-angle-left'></i>",
        "<i class='fa fa-angle-right'></i>"
    ],
    responsiveClass: true,
    responsive: {
        0: {
            items: 1,
            nav: true
        },
        900: {
            items: 2,
            nav: false
        },
        1200: {
            items: 3,
            nav: false
        }
    }
});

/**  testimonians carousel **/

var owl5 = $("#testimonials-carousel");
owl5.owlCarousel({
    loop: true,
    margin: 0,
    items: 3,
    autoplay: true,
    autoplayTimeout: 4000,
    nav: false,
    dots: true,
    navSpeed: 1000,
    singleItem: true,
    navText: [
        "<i class='fa fa-angle-left'></i>",
        "<i class='fa fa-angle-right'></i>"
    ],
    responsiveClass: true,
    responsive: {
        0: {
            items: 1,
            nav: true
        },
        700: {
            items: 3,
            nav: false
        }
    }
});





// 5. COUNTDOWN
//================================================================

var weddingTime = '2018/05/31';

$('#countdown').countdown(weddingTime, function (event) {

    $('#text-countdown').html(event.strftime('%w <span class="grey">weeks</span> %d <span class="grey">days</span> %H:%M:%S'));

    $('#day-countdown').html(event.strftime('<span class="big-num">%D</span> <span class="grey">days</span>'));
    $('#clock-countdown').html(event.strftime('' +
        '<div class="countdown-section"><div class="countdown-amount">%-m</div><div class="countdown-text"><span class="grey">Month%!m</span></div></div>' +
        '<div class="countdown-section"><div class="countdown-amount">%-d</div><div class="countdown-text"><span class="grey">Day%!d</span></div></div>' +
        '<div class="countdown-section"><div class="countdown-amount">%H</div><div class="countdown-text"><span class="grey">Hours</span></div></div>' +
        '<div class="countdown-section"><div class="countdown-amount">%M</div><div class="countdown-text"><span class="grey">Minuts</span></div></div>' +
        '<div class="countdown-section"><div class="countdown-amount">%S</div><div class="countdown-text"><span class="grey">Seconds</span></div></div>'));
});





// 6. SCROLL TOP
//================================================================

$(window).scroll(function () {
    if ($(this).scrollTop() > 600) {
        $('.scrollup').fadeIn();
    } else {
        $('.scrollup').fadeOut();
    }
});

$(document).on('click', '.scrollup', function () {
    $("html, body").animate({
        scrollTop: 0
    }, 800);
    return false;
});





// 7. ACCORDION ELEMENT
//================================================================

$('.toggle:first').addClass('current');
$('li .inner:first').toggleClass('show');
$('li .inner:first').slideToggle(350);

$('.toggle').on('click', function (e) {
    e.preventDefault();

    var $this = $(this);
    if ($this.next().hasClass('show')) {
        $('.toggle').removeClass('current');
        $(this).toggleClass('current');
        $this.next().removeClass('show');
        $this.next().slideUp(350);
    } else {
        $('.toggle').removeClass('current');
        $(this).toggleClass('current');
        $this.parent().parent().find('li .inner').removeClass('show');
        $this.parent().parent().find('li .inner').slideUp(350);
        $this.next().toggleClass('show');
        $this.next().slideToggle(350);
    }
});





// 8. TAB ELEMENT
//================================================================

$('ul.tab-element-tabs li').on('click', function () {
    var tab_id = $(this).attr('data-tab');

    $('ul.tab-element-tabs li').removeClass('current');
    $('.tab-content').removeClass('current');

    $(this).addClass('current');
    $("#" + tab_id).addClass('current');
})





// 9. CONTACT FORM
//================================================================

/** form with labels **/

$("form#form-labels").validate({
    rules: {
        name: "required",
        email: {
            required: true,
            email: true
        }
    },
    messages: {
        name: "Please enter your name",
        email: "Please enter a valid email address"
    },
    submitHandler: function (form) {

        // values from form
        var name = $("#name").val();
        var email = $("#email").val();
        var phone = $("#phone").val();
        var message = $("#message").val();

        $.ajax({
            url: "./php/form.php",
            type: "POST",
            data: { name: name, phone: phone, email: email, message: message },
            cache: false,
            success: function () {
                $('.form-block').prepend("<p class='success-message'>Thank You! Your message has been sent.</p><br>");
                $('.form-block').trigger("reset");
            },
            error: function () {

            },
        })

    }
});

/** form without labels **/

$("form#form-without-labels").validate({
    rules: {
        name: "required",
        email: {
            required: true,
            email: true
        }
    },
    messages: {
        name: "Please enter your name",
        email: "Please enter a valid email address"
    },
    submitHandler: function (form) {

        // values from form
        var name = $("#name").val();
        var email = $("#email").val();
        var phone = $("#phone").val();
        var guests = $("#guests").val();
        var message = $("#message").val();

        $.ajax({
            url: "./php/form.php",
            type: "POST",
            data: { name: name, phone: phone, email: email, guests: guests, message: message },
            cache: false,
            success: function () {
                $('.form-block').prepend("<p class='success-message'>Thank You! Your message has been sent.</p><br>");
                $('.form-block').trigger("reset");
            },
            error: function () {

            },
        })

    }
});

/** form parallax **/

$("form#form-parallax").validate({
    rules: {
        name: "required",
        email: {
            required: true,
            email: true
        }
    },
    messages: {
        name: "Please enter your name",
        email: "Please enter a valid email address"
    },
    submitHandler: function (form) {

        // values from form
        var name = $("#name").val();
        var email = $("#email").val();
        var phone = $("#phone").val();
        var guests = $("#guests").val();
        var message = $("#message").val();

        $.ajax({
            url: "./php/form.php",
            type: "POST",
            data: { name: name, phone: phone, email: email, guests: guests, message: message },
            cache: false,
            success: function () {
                $('.form-block').prepend("<p class='success-message'>Thank You! Your message has been sent.</p><br>");
                $('.form-block').trigger("reset");
            },
            error: function () {

            },
        })

    }
});





// 10. COUNTER
//================================================================

var a = 0;
$(window).scroll(function () {

    if ($("#counter").length > 0) {
        var oTop = $('#counter').offset().top - window.innerHeight;
        if (a == 0 && $(window).scrollTop() > oTop) {
            $('.counter-value').each(function () {
                var $this = $(this),
                    countTo = $this.attr('data-count');
                $({
                    countNum: $this.text()
                }).animate({
                    countNum: countTo
                }, {
                    duration: 2000,
                    easing: 'swing',
                    step: function () {
                        $this.text(Math.floor(this.countNum));
                    },
                    complete: function () {
                        $this.text(this.countNum);
                    }
                });
            });
            a = 1;
        }
    }

});





// 11. MASONRY GALLERY
//================================================================

$(window).load(function () {

    $('.grid-5').masonry({
        itemSelector: '.masonry-item-5',
        columnWidth: '.grid-sizer-5',
        percentPosition: true
    })


    $('.grid-4').masonry({
        itemSelector: '.masonry-item-4',
        columnWidth: '.grid-sizer-4',
        percentPosition: true
    })


    $('.grid-3').masonry({
        itemSelector: '.masonry-item-3',
        columnWidth: '.grid-sizer-3',
        percentPosition: true
    })

});





// 12. GALLERY WITH FILTERS
//================================================================

var contents = $('#gallery');
var filter = $('.filter');
var mix = $('.mix');

filter.on('click', function () {
    var th = $(this),
        matches = th.data('filter');
    if (matches == 'all') {
        mix.addClass("hide");
        setTimeout(function () { $(".mix").removeClass("hide") }, 100);
        $(".filter").removeClass("active");
        $(".mix").addClass("fade");
        th.addClass("active")
    } else {
        mix.addClass("hide");
        setTimeout(function () {
            var matchesMix = contents.find('.' + matches);
            matchesMix.removeClass('hide').addClass("fade");
        }, 100);

        $(".filter").removeClass("active");
        th.addClass("active")
    }


});






// 13. INSTAGRAM FEED
//================================================================

$('.il-instagram-feed').instagramLite({
    accessToken: '34626526.e03499d.ce25ff5470e246aab7e54dcb22c754c0',
    urls: true,
    limit: 6,
    success: function () {
        console.log('The request was successful!');
    },
    error: function () {
        console.log('There was an error with your request.');
    }
});





// 14. IMAGE POPUP
//================================================================


$(function () { // document ready
    $(".lighterbox").lighterbox({ overlayColor: "white" });
});





// 15. SCROLL ANIMATE
//================================================================

function scrollToDiv(element, navheight) {
    var offset = element.offset();
    var offsetTop = offset.top;
    var totalScroll = offsetTop - navheight;
    $('body, html').animate({ scrollTop: totalScroll }, 1800);
}

$('.scroll-nav a').on('click', function (e) {
    e.preventDefault();
    var el = $(this).attr('href');
    var elWrapped = $(el);
    scrollToDiv(elWrapped, 0);

    $('#menu a').removeClass('active');
    $(this).addClass('active');

});

$('section').waypoint(function (direction) {
    var $active = $(this);
    if (direction === "up") {
        $active = $active.prev();
    }
    if (!$active.length) {
        $active.end();
    }
}, { offset: '30%' });





// 16. MAPS
//================================================================

jQuery(function ($) {
    // Asynchronously Load the map API 
    var script = document.createElement('script');
    script.src = "https://maps.googleapis.com/maps/api/js?key=AIzaSyBCl5awPaVjlMrXKfwC8nQhPE8NEjz4qnk&callback=initialize";
    document.body.appendChild(script);
});

function initialize() {
    var map;
    var bounds = new google.maps.LatLngBounds();

    var mapOptions = {
        scrollwheel: false,
        center: markers,
        zoom: 7,
        styles: [{ "featureType": "water", "elementType": "geometry", "stylers": [{ "color": "#e9e9e9" }, { "lightness": 17 }] }, { "featureType": "landscape", "elementType": "geometry", "stylers": [{ "color": "#f5f5f5" }, { "lightness": 20 }] }, { "featureType": "road.highway", "elementType": "geometry.fill", "stylers": [{ "color": "#ffffff" }, { "lightness": 17 }] }, { "featureType": "road.highway", "elementType": "geometry.stroke", "stylers": [{ "color": "#ffffff" }, { "lightness": 29 }, { "weight": 0.2 }] }, { "featureType": "road.arterial", "elementType": "geometry", "stylers": [{ "color": "#ffffff" }, { "lightness": 18 }] }, { "featureType": "road.local", "elementType": "geometry", "stylers": [{ "color": "#ffffff" }, { "lightness": 16 }] }, { "featureType": "poi", "elementType": "geometry", "stylers": [{ "color": "#f5f5f5" }, { "lightness": 21 }] }, { "featureType": "poi.park", "elementType": "geometry", "stylers": [{ "color": "#dedede" }, { "lightness": 21 }] }, { "elementType": "labels.text.stroke", "stylers": [{ "visibility": "on" }, { "color": "#ffffff" }, { "lightness": 16 }] }, { "elementType": "labels.text.fill", "stylers": [{ "saturation": 36 }, { "color": "#333333" }, { "lightness": 40 }] }, { "elementType": "labels.icon", "stylers": [{ "visibility": "off" }] }, { "featureType": "transit", "elementType": "geometry", "stylers": [{ "color": "#f2f2f2" }, { "lightness": 19 }] }, { "featureType": "administrative", "elementType": "geometry.fill", "stylers": [{ "color": "#fefefe" }, { "lightness": 20 }] }, { "featureType": "administrative", "elementType": "geometry.stroke", "stylers": [{ "color": "#fefefe" }, { "lightness": 17 }, { "weight": 1.2 }] }]
    };

    // Display a map on the page

    if (document.getElementById("white-map")) {
        map = new google.maps.Map(document.getElementById("white-map"), mapOptions);
        map.setTilt(45);

        var d_image = 'img/dark-marker.png';
        var l_image = 'img/light-marker.png';

        // Multiple Markers
        var markers = [
            ['1. Reseption', 51.503454, -0.119562],
            ['2. Ceremony location', 51.499633, -0.124755],
            ['3. Gift registry', 51.489633, -0.114755],
            ['4. Wedding party', 51.481633, -0.124758],
        ];

        // Info Window Content
        var infoWindowContent = [
            ['<div class="info_content">' +
                '<h6>1. Reseption</h6>' +
                '<p>Et harum quidem rerum facilis est et expedita distinctio. Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere possimus, omnis voluptas assumenda est, omnis dolor repellendus..</p>' + '</div>'
            ],
            ['<div class="info_content">' +
                '<h6>2. Ceremony location</h6>' +
                '<p>Et harum quidem rerum facilis est et expedita distinctio. Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere possimus, omnis voluptas assumenda est, omnis dolor repellendus.</p>' +
                '</div>'
            ],
            ['<div class="info_content">' +
                '<h6>3. Gift registry</h6>' +
                '<p>Et harum quidem rerum facilis est et expedita distinctio. Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere possimus, omnis voluptas assumenda est, omnis dolor repellendus.</p>' + '</div>'
            ],
            ['<div class="info_content">' +
                '<h6>4. Wedding party</h6>' +
                '<p>Et harum quidem rerum facilis est et expedita distinctio. Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere possimus, omnis voluptas assumenda est, omnis dolor repellendus.</p>' +
                '</div>'
            ]
        ];

        // Display multiple markers on a map
        var infoWindow = new google.maps.InfoWindow(),
            marker, i;

        // Loop through our array of markers & place each one on the map  
        for (i = 0; i < markers.length; i++) {
            var position = new google.maps.LatLng(markers[i][1], markers[i][2]);
            bounds.extend(position);
            marker = new google.maps.Marker({
                position: position,
                map: map,
                icon: d_image
            });

            // Allow each marker to have an info window    
            google.maps.event.addListener(marker, 'click', (function (marker, i) {
                return function () {
                    infoWindow.setContent(infoWindowContent[i][0]);
                    infoWindow.open(map, marker);
                }
            })(marker, i));

            // Automatically center the map fitting all markers on the screen
            map.fitBounds(bounds);
        }

        // Override our map zoom level once our fitBounds function runs (Make sure it only runs once)
        var boundsListener = google.maps.event.addListener((map), 'bounds_changed', function (event) {
            google.maps.event.removeListener(boundsListener);
        });
    }



}





// 17. ANIMATION ELEMENTS
//================================================================

window.addEventListener('load', function () {

    var $window = $(window),
        win_height_padded = $window.height() * 1.1,
        isTouch = Modernizr.touch;

    if (isTouch) { $('.animate').addClass('animated'); }

    $window.on('scroll', animateScroll);

    function animateScroll() {
        var scrolled = $window.scrollTop(),
            win_height_padded = $window.height() * 1.1;

        $(".animate:not(.animated)").each(function () {
            var $this = $(this),
                offsetTop = $this.offset().top;

            if (scrolled + win_height_padded > offsetTop) {
                if ($this.data('timeout')) {
                    window.setTimeout(function () {
                        $this.addClass('animated ' + $this.data('animation'));
                    }, parseInt($this.data('timeout'), 10));
                } else {
                    $this.addClass('animated ' + $this.data('animation'));
                }
            }
        });

    }

    animateScroll();
});