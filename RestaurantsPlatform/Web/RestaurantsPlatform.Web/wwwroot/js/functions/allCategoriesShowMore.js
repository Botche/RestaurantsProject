(() => {
    const medias = document.getElementsByClassName('category-body');

    Array.from(medias).forEach(media => {
        const shortDescription = media.getElementsByClassName('category-description')[0];
        const fullDescription = media.getElementsByClassName('category-description')[1];

        shortDescription.title = 'Show full description';
        fullDescription.title = 'Hide full description';

        if (shortDescription.innerText.endsWith('...')) {
            shortDescription.innerHTML += '&nbsp;<a class="text-primary">Read more</a>';
            fullDescription.innerHTML += '&nbsp;<a class="text-primary">Show less</a>';

            shortDescription.style.cursor = 'pointer';
            fullDescription.style.cursor = 'pointer';

            shortDescription.onclick = showFullDescription;
            fullDescription.onclick = showShortDescription;

            function showFullDescription() {
                fullDescription.classList.replace('d-none', 'd-block');
                shortDescription.classList.replace('d-block', 'd-none');
            }

            function showShortDescription() {
                shortDescription.classList.replace('d-none', 'd-block');
                fullDescription.classList.replace('d-block', 'd-none');
            }
        }
    });
})();