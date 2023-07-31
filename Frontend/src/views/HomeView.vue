<script>
  import ProductComponent from "../components/ProductComponent.vue";
  import ShelfComponent from "../components/ShelfComponent.vue";
  import axios from "axios";
  import {v4 as uuidv4} from "uuid";
  
  export default {
    components: {
      ShelfComponent,
      ProductComponent
    },
    
    data() {
      return {
        blocks: [],
        
        products: [{id: -1, imagePath: "assets/Hat.svg", name: "Hat", price: 99.9, quantity: 1000000, sku: "HX421RO"},
          {id: -2, imagePath: "assets/Hoodie.svg", name: "Hoodie", price: 16.9, quantity: 0, sku: "HX421RO"},
          {id: -3, imagePath: "assets/Jacket.svg", name: "Jacket", price: 99999.9, quantity: 40, sku: "HX421RO"},
          {id: -4, imagePath: "assets/Pants.svg", name: "Pants", price: 16.9, quantity: 1, sku: "BB432NX"},
          {id: -5, imagePath: "assets/Shirt.svg", name: "Shirt", price: 16.9, quantity: 40, sku: "HX421RO"},
          {id: -13, imagePath: "assets/Shirt.svg", name: "Shirt", price: 16.9, quantity: 40, sku: "HX421RO"},
          {id: -12, imagePath: "assets/Shirt.svg", name: "Shirt", price: 16.9, quantity: 40, sku: "HX421RO"},
          {id: -11, imagePath: "assets/Shirt.svg", name: "Shirt", price: 16.9, quantity: 40, sku: "HX421RO"},
          {id: -1543, imagePath: "assets/Shirt.svg", name: "Shirt", price: 16.9, quantity: 40, sku: "HX421RO"},
          {id: -132, imagePath: "assets/Shirt.svg", name: "Shirt", price: 16.9, quantity: 40, sku: "HX421RO"},
          {id: -1321, imagePath: "assets/Shirt.svg", name: "Shirt", price: 16.9, quantity: 40, sku: "HX421RO"},
          {id: -1765, imagePath: "assets/Shirt.svg", name: "Shirt", price: 16.9, quantity: 40, sku: "HX421RO"},
          {id: -1432, imagePath: "assets/Shirt.svg", name: "Shirt", price: 16.9, quantity: 40, sku: "HX421RO"},
          {id: -6, imagePath: "assets/Shoes.svg", name: "Shoes", price: 16.9, quantity: 40, sku: "HX421RO"}],
        
        availableProducts: [{id: -1, imagePath: "assets/Hat.svg", name: "A suspiciously long name for a regular hat", price: 99.9, quantity: 10000000, sku: "HX421RO"},
          {id: -2, imagePath: "assets/Hoodie.svg", name: "Hoodie", price: 16.9, quantity: 40, sku: "HX421RO"},
          {id: -3, imagePath: "assets/Jacket.svg", name: "Jacket", price: 99999.9, quantity: 40, sku: "HX421RO"},
          {id: -4, imagePath: "assets/Pants.svg", name: "Pants", price: 16.9, quantity: 1, sku: "HX421RO"},
          {id: -5, imagePath: "assets/Shirt.svg", name: "Shirt", price: 16.9, quantity: 40, sku: "HX421RO"},
          {id: -6, imagePath: "assets/Shoes.svg", name: "Shoes", price: 16.9, quantity: 40, sku: "HX421RO"}],
        
        productsToUpdate: [],
        
        deliveryProducts: [],
        
        mouseX: 0,
        
        mouseY: 0,
        
        filter: null,
        
        opacity: 1,
        
        blur: 0,
        
        showDeliveryPanel: false,
        
        showSearchProducts: false,
        
        showEditableProduct: false
      }
    },
    
    methods: {
      getAvailableProducts() {
        axios.get('https://localhost:7020/Api/Products/V1/Available')
            .then((response) => {
              this.availableProducts = response.data
            })
      },
      
      getAllProducts() {
        axios.get('https://localhost:7020/Api/Products/V1/')
            .then((response) => {
              this.products = response.data
            })
      },

      updateQuantities() {
        if (this.productsToUpdate.length < 1)
          return

        axios({
          method: 'patch',
          url: 'https://localhost:7020/Api/Products/V1/UpdateQuantities',
          data: this.productsToUpdate
        })

        this.productsToUpdate.length = 0
      },
      
      addDelivery () {
        if (this.deliveryProducts.length < 1)
          return
        
        axios({
          method: 'put',
          url: 'https://localhost:7020/Api/Products/V1/Delivery',
          data: this.deliveryProducts
        })
            .then(() => {
              this.deliveryProducts.length = 0
              this.showEditableProduct = false
            })
      },
      
      addShelf() {
        this.blocks.push({id: uuidv4(), type: 'shelf'})
      },
      
      readjustBlockPosition(blockId, blockTop, blockLeft) {
        let gridPos = this.$refs.grid.getBoundingClientRect()
        
        if (
            blockTop + 11 <= gridPos.top || 
            blockTop + 11 >= gridPos.bottom || 
            blockLeft + 11 <= gridPos.left || 
            blockLeft + 11 >= gridPos.right
        ) {
          let index = this.blocks.findIndex(b => b.id === blockId)
          this.blocks.splice(index, 1)
        } else {
          return {
            top: (blockTop - gridPos.top) % 20,
            left: (blockLeft - gridPos.left) % 20
          }
        }
      },

      reduceQuantity(product) {
        if (product.quantity > 0) {
          let index = this.productsToUpdate.findIndex(p => p.id === product.id)

          if (index !== -1) {
            this.productsToUpdate[index].quantity--
            product.quantity--

            if (this.productsToUpdate[index].quantity === 0)
              this.productsToUpdate.splice(index, 1)
          } else {
            this.productsToUpdate.push({id: product.id, name: product.name, quantity: -1})
            product.quantity--
          }
        }
      },

      increaseQuantity(product) {
        let index = this.productsToUpdate.findIndex(p => p.id === product.id)

        if (index !== -1) {
          this.productsToUpdate[index].quantity++
          product.quantity++

          if (this.productsToUpdate[index].quantity === 0)
            this.productsToUpdate.splice(index, 1)
        } else {
          this.productsToUpdate.push({id: product.id, name: product.name, quantity: 1})
          product.quantity++
        }
      },

      openDeliveryPanel() {
        this.showDeliveryPanel = true
        this.productsToUpdate.length = 0
        this.opacity = 0.3
        this.blur = 1
        
        this.getAllProducts()
      },

      closeDeliveryPanel() {
        this.showDeliveryPanel = false
        this.showEditableProduct = false
        this.opacity = 1
        this.blur = 0
        this.deliveryProducts.length = 0
      },

      addDeliveryProduct(product) {
        let index = this.deliveryProducts.findIndex(p => p.id === product.id)

        if (index === -1) {
          this.deliveryProducts.push(
              {
                id: product.id,
                sku: product.sku,
                name: product.name,
                price: product.price,
                quantity: 0,
                imagePath: product.imagePath
              })
        }
      },
      
      removeDeliveryProduct(id) {
        let index = this.deliveryProducts.findIndex(p => p.id === id)
        
        this.deliveryProducts.splice(index, 1)
      },
      
      reduceDeliveryProductQuantity (id) {
        let index = this.deliveryProducts.findIndex(p => p.id === id)
        
        if (this.deliveryProducts[index].quantity > 0)
          this.deliveryProducts[index].quantity--
      },

      increaseDeliveryProductQuantity (id) {
        let index = this.deliveryProducts.findIndex(p => p.id === id)
        
        this.deliveryProducts[index].quantity++
      },
      
      addNewProduct(id, sku, name, quantity, price, imagePath) {
        this.deliveryProducts.unshift(
            {
              id: id, 
              sku: sku, 
              name: name, 
              quantity: quantity, 
              price: price,
              imagePath: imagePath
            })
        
        this.showEditableProduct = false
      },
      
      cancelAddingNewProduct() {
        this.showEditableProduct = false
      }
    },
    
    computed: {
      searchProduct() {
        if (this.filter == null)
          return this.products
        
        if (this.products.filter(p => p.name.toLowerCase().includes(this.filter.toLowerCase())).length < 
            this.products.filter(p => p.sku.toLowerCase().includes(this.filter.toLowerCase())).length) {
          return this.products.filter(p => p.sku.toLowerCase().includes(this.filter.toLowerCase()))
        }

        return this.products.filter(p => p.name.toLowerCase().includes(this.filter.toLowerCase()))
      }
    }
  }
</script>

<template>
  <div class="window" @mousemove="(e) => {mouseX = e.pageX; mouseY = e.pageY}">
    <div class="main" :style="{'opacity': opacity, 'filter': 'blur('+blur+'px)'}" @click="showSearchProducts=false">
      <div class="gridAndBlocks">
        <div class="grid" ref="grid">
          <div class="cell" v-for="i in 30*30" :key="i"/>
        </div>
        <div class="blocks">
          <div class="shelfCreator" @click="addShelf"></div>
          <ShelfComponent
              v-for="block in blocks"
              :key="block.id"
              :id="block.id"
              :mouse-x="mouseX"
              :mouse-y="mouseY"
              :readjust-position="readjustBlockPosition"
          />
        </div>
      </div>
      <div class="productsWrapper">
        <div class="title">
          <p>Products</p>
          <i title="Refresh products" class="bi bi-arrow-clockwise refresh" @click="showDeliveryPanel ? undefined : getAvailableProducts()"></i>
        </div>
        <div class="products">
          <ProductComponent 
              v-for="product in availableProducts" 
              :key="product.id"
              :name="product.name"
              :price="product.price"
              :quantity="product.quantity"
              :image-path="product.imagePath"
              @reduce-quantity="reduceQuantity(product)"
              @increase-quantity="increaseQuantity(product)"
              :show-delivery-panel="showDeliveryPanel"
          />
        </div>
        <div class="productsSummary">
          <div class="summary">
            <p>Summary</p>
          </div>
          
          <div class="productsToUpdate">
            <div v-for="product in productsToUpdate">
              {{product.name}} {{product.quantity > 0 ? '+' + product.quantity : product.quantity}}
            </div>
          </div>
        </div>
        <div class="productActions">
          <button class="action" title="Add delivery" :disabled="showDeliveryPanel" @click="openDeliveryPanel">
            <i class="bi bi-truck"></i>
          </button>
          
          <button 
              class="action"
              title="Adjust quantities" 
              :disabled="productsToUpdate.length < 1 || showDeliveryPanel" 
              @click="updateQuantities"
          >
            <i class="bi bi-tools"></i>
          </button>
          
          <button 
              class="action"
              title="Mark as sold" 
              :disabled="productsToUpdate.length < 1 || productsToUpdate.some(p => p.quantity > 0) || showDeliveryPanel" 
              @click="updateQuantities"
          >
            <i class="bi bi-bag sold"></i>
          </button>
        </div>
      </div>
    </div>
    <div class="deliveryPanel" v-if="showDeliveryPanel">
      <div class="deliveryTitle">
        <p>Delivery</p>
        <div class="searchContainer">
          <input 
              class="search" 
              placeholder="Search for a product..."
              @click="showSearchProducts=true" 
              @keydown.esc="showSearchProducts=false"
              v-model="filter"
          />
          <div class="searchProducts" v-if="showSearchProducts">
            <div 
                class="searchProduct" 
                v-for="product in searchProduct" 
                :key="product.id" 
                @click="addDeliveryProduct(product)"
            >
              {{product.name}} [{{product.sku}}] ({{product.quantity}})
            </div>
          </div>
        </div>
        <i title="Close" class="bi bi-x-circle exitButton" @click="closeDeliveryPanel"></i>
      </div>
      <div class="deliveryProducts" @click="showSearchProducts=false">
        <ProductComponent
            v-if="showEditableProduct"
            :is-editable="showEditableProduct"
            @cancel-adding-product = "cancelAddingNewProduct"
            @add-new-product="addNewProduct"
        />
        <ProductComponent 
            v-for="product in deliveryProducts"
            :key="product.id"
            :sku="product.sku"
            :name="product.name"
            :price="product.price"
            :quantity="product.quantity"
            :image-path="product.imagePath"
            :is-delivery="true"
            @reduce-delivery-quantity="reduceDeliveryProductQuantity(product.id)"
            @increase-delivery-quantity="increaseDeliveryProductQuantity(product.id)"
            @remove-delivery-product="removeDeliveryProduct(product.id)"
        />
      </div>
      <div class="deliveryButtons">
        <button class="action" title="Add new product" @click="showEditableProduct = true">
          <i class="bi bi-bag-plus"></i>
        </button>
        <button class="action" title="Remove all products" @click="deliveryProducts.length = 0">
          <i class="bi bi-cart-x"></i>
        </button>
        <button class="action" title="Finalize delivery" @click="addDelivery">
          <i class="bi bi-cart4"></i>
        </button>
      </div>
    </div>
  </div>
</template>

<style>
  .window {
    display: flex;
    justify-content: center;
    min-height: 100vh;
    min-width: 100vw; /* fix .grid hiding */
  }
  
  .main {
    display: flex;
    column-gap: 3em;
    justify-content: space-around;
    align-self: center;
  }

  .gridAndBlocks {
    display: flex;
    flex-direction: column;
    justify-content: space-between;
  }
  
  .grid {
    display: grid;
    align-self: center;
    grid-template-columns: repeat(30, 20px);
    grid-template-rows: repeat(30, 20px);
    border-left: 1px solid black;
    border-top: 1px solid black;
  }

  .cell {
    box-sizing: border-box;
    height: 20px;
    width: 20px;
    border-right: 1px solid black;
    border-bottom: 1px solid black;
  }
  
  .blocks {
    display: flex;
    justify-content: center;
    align-items: center;
    height: 100px;
  }
  
  .shelfCreator {
    width: 50px;
    height: 50px;
    background-color: #6394e3;
  }
  
  .productsWrapper {
    display: flex;
    flex-direction: column;
    height: 700px;
    width: 355px;
    align-self: center;
    border: 3px solid #6ba7ff;
    border-radius: 15px;
  }
  
  .title {
    display: flex;
    justify-content: center;
    padding: 0.5em;
  }
  
  .title p {
    display: flex;
    justify-content: center;
    margin: 0;
    padding: 5px;
    width: 90%;
    font-size: larger;
  }
  
  .refresh {
    display: flex;
    justify-content: center;
    width: 10%;
    padding: 8px;
  }
  
  .products {
    display: flex;
    justify-content: flex-start;
    flex-wrap: wrap;
    gap: 0.7em;
    min-height: 450px;
    max-height: 450px;
    padding: 0.5em;
    overflow-y: scroll;
    border-top: 3px solid #6ba7ff;
    border-bottom: 3px solid #6ba7ff;
  }
  
  .productsSummary {
    display: flex;
    flex-direction: column;
    height: 100%;
    overflow-y: scroll;
  }
  
  .productsSummary p {
    margin: 0;
    padding: 0.3em;
    font-size: large;
    border-bottom: 1px solid gray;
  }
  
  .productsToUpdate {
    padding: 0.3em;
    user-select: auto;
    -webkit-user-select: auto;
  }
  
  .productActions {
    display: flex;
    justify-content: flex-end;
    padding: 0.5em;
    border-top: 3px solid #6ba7ff;
    column-gap: 0.5em;
  }
  
  .action {
    padding: 0.3em;
    font-size: 1.1em;
    background-color: lightgray;
    border: none;
    border-radius: 5px;
  }
  
  .action:enabled:hover {
    background-color: darkgray;
  }
  
  .deliveryPanel {
    display: flex;
    flex-direction: column;
    position: absolute;
    align-self: center;
    height: 475px;
    width: 675px;
    border: 2px solid black;
    border-radius: 10px;
    background-color: white;
  }
  
  .deliveryTitle {
    display: flex;
    justify-content: space-between;
    height: 6%;
    padding: 0.5em;
    border-bottom: 1px solid black;
  }
  
  .deliveryTitle p {
    margin: 0;
    font-size: larger;
    align-self: center;
  }
  
  .searchContainer {
    position: relative;
  }
  
  .search {
    height: 20px;
    width: 300px;
    font-size: large;
  }
  
  .searchProducts {
    position: absolute;
    z-index: 1;
    width: 304px;
    max-height: 295px;
    overflow-y: auto;
    background-color: white;
    border-left: 1px solid black;
    border-bottom: 1px solid black;
    border-right: 1px solid black;
  }
  
  .searchProduct {
    padding: 0.3em;
    border-bottom: 1px solid black;
  }
  
  .searchProduct:nth-last-child(1) {
    border-bottom: none;
  }
  
  .searchProducts:hover {
    cursor: pointer;
  }
  
  .exitButton {
    display: flex;
    align-self: center;
    height: 21px;
    border-radius: 50%;
    font-size: 130%;
  }
  
  .exitButton:hover {
    background-color: #ff5454;
  }
  
  .deliveryProducts {
    display: flex;
    flex-wrap: wrap;
    align-items: flex-start;
    height: 87%;
    gap: 0.5em;
    padding: 0.5em;
    overflow-y: auto;
  }
  
  .deliveryButtons {
    display: flex;
    justify-content: flex-end;
    column-gap: 0.5em;
    padding: 0.5em;
    height: 7%;
    border-top: 1px solid black;
  }
</style>
