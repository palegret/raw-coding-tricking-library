const initState = () => ({
  uploadPromise: null,
  active: false,
  component: null,
});

export const state = initState;

export const mutations = {
  activate(state, { component }) {
    state.active = true;
    state.component = component;
  },
  hide(state) {
    state.active = false;
  },
  setUploadPromise(state, { uploadPromise }) {
    state.uploadPromise = uploadPromise;
  },
  reset(state) {
    Object.assign(state, initState());
  },
};

export const actions = {
  async startVideoUpload({ commit, dispatch }, { formData }) {
    const uploadPromise = this.$axios.$post('/api/videos', formData);
    commit('setUploadPromise', { uploadPromise });
  },
  async createSubmission({ state, commit, dispatch }, { formData }) {
    if (!state.uploadPromise) {
      console.error('Error: uploadPromise is null.');
      return;
    }

    formData.video = await state.uploadPromise;
    await dispatch('submissions/createSubmission', { formData }, { root: true });
    commit('reset');
  },
};
