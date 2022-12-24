
function fixerUpper() {
    
    // Find all anchors with a class of include-query and add the current query string to the href
    const anchors = document.getElementsByClassName("include-query");
    for (let i = 0; i < anchors.length; i++) {
        const anchor = anchors[i];
        const href = anchor.getAttribute("href");
        console.log(href + "->" + href + window.location.search);
        anchor.setAttribute("href", href + window.location.search);
    }
    
    console.log("Fixed up everything :)");
}
