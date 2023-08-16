<script>
  import ProductComponent from "../components/ProductComponent.vue";
  import ShelfComponent from "../components/ShelfComponent.vue";
  import axios from "axios";
  import {v4 as uuidv4} from "uuid";
  import SearchPanelComponent from "@/components/SearchPanelComponent.vue";
  import NewProductComponent from "@/components/NewProductComponent.vue";
  import {useProductsStore} from "@/stores/products";
  
  export default {
    components: {
      NewProductComponent,
      SearchPanelComponent,
      ShelfComponent,
      ProductComponent
    },
    
    data() {
      return {
        blocks: [],
        
        productsToUpdate: [],
        
        deliveryProducts: [],
        
        mouseX: 0,
        
        mouseY: 0,
        
        gridX: null,
        
        gridY: null,
        
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
              useProductsStore().setAvailableProducts(response.data)
            })
      },
      
      getAllProducts() {
        axios.get('https://localhost:7020/Api/Products/V1/')
            .then((response) => {
              useProductsStore().setProducts(response.data)
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
      
      addShelf(width, height) {
        this.blocks.push({id: uuidv4(), type: 'shelf', width: width, height: height})
      },
      
      readjustBlockPosition(blockId, blockTop, blockLeft, blockWidth, blockHeight) {
        let gridPos = this.$refs.grid.getBoundingClientRect()
        
        if (
            blockTop + 11 <= gridPos.top || 
            blockTop + blockHeight - 11 >= gridPos.bottom || 
            blockLeft + 11 <= gridPos.left || 
            blockLeft + blockWidth - 11 >= gridPos.right
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

            if (this.productsToUpdate[index].quantity === 0)
              this.productsToUpdate.splice(index, 1)
          } else {
            this.productsToUpdate.push({id: product.id, name: product.name, quantity: -1})
          }
        }
      },

      increaseQuantity(product) {
        let index = this.productsToUpdate.findIndex(p => p.id === product.id)

        if (index !== -1) {
          this.productsToUpdate[index].quantity++

          if (this.productsToUpdate[index].quantity === 0)
            this.productsToUpdate.splice(index, 1)
        } else {
          this.productsToUpdate.push({id: product.id, name: product.name, quantity: 1})
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
      
      addNewProduct(id, product) {
        this.deliveryProducts.unshift(
            {
              id: id, 
              sku: product.sku, 
              name: product.name, 
              quantity: product.quantity, 
              price: product.price,
              imagePath: product.imagePath
            })
        
        this.showEditableProduct = false
      },
      
      cancelAddingNewProduct() {
        this.showEditableProduct = false
      }
    },
    
    computed: {
      availableProducts () {
        return useProductsStore().getAvailableProducts 
      },
      
      products () {
        return useProductsStore().getProducts
      }
    }
  }
</script>

<template>
  <div class="window" @mousemove="(e) => {this.mouseX = e.clientX; this.mouseY = e.clientY}">
    <div class="main" :style="{'opacity': opacity, 'filter': 'blur(' + blur + 'px)'}" @click="showSearchProducts=false">
      <div class="gridAndBlocks">
        <div 
            class="grid"
            ref="grid" 
            @mousemove="gridX = this.$refs.grid.getBoundingClientRect().x; gridY = this.$refs.grid.getBoundingClientRect().y"
        >
          <div class="cell" v-for="i in 30*30" :key="i"/>
          <ShelfComponent
              v-for="block in blocks"
              :key="block.id"
              :id="block.id"
              :width="block.width"
              :height="block.height"
              :mouse-x="mouseX"
              :mouse-y="mouseY"
              :grid-x="gridX"
              :grid-y="gridY"
              :readjust-position="readjustBlockPosition"
              :available-products="availableProducts"
          />
        </div>
        <div class="blocks">
          <div class="horizontalBlocks">
            <div class="shelf1x1" @click="addShelf(20,20)"></div>
            <div class="shelf2x1" @click="addShelf(40,20)"></div>
            <div class="shelf3x1" @click="addShelf(60,20)"></div>
            <div class="shelf4x1" @click="addShelf(80,20)"></div>
            <div class="shelf5x1" @click="addShelf(100,20)"></div>
            <div class="shelf6x1" @click="addShelf(120,20)"></div>
          </div>
          <div class="verticalBlocks">
            <div class="shelf1x2" @click="addShelf(20,40)"></div>
            <div class="shelf1x3" @click="addShelf(20,60)"></div>
            <div class="shelf1x4" @click="addShelf(20,80)"></div>
            <div class="shelf1x5" @click="addShelf(20,100)"></div>
            <div class="shelf1x6" @click="addShelf(20,120)"></div>
          </div>
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
              :quantity="product.quantity"
              :price="product.price"
              :image-path="product.imagePath"
              @remove="removeDeliveryProduct"
              @reduce-quantity="reduceQuantity(product)"
              @increase-quantity="increaseQuantity(product)"
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
        
        <SearchPanelComponent
            :products="products"
            :show="showSearchProducts"
            :width="'300px'"
            :height="'295px'"
            @add-product="addDeliveryProduct"
            @click="showSearchProducts=true"
            @keydown.esc="showSearchProducts=false"
            style="align-self: center"
        />
        
        <i title="Close" class="bi bi-x-circle exitButton" @click="closeDeliveryPanel"></i>
      </div>
      <div class="deliveryProducts" @click="showSearchProducts=false">
        <NewProductComponent
            v-if="showEditableProduct"
            @cancel="cancelAddingNewProduct"
            @add="addNewProduct"
        />
        <ProductComponent 
            v-for="product in deliveryProducts"
            :key="product.id"
            :id="product.id"
            :sku="product.sku"
            :name="product.name"
            :quantity="product.quantity"
            :price="product.price"
            :image-path="product.imagePath"
            :is-delivery="true"
            @remove="removeDeliveryProduct"
            @reduce-quantity="reduceDeliveryProductQuantity"
            @increase-quantity="increaseDeliveryProductQuantity"
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

<style scoped>
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
    position: relative;
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
    margin-top: 0.5em;
    display: flex;
    justify-content: space-between;
  }
  
  .horizontalBlocks, .verticalBlocks {
    display: flex;
    gap: 0.3em;
  }
  
  .horizontalBlocks {
    flex-direction: column;
  }
  
  .horizontalBlocks > *, .verticalBlocks > * {
    border: 1px solid black;
    background-color: #6394e3;
    box-sizing: border-box;
    -webkit-box-sizing: border-box;
    -moz-box-sizing: border-box;
  }
  
  .shelf1x1 {
    width: 20px;
    height: 20px;
  }
  
  .shelf2x1 {
    width: 40px;
    height: 20px;
  }

  .shelf3x1 {
    width: 60px;
    height: 20px;
  }
  
  .shelf4x1 {
    width: 80px;
    height: 20px;
  }

  .shelf5x1 {
    width: 100px;
    height: 20px;
  }
  
  .shelf6x1 {
    width: 120px;
    height: 20px;
  }
  
  .shelf1x2 {
    width: 20px;
    height: 40px;
  }

  .shelf1x3 {
    width: 20px;
    height: 60px;
  }

  .shelf1x4 {
    width: 20px;
    height: 80px;
  }
  
  .shelf1x5 {
    width: 20px;
    height: 100px;
  }
  
  .shelf1x6 {
    width: 20px;
    height: 120px;
  }
  
  .productsWrapper {
    display: flex;
    flex-direction: column;
    height: 745px;
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
  
  .refresh:hover {
    cursor: pointer
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
