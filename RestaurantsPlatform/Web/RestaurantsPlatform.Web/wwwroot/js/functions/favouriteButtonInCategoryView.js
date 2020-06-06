(() => {
    const likesBtn = document.getElementsByClassName('likeBtn');

    Array.from(likesBtn).forEach(likeBtn => {

        const icon = likeBtn.getElementsByTagName('i')[0];

        icon.classList.add('text-danger');

        likeBtn.addEventListener('mouseover', function () {
            icon.classList.replace('text-danger', 'text-white');
        });
        likeBtn.addEventListener('mouseout', function () {
            icon.classList.replace('text-white', 'text-danger');
        });
        likeBtn.addEventListener('click', function () {
            const form = likeBtn.parentNode.querySelector('#favourites');
            const token = form.querySelector('input[name=__RequestVerificationToken]').value;
            const restaurantId = +form.querySelector('input[name=Id]').value;
            let json = {};

            if (icon.classList.contains('fa')) {
                icon.classList.replace('fa', 'far');
                json = { isFavourite: false, restaurantId: restaurantId };
            } else {
                icon.classList.replace('far', 'fa');
                json = { isFavourite: true, restaurantId: restaurantId };
            }

            fetch('/api/FavouriteRestaurants', {
                method: 'POST',
                headers: {
                    'X-CSRF-TOKEN': token,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(json)
            });
        });
    });
})();