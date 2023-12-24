const initState = () => ({
  uploadPromise: null
});

export const state = initState;

export const mutations = {
  setTask(state, { uploadPromise }) {
    state.uploadPromise = uploadPromise;
  },
  reset(state) {
    Object.assign(state, initState());
  }
};

export const actions = {
  async startVideoUpload({ commit, dispatch }, { formData }) {
    const uploadPromise = this.$axios.$post('/api/videos', formData);
    commit('setTask', { uploadPromise });
  },
  async createTrick({ commit, dispatch }, { trick }) {
    await await this.$axios.post('/api/tricks', trick);
    dispatch('fetchTricks');
  },
};
