const likeBtn = document.getElementsByClassName('likeBtn')[0];
const icon = document.getElementsByClassName('heartIcon')[0];

icon.classList.add('far');
icon.classList.add('text-danger');

likeBtn.addEventListener('mouseover', function () {
    if (icon.classList.contains('far')) {
        icon.classList.remove('far');
        icon.classList.add('fa');
    } else {
        icon.classList.remove('fa');
        icon.classList.add('far');
    }
});
likeBtn.addEventListener('mouseout', function () {
    if (icon.classList.contains('far')) {
        icon.classList.remove('far');
        icon.classList.add('fa');
    } else {
        icon.classList.remove('fa');
        icon.classList.add('far');
    }
});
likeBtn.addEventListener('click', function () {
    if (icon.classList.contains('far')) {
        icon.classList.remove('far');
        icon.classList.add('fa');
    } else {
        icon.classList.remove('fa');
        icon.classList.add('far');
    }
});
