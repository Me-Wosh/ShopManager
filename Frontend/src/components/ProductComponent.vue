<script>
  export default {
    props: ['id', 'sku', 'name', 'quantity', 'price', 'imagePath', 'isEditable', 'isDelivery'],
    emits: ['update', 'remove', 'reduceQuantity', 'increaseQuantity'],
    
    data () {
      return {
        image: 'src/' + this.imagePath,
        editableMode: false,
        editedProduct: {
          id: this.id,
          name: this.name,
          sku: this.sku,
          quantity: this.quantity,
          price: this.price,
          imagePath: this.imagePath
        },
        validationError: null
      }
    },
    
    methods: {
      makeValidPrice () {
        isNaN(parseFloat(this.editedProduct.price))
            ? this.editedProduct.price = '0.00'
            : this.editedProduct.price = parseFloat(this.editedProduct.price).toFixed(2)
      },
      
      editProduct () {
        if (!this.editedProduct.sku || 
            !this.editedProduct.name || 
            this.editedProduct.quantity === '' || 
            this.editedProduct.price === ''
        ) {
          this.validationError = 'Please fill required fields'
          return
        } else if (this.editedProduct.sku.length > 50 || this.editedProduct.name.length > 50) {
            this.validationError = 'SKU or Name too long. Maximum length: 50'
            return
        }
        
        this.$emit('update', this.editedProduct) 
        this.editableMode = false
        this.validationError = null
      }
    }
  }
</script>

<template>
  <div class="pane">
    <div v-if="isEditable || isDelivery" class="options" :style="{'justify-content': isEditable ? 'space-between' : 'flex-end'}">
      <button v-if="isEditable" @click="editableMode = !editableMode" class="iconButton edit" title="Edit">
        <i class="bi bi-pencil-square"></i>
      </button>
      
      <button @click="this.$emit('remove', id)" class="iconButton remove" title="Remove">
        <i class="bi bi-x-circle"></i>
      </button>
    </div>
    
    <img :src="image" :alt="name + ' image'" @error="image = 'src/assets/No-Image.svg'"/>
    <input v-if="editableMode" v-model="editedProduct.imagePath" type="text" placeholder="Image path" />
    
    <div class="fieldContainer">
      <p>{{ name }}</p><p v-if="editableMode">*</p>
    </div>
    <input v-if="editableMode" v-model="editedProduct.name" type="text" placeholder="Name" />
    
    <div class="fieldContainer">
      <p v-if="sku">[{{ sku }}]</p><p v-if="editableMode">*</p>
    </div>
    <input v-if="editableMode" v-model="editedProduct.sku" type="text" placeholder="SKU" />

    <div class="fieldContainer">
      <p>{{Intl.NumberFormat("en", { style: "currency", currency: "USD" }).format(price)}}</p><p v-if="editableMode">*</p>
    </div>
    <input 
        v-if="editableMode" 
        v-model="editedProduct.price"
        @input="editedProduct.price = editedProduct.price.replace(/[^0-9.]/g, '')"
        @focusout="makeValidPrice"
        type="text" 
        placeholder="Price" 
    />

    <div class="fieldContainer">
      <p v-if="isEditable">Quantity: {{ quantity }}</p><p v-if="editableMode">*</p>
    </div>
    <input v-if="editableMode" v-model="editedProduct.quantity" @input="editedProduct.quantity = editedProduct.quantity.replace(/[^0-9]/g, '')" type="text" placeholder="Quantity" />

    <div v-if="!isEditable" class="quantityContainer">
      <button @click="this.$emit('reduceQuantity', id)" class="quantityButton">
        <i class="bi bi-dash"></i>
      </button>

      <p>{{ quantity }}</p>

      <button @click="this.$emit('increaseQuantity', id)" class="quantityButton">
        <i class="bi bi-plus"></i>
      </button>
    </div>
    
    <p v-if="editableMode" class="required">* required</p>
    <p v-if="validationError" class="validationError">{{ validationError }}</p>
    
    <button v-if="editableMode" @click="editProduct" class="updateButton">
      Update
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
  
  .options {
    display: flex;
    align-self: normal;
  }
  
  img {
    height: 45px;
    width: 45px;
  }
  
  p {
    margin: 0;
    padding: 0;
  }
  
  .fieldContainer {
    display: flex;
  }
  
  .quantityContainer {
    display: flex;
    align-items: center;
    column-gap: 0.4em;
    border: 1px solid black;
    border-radius: 5px;
    font-size: large;
  }

  .quantityButton {
    padding: 0.1em;
    font-size: x-large;
    background-color: transparent;
  }
  
  .quantityButton:active {
    background-color: lightgray;
  }
  
  .quantityButton:first-child {
    border-top-left-radius: 4px;
    border-bottom-left-radius: 4px;
    border-right: 1px solid black;
  }

  .quantityButton:last-child {
    border-top-right-radius: 4px;
    border-bottom-right-radius: 4px;
    border-left: 1px solid black;
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
  
  .updateButton {
    padding: 0.2em;
    height: 30px;
    align-self: center;
    border-radius: 5px;
    background-color: #6ba7ff;
    color: white;
    font-size: 14px;
  }

  .updateButton:hover {
    background-color: #6394e3;
  }

  .updateButton:active {
    background-color: #5981c7;
  }

  .updateButton:disabled {
    background-color: gray;
  }

  .iconButton {
    display: flex;
    justify-content: center;
    align-items: center;
    background-color: transparent;
    font-size: medium;
  }
  
  .iconButton:disabled {
    color: rgba(0, 0, 0, 0.35);
  }

  .edit:enabled:hover {
    color: #6ba7ff;
  }

  .remove:enabled:hover {
    color: #ff5454;
  }
  
  .required {
    font-size: x-small;
  }
  
  .validationError {
    font-size: small;
    color: #ff5454;
  }
</style>