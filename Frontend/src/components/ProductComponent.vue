<script>
  export default {
    name: "ProductComponent",
    
    props: ['name', 'price', 'quantity', 'imagePath'],
    
    data () {
      return {
        image: 'src/' + this.imagePath
      }
    }
  }
</script>

<template>
  <div class="pane">
    <img :src="image" @error="this.image = 'src/assets/No-Image.svg'" :alt="name + ' image'"/>
    <div class="namePrice">
      <p class="name">{{ name }}</p>
      <p class="price">${{ price }}</p>
    </div>
    <div class="quantityContainer">
      <i @click="this.$emit('reduce-quantity')" class="bi bi-dash changeQuantityLeft"></i>
      <p class="quantity">{{ quantity }}</p>
      <i @click="this.$emit('increase-quantity')" class="bi bi-plus changeQuantityRight"></i>
    </div>
  </div>
</template>

<style scoped>
  .pane {
    display: flex;
    flex-direction: column;
    justify-content: center;
    min-height: 100px;
    width: 150px;
    margin: 0.5em;
    border: 1px solid #6394e3;
    border-radius: 10px;
  }
  
  img {
    align-self: center;
    height: 45px;
    width: 45px;
    padding: 0.5em;
  }
  
  img::before {
    background-image: url("@/assets/No-Image.svg");
  }
  
  .namePrice {
    display: flex;
    flex-direction: column;
    padding: 0.5em;
  }
  
  .name {
    font-size: 1.2em;
    align-self: center;
    word-wrap: break-word;
  }
  
  .price {
    align-self: center;
  }
  
  p {
    margin: 0;
    padding: 0;
  }

  .quantityContainer {
    display: flex;
    justify-content: center;
    align-self: center;
    margin-bottom: 0.5em;
    border: 1px solid gray;
    border-radius: 5px;
  }
  
  .quantity {
    display: flex;
    justify-content: center;
    align-self: center;
    min-width: 1.7em;
    font-size: 1.2em;
    
  }
  
  .changeQuantityLeft, .changeQuantityRight {
    font-size: 2em;
    align-self: center;
  }
  
  .changeQuantityLeft {
    border-right: 1px solid gray;
  }
  
  .changeQuantityRight {
    border-left: 1px solid gray;
  }
</style>