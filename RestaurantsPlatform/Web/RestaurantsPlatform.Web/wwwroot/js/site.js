function showLocalTime () {
    $("time").each(function (i, e) {
        const dateTimeValue = $(e).attr("datetime");
        if (!dateTimeValue) {
            return;
        }

        const time = moment.utc(dateTimeValue).local();
        $(e).html(time.format("llll"));
        $(e).attr("title", $(e).attr("datetime"));
    });
};

showLocalTime();

$(function () {
    let current = location.pathname;

    let isActive = false;
    $('nav div .collapse li a').each(function () {
        let $this = $(this);
        // if the current path is like this link, make it active
        if ($this.attr('href').indexOf(current) !== -1 && current != '/') {
            $this.addClass('active');

            isActive = true;
        }
    })

    if (current.includes('/Identity')) {
        $('.account').addClass('active');

        return;
    }

    if (current != '/' && !isActive) {
        $('.categories').addClass('active');
    }
})