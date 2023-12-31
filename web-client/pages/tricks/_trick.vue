<!--suppress ALL-->

<template>
  <div class="d-flex justify-center align-start">
    <div v-if="haveSubmissions" class="mx-2">
      <div v-for="submission in submissions" :key="submission.id">
        <span>
          Submission ID {{submission.id}}: {{submission.description}}
        </span>
        <div>
          <video :src="`http://localhost:5000/api/videos/${submission.video}`" width="400" controls></video>
        </div>
      </div>
    </div>
    <div class="mx-2 sticky">
      <span>
        Trick: {{ trickId }}
      </span>
    </div>
  </div>
</template>

<script>
import { mapState } from 'vuex';

export default {
  name: 'TrickComponent',
  computed: {
    ...mapState('submissions', ['submissions']),
    haveSubmissions() {
      return this.submissions && this.submissions.length > 0;
    },
    trickId() {
      return this.$route.params.trick;
    },
  },
  async fetch() {
    await this.$store.dispatch('submissions/fetchTrickSubmissions', { trickId: this.trickId }), { root: true };
  },
};
</script>

<style scoped>
.sticky {
  position: -webkit-sticky;
  position: sticky;
  top: 48px;
}
</style>
