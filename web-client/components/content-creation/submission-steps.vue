<template>
  <v-card>
    <v-card-title>
      <span class="headline">Create Submission</span>
      <v-spacer></v-spacer>
      <v-btn icon @click="close">
        <v-icon>mdi-close</v-icon>
      </v-btn>
    </v-card-title>
    <v-card-text class="px-0 pb-0">
      <v-stepper v-model="step" class="rounded-0">
        <v-stepper-header class="elevation-0">
          <v-stepper-step :step="submissionStep.UPLOAD_VIDEO" :complete="step > submissionStep.UPLOAD_VIDEO">
            Upload Video
          </v-stepper-step>
          <v-divider></v-divider>
          <v-stepper-step :step="submissionStep.SELECT_TRICK" :complete="step > submissionStep.SELECT_TRICK">
            Select Trick
          </v-stepper-step>
          <v-divider></v-divider>
          <v-stepper-step :step="submissionStep.SUBMISSION_INFORMATION" :complete="step > submissionStep.SUBMISSION_INFORMATION">
            Submission Information
          </v-stepper-step>
          <v-divider></v-divider>
          <v-stepper-step :step="submissionStep.REVIEW">
            Review
          </v-stepper-step>
        </v-stepper-header>
        <v-stepper-items>
          <!-- Upload Video -->
          <v-stepper-content :step="submissionStep.UPLOAD_VIDEO">
            <v-card class="mb-2">
              <v-card-text>
                <v-file-input accept="video/*" @change="handleFile"></v-file-input>
              </v-card-text>
            </v-card>
          </v-stepper-content>
          <!-- Select Trick -->
          <v-stepper-content :step="submissionStep.SELECT_TRICK">
            <v-card class="mb-2">
              <v-card-text>
                <v-select v-model="formData.trickId" :items="trickItems" label="Select Trick"></v-select>
              </v-card-text>
              <v-card-actions>
                <v-spacer/>
                <v-btn color="primary" @click="setStep({ step: submissionStep.SUBMISSION_INFORMATION })">
                  Next
                </v-btn>
              </v-card-actions>
            </v-card>
          </v-stepper-content>
          <!-- Submission Information -->
          <v-stepper-content :step="submissionStep.SUBMISSION_INFORMATION">
            <v-card class="mb-2">
              <v-card-text>
                <v-text-field label="Description" v-model="formData.description" required />
              </v-card-text>
              <v-card-actions>
                <v-spacer/>
                <v-btn color="primary" @click="setStep({ step: submissionStep.REVIEW })">
                  Next
                </v-btn>
              </v-card-actions>
            </v-card>
          </v-stepper-content>
          <!-- Review -->
          <v-stepper-content :step="submissionStep.REVIEW">
            <v-card class="mb-2">
              <v-card-title>
                Review Submission Information
              </v-card-title>
              <v-card-subtitle>
                Here is where you review your submission information. If
                everything looks correct, click the 'Save Submission' button.
              </v-card-subtitle>
              <v-card-text>
                <p>Submission information would go here.</p>
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
    </v-card-text>
  </v-card>
</template>

<script>
import { mapGetters, mapActions, mapMutations } from 'vuex';
import { SUBMISSION_STEP } from '../../data/enum.js';
import { close } from '../../mixins/close.js';

const initState = () => ({
  step: SUBMISSION_STEP.UPLOAD_VIDEO,
  formData: {
    trickId: '',
    video: '',
    description: '',
  },
});

export default {
  name: 'submission-steps',
  mixins: [
    close
  ],
  data: initState,
  computed: {
    ...mapGetters('tricks', ['trickItems']),
    submissionStep() {
      return SUBMISSION_STEP
    },
  },
  methods: {
    ...mapMutations('video-upload', ['hide']),
    ...mapActions('video-upload', ['startVideoUpload', 'createSubmission']),
    setStep({ step }) {
      this.step = step;
    },
    resetData() {
      Object.assign(this.$data, initState());
    },
    async handleFile(file) {
      if (!file) {
        console.error('Error: file is null.');
        return;
      }

      const formData = new FormData();
      formData.append('video', file);

      this.startVideoUpload({ formData });
      this.setStep({ step: this.submissionStep.SELECT_TRICK });
    },
    save() {
      console.log('save', this.formData);
      this.createSubmission({ formData: this.formData });
      this.hide();
    },
  },
}
</script>
