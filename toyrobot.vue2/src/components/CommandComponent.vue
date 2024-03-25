<template>
    <v-card>
        <v-container :key="selectedCommand?.commandName">
            <div class="text-h5">Command</div>
            <v-select v-model="selectedCommand" :items="state.commands" item-text="commandName" @change="selectedItem" label="select a command" single-line></v-select>
            <div v-if="selectedCommandHasParameters()" :key="selectedCommandName">
                <span>Parameters</span>
                <v-container class="pl-8">
                    <v-row v-for="item in selectedCommandParameters()" :key="item.name">
                        <v-col cols="3" v-if="item.name">{{capitalizeFirstLetter(item.name)}}</v-col>
                        <v-col v-if="item.parameterTypeName!==undefined && item.name">
                            <span v-if="item.acceptedValues!==null">
                                <v-select v-model="state.parameterValue[item.name]" :items="item.acceptedValues" label="select" single-line></v-select>
                            </span>
                            <span v-else-if="item.parameterTypeName==='Int32'">
                                <v-slider
                                    v-model="state.parameterValue[item.name]"
                                    class="align-center"
                                    :max="20"
                                    :min="1"
                                    hide-details
                                >
                                    <template v-slot:append>
                                    <v-text-field
                                        v-model="state.parameterValue[item.name]"
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
                                    v-model="state.parameterValue[item.name]"
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
import { ICommandText, ICommandParameter } from 'toy-robot-axios'
import store from '@/store'
export default defineComponent({
    name: 'UserButtonComponent',
    emits: ['command-text'],
    setup(props,context){        
        type ParameterValues = {
            // eslint-disable-next-line
            [key: string]: any; // Use `any` or a more specific type as needed
        };        
        const selectedCommand = ref<ICommandText|null>(null);
        const selectedCommandParameters = () => {
            if(!selectedCommand.value)
            {
                console.log('selectedCommandParameters: no selected command')
                return [];
            }
            console.log('selectedCommandParameters parameters ',selectedCommand.value.commandParameters)
            return selectedCommand.value.commandParameters as Array<ICommandParameter>;
        };
        const selectedCommandName = computed(()=>{
            return selectedCommand.value ? selectedCommand.value.commandName??'' : '';
        });
        const selectedCommandHasParameters = ()=>{
            return selectedCommand.value && selectedCommand.value.commandParameters ? selectedCommand.value.commandParameters.length>0 : false;
        };
        const state = reactive({
            commands: computed(() => {
                console.log('all commands',store.state.commands);
                return store.state.commands;
            }),                        
            parameterValue: {} as ParameterValues
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
            console.log('selectedItem', item);
            selectedCommand.value = item;
            for (let key in state.parameterValue) {
                delete state.parameterValue[key];
            }
            item.commandParameters?.forEach(param => {
                if(param.name)
                {
                    state.parameterValue[param.name] = 0;
                }
            });
            console.log('all command parameters and values',state.parameterValue);
        }
        const send = () => {
            const command:ICommandText|null = selectedCommand.value;
            if(command!=null)
            {
                let commandText = command.commandName;
                
                if(command.commandParameters!=null)
                {
                    command.commandParameters.forEach(item => {
                        if(item.name)
                        {
                            const dvalue = state.parameterValue[item.name];
                            commandText += "," + dvalue;
                        }
                    });
                }
                context.emit('command-text', commandText);
            }
        }
        const capitalizeFirstLetter = (text:string) => {
            console.log('capitalize first ' + text);
            return text.charAt(0).toUpperCase() + text.slice(1);
        }
        return{
            state,
            selectedItem,
            selectedCommand,
            selectedCommandParameters,
            selectedCommandHasParameters,
            selectedCommandName,
            send,
            capitalizeFirstLetter
        }
    }
})      
</script>