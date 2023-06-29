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
        products: [{id: 1, imagePath: "assets/Hat.svg", name: "Hat", price: 16.9, quantity: 40, sku: "HX421RO"},
          {id: 2, imagePath: "assets/Hoodie.svg", name: "Hoodie", price: 16.9, quantity: 40, sku: "HX421RO"},
          {id: 3, imagePath: "assets/Jacket.svg", name: "Jacket", price: 16.9, quantity: 40, sku: "HX421RO"},
          {id: 4, imagePath: "assets/Pants.svg", name: "Pants", price: 16.9, quantity: 40, sku: "HX421RO"},
          {id: 5, imagePath: "assets/Shirt.svg", name: "Shirt", price: 16.9, quantity: 40, sku: "HX421RO"},
          {id: 6, imagePath: "assets/Shoes.svg", name: "Sweater", price: 16.9, quantity: 40, sku: "HX421RO"}
        ]
      }
    },
    
    methods: {
      getAvailableProducts() {
        axios.get('https://localhost:7020/Api/Products/V1/Available')
            .then((response) => {
              this.products = response.data
              console.log(this.products)
            })
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
          />
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
  .window {
    display: flex;
    justify-content: center;
    
  }

  .main {
    display: flex;
    height: 90%;
    width: 90%;
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
    min-height: 600px;
    width: 350px;
    align-self: center;
    border: 3px solid #6ba7ff;
    border-radius: 25px;
  }
  
  .title {
    display: flex;
    justify-content: center;
    padding: 0.5em;
  }
  
  p {
    display: flex;
    justify-content: center;
    margin: 0;
    padding: 5px;
    width: 90%;
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
    
  }
</style>
