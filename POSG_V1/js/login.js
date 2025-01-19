const inputs = document.querySelectorAll(".input");

function addcl() {
    let parent = this.parentNode.parentNode;
    parent.classList.add("focus");
}

function remcl() {
    let parent = this.parentNode.parentNode;
    if (this.value == "") {
        parent.classList.remove("focus");
    }
}

// Mantener el texto flotante si ya hay un valor en el campo
inputs.forEach(input => {
    if (input.value !== "") {
        let parent = input.parentNode.parentNode;
        parent.classList.add("focus");
    }

    input.addEventListener("focus", addcl);
    input.addEventListener("blur", remcl);
});
