<template>
  <div id="app">
    <nav>
      <router-link to="/">Home</router-link> |
      <router-link to="/about">About</router-link>
    </nav>
    <router-view/>
  </div>
</template>
<script lang="ts">
import { defineComponent, onBeforeMount } from '@vue/composition-api'
import store from '@/store'

export default defineComponent({
    name: 'App',
    setup(){
      console.log('App.setup');

      onBeforeMount(() => {
        console.log('App.onBeforeMount');
        store.dispatch('createUser')
        .then(()=>{
          store.dispatch('loadCommandList');
        });        
      })
    }
})
</script>
<style lang="scss">
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

nav {
  padding: 30px;

  a {
    font-weight: bold;
    color: #2c3e50;

    &.router-link-exact-active {
      color: #42b983;
    }
  }
}
</style>
