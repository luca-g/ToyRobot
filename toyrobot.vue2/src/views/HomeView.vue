<template>
    <div>
        <CommandComponent @command-text="commandText" class="mb-4"/>
        <CommandsResultComponent />
    </div>
</template>

<script lang="ts">
import { defineComponent } from '@vue/composition-api'
import { onBeforeMount } from '@vue/composition-api'
import CommandComponent from '@/components/CommandComponent.vue'
import CommandsResultComponent from '@/components/CommandsResultComponent.vue'
import store from '@/store'
export default defineComponent({
    name: 'HomeView',
    components: {
      CommandComponent,
      CommandsResultComponent,
    },     
    setup(){
        const showError = (error:string) => {
            console.log(error);
        }
        onBeforeMount(()=>{
            store.dispatch('loadCommandList')
            .catch(()=>showError('Error loading commands'));
        });
        const commandText = async (text:string) => {
            console.log("home command text", text);
            await store.dispatch('executeCommand', text);
            console.log('commandText completed');
        }
        return{
            commandText
        }
    }
})      
</script>