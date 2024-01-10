<template>
  <v-card>
    <v-card-title>
      <span class="headline">Create Category</span>
      <v-spacer></v-spacer>
      <v-btn icon @click="close">
        <v-icon>mdi-close</v-icon>
      </v-btn>
    </v-card-title>
    <v-card-text>
      <v-form>
        <v-text-field v-model="formData.name" label="Name" required />
        <v-text-field v-model="formData.description" label="Description" required />
      </v-form>
    </v-card-text>
    <v-card-actions>
      <v-spacer />
      <v-btn color="primary" @click="save">Save</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script>
import { close } from '../../mixins/close.js';

const initState = () => ({
  formData: {
    name: '',
    description: '',
  },
});

export default {
  name: 'category-form',
  mixins: [
    close
  ],
  data: initState,
  methods: {
    resetData() {
      Object.assign(this.$data, initState());
    },
    save() {
      this.$axios.post('api/categories', this.formData);
      this.close();
    },
  }
}
</script>
