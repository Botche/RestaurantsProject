(() => {
    const navIcon = document.getElementsByClassName('nav-icon')[0];

    navIcon.addEventListener('click', clickNavigationButton);

    function clickNavigationButton() {

        if (navIcon.parentNode.getAttribute('aria-expanded') === 'true') {
            navIcon.classList.remove('nav-icon-clicked');
        } else {
            navIcon.classList.add('nav-icon-clicked');
        }
    };
})();