import { defineStore } from "pinia";

export const useJWTStore = defineStore('jwt', {
    state: () => {
        return {
            token: null,
            expires: null
        }
    },
    
    getters: {
        getJWT () {
            if (this.expires) {
                let interval = setInterval(() => {
                    if (this.expires < Date.now()) {
                        this.token = null
                        this.expires = null

                        clearInterval(interval)
                    }
                }, 1000)
            }
            
            return this.token
        }
    },
    
    actions: {
        setJWT (responseToken) {
            this.token = responseToken
            this.expires = Date.now() + 10 * 60 * 1000 // 10 minutes
        }
    }
})