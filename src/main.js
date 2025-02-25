import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import { createI18n } from 'vue-i18n';
import { getCookie } from "@/assets/js/utils.js";
import useUserStore from "@/assets/js/user.js";

import default_lang from './assets/locales/default.json';
import afr from './assets/locales/afr.json';
import bin from './assets/locales/bin.json';
import de from './assets/locales/de.json';
import en from './assets/locales/en.json';
import en_au from './assets/locales/en_au.json';
import en_uk from './assets/locales/en_uk.json';
import en_us from './assets/locales/en_us.json';
import en_pi from './assets/locales/en_pi.json';
import lol from './assets/locales/lol.json';
import pen from './assets/locales/pen.json';
import po from './assets/locales/pol.json';
import es from './assets/locales/es.json';
import tes from './assets/locales/tes.json';
import tok from './assets/locales/tok.json';
import uwu from './assets/locales/uwu.json';

const i18n = createI18n({
    legacy: false,
    locale: getCookie('locale') || 'en', // Set the default locale
    fallbackLocale: 'default', // Set a fallback locale in case the chosen locale is missing translations
    messages: {
        default: default_lang,
        afr,
        bin,
        de,
        en,
        en_au,
        en_uk,
        en_us,
        en_pi,
        lol,
        pen,
        po,
        es,
        tes,
        tok,
        uwu
    },
});

const app = createApp(App);

app.use(i18n);
app.use(router);

const userStore = useUserStore();
userStore.initializeAuth();  // Can be awaited

app.provide('userStore', userStore);

app.mount('#app');
