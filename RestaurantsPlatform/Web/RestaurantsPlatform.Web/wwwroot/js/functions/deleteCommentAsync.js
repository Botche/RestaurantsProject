function deleteCommentAsync () {
    const deleteBtns = document.getElementsByClassName('delete-comment');

    Array.from(deleteBtns).forEach(deleteBtn => {
        deleteBtn.addEventListener('click', function (e) {
            e.preventDefault();
            const href = this.href;

            fetch(href)
                .then(response => {
                    if (response.ok) {
                        toastr.success('Successfully deleted comment!');
                        const elementToRemove = this.parentNode.parentNode;
                        elementToRemove.parentNode.removeChild(elementToRemove);
                    } else {
                        toastr.error('Something went wrong!');
                    }
                });
        });
    });
};

deleteCommentAsync();