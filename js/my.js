

jQuery(document).ready(function() {

    jQuery("img.lazy").lazy(
        {
            appendScroll    : window,
            scrollDirection : "both",
            effect: "fadeIn",
            effectTime: 1000,
            delay:500,
            enableQueueing  : true
        }
    );
});