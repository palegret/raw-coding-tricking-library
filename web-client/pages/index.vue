<!--suppress ALL-->

<template>
  <div>
    <v-card v-if="haveTricks">
      <v-card-title>
        <span class="headline">Tricks</span>
      </v-card-title>
      <v-card-text>
        <div v-for="trick in tricks" :key="trick.id">
          <span>
            Trick ID {{trick.id}}: {{trick.name}}
          </span>
        </div>
      </v-card-text>
    </v-card>
    <v-divider v-if="haveTricks" class="my-3"></v-divider>
    <v-card v-if="haveSubmissions">
      <v-card-title>
        <span class="headline">Submissions</span>
      </v-card-title>
      <v-card-text>
        <div v-for="submission in submissions" :key="submission.id">
          <span>
            Trick ID {{submission.trickId}}, Submission ID {{submission.id}}: {{submission.description}}
          </span>
          <div>
            <video
              :src="`http://localhost:5000/api/videos/${submission.video}`"
              width="400"
              controls
            ></video>
          </div>
        </div>
      </v-card-text>
    </v-card>
  </div>
</template>

<script>
import { mapState } from 'vuex';

export default {
  computed: {
    ...mapState('tricks', ['tricks']),
    ...mapState('submissions', ['submissions']),
    haveTricks() {
      return this.tricks && this.tricks.length > 0;
    },
    haveSubmissions() {
      return this.submissions && this.submissions.length > 0;
    },
  },
}
</script>
