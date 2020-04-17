(() => {
    const likesBtn = document.getElementsByClassName('likeBtn');

    Array.from(likesBtn).forEach(likeBtn => {

        const icon = likeBtn.getElementsByTagName('i')[0];

        icon.classList.add('far', 'text-danger');

        likeBtn.addEventListener('mouseover', function () {
            icon.classList.replace('text-danger', 'text-white');
        });
        likeBtn.addEventListener('mouseout', function () {
            icon.classList.replace('text-white', 'text-danger');
        });
        likeBtn.addEventListener('click', function () {
            if (icon.classList.contains('fa')) {
                icon.classList.replace('fa', 'far');
            } else {
                icon.classList.replace('far', 'fa');
            }
        });
    });
})();