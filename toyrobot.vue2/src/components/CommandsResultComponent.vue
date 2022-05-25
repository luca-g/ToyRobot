<template>
    <div>
        <div v-for="cmd in state.commandResults" :key="cmd.dateTime.toString()">
            <div>{{cmd.command}}</div>
            <div>{{cmd.result}}</div>
            <div>{{cmd.dateTime}}</div>
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent, computed, reactive } from '@vue/composition-api'
import store from '@/store'
export default defineComponent({
    name: 'CommandsResultComponent',    
    setup(){
        const state = reactive({
            commandResults:computed(() => store.state.commandAndResults),
        });
        return{
            state,
        }
    }
})      
</script>