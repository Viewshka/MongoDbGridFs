<template>
  <div>

  </div>
</template>

<script>
import DxButton from 'devextreme-vue/button';
export default {
  name: "Home",
  data() {
    return {
    };
  },
  components:{
    DxButton
  }
}
</script>

<style lang="scss">
.logos-container {
  margin: 20px 0 40px 0;
  text-align: center;
  svg {
    display: inline-block;
  }
}

.plus {
  margin: 20px 10px;
  width: 22px;
  height: 22px;
}

.screen-x-small .logos-container {
  svg {
    width: 100%;
    display: block;
    &.plus {
      margin: 0;
    }
  }
}
</style>
