<script>
  import CellComponent from "../components/CellComponent.vue";
  import ProductComponent from "../components/ProductComponent.vue";
  import axios from "axios";
  
  export default {
    components: {
      CellComponent, 
      ProductComponent
    },
    
    data() {
      return {
        products: [{id: -1, imagePath: "assets/Hat.svg", name: "A suspiciously long name for a regular hat", price: 99.9, quantity: 1000000, sku: "HX421RO"},
          {id: -2, imagePath: "assets/Hoodie.svg", name: "Hoodie", price: 16.9, quantity: 40, sku: "HX421RO"},
          {id: -3, imagePath: "assets/Jacket.svg", name: "Jacket", price: 99999.9, quantity: 40, sku: "HX421RO"},
          {id: -4, imagePath: "assets/Pants.svg", name: "Pants", price: 16.9, quantity: 1, sku: "HX421RO"},
          {id: -5, imagePath: "assets/Shirt.svg", name: "Shirt", price: 16.9, quantity: 40, sku: "HX421RO"},
          {id: -6, imagePath: "assets/Shoes.svg", name: "Shoes", price: 16.9, quantity: 40, sku: "HX421RO"}],
        
        productsToUpdate: []
      }
    },
    
    methods: {
      getAvailableProducts() {
        axios.get('https://localhost:7020/Api/Products/V1/Available')
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
      
      reduceQuantity(product) {
        let index = this.productsToUpdate.findIndex(p => p.id === product.id)
        
        if (index !== -1) {
          if (product.quantity > 0) {
            this.productsToUpdate[index].quantity--
            product.quantity--
          }
          if (this.productsToUpdate[index].quantity === 0)
            this.productsToUpdate.splice(index, 1)
        } else {
          this.productsToUpdate.push({id: product.id, name: product.name, quantity: -1})
          product.quantity--
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
      }
    }
  }
</script>

<template>
  <div class="window">
    <div class="main">
      <div class="grid">
        <CellComponent 
        v-for="i in 30*30"
        />
      </div>
      <div class="productsWrapper">
        <div class="title">
          <p>Products</p>
          <i class="bi bi-arrow-clockwise refresh"
          @click="getAvailableProducts()"
          ></i>
        </div>
        <div class="products">
          <ProductComponent 
          v-for="product in products" 
          :key="product.id"
          :name="product.name"
          :price="product.price"
          :quantity="product.quantity"
          :image-path="product.imagePath"
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
          <button :disabled="productsToUpdate.length < 1" @click="updateQuantities">
            <i title="Modify quantities" class="bi bi-tools"></i>
          </button>
          
          <button :disabled="productsToUpdate.length < 1 || productsToUpdate.some(p => p.quantity > 0)" @click="updateQuantities">
            <i title="Mark as sold" class="bi bi-bag sold"></i>
          </button>
        </div>
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
    border: 1px solid red;
    justify-content: space-around;
    align-self: center;
  }
  
  .grid {
    display: grid;
    align-self: center;
    grid-template-columns: repeat(30, 20px);
    grid-template-rows: repeat(30, 20px);
    border: 1px solid black;
  }
  
  .productsWrapper {
    display: flex;
    flex-direction: column;
    height: 700px;
    width: 355px;
    align-self: center;
    border: 3px solid #6ba7ff;
    border-radius: 20px;
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
    justify-content: center;
    flex-wrap: wrap;
    min-height: 450px;
    max-height: 450px;
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
  }
  
  .productActions {
    display: flex;
    justify-content: flex-end;
    padding: 0.5em;
    border-top: 3px solid #6ba7ff;
    column-gap: 0.5em;
  }
  
  button {
    padding: 0.3em;
    font-size: 1.1em;
    background-color: lightgray;
    border: none;
    border-radius: 5px;
  }
</style>
