<template>
  <item-content-layout>
    <template v-slot:content>
      <trick-list v-if="haveTricks" :tricks="tricks" />
    </template>
    <template v-slot:item>
      <div v-if="category">
        <p class="text-h6 ma-0">Category: {{ category.name }}</p>
        <v-divider class="my-1"></v-divider>
        <p class="text-body-2 ma-0">{{ category.description }}</p>
      </div>
    </template>
  </item-content-layout>
</template>

<script>
import { mapGetters } from 'vuex';
import ItemContentLayout from '../../components/item-content-layout';
import TrickList from '../../components/trick-list';

export default {
  name: 'CategoryPage',
  components: {
    ItemContentLayout,
    TrickList,
  },
  data: () => ({
    tricks: [],
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
  },
  methods: {
  },
  async fetch() {
    this.tricks = await this.$axios.$get(`/api/categories/${this.categoryId}/tricks`);
  },
  head() {
    if (!this.category)
      return {};
    
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
