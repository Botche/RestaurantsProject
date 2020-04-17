(() => {
    const likeBtn = document.getElementsByClassName('likeBtn')[0];
    const icon = document.getElementsByClassName('heartIcon')[0];

    icon.classList.add('far', 'text-danger');

    likeBtn.addEventListener('mouseover', function () {
        if (icon.classList.contains('far')) {
            icon.classList.replace('far', 'fa');
        } else {
            icon.classList.replace('fa', 'far');
            icon.classList.add();
        }
    });
    likeBtn.addEventListener('mouseout', function () {
        if (icon.classList.contains('far')) {
            icon.classList.replace('far', 'fa');
        } else {
            icon.classList.replace('fa', 'far');
        }
    });
    likeBtn.addEventListener('click', function () {
        if (icon.classList.contains('far')) {
            icon.classList.replace('far', 'fa');
        } else {
            icon.classList.replace('fa','far');
        }
    });
})();
