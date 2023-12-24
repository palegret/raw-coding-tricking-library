<template>
  <v-app dark>
    <v-app-bar app dense>
      <v-toolbar-title>Tricking Library</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-menu left bottom>
        <template v-slot:activator="{ on, attrs }">
          <v-btn depressed v-bind="attrs" v-on="on">
            Upload...
          </v-btn>
        </template>
        <v-list>
          <v-list-item @click="()=>{}">
            <v-list-item-title>Trick</v-list-item-title>
          </v-list-item>
          <v-list-item @click="()=>{}">
            <v-list-item-title>Submission</v-list-item-title>
          </v-list-item>
        </v-list>
      </v-menu>
    </v-app-bar>
    <v-main>
      <nuxt />
    </v-main>
  </v-app>
</template>

<script>
export default {
}
</script>
