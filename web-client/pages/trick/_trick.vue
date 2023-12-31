<!--suppress ALL-->

<template>
  <item-content-layout>
    <template v-slot:content>
      <div v-if="haveSubmissions">
        <v-card v-for="submission in submissions" :key="submission.id" class="mb-2">
          <video-player :video="submission.video" />
          <v-card-text>
            {{submission.description}}
          </v-card-text>
        </v-card>
      </div>    
    </template>
    <template v-slot:item v-if="trick">
      <p class="text-h5 mt-0 mb-4 mx-0">
        <span class="pr-2">{{ trick.name }}</span>
        <v-chip small class="mb-1" :to="`/difficulty/${difficulty.id}`">
          {{ difficulty.name }}
        </v-chip>
      </p>
      <v-divider class="my-1"></v-divider>
      <p class="text-body-2 ma-0">
        {{ trick.description }}
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
    </template>
  </item-content-layout>
</template>

<script>
import { mapState, mapGetters } from 'vuex';
import ItemContentLayout from '../../components/item-content-layout';
import VideoPlayer from '../../components/video-player';

export default {
  name: 'TrickPage',
  components: {
    ItemContentLayout,
    VideoPlayer,
  },
  computed: {
    ...mapState('submissions', ['submissions']),
    ...mapState('tricks', ['tricks', 'categories']),
    ...mapGetters('tricks', ['trickById', 'difficultyById']),
    haveSubmissions() {
      return this.submissions && this.submissions.length > 0;
    },
    trickId() {
      return this.$route.params.trick;
    },
    trick() {
      return this.trickById(this.trickId);
    }, 
    difficulty() {
      return this.difficultyById(this.trick.difficulty);
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
    if (!this.trick)
      return {};

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
