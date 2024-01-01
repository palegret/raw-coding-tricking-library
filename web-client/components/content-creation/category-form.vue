<template>
  <v-card>
    <v-card-title>
      <span class="headline">Create Category</span>
    </v-card-title>
    <v-card-text>
      <v-form>
        <v-text-field v-model="formData.name" label="Name" required />
        <v-textarea v-model="formData.description" label="Description" required />
      </v-form>
    </v-card-text>
    <v-card-actions>
      <v-spacer />
      <v-btn @click="save">Save</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script>
const initState = () => ({
  formData: {
    name: '',
    description: '',
  },
});

export default {
  name: 'category-form',
  data: initState,
  methods: {
    resetData() {
      Object.assign(this.$data, initState());
    },
    save() {
      this.$axios.post('api/categories', this.formData);
      this.resetData();
    },
  }
}
</script>
