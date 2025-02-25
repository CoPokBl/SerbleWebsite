<script>
import {loginUser} from "@/assets/js/serble.js";
import {inject, computed, ref} from 'vue';
import router from "@/router/index.js";

export default {
  setup() {
    const userStore = inject('userStore');

    const user = computed(() => userStore.state.user);
    const isAdmin = user.permLevel > 1;

    if (userStore.state.user) {
      router.push("/");
      console.log("User is already logged in, redirecting to home page.");
    }

    const error = ref(0);
    return {
      user,
      isAdmin,
      error
    };
  },
  methods: {
    async login() {
      console.log("Logging in...");
      const username = document.getElementById("floatingUsername").value;
      const password = document.getElementById("floatingPassword").value;

      // if (username === "" || password === "") {
      //   this.error = 1;
      //   return;
      // }

      const resp = await loginUser(username, password);
      if (!resp) {
        this.error = 2;
        console.log("Invalid credentials, need account.");
        return false;
      }

      // Success, navigate home, and force reload (to update user info), so don't use router
      window.location = "/";
      console.log("Logged in successfully: ", resp);
      return false;
    },
  },
};
</script>

<template>
  <div class="text-center form-signin">
    <form onsubmit="return false" @onsubmit="login">
      <img class="mb-4" src="/images/icon.png" alt="" width="72" height="72">
      <h1 class="h3 mb-3 fw-normal">{{ $t('sign-in') }}</h1>

      <div style="color: red;">
        <div v-if="error === 0"></div>
        <p v-else-if="error === 1">{{ $t('username-password-required') }}</p>
        <p v-else-if="error === 2">{{ $t('invalid-creds-need-account') }} <a href="/register" onclick="window.location='register'+window.location.search;">{{ $t('register') }}</a></p>
        <p v-else-if="error === 3">{{ $t('account-disabled') }}</p>
      </div>

      <div class="form-floating">
        <input
            type="text"
            class="form-control"
            id="floatingUsername"
            placeholder="{{ $t('username') }}"
        style="background-color: rgb(34, 34, 34); color: #ffffff">
        <label for="floatingUsername">{{ $t('username') }}</label>
      </div>
      <div class="form-floating">
        <input
            type="password"
            class="form-control"
            id="floatingPassword"
            placeholder="{{ $t('password') }}"
        style="background-color: rgb(34, 34, 34); color: #ffffff">
        <label for="floatingPassword">{{ $t('password') }}</label>
      </div>

      <div class="checkbox mb-3">
        <label>
          <input
              type="checkbox"
              value="remember-me"> {{ $t('remember-me') }}
        </label>
      </div>
      <button class="w-100 btn btn-lg btn-primary" @click="login" style="padding-bottom: 10px">{{ $t('sign-in') }}</button>

      <!-- Passkey Stuff, WIP -->
<!--      <button class="w-100 btn btn-lg btn-secondary" style="padding-bottom: 10px; padding-top: 10px" onclick="PasskeyLogin">{{ $t('login-with-passkey') }}</button>-->

      <p>{{ $t('dont-have-account') }} <a href="/register" onclick="window.location='register'+window.location.search;">{{ $t('register-for-free') }}</a></p>
    </form>

    <form onsubmit='console.log("Submitted webauthn field")' hidden>
      <input type="text" name="username" autocomplete="username webauthn" hidden>
    </form>

  </div>
</template>

<style scoped>
html,
body {
  height: 100%;
}

/*max width was 330*/
.form-signin {
  width: 100%;
  max-width: 400px;
  padding: 15px;
  margin: auto;
}

.form-signin .checkbox {
  font-weight: 400;
}

.form-signin .form-floating:focus-within {
  z-index: 2;
}

.form-signin input[type="email"] {
  margin-bottom: -1px;
  border-bottom-right-radius: 0;
  border-bottom-left-radius: 0;
}

.form-signin input[type="password"] {
  margin-bottom: 10px;
  border-top-left-radius: 0;
  border-top-right-radius: 0;
}
</style>