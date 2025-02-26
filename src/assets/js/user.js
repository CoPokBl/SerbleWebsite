import { reactive, readonly } from 'vue';
import {checkLogin, logout} from "@/assets/js/serble.js";

const state = reactive({
    user: null
});

// Exposing read-only state to prevent direct mutations
const useUserStore = () => {
    return {
        state: readonly(state),
        async initializeAuth() {
            const user = await checkLogin();
            if (user) {
                state.user = user;
                console.log(user);
            }
        },
        logout() {
            state.user = null;
            logout();
        }
    };
};

export default useUserStore;