<template>
    <v-btn v-if="state.isLoggedIn"
      tile
      color="success"
      @click="logout()"
    >
      <v-icon left>
        mdi-account
      </v-icon>
      Logout
    </v-btn>      
</template>

<script lang="ts">
import { defineComponent, computed, reactive } from '@vue/composition-api'
import store from '@/store'
export default defineComponent({
    name: 'UserButtonComponent',
    setup(props,context){
        const router = context.root.$router;
        const state = reactive({
            isLoggedIn: computed(() => store.state.currentUser!==null),
        });
        const showError = (error:string) => {
            console.log(error);
        }        
        const logout = () => {            
            store.dispatch('logoff')
            .then(()=> {
                router.push('login');                
            })
            .catch(()=>showError('Error logging off '));             
        }
        return{
            state,
            logout
        }
    }
})      
</script>