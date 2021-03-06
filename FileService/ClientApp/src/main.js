import Vue from 'vue';
import App from './App.vue';
import router from "./router";
import axios from 'axios';
import enums from './plugins/enums_plugin'

import 'devextreme/dist/css/dx.common.css';
import 'devextreme/dist/css/dx.material.lime.dark.css'

import moment from 'moment';

Vue.prototype.$moment = moment;
axios.defaults.withCredentials = true
/**
 * Русификация DevExtreme компонентов.
 */
import {locale, loadMessages} from 'devextreme/localization';
import ruMessages from 'devextreme/localization/messages/ru.json';
import config from 'devextreme/core/config';


loadMessages(ruMessages);
locale('ru');

/**
 * Глобальная настройка DevExtreme компонентов.
 */

config({
  forceIsoDateParsing: true
});

/**
 * Инициализация Vue приложения.
 */
window.Vue = Vue;
Vue.use(enums)
new Vue({
  router,
  render: h => h(App)
}).$mount("#app");