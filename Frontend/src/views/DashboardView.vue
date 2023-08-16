<script>
  import {useJWTStore} from "@/stores/jwt";
  import axios from "axios";
  import ProductComponent from "@/components/ProductComponent.vue";
  import {useProductsStore} from "@/stores/products";
  
  export default {
    components: {ProductComponent},
    data () {
      return {
        username: null,
        password: null,
        validationError: {
          username: false,
          password: false
        },
        loginErrorMessage: null,
        responseErrorMessage: null,
        seconds: 600,
        interval: null
      }
    },
    
    methods: {
      getAllProducts () {
        axios.get('https://localhost:7020/Api/Products/V1/')
            .then((response) => {
              useProductsStore().setProducts(response.data)
            })
      },
      
      login() {
        this.loginErrorMessage = null
        this.seconds = 600
        clearInterval(this.interval)

        if (!this.username) {
          this.validationError.username = true
        }

        if (!this.password) {
          this.validationError.password = true
        } else {
          this.validationError.username = false
          this.validationError.password = false

          this.$refs.login.disabled = true
          this.$refs.loading.style.visibility = 'visible'
          let animation = this.$refs.loading.animate({transform: 'rotate(360deg)'}, {
            duration: 1000,
            iterations: Infinity
          })

          axios.post('https://localhost:7020/Api/Auth/V1/Login', {
            username: this.username,
            password: this.password
          })
              .then((response) => {
                this.$refs.login.disabled = false
                this.$refs.loading.style.visibility = 'hidden'
                animation.cancel()
                useJWTStore().setJWT(response.data)
                this.getAllProducts()
                this.timer()
              })

              .catch((error) => {
                this.$refs.login.disabled = false
                this.$refs.loading.style.visibility = 'hidden'
                animation.cancel()

                error.response !== undefined
                    ? this.loginErrorMessage = error.response.data
                    : this.loginErrorMessage = 'Something went wrong'
              })

              .finally(() => {
                this.username = null
                this.password = null
              })
        }
      },
      
      logout () {
        useJWTStore().$reset()
        this.seconds = 600
        clearInterval(this.interval)
      },
      
      updateProduct (editedProduct) {
        axios({
          method: 'patch',
          url: 'https://localhost:7020/Api/Products/V1/Edit/',
          data: editedProduct,
          headers: {'Authorization': 'Bearer ' + this.token}
        })
            .then (this.getAllProducts)

            .catch((error) => {
              if (error.response.status === 401) {
                this.loginErrorMessage = 'Session expired, please login'
                this.responseErrorMessage = null
                useJWTStore().$reset()
              } else if (error.response.status === 404) {
                this.responseErrorMessage = error.response.data
              } else {
                this.responseErrorMessage = 'Something went wrong'
              }
            })
      },

      removeProduct (id) {
        axios.delete('https://localhost:7020/Api/Products/V1/Delete/' + id, {
          headers: {'Authorization': 'Bearer ' + this.token}
        })
            .then (this.getAllProducts)
            
            .catch((error) => {
              if (error.response.status === 401) {
                this.loginErrorMessage = 'Session expired, please login'
                this.responseErrorMessage = null
                useJWTStore().$reset()
              } else if (error.response.status === 404) {
                this.responseErrorMessage = error.response.data
              } else {
                this.responseErrorMessage = 'Something went wrong'
              }
            })
      },
      
      timer () {
        const tenMinutes = Date.now() + 10 * 60 * 1000
        
        this.interval = setInterval (() => {
          this.seconds = Math.ceil((tenMinutes - Date.now()) / 1000)
          
          if (this.seconds < 0) {
            this.seconds = 600
            clearInterval(this.interval)
          }
        }, 500)
      }
    },
    
    computed: {
      products () {
        return useProductsStore().getProducts
      },
      
      token () {
        return useJWTStore().getJWT
      },
      
      time () {
        return {
          minutes: Math.floor(this.seconds / 60) < 10 ? '0' + Math.floor(this.seconds / 60) : Math.floor(this.seconds / 60),
          seconds: this.seconds % 60 < 10 ? '0' + this.seconds % 60 : this.seconds % 60
        }
      }
    }
  }
  
</script>

<template>
  <div class="window">
    <div v-if="token" class="timerAndLogout">
      <p>Session:</p>
      
      <div class="timer">
        <p>{{ time.minutes }}</p><p>:</p><p>{{ time.seconds }}</p>
      </div>
    
      <button @click="logout" class="logout">Logout</button>
    </div>
    
    <div class="formContainer" v-if="!token">
      <p>This is for authorized users only, please login to continue</p>
      <form>
        <div class="username">
          <label for="username">Username</label>
          <input v-model="username" id="username" type="text" required />
          <p v-if="validationError.username" class="error">This field is required</p>
        </div>
        
        <div class="password">
          <label for="password">Password</label>
          <input v-model="password" id="password" type="password" required />
          <p v-if="validationError.password" class="error">This field is required</p>
        </div>
        
        <div class="loginLoading">
          <button ref="login" @click.prevent="login">Login</button>
          <div ref="loading" class="loading"></div>
        </div>
      </form>
    </div>
    
    <p v-if="loginErrorMessage" class="error responseError">{{ loginErrorMessage }}</p>

    <button v-if="token" class="refresh" @click="getAllProducts">
      <i class="bi bi-arrow-clockwise"></i>
    </button>
    <div v-if="token" class="productsContainer">
      <div class="products">
        <ProductComponent
            v-for="product in products"
            :key="product.id"
            :id="product.id"
            :name="product.name"
            :sku="product.sku"
            :quantity="product.quantity"
            :price="product.price"
            :image-path="product.imagePath"
            :is-editable="true" 
            @remove="removeProduct"
            @update="updateProduct"
        />
      </div>
    </div>
    <p v-if="responseErrorMessage" class="error responseError">{{ responseErrorMessage }}</p>
  </div>
</template>

<style scoped>
  .window {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    row-gap: 0.5em;
    height: 100vh;
    width: 100vw;
  }
  
  .timerAndLogout {
    display: flex;
    column-gap: 0.5em;
    align-items: center;
    position: absolute;
    top: 5px;
    right: 5px;
  }
  
  .timer {
    display: flex;
    font-size: larger;
  }

  .formContainer {
    display: flex;
    flex-direction: column;
    row-gap: 1em;
    padding: 0.5em;
    border-radius: 10px;
    border: 1px solid #6ba7ff;
  }
  
  form {
    display: flex;
    flex-direction: column;
    row-gap: 1em;
  }
  
  .username, .password {
    display: flex;
    flex-direction: column;
    row-gap: 0.4em;
  }
  
  .error {
    font-size: 12px;
    color: #ff5454;
  }
  
  .responseError {
    align-self: center;
    animation-name: fade;
    animation-delay: 5s;
    animation-duration: 2s;
    animation-fill-mode: forwards;
  }
  
  @keyframes fade {
    to {
      color: transparent;
    }
  }
  
  .loginLoading {
    position: relative;
    align-self: center;
    display: flex;
  }
  
  button {
    appearance: none;
    -webkit-appearance: none;
    -moz-appearance: none;
    height: 30px;
    width: 50px;
    align-self: center;
    border-radius: 5px;
    border: none;
    background-color: #6ba7ff;
    color: white;
    font-size: 14px;
  }
  
  button:hover {
    background-color: #6394e3;
    cursor: pointer;
  }
  
  button:active {
    background-color: #5981c7;
    cursor: pointer;
  }
  
  button:disabled {
    background-color: gray;
  }
  
  .loading {
    visibility: hidden;
    position: absolute;
    left: 55px;
    width: 30px;
    height: 28px;
    background-color: transparent;
    border-radius: 50%;
    border: 2px solid transparent;
    border-top: 2px dotted #6ba7ff;
  }
  
  .refresh {
    width: 30px;
  }
  
  .productsContainer {
    max-height: 540px;
    max-width: 830px;
    overflow: auto;
  }
  
  .products {
    display: flex;
    align-items: flex-start;
    flex-wrap: wrap;
    gap: 0.5em;
  }
</style>