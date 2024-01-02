<!--suppress ALL-->

<template>
  <div>
    <v-card v-if="haveTricks">
      <v-card-text>
        <div v-for="trick in tricks" :key="trick.id" class="my-2">
          <v-btn :to="`/trick/${trick.id}`">{{trick.name}}</v-btn> 
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
}
</script>
