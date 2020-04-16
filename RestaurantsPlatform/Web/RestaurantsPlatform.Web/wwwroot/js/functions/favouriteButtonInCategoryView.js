(() => {
    const likesBtn = document.getElementsByClassName('likeBtn');

    Array.from(likesBtn).forEach(likeBtn => {

        const icon = likeBtn.getElementsByTagName('i')[0];

        icon.classList.add('far');
        icon.classList.add('text-danger');

        likeBtn.addEventListener('mouseover', function () {
            icon.classList.add('text-white');
            icon.classList.remove('text-danger');
        });
        likeBtn.addEventListener('mouseout', function () {
            icon.classList.remove('text-white');
            icon.classList.add('text-danger');
        });
        likeBtn.addEventListener('click', function () {
            if (icon.classList.contains('fa')) {
                icon.classList.remove('fa');
                icon.classList.add('far');
            } else {
                icon.classList.remove('far');
                icon.classList.add('fa');
            }
        });
    });
})();