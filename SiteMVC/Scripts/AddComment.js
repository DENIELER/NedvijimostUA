function AddCommentSuccess(data) {
    var add_comment_container = $('#advertisment_' + data.AdvertismentID + ' .add_comment_container');
    var add_comment = $('#advertisment_' + data.AdvertismentID + ' .add_comment');
    add_comment_container.fadeOut(300);
    add_comment.empty();

    var commentsContrainer = $('#advertisment_' + data.AdvertismentID + ' .comments_container');
    var newComment = '<div class="comment row-fluid new_comment' + data.AdvertismentID + '" style="display: none;">';
    newComment += '<div class="left span4">';
    newComment += data.DisplayLogin;
    newComment += '<span class="date muted" style="font-size: 0.8em;">' + data.CreateDateDisplay + '</span>';
    newComment += '</div>';
    newComment += '<div class="right span8">';
    newComment += data.Message;
    newComment += '</div>';
    newComment += '</div>';

    commentsContrainer.append(newComment);
    $('.new_comment' + data.AdvertismentID).fadeIn(300);
}