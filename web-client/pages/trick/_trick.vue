<!--suppress ALL-->

<template>
  <div class="d-flex justify-center align-start mt-2">
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
    <v-sheet v-if="trick" class="mx-2 pa-3 sticky">
      <p class="text-h6 ma-0">
        Trick: {{ trick.name }}
      </p>
      <v-divider class="my-1"></v-divider>
      <p class="text-body-2 ma-0">
        {{ trick.description }}
      </p>
      <p class="text-body-2 ma-0">
        {{ trick.difficulty }}
      </p>
      <v-divider class="my-1"></v-divider>
      <div v-for="(rd, i) in relatedData" :key="rd.rowKey(i)">
        <div v-if="rd.data.length">
          <p class="text-subtitle-1 ma-0">
            {{ rd.title }}
          </p>
          <v-chip-group>
            <v-chip small v-for="d in rd.data" :key="rd.dataKey(d)" :to="rd.dataRoute(d)">
              {{ d.name }}
            </v-chip>
          </v-chip-group>
        </div>
      </div>
    </v-sheet>
  </div>
</template>

<script>
import { mapState, mapGetters } from 'vuex';

export default {
  name: 'TrickPage',
  computed: {
    ...mapState('submissions', ['submissions']),
    ...mapState('tricks', ['tricks', 'categories']),
    ...mapGetters('tricks', ['trickById']),
    haveSubmissions() {
      return this.submissions && this.submissions.length > 0;
    },
    trickId() {
      return this.$route.params.trick;
    },
    trick() {
      return this.trickById(this.trickId);
    }, 
    relatedData() {
      return [
        {
          rowKey: index => `categories-${index}`,
          title: 'Categories',
          data: this.categories.filter(c => this.trick.categories.includes(c.name)),
          dataKey: category => `category-${category.id}`,
          dataRoute: category => `/category/${category.id}`,
        },
        {
          rowKey: index => `prerequisites-${index}`,
          title: 'Prerequisites',
          data: this.tricks.filter(t => this.trick.prerequisites.includes(t.id)),
          dataKey: trick => `trick-prerequisite-${trick.id}`,
          dataRoute: trick => `/trick/${trick.id}`,
        },
        {
          rowKey: index => `progressions-${index}`,
          title: 'Progressions',
          data: this.tricks.filter(t => this.trick.progressions.includes(t.id)),
          dataKey: trick => `trick-progression-${trick.id}`,
          dataRoute: trick => `/trick/${trick.id}`,
        },
      ];
    },
  },
  async fetch() {
    await this.$store.dispatch('submissions/fetchTrickSubmissions', { trickId: this.trickId }), { root: true };
  },
  head() {
    return {
      title: this.trick.name,
      meta: [
        { 
          hid: 'description', 
          name: 'description', 
          content: this.trick.description 
        },
      ],
    };
  },
};
</script>

<style scoped>
/* Your component styles here */
</style>
