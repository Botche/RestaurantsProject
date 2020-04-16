(() => {
    window.onscroll = function () {
        scrollFunction()
    };

    const scrollToTopBtn = document.getElementById("scrollToTop");
    scrollToTopBtn.style.display = "none";
    scrollToTopBtn.addEventListener('click', topFunction);

    function scrollFunction() {
        if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
            scrollToTopBtn.style.display = "block";
        }
        else {
            scrollToTopBtn.style.display = "none";
        }
    }

    function topFunction() {
        document.body.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
})();