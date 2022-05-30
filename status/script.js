/*

Load the statuses of various services

*/

console.log("Loading service statuses...");

const onlineClass = 'text-success';
const offlineClass = 'text-muted';
const errorClass = 'text-warning';
const uncheckedClass = 'text-info';

const services = [
    {
        "name": "Homework Tracker",
        "url": "https://homeworktrack.serble.net:9897/api",
        "html_element": "homeworkserver",
        "get_status": true,
        "display_response": true
    },
    {
        "name": "Link Shortener",
        "url": "https://link.serble.net/",
        "html_element": "shortlink",
        "get_status": true,
        "display_response": false
    },
    {
        "name": "Searx Instance",
        "url": "https://search.serble.net/",
        "html_element": "searx",
        "get_status": true,
        "display_response": false
    },
    {
        "name": "File Host",
        "url": "https://files.serble.net/",
        "html_element": "filehost",
        "get_status": true,
        "display_response": false
    },
    {
        "name": "NextCloud",
        "url": "https://cloud.serble.net/",
        "html_element": "nextcloud",
        "get_status": true,
        "display_response": false
    },
    {
        "name": "MySQL",
        "url": "https://cloud.serble.net/",
        "html_element": "somethingcool",
        "get_status": true,
        "display_response": false
    }
];

// for each service check if it is online
services.forEach(function(service) {
    if (service.get_status) {
        console.log("Checking " + service.name + "...");
        fetch(service.url)
            .then(function(response) {
                if (response.status === 200) {
                    document.getElementById(service.html_element).getElementsByClassName('status-text')[0].classList.add(onlineClass);
                    // Read response as text
                    if (service.display_response) {
                        response.text().then(function(text) {
                            document.getElementById(service.html_element).getElementsByClassName('status-text')[0].innerText = text;
                        });
                    } else {
                        document.getElementById(service.html_element).getElementsByClassName('status-text')[0].innerText = "Online";
                    }
                } else {
                    document.getElementById(service.html_element).getElementsByClassName('status-text')[0].innerText = "Error: " + response.status;
                    document.getElementById(service.html_element).classList.add(errorClass);
                }
            })
            .catch(function(error) {
                document.getElementById(service.html_element).getElementsByClassName('status-text')[0].innerText = "Offline";
                document.getElementById(service.html_element).getElementsByClassName('status-text')[0].classList.add(offlineClass);
            });
    } else {
        console.log("Not checking " + service.name);
        document.getElementById(service.html_element).getElementsByClassName('status-text')[0].classList.add(uncheckedClass);
        document.getElementById(service.html_element).getElementsByClassName('status-text')[0].innerText = "Not checked";
    }

});

console.log("Done.");
