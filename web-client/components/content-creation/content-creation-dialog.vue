<template>
  <v-dialog dark :value="active" width="700" persistent>
    <template v-slot:activator="{ on }">
      <v-menu offset-y v-on="on">
        <template v-slot:activator="{ on, attrs }">
          <v-btn dark depressed v-bind="attrs" v-on="on">
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
import CategoryForm from './category-form';
import DifficultyForm from './difficulty-form';
import SubmissionSteps from './submission-steps';
import TrickSteps from './trick-steps';

export default {
  name: 'content-creation-dialog',
  components: {
    CategoryForm,
    DifficultyForm,
    SubmissionSteps,    
    TrickSteps,
  },
  computed: {
    ...mapState('video-upload', ['active', 'component']),
    menuItems() {
      return [
        {
          title: 'Category',
          component: CategoryForm,
        },
        {
          title: 'Difficulty',
          component: DifficultyForm,
        },
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
  methods: {
    ...mapMutations('video-upload', ['activate']),
  }
}
</script>
