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
      <v-dialog v-model="showDialog" width="500">
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
                <v-file-input label="Trick Video" accept="video/*" @change="handleFile"></v-file-input>
              </v-card-text>
            </v-card>
          </v-stepper-content>
          <v-stepper-content step="2">
            <v-card class="mb-12" height="200px">
              <v-card-text>
                <v-text-field label="Trick Name" v-model="trickName" required />
              </v-card-text>
              <v-card-actions>
                <v-spacer/>
                <v-btn color="primary" @click="saveTrick">
                  Save Trick
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
    </v-dialog>
  </div>
</template>

<script>
import { mapState, mapActions, mapMutations } from 'vuex';

export default {
  data() {
    return {
      trickName: '',
      step: 1,
      showDialog: false,
    }
  },
  computed: {
    ...mapState('tricks', ['tricks']),
    ...mapState('videos', ['uploadPromise']),
    haveTricks() {
      return this.tricks && this.tricks.length > 0;
    }
  },
  methods: {
    ...mapMutations('videos', {
      resetVideos: 'reset',
    }),
    ...mapActions('videos', ['startVideoUpload', 'createTrick']),
    async handleFile(file) {
      if (!file) {
        console.error('Error: file is null.');
        return;
      }

      const formData = new FormData();
      formData.append('video', file);

      this.startVideoUpload({ formData });
      this.step = 2;
    },
    async saveTrick() {
      if (!this.uploadPromise) {
        console.error('Error: uploadPromise is null.');
        return;
      }

      const video = await this.uploadPromise;

      await this.createTrick({
        trick: {
          name: this.trickName,
          video
        }
      });

      this.trickName = '';
      this.step = 3;
      this.resetVideos();
    },
  },
}
</script>
