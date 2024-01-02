<template>
  <div>
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
</template>

<script>
export default {
  name: 'trick-list',
  props: {
    tricks: {
      type: Array,
      required: true,
    },
  },
  data: () => ({
    filter: '',
  }),
  computed: {
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
};
</script>

<style scoped>
/* Your component styles here */
</style>
