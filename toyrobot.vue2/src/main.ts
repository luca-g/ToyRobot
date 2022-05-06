import Vue from 'vue'
import router from './router'
import VueCompositionAPI from '@vue/composition-api'
import vuetify from './plugins/vuetify'

Vue.use(VueCompositionAPI)
Vue.config.productionTip = false

import App from './App.vue'
import store from './store'

new Vue({
  router,
  store,
  vuetify,
  render: h => h(App)
}).$mount('#app')
