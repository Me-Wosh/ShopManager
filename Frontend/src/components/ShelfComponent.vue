<script>
  import SearchPanelComponent from "@/components/SearchPanelComponent.vue";

  export default {
    name: "ShelfComponent",
    components: {SearchPanelComponent},
    
    props: ['id', 'width', 'height', 'mouseX', 'mouseY', 'gridX', 'gridY', 'readjustPosition', 'availableProducts'],
    
    data () {
      return {
        previousTop: null,
        top: null,
        previousLeft: null,
        left: null,
        move: false,
        hovered: false,
        showItems: false,
        items: [],
        showSearchItems: false
      }
    },
    
    methods: {
      readjust () {
        this.move = false
        let offset = this.readjustPosition(this.id, this.top + this.gridY, this.left + this.gridX, this.width, this.height)
        
        if (offset !== undefined) {
          this.top = offset.top <= 10 ? this.top - offset.top : this.top + (20 - offset.top)
          this.left = offset.left <= 10 ? this.left - offset.left : this.left + (20 - offset.left)
        }
      },
      
      openItemsPanel () {
        if (Math.max(Math.abs(this.top - this.previousTop), Math.abs(this.left - this.previousLeft)) < 20) {
          this.showItems = true
        }
      },
      
      addItem (item) {
        if (!this.items.some(i => i.id === item.id)) {
          this.items.push(item)
        }
      },
      
      removeItem(id) {
        this.items.splice(this.items.findIndex(i => i.id === id), 1)
      }
    },
    
    computed: {
      position () {
        if (!this.move) {
          return {
            top: this.top,
            left: this.left
          }
        }
        
        this.top = (this.mouseY - this.gridY) - 10
        this.left = (this.mouseX - this.gridX) - 10
        
        return {
          top: this.mouseY - 10 - this.gridY,
          left: this.mouseX - 10 - this.gridX
        }
      }
    }
  }
</script>

<template>
  <div 
      class="shelf"
      @mousedown="previousTop = top; previousLeft = left; move = true"
      @mouseenter="hovered = true"
      @mouseout="hovered = false"
      @mouseup="readjust"
      @click="openItemsPanel"
      :style="{
        'top': position.top + 'px', 
        'left': position.left + 'px', 
        'box-shadow': hovered ? '0 0 5px black' : 'none'
      }"
  >
  </div>

  <div class="itemsContainer" v-if="showItems" :style="{'top': top + 'px', 'left': left + 'px'}">
    <i class="bi bi-x-circle close" @click="showItems=false"></i>
    <SearchPanelComponent
        :show="showSearchItems"
        :products="availableProducts"
        :width="'93%'"
        :height="'130px'"
        :font-size="'small'"
        :is-shelf="true"
        @click="showSearchItems=true"
        @keydown.esc="showSearchItems=false"
        @add-product="addItem"
    />
    <div class="items">
      <div v-for="item in items" :key="item.id">{{item.name}} <i class="bi bi-x-circle removeItem" @click="removeItem(item.id)"></i></div>
    </div>
    <i class="bi bi-trash removeItems" @click="this.items.length = 0"></i>
  </div>
</template>

<style scoped>
  .shelf {
    position: absolute;
    height: v-bind(height + 'px');
    width: v-bind(width + 'px');
    top: 620px;
    left: v-bind(310 - width/2 + 'px');
    background-color: #6ba7ff;
    user-select: none;
    -webkit-user-select: none;
  }
  
  .shelf:hover{
    cursor: move;
  }
  
  .itemsContainer {
    position: absolute;
    z-index: 1;
    margin-left: 10px;
    margin-top: 10px;
    padding: 0.2em;
    min-height: 40px;
    max-height: 140px;
    width: 200px;
    border: 2px solid saddlebrown;
    border-radius: 5px;
    background-color: sandybrown;
  }
  
  .close {
    position: absolute;
    display: flex;
    align-self: center;
    background-color: white;
    height: 16px;
    border-radius: 50%;
  }

  .close {
    top: 2px;
    right: 2px;
  }
  
  .close:hover {
    background-color: #ff5454;
  }
  
  .items {
    min-height: 20px;
    max-height: 120px;
    width: 182px;
    overflow: auto;
  }
  
  .removeItem:hover, .removeItems:hover {
    color: #ff0000;
  }
  
  .removeItems {
    position: absolute;
    right: 2px;
    bottom: 2px;
  }
</style>