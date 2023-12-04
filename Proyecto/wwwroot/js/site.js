
function mostrarImagen(input) {
    if (input.files && input.files[0]) {
        var mostrar = new FileReader();
        mostrar.onload = function (e) {
            document.getElementsByTagName("img")[0].setAttribute("src", e.target.result);
        }
        mostrar.readAsDataURL(input.files[0]);
    }
}

