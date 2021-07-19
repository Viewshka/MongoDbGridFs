<template>
  <DxResponsiveBox>
    <DxRow :ratio="0"></DxRow>
    <DxRow :ratio="0"></DxRow>
    <DxCol :ratio="1"></DxCol>
    <DxCol :ratio="0.5"></DxCol>
    <DxCol :ratio="0.5"></DxCol>

    <DxItem>
      <DxLocation screen="sm md lg" :row="0" :col="0" :colspan="3"/>
      <DxLocation screen="xs " :row="0" :col="0"/>
      <template #default>
        <div style="background: lightgoldenrodyellow">
          <DxToolbar></DxToolbar>
        </div>
      </template>
    </DxItem>

    <DxItem>
      <DxLocation :row="1" :col="0"/>
      <template #default>
        <div>

        </div>
      </template>
    </DxItem>
  </DxResponsiveBox>
</template>

<script>
import DxTextBox from 'devextreme-vue/text-box';
import DxButton from 'devextreme-vue/button';
import DxList from 'devextreme-vue/list'
import DxResponsiveBox, {DxRow, DxCol, DxItem, DxLocation} from 'devextreme-vue/responsive-box';
import DxToolbar from 'devextreme-vue/toolbar'
import axios from "axios";

export default {
  name: 'Home',
  data() {
    return {
    }
  },

  components: {
    DxTextBox,
    DxButton,
    DxResponsiveBox,
    DxRow,
    DxCol,
    DxItem,
    DxLocation,
    DxList,
    DxToolbar
  },
  async created() {

  },
  
  methods: {}
}
</script>
