<template>
  <v-dialog dark v-model="active" persistent>
    <template v-slot:activator="{ on }">
      <v-menu offset-y v-on="on">
        <template v-slot:activator="{ on, attrs }">
          <v-btn color="primary" dark v-bind="attrs" v-on="on">
            Create
          </v-btn>
        </template>
        <v-list>
          <v-list-item v-for="(item, index) in menuItems" :key="`ccd-menu-${index}`" @click="activate(item)">
            <v-list-item-title>{{ item.title }}</v-list-item-title>
          </v-list-item>
        </v-list>
      </v-menu>
    </template>
    <div v-if="component">
      <component :is="component" />
    </div>
  </v-dialog>
</template>

<script>
import { mapState, mapMutations } from 'vuex';
import SubmissionSteps from './submission-steps';
import TrickSteps from './trick-steps';

export default {
  name: 'content-creation-dialog',
  components: {
    SubmissionSteps,    
    TrickSteps,
  },
  computed: {
    ...mapState('video-upload', ['active', 'component']),
    menuItems() {
      return [
        {
          title: 'Trick',
          component: TrickSteps,
        },
        {
          title: 'Submission',
          component: SubmissionSteps,
        },
      ];
    },
  },
  methods: mapMutations('video-upload', ['reset', 'activate']),
}
</script>
