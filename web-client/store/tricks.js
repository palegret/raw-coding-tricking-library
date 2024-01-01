const initState = () => ({
  tricks: [],
  categories: [],
  difficulties: [],
});

export const state = initState;

export const getters = {
  trickById: state => id => state.tricks.find(trick => trick.id === id),
  categoryById: state => id => state.categories.find(category => category.id === id),
  trickItems: state => state.tricks.map(trick => ({
    text: trick.name,
    value: trick.id
  })),
  categoryItems: state => state.categories.map(category => ({
    text: category.name,
    value: category.id
  })),
  difficultyItems: state => state.difficulties.map(difficulty => ({
    text: difficulty.name,
    value: difficulty.id
  })),
};

export const mutations = {
  setTricks(state, { tricks, categories, difficulties }) {
    state.tricks = tricks;
    state.categories = categories;
    state.difficulties = difficulties;
  },
  reset(state) {
    Object.assign(state, initState());
  }
};

export const actions = {
  async fetchTricks({ commit }) {
    const tricks = await this.$axios.$get('/api/tricks');
    const categories = await this.$axios.$get('/api/categories');
    const difficulties = await this.$axios.$get('/api/difficulties');
    commit('setTricks', { tricks, categories, difficulties });
  },
  createTrick({ state, commit, dispatch }, { formData }) {
    return this.$axios.$post('/api/tricks', formData);
  },
};
