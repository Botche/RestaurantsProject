(() => {
    const deleteBtn = document.getElementsByClassName('delete')[0];

    deleteBtn.addEventListener('click', function (e) {
        e.preventDefault();

        toastr.info('Deleting picture...');
        fetch('account/deleteImage?userName=' + document.getElementsByClassName('username')[0].innerText)
            .then(response => {
                if (response.status === 200) {
                    toastr.success('Successfully deleted profile picture!');

                    return response.text();
                } else {
                    toastr.error('Something went wrong!');
                    console.clear();
                    console.log(response);
                }
            })
            .then(text => this.parentNode.parentNode.getElementsByTagName('img')[0].src = text);
    });
})();