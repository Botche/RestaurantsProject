(() => {
    const likeBtn = document.getElementsByClassName('likeBtn')[0];
    const icon = document.getElementsByClassName('heartIcon')[0];

    likeBtn.style.cursor = 'pointer';
    icon.classList.add('text-danger');

    likeBtn.addEventListener('click', function () {
        const form = document.getElementById('favourites');
        const token = form.querySelector('input[name=__RequestVerificationToken]').value;
        const restaurantId = +form.querySelector('input[name=Id]').value;
        let json = {};

        if (icon.classList.contains('far')) {
            icon.classList.replace('far', 'fa');

            json = { isFavourite: true, restaurantId: restaurantId };
        } else {
            icon.classList.replace('fa', 'far');

            json = { isFavourite: false, restaurantId: restaurantId };
        }

        fetch('/api/FavouriteRestaurants', {
            method: 'POST',
            headers: {
                'X-CSRF-TOKEN': token,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(json)
        })
            .catch(error=>console.log(error));
    });
})();
