const imageContainers = document.getElementsByClassName('imageContainer');

Array.from(imageContainers).forEach(imageContainer => {
    const imagesBtn = imageContainer.getElementsByClassName('pictureBtn');

    Array.from(imagesBtn).forEach(imageBtn => {
        imageBtn.style.display = 'none';

        imageContainer.addEventListener('mouseover', function () {
            imageBtn.style.display = 'block';
        });
        imageContainer.addEventListener('mouseout', function () {
            imageBtn.style.display = 'none';
        });
    });
});