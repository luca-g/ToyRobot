<template>
    <div>
        <CommandComponent />
    </div>
</template>

<script lang="ts">
import { defineComponent, computed, reactive } from '@vue/composition-api'
import CommandComponent from '@/components/CommandComponent.vue'
import store from '@/store'
export default defineComponent({
    name: 'MainComponent',
    components: {
      CommandComponent,
    },     
    setup(props,context){
        const router = context.root.$router;
        const state = reactive({
            commands:computed(() => store.state.commands),
            selectedCommand: store.state.commands[0],
        });
        const showError = (error:string) => {
            console.log(error);
        }        
        return{
            state,
        }
    }
})      
</script>