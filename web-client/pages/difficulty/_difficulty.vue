<template>
  <div class="d-flex justify-center align-start mt-2">
    <trick-list v-if="haveTricks" :tricks="tricks" class="mx-2" />

    <v-sheet v-if="difficulty" class="mx-2 pa-3 sticky">
      <p class="text-h6 ma-0">Difficulty: {{ difficulty.name }}</p>
      <v-divider class="my-1"></v-divider>
      <p class="text-body-2 ma-0">{{ difficulty.description }}</p>
    </v-sheet>
  </div>
</template>

<script>
import { mapGetters } from 'vuex';
import TrickList from '../../components/trick-list';

export default {
  name: 'DifficultyPage',
  components: {
    TrickList,
  },
  data: () => ({
    tricks: [],
  }),
  computed: {
    ...mapGetters('tricks', ['difficultyById']),
    haveTricks() {
      return this.tricks && this.tricks.length > 0;
    },
    difficultyId() {
      return this.$route.params.difficulty;
    },
    difficulty() {
      return this.difficultyById(this.difficultyId);
    },
  },
  methods: {
  },
  async fetch() {
    this.tricks = await this.$axios.$get(`/api/difficulties/${this.difficultyId}/tricks`);
  },
  head() {
    if (!this.difficulty)
      return {};
    
    return {
      title: this.difficulty.name,
      meta: [
        {
          hid: 'description',
          name: 'description',
          content: this.difficulty.description
        },
      ],
    };
  },
};
</script>

<style scoped>
/* Your component styles here */
</style>