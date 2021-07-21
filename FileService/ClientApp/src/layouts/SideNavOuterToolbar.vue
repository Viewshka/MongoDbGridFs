<template>
  <div class="side-nav-outer-toolbar">
    <NavBar
        class="layout-header"
        :title="title"
    />
    <DxScrollView ref="scrollViewRef" class="with-footer">
      <slot/>
      <slot name="footer"/>
    </DxScrollView>
  </div>
</template>

<script>
import DxScrollView from "devextreme-vue/scroll-view";
import NavBar from "../components/static/NavBar";

export default {
  props: {
    title: String,
  },
  methods: { },
  data() {
    return {
      menuOpened: false,
      menuTemporaryOpened: false,
    };
  },
  computed: {
    scrollView() {
      return this.$refs["scrollViewRef"].instance;
    }
  },
  watch: {
    $route() {
      this.scrollView.scrollTo(0);
    }
  },
  components: {
    DxScrollView,
    NavBar,
  }
};
</script>

<style lang="scss">
.side-nav-outer-toolbar {
  flex-direction: column;
  display: flex;
  height: 100%;
  width: 100%;
}

.container {
  height: 100%;
  flex-direction: column;
  display: flex;
}

.layout-header {
  z-index: 1501;
}

.layout-body {
  flex: 1;
  min-height: 0;
}

.content {
  flex-grow: 1;
}

</style>
