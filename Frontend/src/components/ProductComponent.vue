<script>
  export default {
    name: "ProductComponent",
    
    props: ['name', 'price', 'quantity', 'imagePath']
  }
</script>

<template>
  <div class="pane">
    <img :src="'src/'+imagePath" :alt="name + ' image'"/>
    <div class="namePrice">
    <p>{{ name }}</p>
    <p>${{ price }}</p>
    </div>
    <div class="quantity">
      <i class="bi bi-dash"></i>
      {{ quantity }}
      <i class="bi bi-plus"></i>
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
    border: 1px solid blue;
  }
  
  img {
    align-self: center;
    height: 45px;
    width: 45px;
    padding: 0.5em;
  }
  
  .namePrice {
    display: flex;
    justify-content: space-between;
    padding: 0.5em;
  }
  
  p {
    margin: 0;
    padding: 0;
  }
  
  .quantity {
    align-self: center;
    padding: 0.5em;
  }
</style>