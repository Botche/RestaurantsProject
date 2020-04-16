(() => {
    const uploadPictureBtn = document.getElementById('uploadPicture');

    uploadPictureBtn.addEventListener('click', function () {
        try {
            new URL(document.getElementById('ImageUrl').value);
            toastr.info('Uploading picture...');
        } catch (_) {
        }
    });
})();