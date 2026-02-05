// JavaScript Document
$(document).ready(function () {

    "use strict";

    $("#appointment-form").submit(function (e) {
        e.preventDefault();
        var doctor = $("#doctorData");
        var patient = $(".patient");
        var date = $(".date");
        var name = $(".name");
        var email = $(".email");
        var phone = $(".phone");
        var msg = $(".message");
        var flag = false;
        $(".error-msg").remove();

        if (doctor.val() == "Select Doctor") {
            doctor.addClass("error-select");

            // Check if error message already exists
            if (doctor.next(".error-msg").length === 0) {
                doctor.after('<span class="error-msg">This field is required.</span>');
            }
        }
        if (patient.val() == "") {
            patient.closest(".form-control").addClass("error");
            patient.focus();
            flag = false;
            return false;
        } else {
            patient.closest(".form-control").removeClass("error").addClass("success");
        } if (date.val() == "") {
            date.closest(".form-control").addClass("error");
            date.focus();
            flag = false;
            return false;
        } else {
            date.closest(".form-control").removeClass("error").addClass("success");
        } if (name.val() == "") {
            name.closest(".form-control").addClass("error");
            name.focus();
            flag = false;
            return false;
        } else {
            name.closest(".form-control").removeClass("error").addClass("success");
        } if (email.val() == "") {
            email.closest(".form-control").addClass("error");
            email.focus();
            flag = false;
            return false;
        } else {
            email.closest(".form-control").removeClass("error").addClass("success");
        } if (phone.val() == "") {
            phone.closest(".form-control").addClass("error");
            phone.focus();
            flag = false;
            return false;
        } else {
            phone.closest(".form-control").removeClass("error").addClass("success");
        } if (msg.val() == "") {
            msg.closest(".form-control").addClass("error");
            msg.focus();
            flag = false;
            return false;
        } else {
            msg.closest(".form-control").removeClass("error").addClass("success");
            flag = true;
        }
        var formData = {
            DoctorName: $("#doctorData").val(),
            PatientStatus: $("#inlineFormCustomSelect3").val(),
            FullName: $("input[name='name']").val(),
            Gender: $("select[name='gender']").val(),
            Age: $("input[name='age']").val(),
            Email: $("input[name='email']").val(),
            PhoneNumber: $("input[name='phone']").val(),
            AppointmentDate: $("input[name='date']").val(),
            Message: $("textarea[name='message']").val()
        };

        $(".loading").fadeIn("slow").html("Loading...");

        $.ajax({
            type: "POST",
            url: "/bookingappointment/",
            contentType: "application/json",
            data: JSON.stringify(formData),
            success: function (response) {
                if (response.success) {
                    $(".loading").fadeOut("slow")
                    let formattedDate = new Date(response.appointmentDate).toLocaleDateString('en-US', {
                        month: 'short',  // "Feb"
                        day: 'numeric',  // "17"
                        year: '2-digit'  // "25"
                    });
                    Swal.fire({
                        title: 'Success!',
                        html: 'Thank you for your booking request.<br><br>' +
                            'Our executive will get in touch with you shortly to confirm a time slot for your appointment on ' +
                            `<b>${formattedDate}</b>.<br><br>` +
                            'Alternatively, you can contact us at <b>+91 9230648141, +91 9230648141</b> or email us at ' +
                            '<a href="mailto:ratnakalmcoe@gmail.com">ratnakalmcoe@gmail.com</a> to select your preferred time slot.<br><br>' +
                            '<b>Please ensure you arrive at the hospital at least 30 minutes before your appointment.</b><br><br>' +
                            'Best regards,<br><b>Ratnakamal Medical Centre of Excellence</b>',
                        icon: 'success',
                        confirmButtonColor: '#0C5D87',
                        confirmButtonText: 'OK'
                    }).then(() => {

                        // ✅ CLEAR FIELDS HERE (ONE PLACE ONLY)
                        $("#appointment-form")[0].reset();

                        // optional cleanup
                        $(".form-control").removeClass("error success");
                        $(".error-msg").remove();
                    });


                } else {
                    Swal.fire({
                        title: 'Error!',
                        text: 'Error Sending Request. Please Try Again.',
                        icon: 'error',
                        confirmButtonText: 'OK'
                    });
                    $(".loading").html('<font color="#ff5607">Error Sending Request. Please Try Again.</font>');
                }
            },
            error: function () {
                $(".loading").html('<font color="#ff5607">Server Error. Please Try Again.</font>');
            }
        });
        return false;
    });

    $("#departmentData").change(function () {
        if ($(this).val() !== "Pick Service") {
            $(this).siblings(".error-msg").remove(); // Removes only the related error message
        }
        else {
            $(this).addClass("error-select");
            if ($(this).siblings(".error-msg").length === 0) {
                $(this).after('<span class="error-msg">This field is required.</span>');
            }
        }
    });

    $("#doctorData").change(function () {
        if ($(this).val() !== "Select Doctor") {
            $(this).siblings(".error-msg").remove(); // Removes only the related error message
        }
        else {
            $(this).addClass("error-select");
            if ($(this).siblings(".error-msg").length === 0) {
                $(this).after('<span class="error-msg">This field is required.</span>');
            }
        }
    });

    $("#reset").on('click', function () {
        $(".form-control").removeClass("success").removeClass("error");
    });

})



