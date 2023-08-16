import { defineStore } from 'pinia'

export const useProductsStore = defineStore('products', {
    state () {
        return {
            availableProducts: null,
            products: null
        }
    },
    
    getters: {
        getAvailableProducts () {
            return this.availableProducts
        },
        
        getProducts () {
            return this.products
        }
    },
    
    actions: {
        setAvailableProducts (products) {
            this.availableProducts = products
        },
        
        setProducts (products) {
            this.products = products
        }
    }
})