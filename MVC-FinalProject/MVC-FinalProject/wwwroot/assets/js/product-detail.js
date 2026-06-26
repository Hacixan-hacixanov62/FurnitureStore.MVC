"use strict"


// ------------------- TOKEN -------------------
function getToken() {
    return 'HttpContextAccessor.HttpContext.Session.GetString("AuthToken")';
}

// ------------------- EDIT -------------------
function toggleEditForm(id) {
    $("#edit-form-" + id).toggle();
}

$(document).on("submit", "form[id^='edit-form-']", function (e) {
    e.preventDefault();
    const form = $(this);
    const data = form.serialize();
    const reviewId = form.find("input[name='Id']").val();

    $.ajax({
        url: `/ProductDetail/EditReview/${reviewId}`,
        type: "POST",
        data: data,
        headers: {
            "Authorization": "Bearer " + getToken()
        },
        success: function (res) {
            $(`li[data-review-id='${reviewId}'] .review-comment-text`).text(res.comment);
            toggleEditForm(reviewId);
        },

        error: function (xhr) {
            // alert("Redaktə zamanı xəta baş verdi: " + xhr.responseText);
        }
    });
});

// ------------------- DELETE -------------------
$(document).on("submit", ".delete-review-form", function (e) {
    e.preventDefault();

    const form = $(this);
    const data = form.serialize();
    const reviewId = form.find("input[name='id']").val();

    $.ajax({
        url: `/ProductDetail/DeleteReview`,
        type: "POST",
        data: data,
        success: function () {
            $(`li[data-review-id='${reviewId}']`).remove();
        },
        error: function (xhr) {
            // alert("Silinmə zamanı xəta: " + xhr.responseText);
        }
    });
});


document.addEventListener("DOMContentLoaded", function () {
    const colorRadios = document.querySelectorAll('input[name="colorId"]');
    const addToCartBtn = document.getElementById("addToCartBtn");

    colorRadios.forEach(function (radio) {
        radio.addEventListener("change", function () {
            if (document.querySelector('input[name="colorId"]:checked')) {
                addToCartBtn.disabled = false;
            }
        });
    });
});

function toggleEditForm(id) {
    const form = document.getElementById(`edit-form-${id}`);
    if (form) {
        form.style.display = (form.style.display === "none" || form.style.display === "") ? "block" : "none";
    }
}

