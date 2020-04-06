const workingTimes = document.getElementsByClassName('workingTime');
var today = new Date();

Array.from(workingTimes).forEach(element => {
    const statusElement = element.parentNode.getElementsByClassName('status')[0]
        || document.getElementsByClassName('status')[0];

    statusElement.classList.remove('badge-success');
    statusElement.classList.remove('badge-danger');

    const times = element.innerText.split(' - ').filter(x => x);
    const opening = times[0].split(':').map(x => +x);
    let closing = times[1].split(':').map(x => +x);

    let hoursRightNow = today.getHours();
    const minutesRightNow = today.getMinutes();
    if (opening[0] > closing[0]) {
        closing[0] += 24;
        if (hoursRightNow < opening[0]) {
            hoursRightNow += 24;
        }
    }

    if (hoursRightNow > opening[0]
        && today.getHours() < closing[0]) {
        statusElement.innerText = 'OPEN';
        statusElement.classList.add('badge-success');
    } else if (hoursRightNow === opening[0] && minutesRightNow >= opening[1]) {
        statusElement.innerText = 'OPEN';
        statusElement.classList.add('badge-success');
    } else if (hoursRightNow === closing[0] && minutesRightNow < closing[1]) {
        statusElement.innerText = 'OPEN';
        statusElement.classList.add('badge-success');
    }
    else {
        statusElement.innerText = 'CLOSED';
        statusElement.classList.add('badge-danger');
    }
});