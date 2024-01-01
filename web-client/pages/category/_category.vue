<template>
  <div class="d-flex justify-center align-start mt-2">
    <div v-if="haveTricks" class="mx-2">
      <v-text-field 
        v-model="filter" 
        label="Search Tricks" 
        placeholder="E.g., flip, kick, etc." 
        prepend-inner-icon="mdi-magnify" 
        outlined
      ></v-text-field>
      <div v-for="trick in filteredTricks" :key="trick.id">
        <span>
          {{trick.name}} - {{trick.description}}
        </span>
      </div>
    </div>
    <v-sheet v-if="category" class="mx-2 pa-3 sticky">
      <p class="text-h6 ma-0">Category: {{ category.name }}</p>
      <v-divider class="my-1"></v-divider>
      <p class="text-body-2 ma-0">{{ category.description }}</p>
    </v-sheet>
  </div>
</template>

<script>
import { mapGetters } from 'vuex';

export default {
  name: 'CategoryPage',
  data: () => ({
    tricks: [],
    filter: '',
  }),
  computed: {
    ...mapGetters('tricks', ['categoryById']),
    haveTricks() {
      return this.tricks && this.tricks.length > 0;
    },
    categoryId() {
      return this.$route.params.category;
    },
    category() {
      return this.categoryById(this.categoryId);
    },
    filteredTricks() {
      if (!this.filter)
        return this.tricks;

      return this.tricks.filter(t => this.trickMatchesFilter(t));
    },
  },
  methods: {
    trickMatchesFilter(trick) {
      if (!this.filter)
        return true;

      const normalizedFilter = this.filter.trim().toLowerCase();
      const normalizedName = trick.name.toLowerCase();
      const normalizedDescription = trick.description.toLowerCase();

      return normalizedName.includes(normalizedFilter) || normalizedDescription.includes(normalizedFilter);
    },
  },
  async fetch() {
    this.tricks = await this.$axios.$get(`/api/categories/${this.categoryId}/tricks`);
  },
  head() {
    return {
      title: this.category.name,
      meta: [
        { 
          hid: 'description', 
          name: 'description', 
          content: this.category.description 
        },
      ],
    };
  },
};
</script>

<style scoped>
/* Your component styles here */
</style>
