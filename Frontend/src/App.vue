<script setup>
  import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <div class="app">
    <header>
      <nav>
        <RouterLink to="/" class="link home"><i class="bi bi-house"></i></RouterLink>
        <RouterLink to="/dashboard" class="link dashboard"><i class="bi bi-speedometer"></i></RouterLink>
      </nav>
    </header>
    <RouterView class="view" />
  </div>
</template>

<style scoped>
  .app {
    display: flex;
  }
  
  header {
    position: absolute;
    height: 100vh;
    display: flex;
    flex-direction: column;
    justify-content: center;
    z-index: 10;
  }
  
  nav {
    display: flex;
    flex-direction: column;
    row-gap: 0.5em;
    padding: 0.5em;
    font-size: 30px;
  }
  
  .link:link, .link:visited {
    display: flex;
    align-items: center;
    column-gap: 0.1em;
    color: #5981c7;
    text-decoration: none;
  }
  
  .home:hover:after, .dashboard:hover:after {
    font-size: 16px;
    color: black;
  }

  .home:hover:after {
    font-size: 16px;
    content: 'Home';
  }
  
  .dashboard:hover:after {
    font-size: 16px;
    content: 'Dashboard';
  }
</style>
