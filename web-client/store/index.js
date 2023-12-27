const initState = () => ({});

export const state = initState;

export const mutations = {
  reset(state) {
    Object.assign(state, initState());
  }
};

export const actions = {
  async nuxtServerInit({ commit, dispatch }) {
    await dispatch('tricks/fetchTricks', null, { root: true });
    await dispatch('submissions/fetchSubmissions', null, { root: true });
  }
};
