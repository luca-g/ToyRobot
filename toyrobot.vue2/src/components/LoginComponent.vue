<template>
    <div id="login">
        <h1>Login</h1>
        <v-select v-model="state.selectedUser" :items="state.users" label="select" single-line></v-select>
        <v-btn @click="login()" :disabled="state.selectedUser==''" >Login</v-btn>
        <v-btn @click="createUser()">Create User</v-btn>
    </div>
</template>

<script lang="ts">
import { defineComponent, onBeforeMount, reactive } from '@vue/composition-api'
import store from '@/store'
export default defineComponent({
    name: 'App',
    setup(props,context){
        const router = context.root.$router;
        const state = reactive({
            users:store.state.userLogins,
            selectedUser:'',
        });
        const showError = (error:string) => {
            console.log(error);
        }
        const loadHome = () => {
            store.dispatch('loadCommandList')
            .then(()=>router.push('home'))
            .catch(()=>showError('Error loading commands'));
        }
        const createUser = () => {
            store.dispatch('createUser')
            .then(()=>{
                loadHome();
            })
            .catch(()=>showError('Error creating the user'));             
        }
        const login = () => {
            if(state.selectedUser != "") {
                store.dispatch('loginUser', state.selectedUser)
                .then(()=>loadHome())
                .catch(()=>showError('Login error'));
            } else {
                showError('Please select an user');
            }
        }
        return{
            state,
            login,
            createUser
        }
    }
})      
</script>