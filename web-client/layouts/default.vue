<template>
  <v-app dark>
    <v-app-bar app dense>
      <v-toolbar-title>
        <nuxt-link class="text-h5 text--primary text-decoration-none" to="/">Tricking Library</nuxt-link>
      </v-toolbar-title>
      <v-spacer></v-spacer>
      <content-creation-dialog />
    </v-app-bar>
    <v-main>
      <v-container>
        <nuxt />
      </v-container>
    </v-main>
  </v-app>
</template>

<script>
import ContentCreationDialog from '../components/content-creation/content-creation-dialog';

export default {
  name: 'DefaultLayout',
  components: {
    ContentCreationDialog,
  },
}
</script>
