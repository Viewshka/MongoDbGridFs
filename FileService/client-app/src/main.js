import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import 'devextreme/dist/css/dx.common.css';
import 'devextreme/dist/css/dx.softblue.css';
import axios from "axios";
import moment from "moment";

Vue.prototype.$moment = moment;
axios.defaults.withCredentials = true
Vue.config.productionTip = false

import {locale, loadMessages} from 'devextreme/localization';
import ruMessages from 'devextreme/localization/messages/ru.json';
import config from 'devextreme/core/config';

loadMessages(ruMessages);
locale('ru');
config({
  forceIsoDateParsing: true
});

window.Vue = Vue;
new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
