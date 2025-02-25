import {computed, inject} from "vue";
import router from "@/router/index.js";

export function getCookie(cname) {
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for(let i = 0; i <ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) === ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) === 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

export function ensureLoggedIn() {
    const userStore = inject('userStore');
    const user = computed(() => userStore.state.user);

    if (!user) {
        router.push("/login");
        console.log("User not logged in, redirecting to login page");
        return null;
    }

    return user;
}

// Set the value of a cookie
export function setCookie(cname, cvalue, exdays) {
    let d = new Date();
    d.setTime(d.getTime() + (exdays*24*60*60*1000));
    let expires = "expires="+ d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

export function getLocalStorage(key) {
    let val = localStorage.getItem(key);
    if (val === undefined) {
        return "";
    }

    return val;
}

export function setLocalStorage(key, val) {
    localStorage.setItem(key, val);
}
