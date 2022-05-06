<template>
    <div v-if="!state.isLoggedIn"  id="login">
        <h1>Login</h1>
        <v-select v-model="state.selectedUser" :items="state.users" label="select or create a user" single-line></v-select>
        <v-btn @click="login()" :disabled="state.selectedUser==''" >Login</v-btn>
        <v-btn @click="createUser()">Create User</v-btn>
    </div>
    <div v-else id="logoff">
        <h1>Login</h1>
        <v-text-field :value="state.selectedUser" readonly />
        <v-btn @click="logout()">Logout</v-btn>
    </div>
</template>

<script lang="ts">
import { defineComponent, computed, reactive } from '@vue/composition-api'
import store from '@/store'
export default defineComponent({
    name: 'App',
    setup(props,context){
        const router = context.root.$router;
        const state = reactive({
            users:computed(() => store.state.userLogins),
            isLoggedIn: computed(() => store.state.currentUser!==null),
            selectedUser: store.state.currentUser??'',
        });
        const showError = (error:string) => {
            console.log(error);
        }
        const loadHome = () => {
            store.dispatch('loadCommandList')
            .then(()=>router.push('/'))
            .catch(()=>showError('Error loading commands'));
        }
        const logout = () => {            
            store.dispatch('logoff')
            .then(()=>{
                state.selectedUser='';
            })
            .catch(()=>showError('Error logging off'));             
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
            createUser,
            logout
        }
    }
})      
</script>