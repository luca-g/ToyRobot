<template>
  <LoginComponent/>
</template>
<script lang="ts">
import { defineComponent } from '@vue/composition-api'
import LoginComponent from '@/components/LoginComponent.vue'
export default defineComponent({
    name: 'LoginView',
    components: {
      LoginComponent,
    },    
});
</script>