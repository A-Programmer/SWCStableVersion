var mainslider;

$(document).ready(function(){
    var options = {
        slides: '.slide', // The name of a slide in the slidesContainer
        swipe: true,    // Add possibility to Swipe > note that you have to include touchSwipe for this
        transition: "slide", // Accepts "slide" and "fade" for a slide or fade transition
        slideTracker: true, // Add a UL with list items to track the current slide
        slideTrackerID: 'slideposition', // The name of the UL that tracks the slides
        slideOnInterval: true, // Slide on interval
        interval: 9000, // Interval to slide on if slideOnInterval is enabled
        animateDuration: 1000, // Duration of an animation
        animationEasing: 'easeInOutSine', // Accepts: linear ease in out in-out snap easeOutCubic easeInOutCubic easeInCirc easeOutCirc easeInOutCirc easeInExpo easeOutExpo easeInOutExpo easeInQuad easeOutQuad easeInOutQuad easeInQuart easeOutQuart easeInOutQuart easeInQuint easeOutQuint easeInOutQuint easeInSine easeOutSine easeInOutSine easeInBack easeOutBack easeInOutBack
        pauseOnHover: true // Pause when user hovers the slide container
    };


    $(".slider").on("init", function(event){
        console.log(event);
    });

    $(".slider").simpleSlider(options);
    mainslider = $(".slider").data("simpleslider");
    /* yes, that's all! */

    $(".slider").on("beforeSliding", function(event){
        var prevSlide = event.prevSlide;
        var newSlide = event.newSlide;
        $(".slider .slide[data-index='"+prevSlide+"'] .slidecontent").fadeOut();
        $(".slider .slide[data-index='"+newSlide+"'] .slidecontent").hide();
    });

    $(".slider").on("afterSliding", function(event){
        var prevSlide = event.prevSlide;
        var newSlide = event.newSlide;
        $(".slider .slide[data-index='"+newSlide+"'] .slidecontent").fadeIn();
    });

    $(".slide#first").backstretch("http://localhost:48902/Templates/Default/images/bg1.jpg");
    $(".slide#sec").backstretch("http://localhost:48902/Templates/Default/images/bg2.jpg");
    $(".slide#thirth").backstretch("http://localhost:48902/Templates/Default/images/bg3.jpg");
    $(".slide#fourth").backstretch("http://localhost:48902/Templates/Default/images/bg4.jpg");
    $('.slide .backstretch img').on('dragstart', function(event) { event.preventDefault(); });

    $(".slideshowSection").each(function () {
        $(this).css('margin-top', -$(this).height()/2);
    });
    $(function () {
        $('.slide').each(function () {
            var headerHeight = $('.header').height();
             //headerHeight+=15; // maybe add an offset too?
            $(this).css('margin-top', headerHeight + 'px');
        });
    });
});
