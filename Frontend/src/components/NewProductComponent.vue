<script>
  import { v4 as uuidv4 } from 'uuid'
  export default {
    emits: ['add', 'cancel'],
    
    data () {
      return {
        newProduct: {
          imagePath: null, 
          name: null, 
          sku: null, 
          price: '0', 
          quantity: '0'
        },
        
        validationError: null
      }
    },
   
    methods: {
      makeValidPrice () {
        isNaN(parseFloat(this.newProduct.price)) 
            ? this.newProduct.price = '0.00' 
            : this.newProduct.price = parseFloat(this.newProduct.price).toFixed(2)
      },
      
      addProduct () {
        if (!this.newProduct.sku || 
            !this.newProduct.name || 
            this.newProduct.quantity === '' ||
            this.newProduct.price === ''
        ) {
          this.validationError = 'Fill required fields'
          return
        } else if (this.newProduct.sku.length > 50 || this.newProduct.name.length > 50) {
          this.validationError = 'SKU or Name too long. Maximum length: 50'
          return
        }
        
        let id = uuidv4()
        
        this.$emit('add', id, this.newProduct)
      }
    }
  }
</script>

<template>
  <div class="pane">
    <div class="cancelButtonContainer">
      <button @click="this.$emit('cancel')" class="iconButton cancel" title="Cancel">
        <i class="bi bi-x-circle"></i>
      </button>
    </div>
    
    <label for="image">Image path</label>
    <input v-model="newProduct.imagePath" id="image" type="text" />

    <label for="image">Name*</label>
    <input v-model="newProduct.name" type="text"/>

    <label for="image">SKU*</label>
    <input v-model="newProduct.sku" type="text"/>

    <label for="image">Price*</label>
    <input 
        v-model="newProduct.price" 
        @input="newProduct.price = newProduct.price.replace(/[^0-9.]/g, '')"
        @focusout="makeValidPrice"
        type="text"/>

    <label for="image">Quantity*</label>
    <input v-model="newProduct.quantity" @input="newProduct.quantity = newProduct.quantity.replace(/[^0-9]/g, '')" type="text"/>

    <p class="required">* required</p>
    <p v-if="validationError" class="validationError">{{ validationError }}</p>

    <button @click="addProduct" class="addButton" title="Add">
      <i class="bi bi-plus-square"></i>
    </button>
  </div>
</template>

<style scoped>
  .pane {
    position: relative;
    display: flex;
    flex-direction: column;
    justify-content: flex-start;
    align-items: center;
    gap: 0.2em;
    padding: 0.5em;
    min-height: 100px;
    width: 155px;
    border: 1px solid #6394e3;
    border-radius: 10px;
    box-sizing: border-box;
    user-select: auto;
    -webkit-user-select: auto;
  }
  
  p {
    margin: 0;
    padding: 0;
  }
  
  button {
    appearance: none;
    -webkit-appearance: none;
    -moz-appearance: none;
    border: none;
  }
  
  button:enabled:hover {
    cursor: pointer;
  }
  
  .iconButton {
    background-color: transparent;
    font-size: medium;
  }

  .cancelButtonContainer {
    display: flex;
    align-self: flex-end;
  }
  
  .cancel:hover {
    color: #ff5454;
  }
  
  .required {
    font-size: x-small;
  }
  
  .validationError {
    font-size: small;
    color: #ff5454;
  }
  
  .addButton {
    width: 35px;
    height: 35px;
    border-radius: 5px;
    background-color: transparent;
    font-size: x-large;
  }
  
  .addButton:hover {
    color: #1dbb1d;
  }
</style>