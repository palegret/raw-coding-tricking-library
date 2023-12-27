const initState = () => ({
  submissions: []
});

export const state = initState;

export const mutations = {
  setSubmissions(state, { submissions }) {
    state.submissions = submissions;
  },
  reset(state) {
    Object.assign(state, initState());
  }
};

export const actions = {
  async fetchSubmissions({ commit }) {
    const submissions = await this.$axios.$get('/api/submissions');
    commit('setSubmissions', { submissions });
  },
  async fetchTrickSubmissions({ commit }, { trickId }) {
    const submissions = await this.$axios.$get(`/api/tricks/${trickId}/submissions`);
    commit('setSubmissions', { submissions });
  },
};
