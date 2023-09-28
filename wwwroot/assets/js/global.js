/*

This file should be included in all pages and contains useful functions.
It should be linked before all other scripts

*/

// Replace all occurrences of the first string with the second string
function replaceAll(str, find, replace) {
    return str.replace(new RegExp(find, 'g'), replace);
}

// http request
function httpSendAsync(theUrl, verb, headers, body, callback) {
    const xmlHttp = new XMLHttpRequest();
    xmlHttp.onreadystatechange = function() {
        if (xmlHttp.readyState != 4) {
            return;
        }
        callback(xmlHttp.responseText, xmlHttp.status);
    }
    xmlHttp.open(verb, theUrl, true);
    headers.forEach((item, i) => {
        xmlHttp.setRequestHeader(item.split(":")[0],item.split(":")[1]);
    });
    xmlHttp.send(body);
}

// Get the value of a cookie
function getCookie(cname) {
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for(let i = 0; i <ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

// Set the value of a cookie
function setCookie(cname, cvalue, exdays) {
    let d = new Date();
    d.setTime(d.getTime() + (exdays*24*60*60*1000));
    let expires = "expires="+ d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

// rgb to hex
function componentToHex(c) {
    const hex = c.toString(16);
    return hex.length == 1 ? "0" + hex : hex;
}
function rgbToHex(r, g, b) {
    // convert r g and b to numbers
    r = parseInt(r);
    g = parseInt(g);
    b = parseInt(b);
    return "#" + componentToHex(r) + componentToHex(g) + componentToHex(b);
}

// hex to rgb
function hexToRgb(hex) {
    const result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
    return result ? {
        r: parseInt(result[1], 16),
        g: parseInt(result[2], 16),
        b: parseInt(result[3], 16)
    } : null;
}

window.blazorExtensions = {

    WriteCookie: function (name, value, days) {

        let expires;
        if (days) {
            const date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            expires = "; expires=" + date.toGMTString();
        }
        else {
            expires = "";
        }
        document.cookie = name + "=" + value + expires + "; path=/";
    }
}

window.getCaptcha = async function () {
    await grecaptcha.ready(function() {});
    return await grecaptcha.execute('XXX', {action: 'formSubmission'});
}

let token = "";

function onReCaptcha(action, functionName) {
    grecaptcha.ready(function() {
        //                  SITE KEY
        grecaptcha.execute('6Le7pVAhAAAAANwwG02GCFHlbKif9yVJwPC0WdQZ', {action: action}).then(function(token) {
            // Add your logic to submit to your backend server here.
            DotNet.invokeMethodAsync('SerbleWebsite', functionName, token);
        });
    });
}

function randomize(collection) {
    var randomNumber = Math.floor(Math.random() * collection.length);
    return collection[randomNumber];
}

// loadScript: returns a promise that completes when the script loads
window.loadScript = function (scriptPath) {
    // check list - if already loaded we can ignore
    if (loaded[scriptPath]) {
        console.log(scriptPath + " already loaded");
        // return 'empty' promise
        return new this.Promise(function (resolve, reject) {
            resolve();
        });
    }

    return new Promise(function (resolve, reject) {
        // create JS library script element
        var script = document.createElement("script");
        script.src = scriptPath;
        script.type = "text/javascript";
        console.log(scriptPath + " created");

        // flag as loading/loaded
        loaded[scriptPath] = true;

        // if the script returns okay, return resolve
        script.onload = function () {
            console.log(scriptPath + " loaded ok");
            resolve(scriptPath);
        };

        // if it fails, return reject
        script.onerror = function () {
            console.log(scriptPath + " load failed");
            reject(scriptPath);
        }

        // scripts will load at end of body
        document["body"].appendChild(script);
    });
}
// store list of what scripts we've loaded
loaded = [];

window.addAttribute = (elementId, attributeName, attributeValue) => {
    document.getElementById(elementId).setAttribute(attributeName, attributeValue);
}

window.cryptoApi = {
    encrypt: async function (plainText, password) {
        const ptUtf8 = new TextEncoder().encode(plainText);
        const pwUtf8 = new TextEncoder().encode(password);
        const pwHash = await window.crypto.subtle.digest('SHA-256', pwUtf8);
        const iv = window.crypto.getRandomValues(new Uint8Array(12));
        const alg = { name: 'AES-GCM', iv: iv };
        const key = await window.crypto.subtle.importKey('raw', pwHash, alg, false, ['encrypt']);
        const ctBuffer = await window.crypto.subtle.encrypt(alg, key, ptUtf8);
        const ctArray = Array.from(new Uint8Array(ctBuffer));
        const ctStr = ctArray.map(byte => String.fromCharCode(byte)).join('');
        const ctBase64 = window.btoa(ctStr);
        const ivHex = Array.from(iv).map(b => ('00' + b.toString(16)).slice(-2)).join('');
        return ivHex + ctBase64;
    },

    decrypt: async function (cipherText, password) {
        const iv = cipherText.slice(0,24).match(/.{2}/g).map(byte => parseInt(byte, 16));
        const ctStr = window.atob(cipherText.slice(24));
        const ctUint8 = new Uint8Array(ctStr.match(/[\s\S]/g).map(ch => ch.charCodeAt(0)));
        const pwUtf8 = new TextEncoder().encode(password);
        const pwHash = await window.crypto.subtle.digest('SHA-256', pwUtf8);
        const alg = { name: 'AES-GCM', iv: new Uint8Array(iv) };
        const key = await window.crypto.subtle.importKey('raw', pwHash, alg, false, ['decrypt']);
        const plainBuffer = await window.crypto.subtle.decrypt(alg, key, ctUint8);
        const plaintext = new TextDecoder().decode(plainBuffer);
        return plaintext;
    }
};

window.unsavedChanges = function () {
    window.onbeforeunload = function () {
        return "You have unsaved changes. Do you really want to leave this page?";
    }
}

window.savedChanges = function () {
    window.onbeforeunload = null;
}