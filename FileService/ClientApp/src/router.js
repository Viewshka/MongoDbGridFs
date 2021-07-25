import Vue from "vue";
import Router from "vue-router";
import Home from "./views/home";

Vue.use(Router);

const router = new Router({
    routes: [
        {
            path: "/home",
            name: "home",
            component: Home
        },
        {
            path: "*",
            redirect: "/home"
        }
    ]
});

export default router;
