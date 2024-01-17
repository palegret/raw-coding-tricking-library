<!--suppress ALL-->

<template>
  <div>
    <v-card>
      <v-card-text>
        <div v-for="(section, i) in sections" :key="i" class="mt-4">
          <div class="d-flex flex-column align-center mb-5 pb-2">
            <p class="text-h5">{{ section.title }}</p>
            <div>
              <v-btn
                v-for="item in section.items"
                :key="`${section.title}-${item.id}`"
                :to="section.route(item.id)"
                class="mx-1"
              >
              {{item.name}}
            </v-btn>
          </div>
        </div>
        <v-divider v-if="i + 1 != sections.length" class="my-5"></v-divider>
      </div>
      </v-card-text>
    </v-card>
  </div>
</template>

<script>
import { mapState } from 'vuex';

export default {
  computed: {
    ...mapState('tricks', ['categories', 'difficulties', 'tricks']),
    sections() {
      return [
        {
          title: 'Tricks',
          items: this.tricks,
          route: id => `/trick/${id}`,
        },
        {
          title: 'Categories',
          items: this.categories,
          route: id => `/category/${id}`,
        },
        {
          title: 'Difficulties',
          items: this.difficulties,
          route: id => `/difficulty/${id}`,
        },
      ];
    },
  },
}
</script>
