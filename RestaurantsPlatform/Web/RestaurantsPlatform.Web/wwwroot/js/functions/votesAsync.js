function voteAsync () {
    const upvoteBtns = document.getElementsByClassName('upvote');
    const downvoteBtns = document.getElementsByClassName('downvote');

    Array.from(upvoteBtns).forEach(upvoteBtn => {
        upvoteBtn.style.cursor = 'pointer';
        upvoteBtn.addEventListener('click', upvote);
    })
    Array.from(downvoteBtns).forEach(downvoteBtn => {
        downvoteBtn.style.cursor = 'pointer';
        downvoteBtn.addEventListener('click', downvote);
    })

    function upvote(e) {
        e.preventDefault();
        const token = this.parentNode.parentNode.getElementsByTagName('input')[1].value;
        const commentId = this.parentNode.parentNode.getElementsByTagName('input')[0].value;
        const isUpVote = true;
        const json = { commentId: commentId, isUpVote: isUpVote };

        const votes = this.parentNode.parentNode.getElementsByClassName('votes')[0];
        const upvote = this.parentNode.parentNode.getElementsByClassName('upvote')[0].firstElementChild;
        const downvote = this.parentNode.parentNode.getElementsByClassName('downvote')[0].firstElementChild;
        fetch('/api/votes', {
            method: 'POST',
            headers: {
                'X-CSRF-TOKEN': token,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(json)
        })
            .then(response => {
                if (response.ok) {
                    return response.json();
                } else {
                    toastr.error('Something went wrong!');
                }
            })
            .then(json => {
                if (upvote.classList.contains('fa-arrow-up')) {
                    upvote.classList.replace('fa-arrow-up', 'fa-arrow-circle-up');
                    if (downvote.classList.contains('fa-arrow-circle-down')) {
                        downvote.classList.replace('fa-arrow-circle-down', 'fa-arrow-down');
                    }
                } else {
                    upvote.classList.replace('fa-arrow-circle-up', 'fa-arrow-up');
                }
                votes.innerText = json.votesCount
            });
    }

    function downvote(e) {
        e.preventDefault();
        const token = this.parentNode.parentNode.getElementsByTagName('input')[1].value;
        const commentId = this.parentNode.parentNode.getElementsByTagName('input')[0].value;
        const isUpVote = false;
        const json = { commentId: commentId, isUpVote: isUpVote };

        const upvote = this.parentNode.parentNode.getElementsByClassName('upvote')[0].firstElementChild;
        const downvote = this.parentNode.parentNode.getElementsByClassName('downvote')[0].firstElementChild;
        const votes = this.parentNode.parentNode.getElementsByClassName('votes')[0];
        fetch('/api/votes', {
            method: 'POST',
            headers: {
                'X-CSRF-TOKEN': token,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(json)
        })
            .then(response => {
                if (response.ok) {
                    return response.json();
                } else {
                    toastr.error('Something went wrong!');
                }
            })
            .then(json => {
                if (downvote.classList.contains('fa-arrow-down')) {
                    downvote.classList.replace('fa-arrow-down', 'fa-arrow-circle-down');
                    if (upvote.classList.contains('fa-arrow-circle-up')) {
                        upvote.classList.replace('fa-arrow-circle-up', 'fa-arrow-up');
                    }
                } else {
                    downvote.classList.replace('fa-arrow-circle-down', 'fa-arrow-down');
                }
                votes.innerText = json.votesCount
            });
    }
};

voteAsync();