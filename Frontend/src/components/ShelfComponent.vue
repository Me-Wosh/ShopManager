<script>
  export default {
    name: "ShelfComponent",
    
    props: ['id', 'mouseX', 'mouseY', 'readjustPosition'],
    
    data () {
      return {
        top: null,
        left: null,
        move: false,
        hovered: false,
        zIndex: 0,
        items: []
      }
    },
    
    methods: {
      readjust () {
        this.zIndex = 0
        this.move = false
        let offset = this.readjustPosition(this.id, this.top, this.left)
        
        if (offset !== undefined) {
          this.top = offset.top <= 10 ? this.top - offset.top : this.top + (20 - offset.top)
          this.left = offset.left <= 10 ? this.left - offset.left : this.left + (20 - offset.left)
        }
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
        
        this.top = this.mouseY - 10
        this.left = this.mouseX - 10
        
        return {
          top: this.mouseY - 10,
          left: this.mouseX - 10
        }
      }
    }
  }
</script>

<template>
  <div 
      class="shelf"
      @mousedown="move = true; zIndex = 1"
      @mouseenter="hovered = true; zIndex = 1"
      @mouseout="hovered = false; zIndex = 0"
      @mouseup="readjust"
      :style="{
        'top': position.top + 'px', 
        'left': position.left + 'px', 
        'z-index': zIndex, 
        'box-shadow': hovered ? '0 0 5px black' : 'none'
      }"
  >
    
  </div>
</template>

<style scoped>
  .shelf {
    position: absolute;
    height: 20px;
    width: 20px;
    background-color: #6ba7ff;
    user-select: none;
    -webkit-user-select: none;
  }

  .shelf:hover{
    cursor: move;
  }
</style>