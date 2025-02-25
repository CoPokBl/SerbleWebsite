<script>
import {useI18n} from "vue-i18n";
import {setCookie} from "@/assets/js/utils.js";

export default {
  setup() {
    const { unused, locale } = useI18n({ useScope: 'global' });
    return { locale };
  },
  methods: {
    changeLanguage(lang) {
      console.log("setting language to: ", lang);

      this.locale = lang;
      setCookie("locale", lang, 9999);
    }
  }
}
</script>

<template>
  <div class="dropdown">
    <button class="dropdown-toggle btn-link btn" type="button" data-bs-toggle="dropdown" aria-expanded="false">{{ $t('language') }}</button>
    <div class="dropdown-menu">
      <button class="dropdown-item" @click="changeLanguage('en')">English</button>
      <button class="dropdown-item" @click="changeLanguage('tok')">toki pona</button>
      <button class="dropdown-item" @click="changeLanguage('po')">Polish</button>
      <button class="dropdown-item" @click="changeLanguage('bin')">Binary</button>
      <button class="dropdown-item" @click="changeLanguage('de')">German</button>
      <button class="dropdown-item" @click="changeLanguage('es')">Spanish</button>
      <button class="dropdown-item" @click="changeLanguage('en_uk')">English (UK)</button>
      <button class="dropdown-item" @click="changeLanguage('en_au')">English (Australia)</button>
      <button class="dropdown-item" @click="changeLanguage('en_us')">English (US)</button>
      <button class="dropdown-item" @click="changeLanguage('afr')">Afrikaans</button>
      <button class="dropdown-item" @click="changeLanguage('lol')">LOL Cat</button>
    </div>
  </div>
</template>

<style scoped>

</style>