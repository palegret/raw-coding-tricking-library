const initState = () => ({
  uploadPromise: null,
  uploadComplete: false,
  uploadCancelTokenSource: null,
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
  setUploadPromise(state, { uploadPromise, uploadCancelTokenSource }) {
    state.uploadPromise = uploadPromise;
    state.uploadCancelTokenSource = uploadCancelTokenSource;
  },
  completeUpload(state) {
    state.uploadComplete = true;
  },
  reset(state) {
    Object.assign(state, initState());
  },
};

export const actions = {
  async startVideoUpload({ commit, dispatch }, { formData }) {
    const uploadCancelTokenSource = this.$axios.CancelToken.source();
    const options = { cancelToken: uploadCancelTokenSource.token };
    const uploadPromise = this.$axios.post('/api/videos', formData, options)
      .then(({ data }) => {
        commit('completeUpload');
        return data;
      })
      .catch((error) => {
        if (this.$axios.isCancel(error)) {
          console.log('Video upload was cancelled.');
        } else {
          console.error(error);
          // Future code:
          // commit('reset');
          // const notificationMessage = { message: 'Error uploading video.', type: 'error' };
          // dispatch('notifications/add', notificationMessage, { root: true });
        }
      });

    commit('setUploadPromise', { uploadPromise, uploadCancelTokenSource });
  },
  async cancelVideoUpload({ state, commit }) {
    if (state.uploadPromise) {
      commit('hide');

      if (state.uploadComplete) {
        const video = await state.uploadPromise;
        this.$axios.delete('/api/videos/' + video)
      } else {
        state.uploadCancelTokenSource.cancel();
      }
    }

    commit('reset');
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
