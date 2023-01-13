window.getCultureLang = () => {
    return window.localStorage['lang']
};

let languageSet = (localStorage.getItem('lang') || 'en');
console.log("Language: " + languageSet);
// Blazor.start({
//     applicationCulture: languageSet
// });