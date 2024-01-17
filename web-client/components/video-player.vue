<template>
  <div class="video-container">
    <div class="play-button" :class="{ 'hide' : playing }" @click="playing = !playing">
      <v-icon size="78">mdi-play</v-icon>
    </div>
    <video
      ref="video"
      :src="`http://localhost:5000/api/videos/${video.videoLink}`"
      :poster="`http://localhost:5000/api/videos/${video.thumbLink}`"
      width="400"
      preload="none"
      muted loop playsinline
    ></video>
  </div>
</template>

<script>
export default {
  name: 'video-player',
  props: {
    video: {
      type: Object,
      required: true,
    },
  },
  data: () => ({
    playing: false,
  }),
  watch: {
    playing(newValue) {
      const video = this.$refs.video;

      if (newValue) {
        video.play();
      } else {
        video.pause();
      }
    },
  },
  methods: {
  },
};
</script>

<style lang="scss" scoped>
.video-container {
  cursor: pointer;
  border-top-left-radius: inherit;
  border-top-right-radius: inherit;
  display: flex;
  position: relative;
  width: 100%;

  .play-button {
    align-items: center;
    background-color: rgba(0, 0, 0, 0.36);
    display: flex;
    height: 100%;
    justify-content: center;
    position: absolute;
    width: 100%;
    z-index: 2;

    &.hide {
      opacity: 0;
    }
  }

  video {
    border-top-left-radius: inherit;
    border-top-right-radius: inherit;
    width: 100%;
    z-index: 1;
  }
}
</style>
