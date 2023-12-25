<template>
  <v-dialog dark :value="active" persistent>
    <v-stepper v-model="step">
      <v-stepper-header>
        <v-stepper-step :complete="step > 1" step="1">
          Select Type
        </v-stepper-step>
        <v-stepper-step :complete="step > 2" step="2">
          Upload Video
        </v-stepper-step>
        <v-divider></v-divider>
        <v-stepper-step :complete="step > 3" step="3">
          Trick Information
        </v-stepper-step>
        <v-divider></v-divider>
        <v-stepper-step step="4">
          Review
        </v-stepper-step>
      </v-stepper-header>
      <v-stepper-items>
        <v-stepper-content step="1">
          <v-card class="mb-2">
            <v-card-text>
              <div class="d-flex flex-column align-center">
                <v-btn class="my-2" @click="setType(uploadType.TRICK)">Trick</v-btn>
                <v-btn class="my-2" @click="setType(uploadType.SUBMISSION)">Submission</v-btn>
              </div>
            </v-card-text>
          </v-card>
        </v-stepper-content>
        <v-stepper-content step="2">
          <v-card class="mb-2">
            <v-card-text>
              <v-file-input label="Trick Video" accept="video/*" @change="handleFile"></v-file-input>
            </v-card-text>
          </v-card>
        </v-stepper-content>
        <v-stepper-content step="3">
          <v-card class="mb-2">
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
        <v-stepper-content step="4">
          <v-card class="mb-2">
            <v-card-text>
              <h1>Success!</h1>
            </v-card-text>
          </v-card>
        </v-stepper-content>
      </v-stepper-items>
    </v-stepper>
    <div class="d-flex justify-center my-5">
      <v-btn @click="toggleActivity">Close</v-btn>
    </div>
  </v-dialog>  
</template>

<script>
import { mapState, mapActions, mapMutations } from 'vuex';
import { UPLOAD_TYPE } from '../data/enum.js';

export default {
  data() {
    return {
      trickName: '',
      uploadType: UPLOAD_TYPE,
    }
  },
  computed: {
    ...mapState('video-upload', ['uploadPromise', 'active', 'type', 'step']),
  },
  methods: {
    ...mapMutations('video-upload', ['setType', 'toggleActivity', 'reset']),
    ...mapActions('video-upload', ['startVideoUpload', 'createTrick']),
    async handleFile(file) {
      if (!file) {
        console.error('Error: file is null.');
        return;
      }

      const formData = new FormData();
      formData.append('video', file);

      this.startVideoUpload({ formData });
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
      this.reset();
    },
  },
}
</script>