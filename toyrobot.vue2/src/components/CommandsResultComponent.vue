<template>
    <v-card>
        <v-container>
            <div class="text-h5 mb-2">Command History</div>
            <v-row v-for="cmd in state.commandResults" :key="cmd.dateTime.toLocaleString()" no-gutters>
                <v-col cols="3" sm="3" >{{cmd.command}}</v-col>
                <v-col cols="5" sm="6">{{cmd.result}}</v-col>
                <v-col cols="4" sm="3">{{cmd.dateTime.toLocaleString()}}</v-col>
            </v-row>
        </v-container>
    </v-card>
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