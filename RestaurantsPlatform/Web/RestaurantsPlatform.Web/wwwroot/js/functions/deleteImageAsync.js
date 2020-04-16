(() => {
    const deleteBtns = document.getElementsByClassName('delete');

    Array.from(deleteBtns).forEach(deleteBtn => {
        deleteBtn.addEventListener('click', function (e) {
            e.preventDefault();

            toastr.info('Deleting picture...');
            fetch(this.href)
                .then(response => {
                    if (response.status === 200) {
                        const gallery = document.getElementsByClassName('gallery')[0];
                        gallery.removeChild(this.parentNode.parentNode);

                        toastr.success('Successfully deleted image!');
                    } else {
                        toastr.error('Something went wrong!');
                        console.clear();
                    }
                });
        });
    });
})();