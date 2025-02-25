import { reactive, readonly } from 'vue';
import {checkLogin} from "@/assets/js/serble.js";

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
    };
};

export default useUserStore;