<template>
  <DxPopup
      height="auto"
      :width="728"
      position="center"
      title="Загрузка файлов"
      :show-title="true"
      :resize-enabled="true"
      :visible="visible"
      :close-on-outside-click="true"
      :value="files"
      
      @hidden="cancel"
  >
    <div>
      <DxFileUploader
          upload-mode="useButtons"
          :multiple="true"
          :upload-url="uploadUrl"
          
          @value-changed="valueChange"
      />
    </div>
  </DxPopup>
</template>

<script>
import DxFileUploader from 'devextreme-vue/file-uploader';
import DxPopup from 'devextreme-vue/popup'

export default {
  name: "UploadForm",
  props: {
    visible: {
      type: Boolean,
      required: true
    },
  },
  components: {
    DxFileUploader,
    DxPopup
  },
  data() {
    return {
      uploadUrl: 'api/file/upload',
      files:[]
    }
  },
  methods: {
    cancel: function () {
      this.$emit('update:visible', false);
      this.files = [];
    },
    valueChange(value){
      this.files = value;
    }
  }
}
</script>

<style scoped>

</style>