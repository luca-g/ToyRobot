<template>
    <v-card>
        <v-container :key="state.selectedCommandName">
            <div class="text-h5">Command</div>
            <v-select v-model="selectedCommand" :items="state.commands" item-text="commandName" @change="selectedItem" label="select a command" single-line></v-select>
            <div v-if="selectedCommand!==null && selectedCommand.commandParameters.length>0">
                <span>Parameters</span>
                <v-container class="pl-8">
                    <v-row v-for="item in selectedCommand.commandParameters" :key="item.name">
                        <v-col cols="3">{{capitalizeFirstLetter(item.name)}}</v-col>
                        <v-col v-if="item.parameterTypeName!==undefined">
                            <span v-if="item.acceptedValues!==null">
                                <v-select v-model="selectedCommandParameters[item.name]" :items="item.acceptedValues" label="select" single-line></v-select>
                            </span>
                            <span v-else-if="item.parameterTypeName==='Int32'">
                                <v-slider
                                    v-model="selectedCommandParameters[item.name]"
                                    class="align-center"
                                    :max="20"
                                    :min="1"
                                    hide-details
                                >
                                    <template v-slot:append>
                                    <v-text-field
                                        v-model="selectedCommandParameters[item.name]"
                                        class="mt-0 pt-0"
                                        hide-details
                                        single-line
                                        type="number"
                                        style="width: 60px"
                                    ></v-text-field>
                                    </template>
                                </v-slider>                        
                            </span>                    
                            <span v-else>
                                <v-text-field
                                    v-model="selectedCommandParameters[item.name]"
                                    class="mt-0 pt-0"
                                    hide-details
                                    single-line
                                    style="width: 60px"
                                ></v-text-field>                        
                            </span>
                        </v-col>
                    </v-row>
                </v-container>
            </div>
            <v-container>
                <v-row>
                    <v-spacer />
                    <v-btn :disabled="selectedCommand===null" @click="send()" class="primary">Send</v-btn>
                </v-row>
            </v-container>
        </v-container>
    </v-card>
</template>

<script lang="ts">
import { defineComponent, computed, reactive, ref } from '@vue/composition-api'
import { ICommandText } from 'toy-robot-axios'
import store from '@/store'
export default defineComponent({
    name: 'UserButtonComponent',
    emits: ['command-text'],
    setup(props,context){        
        const selectedCommand = ref<ICommandText|null>(null);
        const selectedCommandParameters = ref({});
        const state = reactive({
            commands:computed(() => store.state.commands),
            selectedCommandName: '',
            parameterValue: {}
        });
        const showError = (error:string) => {
            console.log(error);
        }        
        const selectedItem = (key:string) => {
            console.log('commands', store.state.commands)
            const item = store.state.commands.find(t=>t.commandName===key);
            if(item===undefined){
                showError('command not found ' + key);
                return;
            }
            state.selectedCommandName = item.commandName??'';
            console.log('selectedItem', item);
            selectedCommand.value = item;
            console.log('selectedCommand', selectedCommand);
            selectedCommandParameters.value = {};
        }
        const send = () => {
            const command:ICommandText|null = selectedCommand.value;
            if(command!=null)
            {
                let commandText = command.commandName;
                
                if(command.commandParameters!=null)
                {
                    command.commandParameters.forEach(item => {
                        const key = item.name??'';
                        const dict = (selectedCommandParameters.value) as any;
                        const dvalue = dict[key];
                        commandText += "," + dvalue;
                    });
                }
                context.emit('command-text', commandText);
            }
        }
        const capitalizeFirstLetter = (text:string) => {
            return text.charAt(0).toUpperCase() + text.slice(1);
        }
        return{
            state,
            selectedItem,
            selectedCommand,
            selectedCommandParameters,
            send,
            capitalizeFirstLetter
        }
    }
})      
</script>