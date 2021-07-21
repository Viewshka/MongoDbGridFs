import Vue from "vue";
import Router from "vue-router";
import Home from "./views/home";
import DefaultLayout from "./layouts/SideNavOuterToolbar"

Vue.use(Router);

const router = new Router({
    routes: [
        {
            path: "/home",
            name: "home",
            components: {
                layout: DefaultLayout,
                content: Home
            }
        },
        {
            path: "/",
            redirect: "/home"
        },
        {
            path: "*",
            redirect: "/home"
        }
    ]
});

export default router;
