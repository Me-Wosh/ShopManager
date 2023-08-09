<script>
  import {v4 as uuidv4} from "uuid";
  export default {
    name: "ProductComponent",
    
    props: ['sku', 'name', 'price', 'quantity', 'imagePath', 'isDelivery', 'isEditable', 'showDeliveryPanel'],
    
    emits: ['addNewProduct', 'removeDeliveryProduct', 'cancelAddingProduct', 'reduceDeliveryQuantity', 
      'reduceQuantity', 'increaseDeliveryQuantity', 'increaseQuantity'],
    
    data () {
      return {
        image: 'src/' + this.imagePath,
        editableProduct: {sku: '', name: '', quantity: null, price: null, imagePath: null}
      }
    },
    
    methods: {
      addNewProduct() {
        if (this.invalidData) {
          return
        }
        
        let id = uuidv4()
        
        let price = this.editableProduct.price ?? 0
        
        if (price > 0 && price < 0.01)
          price = 0.01
        
        let quantity = this.editableProduct.quantity ?? 0
        
        this.$emit('add-new-product', id, this.editableProduct.sku, this.editableProduct.name, quantity, price, 
                                      this.editableProduct.imagePath)
      }
    },
    
    computed: {
      invalidData() {
        if (!this.editableProduct.sku.trim() || !this.editableProduct.name.trim())
          return "Please fill required information"
        
        if (this.editableProduct.sku.length > 50 || this.editableProduct.name.length > 50)
          return "SKU or Name too long, max length: 50"
      }
    }
  }
</script>

<template>
  <div class="pane">
    <img 
        v-if="!isEditable"
        :src="image" 
        @error="this.image = 'src/assets/No-Image.svg'" 
        :alt="name + ' image'"
    />
    <i 
        v-if="isDelivery || isEditable" 
        :title="isDelivery? 'Remove product' : 'Cancel'" 
        class="bi bi-x-circle removeButton" 
        @click="isDelivery ? this.$emit('remove-delivery-product') : this.$emit('cancel-adding-product')"
    >
    </i>

    <label for="sku" v-if="isEditable">SKU*</label>
    <input id="sku" v-if="isEditable" v-model="editableProduct.sku"/>
    
    <p v-if="!isEditable" class="name">{{ name }}</p>
    
    <label for="name" v-if="isEditable">Name*</label>
    <input id="name" v-if="isEditable" v-model="editableProduct.name"/>
    
    <p class="sku" v-if="isDelivery">[{{sku}}]</p>
    
    <label for="price" v-if="isEditable">Price ($USD)</label>
    <input 
        id="price" 
        v-if="isEditable"
        @input="editableProduct.price = editableProduct.price.replace(/[^0-9.]/g, '')"
        @focusout="isNaN(parseFloat(editableProduct.price)) ? editableProduct.price = 0 : 
                   parseFloat(editableProduct.price) > 0 && parseFloat(editableProduct.price) < 0.01 ? editableProduct.price = 0.01 : 
                   editableProduct.price = parseFloat(editableProduct.price).toFixed(2)"
        v-model="editableProduct.price"
        placeholder="$0.00"
    />
    
    <p v-if="!isEditable" class="price">{{ Intl.NumberFormat("en", { style: "currency", currency: "USD" }).format(price) }}</p>
    
    <label for="imagePath" v-if="isEditable">Image Path</label>
    <input 
        id="imagePath"
        v-if="isEditable" 
        v-model="editableProduct.imagePath" 
        placeholder="None"/>
    
    <div class="quantityContainer">
      <button 
          :disabled="showDeliveryPanel"
          @click="isDelivery ? this.$emit('reduce-delivery-quantity') : 
          isEditable ? (this.editableProduct.quantity > 0 ? this.editableProduct.quantity-- : this.editableProduct.quantity = 0) : 
          this.$emit('reduce-quantity')"
          class="changeQuantityLeft"
      >
        <i class="bi bi-dash changeQuantityIcon"></i>
      </button>
      
      <p v-if="!isEditable" class="quantity">{{ quantity }}</p>
      
      <input
          v-if="isEditable"
          @input="editableProduct.quantity = editableProduct.quantity.replace(/[^0-9]/g, '')"
          v-model="editableProduct.quantity"
          class="quantity"
          placeholder="0"
      />
      
      <button
          :disabled="showDeliveryPanel"
          @click="isDelivery? this.$emit('increase-delivery-quantity') : 
          isEditable ? this.editableProduct.quantity++ : this.$emit('increase-quantity')"
          class="changeQuantityRight"
      >
        <i class="bi bi-plus changeQuantityIcon"></i>
      </button>
    </div>
    
    <p v-if="isEditable" class="required">* required</p>
    
    <p v-if="isEditable" class="errorMessage">{{invalidData}}</p>
    
    <button v-if="isEditable" class="addButton action" @click="addNewProduct">
      <i class="bi bi-plus-lg"></i>
    </button>
  </div>
</template>

<style scoped>
  .pane {
    position: relative;
    display: flex;
    flex-direction: column;
    justify-content: center;
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
  
  input {
    width: 90%;
  }
  
  label {
    margin: 0.2em;
  }
  
  img {
    height: 45px;
    width: 45px;
  }
  
  .removeButton {
    position: absolute;
    display: flex;
    align-self: center;
    top: 5px;
    right: 5px;
    height: 16px;
    border-radius: 50%;
  }
  
  .removeButton:hover {
    background-color: #ff5454;
  }
  
  .name {
    font-size: 1.2em;
    word-wrap: break-word;
  }
  
  p {
    margin: 0;
    padding: 0;
  }

  .quantityContainer {
    display: flex;
    justify-content: center;
    max-width: 150px;
    border: 1px solid gray;
    border-radius: 5px;
  }
  
  .quantity {
    display: flex;
    justify-content: center;
    align-self: center;
    min-width: 1.7em;
    font-size: 1.2em;
    overflow-x: auto;
  }
  
  .changeQuantityLeft, .changeQuantityRight {
    border: none;
    background-color: white;
  }

  .changeQuantityIcon {
    display: flex;
    justify-content: center;
    align-items: center;
    height: 1.35em;
    width: 1.35em;
    font-size: x-large;
  }
  
  .changeQuantityLeft {
    border-right: 1px solid black;
    border-bottom-left-radius: 4px;
    border-top-left-radius: 4px;
  }
  
  .changeQuantityRight {
    border-left: 1px solid black;
    border-bottom-right-radius: 4px;
    border-top-right-radius: 4px;
  }

  .changeQuantityLeft:enabled:active, .changeQuantityRight:enabled:active {
    background-color: lightgray;
  }

  .changeQuantityLeft:disabled, .changeQuantityRight:disabled {
    color: inherit;
  }
  
  .required {
    font-size: smaller;
  }
  
  .errorMessage {
    color: #ff3d3d;
    font-size: small;
  }
  
  .addButton {
    align-self: center;
    border: 1px solid black;
  }
</style>