export default {
  data: () => ({
    filter: '',
    tricks: [],
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
  }
};