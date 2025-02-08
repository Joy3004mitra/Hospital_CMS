document.addEventListener("DOMContentLoaded", function () {
    console.log("Admin panel loaded");
    document.querySelectorAll(".table-image").forEach(img => {
        img.addEventListener("click", () => {
            alert("Clicked image: " + img.src);
        });
    });
});