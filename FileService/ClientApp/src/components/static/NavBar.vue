<template>
  <header class="header-component">
    <DxToolbar class="header-toolbar">
      <DxItem
          v-if="title"
          location="center"
          css-class="header-title dx-toolbar-label"
      >
        <!--eslint-disable-->
        <div slot-scope="_">{{ title }}</div>
        <!--eslint-enable-->
      </DxItem>
    </DxToolbar>
  </header>
</template>

<script>
import DxToolbar, {DxItem} from "devextreme-vue/toolbar";

export default {
  props: {
    title: String,
  },
  created() {
  },
  data() {
    return {};
  },
  methods: {},
  components: {
    DxToolbar,
    DxItem,
  }
};
</script>

<style lang="scss">
@import "../../themes/generated/variables.base.scss";
@import "../../dx-styles.scss";

.header-component {
  flex: 0 0 auto;
  z-index: 1;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.12), 0 1px 2px rgba(0, 0, 0, 0.24);

  .dx-toolbar .dx-toolbar-item.menu-button > .dx-toolbar-item-content .dx-icon {
    color: $base-accent;
  }
}

.dx-toolbar.header-toolbar .dx-toolbar-items-container .dx-toolbar-after {
  padding: 0 40px;

  .screen-x-small & {
    padding: 0 20px;
  }
}

.dx-toolbar .dx-toolbar-item.dx-toolbar-button.menu-button {
  width: $side-panel-min-width;
  text-align: center;
  padding: 0;
}

.header-title .DxItem-content {
  padding: 0;
  margin: 0;
}

.dx-theme-generic {
  .dx-toolbar {
    padding: 10px 0;
  }

  .user-button > .dx-button-content {
    padding: 3px;
  }
}
</style>
