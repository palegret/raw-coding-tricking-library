<!--suppress ALL-->

<template>
  <v-layout
    column
    justify-center
    align-center
  >
    <v-flex
      xs12
      sm8
      md6
    >
      <div class="text-center">
        <logo/>
        <vuetify-logo/>
      </div>

      <v-card v-if="tricks">
        <v-card-title>
          <span class="headline">Tricks</span>
        </v-card-title>
        <v-card-text>
          <ul>
            <li v-for="trick in tricks" :key="trick.id">
              {{trick.name}}
            </li>
          </ul>
        </v-card-text>      
      </v-card>
      <v-divider class="my-3"></v-divider>
      <v-card class="mb-3">
        <v-card-title>
          <span class="headline">Create a new trick</span>
        </v-card-title>
        <v-card-text>
          <v-text-field 
            label="Trick name" 
            v-model="trickName" 
            required 
          />
        </v-card-text>
        <v-card-actions>
          <v-spacer/>
          <v-btn color="primary" @click="saveTrick">
            Save
          </v-btn>
        </v-card-actions>
      </v-card>
      <v-card>
        <v-card-title class="headline">
          {{message}}
          <v-btn @click="reset" class="ml-2">Reset</v-btn>
          <v-btn @click="resetTricks" class="ml-1">Reset Tricks</v-btn>
        </v-card-title>
        <v-card-text>
          <p>Vuetify is a progressive Material Design component framework for Vue.js. It was designed to empower
            developers to create amazing applications.</p>
          <p>
            For more information on Vuetify, check out the <a
            href="https://vuetifyjs.com"
            target="_blank"
          >
            documentation
          </a>.
          </p>
          <p>
            If you have questions, please join the official <a
            href="https://chat.vuetifyjs.com/"
            target="_blank"
            title="chat"
          >
            discord
          </a>.
          </p>
          <p>
            Find a bug? Report it on the github <a
            href="https://github.com/vuetifyjs/vuetify/issues"
            target="_blank"
            title="contribute"
          >
            issue board
          </a>.
          </p>
          <p>Thank you for developing with Vuetify and I look forward to bringing more exciting features in the
            future.</p>
          <div class="text-xs-right">
            <em><small>&mdash; John Leider</small></em>
          </div>
          <hr class="my-3">
          <a
            href="https://nuxtjs.org/"
            target="_blank"
          >
            Nuxt Documentation
          </a>
          <br>
          <a
            href="https://github.com/nuxt/nuxt.js"
            target="_blank"
          >
            Nuxt GitHub
          </a>
        </v-card-text>
        <v-card-actions>
          <v-spacer/>
          <v-btn
            color="primary"
            nuxt
            to="/inspire"
          >
            Continue
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-flex>
  </v-layout>
</template>

<script>
import Logo from '~/components/Logo.vue'
import VuetifyLogo from '~/components/VuetifyLogo.vue'
import { mapState, mapActions, mapMutations } from 'vuex';

export default {
  components: {
    Logo,
    VuetifyLogo
  },
  data() {
    return {
      trickName: '',
    }
  },
  computed: {
    ...mapState({
      message: state => state.message
    }),
    ...mapState('tricks', {
      tricks: state => state.tricks
    }),
  },
  methods: {
    ...mapMutations(['reset']),
    ...mapMutations('tricks', {
      resetTricks: 'reset',
    }),
    ...mapActions('tricks', ['createTrick']),
    async saveTrick() {
      await this.createTrick({ trick: { name: this.trickName }});
      this.trickName = '';
    }
  },
}
</script>
