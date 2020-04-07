const formDisplayManipulateBtn = document.getElementById('formToShow');
document.getElementById('showForm').onclick = showInputForm;

formDisplayManipulateBtn.style.display = 'none';

function showInputForm() {
    formDisplayManipulateBtn.style.display = 'block';

    this.innerText = 'Hide image input';
    this.onclick = hideInputForm;
}

function hideInputForm() {
    formDisplayManipulateBtn.style.display = 'none';

    this.innerText = 'Add picture to gallery';
    this.onclick = showInputForm;
}