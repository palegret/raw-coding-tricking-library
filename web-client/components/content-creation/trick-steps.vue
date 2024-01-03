<template>
  <v-card class="pt-0 elevation-1">
    <v-card-title>
      <span class="headline">Create Trick</span>
    </v-card-title>
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
import { mapState, mapGetters, mapMutations, mapActions } from 'vuex';
import { TRICK_STEP } from '../../data/enum.js';

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
      this.resetData();
    },
  },
}
</script>
