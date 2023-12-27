<!--suppress ALL-->

<template>
  <div>
    <v-card v-if="haveTricks">
      <v-card-title>
        <span class="headline">Tricks</span>
      </v-card-title>
      <v-card-text>
        <div v-for="trick in tricks" :key="trick.id">
          <v-btn :to="`/tricks/${trick.id}`">{{trick.name}}</v-btn> 
        </div>
      </v-card-text>
    </v-card>
  </div>
</template>

<script>
import { mapState } from 'vuex';

export default {
  computed: {
    ...mapState('tricks', ['tricks']),
    haveTricks() {
      return this.tricks && this.tricks.length > 0;
    },
  },
  async fetch() {
    await this.$store.dispatch('tricks/fetchTricks');
  },
}
</script>
