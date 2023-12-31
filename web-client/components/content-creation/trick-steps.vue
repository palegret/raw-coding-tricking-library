<template>
  <v-card class="pt-0">
    <v-card-text>
      <v-stepper v-model="step">
        <v-stepper-header>
          <v-stepper-step :step="trickStep.TRICK_INFORMATION" :complete="step > trickStep.TRICK_INFORMATION">
            Trick Information
          </v-stepper-step>
          <v-divider></v-divider>
          <v-stepper-step :step="trickStep.REVIEW">
            Review
          </v-stepper-step>
        </v-stepper-header>
        <v-stepper-items>
          <!-- Trick Information -->
          <v-stepper-content :step="trickStep.TRICK_INFORMATION">
            <v-card class="mb-2">
              <v-card-text>
                <v-text-field label="Trick Name" v-model="formData.name" required />
              </v-card-text>
              <v-card-actions>
                <v-spacer/>
                <v-btn color="primary" @click="setStep({ step: trickStep.REVIEW })">
                  Save Trick
                </v-btn>
              </v-card-actions>
            </v-card>
          </v-stepper-content>
          <!-- Review -->
          <v-stepper-content :step="trickStep.REVIEW">
            <v-card class="mb-2">
              <v-card-title>
                Review Trick Information
              </v-card-title>
              <v-card-subtitle>
                Here is where you review your trick information. If everything
                looks correct, click the 'Save Trick' button.
              </v-card-subtitle>
              <v-card-text>
                <p>Trick information would go here.</p>
              </v-card-text>
              <v-card-actions>
                <v-spacer/>
                <v-btn color="primary" @click="save">
                  Save Trick
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
import { mapState, mapActions, mapMutations } from 'vuex';
import { TRICK_STEP } from '../../data/enum.js';

const initState = () => ({
  step: TRICK_STEP.TRICK_INFORMATION,
  formData: {
    name: '',
  },
});

export default {
  name: 'trick-steps',
  data: initState,
  computed: {
    ...mapState('video-upload', ['active']),
    trickStep() { 
      return TRICK_STEP 
    },
  },
  watch: {
    active(newValue) {
      if (!newValue) {
        this.resetData();
      }
    },
  },
  methods: {
    ...mapMutations('video-upload', ['reset']),
    ...mapActions('tricks', ['createTrick']),
    setStep({ step }) {
      this.step = step;
    },
    resetData() {
      Object.assign(this.$data, initState());
    },
    async save() {
      await this.createTrick({ formData: this.formData });
      this.reset();
      this.resetData();
    },
  },
}
</script>
