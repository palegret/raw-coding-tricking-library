<template>
  <v-card>
    <v-card-title>
      <span class="headline">Create Trick</span>
      <v-spacer></v-spacer>
      <v-btn icon @click="close">
        <v-icon>mdi-close</v-icon>
      </v-btn>
    </v-card-title>
    <v-card-text class="px-0 pb-0">
      <v-stepper v-model="step" class="rounded-0">
        <v-stepper-header class="elevation-0">
          <v-stepper-step :step="trickStep.TRICK_INFORMATION" :complete="step > trickStep.TRICK_INFORMATION">
            Trick Information
          </v-stepper-step>
          <v-divider></v-divider>
          <v-stepper-step :step="trickStep.REVIEW">
            Review
          </v-stepper-step>
        </v-stepper-header>
        <v-stepper-items class="fpt-0">
          <!-- Trick Information -->
          <v-stepper-content :step="trickStep.TRICK_INFORMATION">
            <v-card class="mb-2">
              <v-card-text>
                <v-row dense>
                  <v-col>
                    <v-text-field
                      v-model="formData.name"
                      label="Name"
                      required
                    />
                  </v-col>
                  <v-col>
                    <v-select
                      v-model="formData.difficulty"
                      :items="difficultyItems"
                      label="Difficulty"
                    ></v-select>
                  </v-col>
                </v-row>
                <v-row dense>
                  <v-col>
                    <v-text-field
                      v-model="formData.description"
                      label="Description"
                      required
                    />
                  </v-col>
                </v-row>
                <v-row dense>
                  <v-col>
                    <v-select
                      v-model="formData.categories"
                      :items="categoryItems"
                      label="Categories"
                      multiple chips small-chips deletable-chips
                    ></v-select>
                  </v-col>
                </v-row>
                <v-row dense>
                  <v-col>
                    <v-select
                      v-model="formData.prerequisites"
                      :items="testData"
                      label="Prerequisites"
                      multiple chips small-chips deletable-chips
                    ></v-select>
                  </v-col>
                  <v-col>
                    <v-select
                      v-model="formData.progressions"
                      :items="testData"
                      label="Progressions"
                      multiple chips small-chips deletable-chips
                    ></v-select>
                  </v-col>
                </v-row>
              </v-card-text>
              <v-card-actions>
                <v-spacer/>
                <v-btn color="primary" @click="setStep({ step: trickStep.REVIEW })">
                  Next
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
import { mapGetters, mapActions } from 'vuex';
import { TRICK_STEP } from '../../data/enum.js';
import { close } from '../../mixins/close.js';

const initState = () => ({
  step: TRICK_STEP.TRICK_INFORMATION,
  formData: {
    name: '',
    description: '',
    difficulty: '',
    prerequisites: [],
    progressions: [],
    categories: [],
  },
  testData: [
    { text: 'Foo', value: 1 },
    { text: 'Bar', value: 2 },
    { text: 'Baz', value: 3 },
  ],
});

export default {
  name: 'trick-steps',
  mixins: [
    close
  ],
  data: initState,
  computed: {
    ...mapGetters('tricks', ['trickItems', 'categoryItems', 'difficultyItems']),
    trickStep() {
      return TRICK_STEP
    },
  },
  methods: {
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
    },
  },
}
</script>
