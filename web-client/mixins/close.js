import { mapActions } from 'vuex';

export const close = {
  methods: {
    ...mapActions('video-upload', ['cancelVideoUpload']),
    close() {
      this.cancelVideoUpload();
    }
  }
}
