<!--suppress ALL-->

<template>
  <div>
    <v-card v-if="haveTricks">
      <v-card-title>
        <span class="headline">Tricks</span>
      </v-card-title>
      <v-card-text>
        <div v-for="trick in tricks" :key="trick.id">
          {{trick.name}}
          <div>
            <video
              :src="`http://localhost:5000/api/videos/${trick.video}`"
              width="400"
              controls
            ></video>
          </div>
        </div>
      </v-card-text>
    </v-card>
    <v-divider v-if="haveTricks" class="my-3"></v-divider>
  </div>
</template>

<script>
import { mapState } from 'vuex';

export default {
  computed: {
    ...mapState('tricks', ['tricks']),
    haveTricks() {
      return this.tricks && this.tricks.length > 0;
    }
  },
}
</script>
