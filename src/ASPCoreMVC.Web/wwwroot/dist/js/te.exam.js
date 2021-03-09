$(document).ready(() => {
	$(".replace-count").each(function () {
		var count = 1;
		var paragraph = $(this).html();
		paragraph = paragraph.replace(/{\$\$}/g, function () {
			return count++;
		});
		$(this).html(paragraph);
	});
});