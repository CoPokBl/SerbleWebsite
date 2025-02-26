<script>
import { inject, computed } from 'vue';

export default {
  setup() {
    const userStore = inject('userStore');

    const user = computed(() => userStore.state.user);
    const isAdmin = user.permLevel > 1;

    return {
      user,
      isAdmin,
      userStore
    };
  },
  methods: {
    logout() {
      console.log("Logging out...");
      this.userStore.logout();
      window.location = "/";
    }
  }
};
</script>

<template>
  <nav class="navbar navbar-expand-lg navbar-dark bg-dark sticky-top text-light">
    <a class="navbar-brand" href="/" style="padding-left: 20px"><img src="/images/icon.png" width="41" height="41" alt="Serble Logo"></a>
    <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
      <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="navbarSupportedContent">
      <ul class="navbar-nav mr-auto">

        <li class="nav-item">
          <RouterLink class="nav-link" to="/">{{ $t('home') }}</RouterLink>
        </li>

        <li class="nav-item">
          <a class="nav-link" href="https://status.serble.net">{{ $t('status') }}</a>
        </li>

        <li class="nav-item">
          <RouterLink class="nav-link" to="/store">{{ $t('store') }}</RouterLink>
        </li>

        <li class="nav-item dropdown">
          <a class="nav-link dropdown-toggle" href="#" id="gameDrop" role="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">{{ $t('games') }}</a>
          <div class="dropdown-menu" aria-labelledby="gameDrop">
            <RouterLink class="dropdown-item" to="/wordmaster">{{ $t('word-master') }}</RouterLink>
          </div>
        </li>

        <li class="nav-item dropdown">
          <a class="nav-link dropdown-toggle" href="#" id="infoDrop" role="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">{{ $t('info') }}</a>
          <div class="dropdown-menu" aria-labelledby="infoDrop">
            <RouterLink class="dropdown-item" to="/discord">{{ $t('discord') }}</RouterLink>
            <RouterLink class="dropdown-item" to="/contact">{{ $t('contact') }}</RouterLink>
          </div>
        </li>

        <li class="nav-item dropdown">
          <a class="nav-link dropdown-toggle" href="#" id="vaultDrop" role="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">{{ $t('vault') }}</a>
          <div class="dropdown-menu" aria-labelledby="vaultDrop">
            <RouterLink class="dropdown-item" to="/notes">{{ $t('notes') }}</RouterLink>
          </div>
        </li>
      </ul>
    </div>

    <div v-if="user">
      <div class="dropdown text-end" style="padding-right: 180px;">
        <a href="#" class="d-block link-secondary text-decoration-none dropdown-toggle" id="dropdownUser1" data-bs-toggle="dropdown" aria-expanded="false">
          {{ user.username }}
<!--          <img src="https://github.com/mdo.png" alt="mdo" width="32" height="32" class="rounded-circle">-->
        </a>
        <ul class="dropdown-menu text-small" aria-labelledby="dropdownUser1">
          <li><RouterLink class="dropdown-item" to="/oauthapps">{{ $t('my-applications') }}</RouterLink></li>
          <li><RouterLink class="dropdown-item" to="/authorizedapps">{{ $t('authorized-applications') }}</RouterLink></li>
          <li><RouterLink class="dropdown-item" to="/account">{{ $t('account') }}</RouterLink></li>
          <li><RouterLink class="dropdown-item" to="/account/paymentportal">{{ $t('manage-payments') }}</RouterLink></li>
          <li v-if="isAdmin"><RouterLink class="dropdown-item" to="/admin">{{ $t('admin-dashboard') }}</RouterLink></li>
          <li><hr class="dropdown-divider"></li>
          <li><button class="dropdown-item" type="button" @click="logout">{{ $t('logout') }}</button></li>
        </ul>
      </div>
    </div>
    <div v-else class="dropdown text-end" style="padding-right: 180px;">
      <RouterLink to="/login"><button type="button" class="btn btn-outline-light">{{ $t('login') }}</button></RouterLink>
    </div>
  </nav>
</template>

<style scoped>

</style>