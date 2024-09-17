// Variables
var intervalId;
const captureVideoButton = document.querySelector('#screenshot .capture-button');
const screenshotButton = document.querySelector('#screenshot-button');
const img = document.querySelector('#screenshot img');
const video = document.querySelector('#screenshot video');
const canvas = document.createElement('canvas');

// --------------------------------------
// --- Select2 Methods (Combobox Plugin)
// --------------------------------------

// Match Start (Return Matched Items for Drop Down List)
function matchStart(params, data) {
    // If there are no search terms, return all of the data
    if ($.trim(params.term) === '') {
        return data;
    }

    // Skip if there is no 'children' property
    if (typeof data.children === 'undefined') {
        return null;
    }

    // `data.children` contains the actual options that we are matching against
    var filteredChildren = [];
    $.each(data.children, function (idx, child) {
        //if (child.text.toUpperCase().indexOf(params.term.toUpperCase()) == 0) {
        //    filteredChildren.push(child);
        //}
        if (child.text.toUpperCase().includes(params.term.toUpperCase()) == 0) {
            filteredChildren.push(child);
        }
    });

    // If we matched any of the timezone group's children, then set the matched children on the group
    // and return the group object
    if (filteredChildren.length) {
        var modifiedData = $.extend({}, data, true);
        modifiedData.children = filteredChildren;

        // You can return modified objects from here
        // This includes matching the `children` how you want in nested data sets
        return modifiedData;
    }

    // Return `null` if the term should not be displayed
    return null;
}

// --------------------------------------
// --- Webcam Methods
// --------------------------------------

// Start Video
function StartVideo(section, height, width) {
    try {

        $(section).css({
            'border': '1px solid red',
            'padding': '0px'
        });

        Webcam.set({
            width: 426,
            height: 319,
            dest_width: 320,
            dest_height: 240,
            image_format: 'jpeg',
            jpeg_quality: 30,
            force_flash: false,
            flip_horiz: true,
            fps: 45
        });

        Webcam.attach(section);
    }
    catch (err) {
        LogError(err);
    }
}

// Stop Video
function StopVideo(section) {
    try {

        Webcam.reset(section);

        $(section).css({
            'border': '1px solid #000',
            'padding': '18px'
        });

        $(section).html('<p class="mb-4">Thank you for using Receiptracks</p>');

    }
    catch (err) {
        LogError(err);
    }
}

// Snapshot
function SnapShot(url, section) {
    var DataPath = '';
    try {
        DataPath = Webcam.snap();
        $.ajax({
            type: 'GET',
            url: url,
            data: DataPath,
            success: function (result) {
                $(section).html(result);
            },
            error: function (xhr, opt, err) {
                alert('Error: ' + xhr.status + ' - ' + xhr.statusText + ' - ' + xhr.responseText);
                console.log('Error: ' + xhr.status + ' - ' + xhr.statusText + ' - ' + xhr.responseText);
            },
        });
    }
    catch (err) {
        LogError(err);
    }
}

// Play Audio
function PlayAudio(file) {
    var audio = new Audio(file);
    audio.play();
}

// --------------------------------------
// --- General Purpose Methods
// --------------------------------------

// Show Dialog
function ShowDialog() {
    $("#dialog").dialog();
}

// Restore Login Preferences
function RestoreLoginPreferences() {
    try {
        var chkPersist = document.getElementById("Persist");
        var txtEmail = document.getElementById("Email");
        if (localStorage.checkbox && localStorage.checkbox !== "") {
            chkPersist.setAttribute("checked", "checked");
            txtEmail.value = localStorage.username;
        } else {
            chkPersist.removeAttribute("checked");
            txtEmail.value = "";
        }
    } catch (ex) {
        alert(ex);
    }
}

// Save Login Preferences
function SaveLoginPreferences() {
    try {
        var chkPersist = document.getElementById('Persist');
        var txtEmail = document.getElementById('Email');
        if (chkPersist.checked && txtEmail.value !== '') {
            localStorage.username = txtEmail.value;
            localStorage.checkbox = chkPersist.value;
        } else {
            localStorage.username = '';
            localStorage.checkbox = '';
        }
    } catch (ex) {
        alert(ex);
    }
}

// Login
function Login() {
    var token = $('input[name=__RequestVerificationToken]').val();
    $.ajax({
        type: "POST",
        url: "/Account/Login/",
        beforeSend: function (request) {
            request.setRequestHeader("RequestVerificationToken", token);
        },
        data: { Email: $("#Email").val(), Password: $("#Password").val() },
        success: function () {
            toastr.clear;
            toastr.success("Login successful. Redirecting ...", "Account Login");
            window.location.assign("/Admin");
        },
        error: function (xhr, opt, err) {
            toastr.clear;
            toastr.error(xhr.statusText, "Account Login");
        },
    });
}

// Register (Customer Account)
function Register() {
    var token = $('input[name=__RequestVerificationToken]').val();
    $.ajax({
        type: "POST",
        url: "/Account/Register/",
        beforeSend: function (request) {
            request.setRequestHeader("RequestVerificationToken", token);
        },
        data: { Email: $("#Email").val(), Password: $("#Password").val() },
        success: function () {
            toastr.clear;
            toastr.success("Registration successful. Redirecting ...", "Account Registration");
            window.location.assign("/Account/Validate/");
        },
        error: function (xhr, opt, err) {
            toastr.clear;
            toastr.error(xhr.statusText, "Account Registration");
        },
    });
}

// Recover Password
function Recover() {
    toastr.success("Submitting request ...", "Recover Password");
}

// Upload Files
function uploadFiles(inputId) {
    var input = document.getElementById(inputId);
    var files = input.files;
    var formData = new FormData();

    for (var i = 1; i != files.length; i++) {
        formData.append("files", files[i]);
    }

    startUpdatingProgressIndicator();
    toastr.info("File upload in progress ...", "Prospects")
    $.ajax(
        {
            url: "/upload",
            data: formData,
            processData: false,
            contentType: false,
            type: "POST",
            success: function (data) {
                stopUpdatingProgressIndicator();
                toastr.success("Upload complete.", "Prospects");
            }
        }
    );
}

// Start Updating Progress Indicator
function startUpdatingProgressIndicator() {
    $("#progress").show();

    intervalId = setInterval(
        function () {
            // We use the POST requests here to avoid caching problems (we could use the GET requests and disable the cache instead)
            $.post(
                "/upload/progress",
                function (progress) {
                    $("#bar").css({ width: progress + "%" });
                    $("#label").html(progress + "%");
                }
            );
        },
        10
    );
}

// Stop Updating Progress Indicator
function stopUpdatingProgressIndicator() {
    clearInterval(intervalId);
}

// Sleep
function sleep(milliseconds) {
    var start = new Date().getTime();
    for (var i = 0; i < 1e7; i++) {
        if ((new Date().getTime() - start) > milliseconds) {
            break;
        }
    }
}

// Preview Icon
function PreviewIcon(icon, element) {
    if (icon != '' && element != '') {
        $(element).replaceWith('<span id="fa-preview" class="' + icon + ' ml-3"></span>');
    }
}

// Get Partial View
function GetPartialView(url, section, option = '') {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (result) {
            $(section).html(result);
            if (option != '') {
                $(section).modal(option);
            }
        },
        error: function (xhr, opt, err) {
            alert('Error: ' + xhr.status + ' - ' + xhr.statusText + ' - ' + xhr.responseText);
            console.log('Error: ' + xhr.status + ' - ' + xhr.statusText + ' - ' + xhr.responseText);
        },
    });
}

// Photo Gallery
function PhotoGallery() {
    $('.fancybox').fancybox({
        'transitionIn': 'elastic',
        'transitionOut': 'elastic',
        'speedIn': 600,
        'speedOut': 200,
        'overlayShow': false
    });
}

// Close
function Close(section) {
    $(section).modal('hide');
}

// Browser Upgrade Notification
function BrowserUpgradeNotification() {
    alert('Your Web Browser appears to be out of date. Please upgrade to the latest version or switch to a modern alternative');
}

// Accept Cookies
function AcceptCookies(section) {
    try {
        document.cookie = $(section).dataset.cookieString;
    }
    catch (err) {
        alert(err);
    }
}

// Log Error
function LogError(error) {
    try {
        console.log('Error: ' + error.status + ' - ' + error.statusText + ' - ' + error.responseText);
    }
    catch (err) {
        // Not much else one can do ...
    }
}

// Null or Empty
String.NullOrEmpty = function (value) {
    return !(typeof value === "string" && value.length > 0);
}

// Rotate
function Rotate(id) {
    var a = $(id).data('angle') || 0;
    a = a + 90;
    if (a >= 360) {
        a = 0;
    }
    $(id).css({ 'transform': 'rotate(' + a + 'deg)' });
    $(id).data('angle', a);
}

// Get URL Parameters (Query String)
function getUrlParameter(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}
