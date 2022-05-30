/*

Load the statuses of various services

*/

console.log("Loading services page...");

const onlineClass = 'text-success';
const offlineClass = 'text-danger';
const errorClass = 'text-warning';
const uncheckedClass = 'text-info';

const onlineIcon = 'bg-success';
const offlineIcon = 'bg-danger';
const errorIcon = 'bg-warning';
const uncheckedIcon = 'bg-info';

const services = [
    {
        "name": "Homework Tracker",
        "description": "My Homework Tracker project is an API made in ASP.NET that allows users to track their homework. There are multiple " +
            "clients that I made for it, you can view them in the repos page, just click the button below.",
        "button": "GitHub",
        "button_link": "https://github.com/CoPokBl/HomeworkTrackerServer",
        "url": "https://homeworktrack.serble.net:9897/api",
        "get_status": true,
        "display_response": true
    },
    {
        "name": "Link Shortener",
        "description": "This is a very awesome link shortener that allows you to shorten any link with a custom name for free. " +
            "It's also a very simple to use, just click a button and boom, you have a link! You can check it out " +
            "with the button below. I made it in JavaScript using Cloudflare's free workers and key value storage.",
        "button": "Website",
        "button_link": "https://link.serble.net",
        "url": "https://link.serble.net/",
        "get_status": true,
        "display_response": false
    },
    {
        "name": "Searx Instance",
        "description": "I made a Searx instance for me and anyone who wants to use it. Searx is a search engine that gives you " +
            "complete privacy while still giving you accurate results by taking them from multiple other search engines. " +
            "Go ahead and try it out by clicking the button below.",
        "button": "Website",
        "button_link": "https://search.serble.net/",
        "url": "https://search.serble.net/",
        "get_status": true,
        "display_response": false
    },
    {
        "name": "File Host",
        "description": "My Homework Tracker project is an API made in ASP.NET that allows users to track their homework. There are multiple " +
            "clients that I made for it, you can view them in the repos page, just click the button below.",
        "button": "Website",
        "button_link": "https://files.serble.net/",
        "url": "https://files.serble.net/",
        "get_status": true,
        "display_response": false
    },
    {
        "name": "NextCloud",
        "description": `This is a very awesome link shortener that allows you to shorten any link with a custom name for free. 
            It's also a very simple to use, just click a button and boom, you have a link! You can check it out 
            with the button below. I made it in JavaScript using Cloudflare's free workers and key value storage.`,
        "button": "Website",
        "button_link": "https://cloud.serble.net/",
        "url": "https://cloud.serble.net/",
        "get_status": true,
        "display_response": false
    },
    {
        "name": "MySQL",
        "description": "This is just the MySQL server powering all of these projects. Nothing special.",
        "button": "Info",
        "button_link": "https://www.mysql.com/",
        "url": "https://cloud.serble.net/",
        "get_status": true,
        "display_response": false
    }
];

// Inject each service into the DOM
console.log("Injecting services into DOM...");
let inRow = 0;
let servicesHTML = "";
services.forEach(function(service) {
    console.log("Injecting service: " + service.name);
    // Create the element
    let injectText = ``;
    if (inRow === 0) {
        console.log("Creating new row")
        injectText += `<div class="row align-items-md-stretch" style="padding-left: 100px; padding-right: 100px">`;
    }
    injectText += `
        <div class="col-md-6 text-light">
            <div class="h-100 p-5 bg-dark rounded-3" id="${service.name}">
                <h2 class="service-heading">${service.name}</h2>
                <p>
                    ${service.description}
                </p>
                <div class="" style="display: flex; gap: 5px;">
                    <div class="status-circle" style="width: 12px; height: 12px; border-radius: 12px; margin-top: 5px"></div>
                    <p class="status-text">Loading status...</p>
                </div>
                <p><a class="btn btn-outline-light" href="${service.button_link}" role="button">${service.button} &raquo;</a></p>
            </div>
        </div>
    `;
    if (inRow === 1) {
        injectText += `</div><br>`;
    }
    inRow++;
    if (inRow === 2) {
        inRow = 0;
    }
    servicesHTML += injectText;
});
document.getElementById('services').innerHTML = servicesHTML;

// for each service check if it is online
console.log("Checking services...");
services.forEach(function(service) {
    if (service.get_status) {
        console.log("Checking " + service.name + "...");
        fetch(service.url)
            .then(function(response) {
                if (response.status === 200) {
                    document.getElementById(service.name).getElementsByClassName('status-text')[0].classList.add(onlineClass);
                    document.getElementById(service.name).getElementsByClassName('status-circle')[0].classList.add(onlineIcon);
                    // Read response as text
                    if (service.display_response) {
                        response.text().then(function(text) {
                            document.getElementById(service.name).getElementsByClassName('status-text')[0].innerText = text;
                        });
                    } else {
                        document.getElementById(service.name).getElementsByClassName('status-text')[0].innerText = "Online";
                    }
                } else {
                    document.getElementById(service.name).getElementsByClassName('status-text')[0].innerText = "Error: " + response.status;
                    document.getElementById(service.name).getElementsByClassName('status-text')[0].classList.add(errorClass);
                    document.getElementById(service.name).getElementsByClassName('status-circle')[0].classList.add(errorIcon);
                }
            })
            .catch(function() {
                document.getElementById(service.name).getElementsByClassName('status-text')[0].innerText = "Offline";
                document.getElementById(service.name).getElementsByClassName('status-text')[0].classList.add(offlineClass);
                document.getElementById(service.name).getElementsByClassName('status-circle')[0].classList.add(offlineIcon);
            });
    } else {
        console.log("Not checking " + service.name);
        document.getElementById(service.name).getElementsByClassName('status-text')[0].classList.add(uncheckedClass);
        document.getElementById(service.name).getElementsByClassName('status-text')[0].innerText = "Not checked";
    }

});

console.log("Done.");
