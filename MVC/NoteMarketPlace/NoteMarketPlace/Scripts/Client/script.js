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
            $(".nav-logo img").attr("src", "/images/Logos/top-logo-purple.png");
        } else {
            if ($(window).scrollTop() > 40) {
                // show white navigation
                $('.navigation').addClass("white-nav-top");
                // logo
                $(".nav-logo img").attr("src", "/images/Logos/top-logo-purple.png");
            } else {
                $('.navigation').removeClass("white-nav-top");
                $(".nav-logo img").attr("src", "/images/Logos/top-logo-white.png");
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
                Reload Page 
======================================================================= */
$(function () {
    if (window.localStorage) {
        if (!localStorage.getItem('firstLoad')) {
            localStorage['firstLoad'] = true;
            window.location.reload();
        }
        else {
            localStorage.removeItem('firstLoad');
        }
    }
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


/* =======================================================================
                Verifivation Email
======================================================================= */
function verified() {
    $.ajax({
        type: "post",
        url: "Signup/RegisterConfirm/@ViewBag.regID",
        data: { 'regID': '@ViewBag.regID' },
    })
}

/* =======================================================================
                Enable/Diable Radio btn In AddNote
======================================================================= */
$(function () {
    $("input[name='IsPaid']").click(function () {
        if ($("#isPaidBtn").is(":checked")) {
            $("#sell-price").removeAttr("disabled");
            $("#sell-price").focus();
        } else {
            $("#sell-price").attr("disabled", "disabled");
        }
    });
})

/* =======================================================================
                NoteDetail - DropDown And Search 
======================================================================= */
$(document).ready(function () {
    $(".input-boxes #type").change(function () {
        this.form.submit();
    });
    $(".input-boxes #search").change(function () {
        this.form.submit();
    });
    $(".input-boxes #category").change(function () {
        this.form.submit();
    });
    $(".input-boxes #university").change(function () {
        this.form.submit();
    });
    $(".input-boxes #course").change(function () {
        this.form.submit();
    });
    $(".input-boxes #country").change(function () {
        this.form.submit();
    });
    $(".input-boxes #rating").change(function () {
        this.form.submit();
    });
});


/* =======================================================================
                Select File
======================================================================= */

var input = document.getElementById('dis-pic');
var input2 = document.getElementById('upload-notes');
var input3 = document.getElementById('note-preview');

var infoArea = document.getElementById('display-picture-filename');
var infoArea2 = document.getElementById('note-filename');
var infoArea3 = document.getElementById('note-preview-filename');

input.addEventListener('change', showFileName);
input2.addEventListener('change', showFileName2);
input3.addEventListener('change', showFileName3);


function showFileName(event) {
    infoArea.textContent = null;
    var input = event.srcElement;
    var fileName = input.files[0].name;
    infoArea.textContent = fileName;
}

function showFileName2(event) {
    infoArea2.textContent = null;
    var input2 = event.srcElement;
    var fileName2 = input2.files[0].name;
    infoArea2.textContent = fileName2;
}

function showFileName3(event) {
    infoArea3.textContent = null;
    var input3 = event.srcElement;
    var fileName3 = input3.files[0].name;
    infoArea3.textContent = fileName3;
}





   