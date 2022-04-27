import Vue from 'vue'
import Vuex from 'vuex'
Vue.use(Vuex)

import {AxiosRequestConfig} from 'axios'
import {ActionContext} from 'vuex'
import {AppState} from './appState'
import {LoginPayload} from './model/loginPayload'
import {UserApiWrapper, CommandApiWrapper} from '@/apis/toy-robot-api'
import {LoginModel,ExecuteCommandModel,ICommandText} from 'toy-robot-axios';
import {MapAndRobotIdPayload} from './model/mapAndRobotIdPayload'
import {CommandAndResult} from './model/commandAndResult'

const userApi = new UserApiWrapper();
const commandApi = new CommandApiWrapper();

const state:AppState = {
  userLogins: [],
  currentUser: null,
  currentUserToken: null,
  currentMapId: null,
  currentRobotId: null,
  commandAndResults: [],
  commands:[]
}
const getAxiosOptions = () :AxiosRequestConfig|undefined => {
  if(state.currentUserToken==null){
    return undefined;
  }
  const req:AxiosRequestConfig = {
    headers: {"Authorization" : `Bearer ${state.currentUserToken}`}
  }
  return req;
}
const getters = {
  userLogins: (state:AppState) => state.userLogins,
  currentUser: (state:AppState) => state.currentUser,
  currentUserToken: (state:AppState) => state.currentUserToken,
  commandAndResults: (state:AppState) => state.commandAndResults,
  commands: (state:AppState) => state.commands,
}
const mutations = {
  addUserLogin: (state:AppState, payload:LoginPayload) =>
  {
    state.userLogins.push(payload.login);
    state.currentUser = payload.login;
    state.currentUserToken = payload.token;
  },  
  setMapAndRobotId: (state:AppState, payload:MapAndRobotIdPayload) => 
  {
    state.currentRobotId = payload.robotId;
    state.currentMapId = payload.mapId;
  },
  addCommandAndResult: (state:AppState, payload:CommandAndResult) =>
  {
    state.commandAndResults.unshift(payload);
  },
  setCommandList: (state:AppState, payload:ICommandText[]) =>
  {
    state.commands = payload;
  }
}
const actions = {
  async loginUser({commit}: ActionContext<AppState,AppState>,userGuid:string) {
    try{
      const loginModel:LoginModel = {
        userGuid
      }
      const response = await userApi.apiV1UserLoginPost(loginModel);
      const loginPayload : LoginPayload = {
        login: userGuid,
        token: response.data
      }
      commit('addUserLogin', loginPayload);
    }
    catch(ex)
    {
      console.log('Login Error',ex);
      throw ex;
    }
  },
  async createUser({commit}: ActionContext<AppState,AppState>) {
    try {
      const response = await userApi.apiV1UserCreatePost();
      if(response.data.token==null){
        throw new Error('Invalid token');
      }
      if(response.data.userGuid==null)
      {
        throw new Error('Invalid userGuid');
      }
      const loginPayload : LoginPayload = {
        login: response.data.userGuid,
        token: response.data.token
      }
      commit('addUserLogin', loginPayload);
    }
    catch(ex)
    {
      console.log('CreateUser error',ex);
      throw ex;
    }
  },
  async executeCommand({commit}: ActionContext<AppState,AppState>, commandText: string) {
    try {
      const executeCommandModel:ExecuteCommandModel = {
        mapId: state.currentMapId,
        robotId: state.currentRobotId,
        text: commandText
      }
      const response = await commandApi.apiV1CommandPost(executeCommandModel, getAxiosOptions());
      const mapAndRobotIdPayload:MapAndRobotIdPayload = {
        mapId: response.data.mapId ?? null,
        robotId: response.data.robotId ?? null,
      }
      commit('setMapAndRobotId', mapAndRobotIdPayload);
      const commandAndResult:CommandAndResult = {
        command: commandText,
        result: response.data.text??'',
        dateTime: new Date()
      }
      commit('addCommandAndResult', commandAndResult);
    }
    catch(ex)
    {
      console.log('ExecuteCommand error',ex);
      throw ex;
    }
  },
  async loadCommandList({commit}: ActionContext<AppState,AppState>) {
    try {      
      console.log('loadCommandList')
      const response = await commandApi.apiV1CommandGet(getAxiosOptions());
      commit('setCommandList', response.data);
    }
    catch(ex)
    {
      console.log('LoadCommandList error',ex);
      throw ex;
    }
  },
}
export default new Vuex.Store({
  state,
  getters,
  mutations,
  actions,
  modules: {
  }
})
