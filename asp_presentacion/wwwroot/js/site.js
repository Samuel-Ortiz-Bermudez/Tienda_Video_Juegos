// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//Variables
const resultado = document.querySelector('#resultado');


//Eventos
document.addEventListener('DOMContentLoaded', () => {
    console.log("site.js cargado");  // Verifica que aparece en la consola
    mostrarJuegos();
    llenarSelectAnios();
});


//Funciones
function mostrarJuegos() {

    Juegos.forEach(Juego => {
        const juegoHTML = document.createElement('p');
        juegoHTML.textContent = `${Juego.Nombre} - ${Juego.Plataforma} - ${Juego.Año} - ${Juego.Genero}`;
        resultado.appendChild(juegoHTML);
    });
}

function llenarSelectAnios() {
    const year = document.querySelector('#year');
    const anios = [...new Set(Juegos.map(j => j.Año))].sort((a, b) => b - a);
    anios.forEach(anio => {
        const opcion = document.createElement('option');
        opcion.value = anio;
        opcion.textContent = anio;
        year.appendChild(opcion);
    });
}



