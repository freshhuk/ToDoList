//VARIABLES

var textInputs = document.querySelectorAll(".text-input");
var buttons = document.querySelectorAll(".aside .button-container .button");
var materialSymbols = document.querySelectorAll(".aside .button-container .button .material-symbols-rounded");
var labels = document.querySelectorAll("label");

var aside = document.querySelector(".aside");
var header = document.querySelector(".header");
var cont = document.querySelector(".container");
var editor = document.querySelector(".editor");
var submit = document.querySelector(".editor form .submit");

//FUNCTIONS

function turnBlack() {
    labels.forEach(label => label.style.color = "whitesmoke");
    materialSymbols.forEach(materialSymbol => materialSymbol.style.color = "whitesmoke");
    buttons.forEach(button => button.style.color = "whitesmoke");
    textInputs.forEach(textInput => {
        textInput.style.background = "#333";
        textInput.style.color = "whitesmoke";
    })
    
    submit.style.background = "#1d1d1d";
    submit.style.boxShadow = "#111 0px 0px 1.5px, #111 0px 0px 0px 1.5px";
    submit.style.color = "whitesmoke";

    editor.style.background = "#222";
    editor.style.border = "2px solid #111";

    cont.style.background = "#2f2f2f";

    header.style.background = "#222";
    header.style.borderBottom = "2px solid #111";
    header.style.boxShadow = "none";
    header.style.color = "whitesmoke";

    aside.style.background = "#222";
    aside.style.borderRight = "2px solid #111";

    button.style.color = "whitesmoke";

    materialSymbol.style.color = "whitesmoke";

}
function turnLight() {
    labels.forEach(label => label.style.color = "#222");
    materialSymbols.forEach(materialSymbol => materialSymbol.style.color = "#222");
    buttons.forEach(button => button.style.color = "#222");
    textInputs.forEach(textInput => {
        textInput.style.background = "rgb(250, 250, 250)";
        textInput.style.color = "#222";
    })

    submit.style.background = "white";
    submit.style.boxShadow = "#9b9b9b 0px 0px 1.5px, #9b9b9b 0px 0px 0px 1.5px";
    submit.style.color = "#222";

    editor.style.background = "white";
    editor.style.border = "2px solid #9b9b9b";

    cont.style.background = "whitesmoke";

    header.style.background = "white";
    header.style.borderBottom = "2px solid #9b9b9b";
    header.style.boxShadow = "rgba(99, 99, 99, 0.2) 0px 2px 8px 0px";
    header.style.color = "#222";

    aside.style.background = "white"
    aside.style.borderRight = "2px solid #9b9b9b"

    button.style.color = "#222";

    materialSymbol.style.color = "#222";

}

function changeFunc() {
    var selectBox = document.getElementById("themeSelect");
    var selectedValue = selectBox.options[selectBox.selectedIndex].value;
    if (selectedValue == "Black") {
        turnBlack();
    } else if (selectedValue == "Light") {
        turnLight();
    }
    localStorage.setItem("SettingsTheme", selectedValue); // Сохраняем выбранную тему в localStorage
}
// Вызываем функцию для установки текущей темы при загрузке страницы
window.onload = function () {
    var themeFromLocalStorage = localStorage.getItem("SettingsTheme");
    if (themeFromLocalStorage) {
        var themeSelect = document.getElementById("themeSelect");
        themeSelect.value = themeFromLocalStorage;
        changeFunc(); // Вызываем функцию для установки текущей темы
    }
};