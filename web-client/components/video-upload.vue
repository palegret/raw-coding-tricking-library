<template>
  <v-dialog dark v-model="active" persistent>
    <template v-slot:activator="{ on }">
      <v-btn depressed @click="toggleActivity" v-on="on">
        Upload
      </v-btn>
    </template>
    <v-card class="pt-5">
      <v-card-text>
        <v-stepper v-model="step">
          <v-stepper-header>
            <v-stepper-step :complete="step > uploadStep.SELECT_TYPE" :step="uploadStep.SELECT_TYPE">
              Select Type
            </v-stepper-step>
            <v-divider v-if="type === uploadType.TRICK"></v-divider>
            <v-stepper-step v-if="type === uploadType.TRICK" :complete="step > uploadStep.TRICK_INFORMATION" :step="uploadStep.TRICK_INFORMATION">
              Trick Information
            </v-stepper-step>
            <v-divider></v-divider>
            <v-stepper-step :complete="step > uploadStep.UPLOAD_VIDEO" :step="uploadStep.UPLOAD_VIDEO">
              Upload Video
            </v-stepper-step>
            <v-divider></v-divider>
            <v-stepper-step :complete="step > uploadStep.SUBMISSION_INFORMATION" :step="uploadStep.SUBMISSION_INFORMATION">
              Submission Information
            </v-stepper-step>
            <v-divider></v-divider>
            <v-stepper-step :step="uploadStep.REVIEW">
              Review
            </v-stepper-step>
          </v-stepper-header>
          <v-stepper-items>
            <!-- Select Type -->
            <v-stepper-content :step="uploadStep.SELECT_TYPE">
              <v-card class="mb-2">
                <v-card-text>
                  <div class="d-flex flex-column align-center">
                    <v-btn class="my-2" @click="setType({ type: uploadType.TRICK })">Trick</v-btn>
                    <v-btn class="my-2" @click="setType({ type: uploadType.SUBMISSION })">Submission</v-btn>
                  </div>
                </v-card-text>
              </v-card>
            </v-stepper-content>
            <!-- Trick Information -->
            <v-stepper-content :step="uploadStep.TRICK_INFORMATION">
              <v-card class="mb-2">
                <v-card-text>
                  <v-text-field label="Trick Name" v-model="trickName" required />
                </v-card-text>
                <v-card-actions>
                  <v-spacer/>
                  <v-btn color="primary" @click="setStep({ step: uploadStep.UPLOAD_VIDEO })">
                    Save Trick
                  </v-btn>
                </v-card-actions>
              </v-card>
            </v-stepper-content>
            <!-- Upload Video -->
            <v-stepper-content :step="uploadStep.UPLOAD_VIDEO">
              <v-card class="mb-2">
                <v-card-text>
                  <v-file-input accept="video/*" @change="handleFile"></v-file-input>
                </v-card-text>
              </v-card>
            </v-stepper-content>
            <!-- Submission Information -->
            <v-stepper-content :step="uploadStep.SUBMISSION_INFORMATION">
              <v-card class="mb-2">
                <v-card-text>
                  <v-text-field label="Description" v-model="submission" required />
                </v-card-text>
                <v-card-actions>
                  <v-spacer/>
                  <v-btn color="primary" @click="setStep({ step: uploadStep.REVIEW })">
                    Save Submission
                  </v-btn>
                </v-card-actions>
              </v-card>
            </v-stepper-content>
            <!-- Review -->
            <v-stepper-content :step="uploadStep.REVIEW">
              <v-card class="mb-2">
                <v-card-text>
                  <h1>Review</h1>
                </v-card-text>
                <v-card-actions>
                  <v-spacer/>
                  <v-btn color="primary" @click="save">
                    Save
                  </v-btn>
                </v-card-actions>
              </v-card>
            </v-stepper-content>
          </v-stepper-items>
        </v-stepper>
        <div class="d-flex justify-center mt-5">
          <v-btn @click="toggleActivity">Close</v-btn>
        </div>
      </v-card-text>
    </v-card>
  </v-dialog>
</template>

<script>
import { mapState, mapActions, mapMutations } from 'vuex';
import { UPLOAD_TYPE, UPLOAD_STEP } from '../data/enum.js';

export default {
  data() {
    return {
      trickName: '',
      submission: '',
      uploadType: UPLOAD_TYPE,
      uploadStep: UPLOAD_STEP
    }
  },
  computed: {
    ...mapState('video-upload', ['uploadPromise', 'active', 'type', 'step']),
  },
  methods: {
    ...mapMutations('video-upload', ['setType', 'toggleActivity', 'setStep', 'reset']),
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
    async save() {
      if (!this.uploadPromise) {
        console.error('Error: uploadPromise is null.');
        return;
      }

      const video = await this.uploadPromise;

      await this.createTrick({
        trick: {
          name: this.trickName,
        },
        submission: {
          trickId: 1,
          video,
          description: this.submission,
        }
      });

      this.trickName = '';
      this.submission = '';
      this.reset();
    },
  },
}
</script>