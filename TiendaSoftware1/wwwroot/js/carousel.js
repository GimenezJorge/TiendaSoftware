let currentSlideIndex = 0;
const slideWidth = document.querySelector('.carousel').clientWidth; // Ancho del carrusel

showSlide(currentSlideIndex);

// Cambia automáticamente cada 10 segundos
setInterval(() => {
    nextSlide();
}, 6000);

function showSlide(index) {
    const slides = document.querySelector('.slides');
    const dots = document.querySelectorAll('.dot');

    // Mueve el contenedor de las imágenes hacia la izquierda según el índice de la imagen actual
    slides.style.transform = `translateX(${-index * slideWidth}px)`;

    dots.forEach(dot => dot.classList.remove('active'));
    dots[index].classList.add('active');
}

function nextSlide() {
    const slides = document.querySelectorAll('.slides img');
    currentSlideIndex = (currentSlideIndex + 1) % slides.length;
    showSlide(currentSlideIndex);
}

function currentSlide(n) {
    currentSlideIndex = n - 1;
    showSlide(currentSlideIndex);
}
