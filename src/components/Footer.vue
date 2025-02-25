<script setup>

import LanguageChanger from "@/components/LanguageChanger.vue";
</script>

<template>
  <div>
    <div class="text-center col-lg-6 offset-lg-3">
      <h4>{{ $t('serble-site') }} </h4>
      <p>&copy; 2020-{{ new Date().getFullYear() }} &middot; CoPokBl &middot; <a href="/">{{ $t('serble') }}</a></p>
      <LanguageChanger/>
    </div>
  </div>
</template>

<style scoped>

</style>