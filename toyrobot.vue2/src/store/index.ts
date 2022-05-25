import Vue from 'vue'
import Vuex from 'vuex'
Vue.use(Vuex)

import VueCookie from 'vue-cookies'
Vue.use(VueCookie, { expire: '7d'})

import {AxiosRequestConfig} from 'axios'
import {ActionContext} from 'vuex'
import {AppState} from './appState'
import {LoginPayload} from './model/loginPayload'
import {UserApiWrapper, CommandApiWrapper} from '@/apis/toy-robot-api'
import {LoginModel,ExecuteCommandModel,ICommandText} from 'toy-robot-axios';
import {MapAndRobotIdPayload} from './model/mapAndRobotIdPayload'
import {CommandAndResult} from './model/commandAndResult'

/*
import VuexPersistence from 'vuex-persist'
const vuexLocal = new VuexPersistence<AppState>({
  storage: window.localStorage
})
*/

const cookies = Vue.$cookies;
const usersKey = 'userLogins';
const userApi = new UserApiWrapper();
const commandApi = new CommandApiWrapper();
const userLoginsFromCookie = () : string[] => {
  const value = cookies.get(usersKey) as string ?? '';
  if(value.length==0) {
    return [];
  }
  return value.split(',');
}
const state:AppState = {
  userLogins: [],
  currentUser: null,
  currentUserToken: null,
  currentMapId: null,
  currentRobotId: null,
  commandAndResults: [],
  commands:[]
}


const persistState = () => {
  cookies.set('userLogins', state.userLogins);
  cookies.set('currentUser', state.currentUser);
  cookies.set('currentUserToken', state.currentUserToken);
  cookies.set('currentMapId', state.currentMapId);
  cookies.set('currentRobotId', state.currentRobotId);
}
const cookieValueOrNull = (name:string) : string|null => {
  const value = cookies.get(name);
  if(value===''||value===null||value==='null')
    return null;
  return value as string;
}
const cookieValueNumberOrNull = (name:string) : number|null => {
  const value = cookieValueOrNull(name);
  if(value===null)
    return null;
  return Number.parseInt(value);
}
const restoreState = () => {
  state.userLogins = userLoginsFromCookie();
  state.currentUser = cookieValueOrNull('currentUser');
  state.currentUserToken = cookieValueOrNull('currentUserToken');
  state.currentMapId = cookieValueNumberOrNull('currentMapId');
  state.currentRobotId = cookieValueNumberOrNull('currentRobotId');
}
restoreState();

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
    if(state.userLogins.indexOf(payload.login)<0)
    {
      state.userLogins.push(payload.login);
      cookies.set(usersKey,state.userLogins);
    }
    state.currentUser = payload.login;
    state.currentUserToken = payload.token;
  },  
  clearUserLogin: (state:AppState) =>
  {
    state.currentUser = null;
    state.currentUserToken = null;
    state.currentMapId = null;
    state.currentRobotId = null;
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
  clearCommandAndResult: (state:AppState) =>
  {
    state.commandAndResults = [];
  },  
  setCommandList: (state:AppState, payload:ICommandText[]) =>
  {
    state.commands = payload;
  }
}
const actions = {
  async logoff({commit}: ActionContext<AppState,AppState>)  {
      try{
        commit('clearUserLogin');
        persistState();
      }
      catch(ex)
      {
        console.log('Logoff Error',ex);
        throw ex;
      }
    
  },
  async loginUser({commit}: ActionContext<AppState,AppState>,userGuid:string) : Promise<string> {
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
      const mapAndRobotIdPayload : MapAndRobotIdPayload = {
        mapId:null,
        robotId:null
      }
      commit('setMapAndRobotId', mapAndRobotIdPayload);
      commit('clearCommandAndResult');
      persistState();
      return userGuid;
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
      const mapAndRobotIdPayload : MapAndRobotIdPayload = {
        mapId:null,
        robotId:null
      }
      commit('setMapAndRobotId', mapAndRobotIdPayload);
      commit('clearCommandAndResult');      
      persistState();
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
      console.log('executeCommand completed');
    }
    catch(ex)
    {
      console.log('ExecuteCommand error',ex);
      throw ex;
    }
  },
  async loadCommandList({commit}: ActionContext<AppState,AppState>) {
    try {      
      if(state.commands.length==0)
      {
        const response = await commandApi.apiV1CommandGet(getAxiosOptions());
        commit('setCommandList', response.data);
      }
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
  },
  //plugins: [vuexLocal.plugin]
})
