<script>
  export default {
    name: "SearchPanelComponent",
    
    props: ['products', 'show', 'width', 'height', 'fontSize', 'isShelf'],
    
    emits: ['addProduct'],
    
    data () {
      return {
        filter: null
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
  <div class="searchContainer">
    <input
        class="search"
        placeholder="Search for a product..."
        v-model="filter"
    />
    <div class="searchProducts" v-if="show">
      <div
          class="searchProduct"
          v-for="product in searchProduct"
          :key="product.id"
          @click="this.$emit('addProduct', product)"
      >
        {{product.name}} [{{product.sku}}] ({{product.quantity}})
      </div>
    </div>
  </div>
</template>

<style scoped>
  .searchContainer {
    position: relative;
    width: v-bind(width);
  }
  
  .search {
    height: 20px;
    width: v-bind(width);
    box-sizing: border-box;
    -webkit-box-sizing: border-box;
    -moz-box-sizing: border-box;
    font-size: medium;
  }
  
  .searchProducts {
    position: absolute;
    z-index: 1;
    width: v-bind(width);
    max-height: v-bind(height);
    font-size: v-bind(fontSize);
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
</style>