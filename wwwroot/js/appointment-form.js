// JavaScript Document
$(document).ready(function() {

    "use strict";

    $("#appointment-form").submit(function(e) {
        e.preventDefault();
        var department = $(".department");
        var doctor = $(".doctor");
        var patient = $(".patient");
        var date = $(".date");
        var name = $(".name");
        var email = $(".email");
        var phone = $(".phone");
        var msg = $(".message");
        var flag = false;
        if (department.val() == "Pick Service") {
            department.closest(".form-control").addClass("error");
            department.focus();
            flag = false;
            return false;
        } else {
            department.closest(".form-control").removeClass("error").addClass("success");
        } if (doctor.val() == "Select Doctor") {
            doctor.closest(".form-control").addClass("error");
            doctor.focus();
            flag = false;
            return false;
        } else {
            doctor.closest(".form-control").removeClass("error").addClass("success");
        } if (patient.val() == "") {
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
            ServiceName: $("#departmentData").val(),
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
            url: "/Home/BookingAppointmentSubmit",
            contentType: "application/json",
            data: JSON.stringify(formData),
            success: function (response) {
                if (response.success) {
                    $(".loading").html('<font color="#48af4b">Appointment Request Sent Successfully.</font>');
                } else {
                    $(".loading").html('<font color="#ff5607">Error Sending Request. Please Try Again.</font>');
                }
            },
            error: function () {
                $(".loading").html('<font color="#ff5607">Server Error. Please Try Again.</font>');
            }
        });
        return false;
    });
    $("#reset").on('click', function() {
        $(".form-control").removeClass("success").removeClass("error");
    });
    
})



