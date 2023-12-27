import { UPLOAD_TYPE, UPLOAD_STEP } from '../data/enum.js';

const initState = () => ({
  uploadPromise: null,
  active: false,
  type: '',
  step: UPLOAD_STEP.SELECT_TYPE
});

export const state = initState;

export const mutations = {
  setType(state, { type }) {
    state.type = type;

    if (type === UPLOAD_TYPE.TRICK) {
      state.step = UPLOAD_STEP.TRICK_INFORMATION;
    } else if (type === UPLOAD_TYPE.SUBMISSION) {
      state.step = UPLOAD_STEP.UPLOAD_VIDEO;
    }
  },
  setUploadPromise(state, { uploadPromise }) {
    state.uploadPromise = uploadPromise;
    state.step = UPLOAD_STEP.SUBMISSION_INFORMATION;
  },
  setStep(state, { step }) {
    state.step = step;
  },
  toggleActivity(state) {
    state.active = !state.active;

    if (!state.active)
      Object.assign(state, initState());
  },
  reset(state) {
    Object.assign(state, initState());
  }
};

export const actions = {
  async startVideoUpload({ commit, dispatch }, { formData }) {
    const uploadPromise = this.$axios.$post('/api/videos', formData);
    commit('setUploadPromise', { uploadPromise });
  },
  async createTrick({ state, commit, dispatch }, { trick, submission }) {

    if (state.type === UPLOAD_TYPE.TRICK) {
      const newTrick = await this.$axios.$post('/api/tricks', trick);
      submission.trickId = newTrick.id;
    }

    await this.$axios.post('/api/submissions', submission);

    await dispatch('tricks/fetchTricks', null, { root: true });
    await dispatch('submissions/fetchSubmissions', null, { root: true });
  },
};
