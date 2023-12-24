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
    console.log('startVideoUpload:', formData);
    const uploadPromise = this.$axios.$post('http://localhost:5000/api/videos', formData);
    commit('setTask', { uploadPromise });
  },
};
