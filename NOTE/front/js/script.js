/* =======================================================================
                Navigation
======================================================================= */
$(function () {

    // on page load
    showHideNav();

    $(window).scroll(function () {
        showHideNav();
    });


    function showHideNav() {
        if ($('.navigation').hasClass('white-nav')) {
            $('.navigation').addClass("white-nav-top");
            $(".nav-logo img").attr("src", "./images/Logos/top-logo-purple.png");
        } else {
            if ($(window).scrollTop() > 40) {
                // show white navigation
                $('.navigation').addClass("white-nav-top");
                // logo
                $(".nav-logo img").attr("src", "./images/Logos/top-logo-purple.png");
            } else {
                $('.navigation').removeClass("white-nav-top");
                $(".nav-logo img").attr("src", "./images/Logos/top-logo-white.png");
            }
        }
    }

});

$(function () {
    $('.menu-toggle').click(function () {
        // show and hide Mobile Toggle Btn
        $(".nav-menu-link").toggleClass("mobile-nav");
        $(this).toggleClass("is-active");
    });
});

// DropDown
$(function () { // DOM ready
    // If a link has a dropdown, add sub menu toggle.
    $('nav ul li a:not(:only-child)').click(function (e) {
        $(this).siblings('.dropdown-menu').toggle();
        // Close one dropdown when selecting another
        $('.dropdown-menu').not($(this).siblings()).hide();
        // e.stopPropagation();
    });
    // Clicking away from dropdown will remove the dropdown class
    $('html').click(function () {
        $('.dropdown-menu').hide();
    });

});




/* =======================================================================
                Login - Password
======================================================================= */
$(".toggle-password").click(function () {

    $(this).toggleClass("fa-eye fa-eye-slash");

    var input = $($(this).attr("toggle"));

    if (input.attr("type") == "password") {
        input.attr("type", "text");
    } else {
        input.attr("type", "password");
    }
});



/* =======================================================================
                Note Detail - Download Thanks
======================================================================= */
function confirmbtn() {
    var r = confirm("Are you sure you want to download this Paid note. Please confirm.");
    if (r == true) {
        $(".download").attr("data-toggle", "modal");
        $(".download").attr("data-target", "#thanksModalCenter");
    } else {
        $(".download").removeAttr("data-toggle", "modal");
        $(".download").removeAttr("data-target", "#thanksModalCenter");
    }
}


/* =======================================================================
                FAQ
======================================================================= */
var coll = document.getElementsByClassName("collapsible");
var i;

for (i = 0; i < coll.length; i++) {
    coll[i].addEventListener("click", function () {
        this.classList.toggle("active");
        var content = this.nextElementSibling;
        if (content.style.maxHeight) {
            content.style.maxHeight = null;
        } else {
            content.style.maxHeight = content.scrollHeight + "px";
        }
    });
}



