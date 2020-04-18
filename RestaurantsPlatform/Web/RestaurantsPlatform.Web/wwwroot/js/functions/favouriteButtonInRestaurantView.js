(() => {
    const likeBtn = document.getElementsByClassName('likeBtn')[0];
    const icon = document.getElementsByClassName('heartIcon')[0];

    likeBtn.style.cursor = 'pointer';
    icon.classList.add('far', 'text-danger');

    likeBtn.addEventListener('mouseover', liked);
    likeBtn.addEventListener('mouseout', liked);
    likeBtn.addEventListener('click', function () {
        if (icon.classList.contains('far')) {
            icon.classList.replace('far', 'fa');
            likeBtn.addEventListener('mouseover', liked);
            likeBtn.addEventListener('mouseout', liked);
        } else {
            icon.classList.replace('fa', 'far');
            likeBtn.addEventListener('mouseover', liked);
            likeBtn.addEventListener('mouseout', liked);
        }
    });

    function liked() {
        if (icon.classList.contains('far')) {
            icon.classList.replace('far', 'fa');
        } else {
            icon.classList.replace('fa', 'far');
        }
    }
})();
