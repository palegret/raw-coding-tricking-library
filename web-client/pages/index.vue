<!--suppress ALL-->

<template>
  <div>
    <v-card v-if="haveTricks">
      <v-card-title>
        <span class="headline">Tricks</span>
      </v-card-title>
      <v-card-text>
        <ul>
          <li v-for="trick in tricks" :key="trick.id">
            {{trick.name}}
          </li>
        </ul>
      </v-card-text>
    </v-card>
    <v-divider v-if="haveTricks" class="my-3"></v-divider>
    <v-stepper v-model="step">
      <v-stepper-header>
        <v-stepper-step :complete="step > 1" step="1">
          Upload Video
        </v-stepper-step>
        <v-divider></v-divider>
        <v-stepper-step :complete="step > 2" step="2">
          Trick Information
        </v-stepper-step>
        <v-divider></v-divider>
        <v-stepper-step step="3">
          Confirmation
        </v-stepper-step>
      </v-stepper-header>
      <v-stepper-items>
        <v-stepper-content step="1">
          <v-card class="mb-12" height="200px">
            <v-card-text>
              <v-file-input accept="video/*" @change="handleFile"></v-file-input>
            </v-card-text>
          </v-card>
        </v-stepper-content>
        <v-stepper-content step="2">
          <v-card class="mb-12" height="200px">
            <v-card-text>
              <v-text-field
                label="Trick name"
                v-model="trickName"
                required
              />
            </v-card-text>
            <v-card-actions>
              <v-spacer/>
              <v-btn color="primary" @click="saveTrick">
                Save Trick
              </v-btn>
              <v-btn @click="resetTricks" class="ml-1">
                Reset Tricks
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-stepper-content>
        <v-stepper-content step="3">
          <v-card class="mb-12" height="200px">
            <v-card-text>
              <h1>Success!</h1>
            </v-card-text>
          </v-card>
        </v-stepper-content>
      </v-stepper-items>
    </v-stepper>
  </div>
</template>

<script>
import Axios from 'axios';
import { mapState, mapActions, mapMutations } from 'vuex';

export default {
  data() {
    return {
      trickName: '',
      step: 1,
    }
  },
  computed: {
    ...mapState('tricks', {
      tricks: state => state.tricks
    }),
    haveTricks() {
      return this.tricks && this.tricks.length > 0;
    }
  },
  methods: {
    ...mapMutations('tricks', {
      resetTricks: 'reset',
    }),
    ...mapActions('tricks', ['createTrick']),
    async saveTrick() {
      await this.createTrick({ trick: { name: this.trickName }});
      this.trickName = '';
    },
    async handleFile(file) {
      if (!file)
        return;

      console.log(file);

      const formData = new FormData();
      formData.append('video', file);

      const result = await Axios.post('http://localhost:5000/api/videos', formData);

      //this.$store.dispatch('uploadFile', this.$refs.file.files[0]);
    }
  },
}
</script>
