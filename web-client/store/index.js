const initState = () => ({});

export const state = initState;

export const mutations = {
  reset(state) {
    Object.assign(state, initState());
  }
};

export const actions = {
  async nuxtServerInit({ store, commit, dispatch }) {
    await dispatch('tricks/fetchTricks', null, { root: true });
  }
};
