<template>
  <v-app>
    <v-app-bar
      app
      color="primary"
      dark
    >
      <div class="d-flex align-center">
        <v-img
          alt="ToyRobot Logo"
          class="shrink mr-2"
          contain
          src="https://cdn.vuetifyjs.com/images/logos/vuetify-logo-dark.png"
          transition="scale-transition"
          width="40"
        />

        <div class="text-h4" inline>ToyRobot</div>
      </div>

      <v-spacer></v-spacer>

      <UserButtonComponent />
    </v-app-bar>

    <v-main>
      <v-container>
        <router-view/>
      </v-container>
    </v-main>
  </v-app>
</template>
<style>
.v-application{
  min-width: 600px;
}
</style>
<script lang="ts">
import { defineComponent } from '@vue/composition-api'
import UserButtonComponent from '@/components/UserButtonComponent.vue'
import store from '@/store'
export default defineComponent({
    name: 'App',
    components: {
      UserButtonComponent,
    },    
    setup(props,context){
      if(store.state.currentUser===null){
        const router = context.root.$router;
        router.push('login');
      }
    }
})      
</script>
