<script>
export default {
  computed: {
    welcome() {
      return this.$t('welcome-to-serble').replace("[", "<\a href='/contact'>").replace("]", "<\/a>");
    },
    lookingformc() {
      return this.$t('looking-for-mc-server').replace("[", "<\a href='https://old.serble.net'>").replace("]", "<\/a>");
    }
  }
}
</script>

<template>
  <main>
    <div class="jumbotron jumbotron-fluid text-center" style="background-color: #333339;color: #ffffff">
      <h1 class="display-4">{{ $t('serble') }}</h1>
      <p class="lead" style="padding-left: 100px; padding-right: 100px" v-html="welcome"></p>
      <br>
      <p class="lead" style="padding-left: 100px; padding-right: 100px" v-html="lookingformc"></p>
      <hr class="my-4">
      <p class="lead" style="padding-bottom: 10px">
        <a class="btn btn-primary btn-lg" href="/discord/" role="button" style="margin-right: 5px;">{{ $t('serble-discord') }}</a>
        <a class="btn btn-primary btn-lg" href="https://status.serble.net" role="button" style="margin-left: 5px;">{{ $t('uptime-monitor') }}</a>
      </p>
    </div>
  </main>
</template>
