<template>
  <div class="full-height">
    <DxDataGrid
        :ref="gridRefName"

        :data-source="dataSource"
        :remote-operations="true"
        :focused-row-enabled="true"
        :allow-column-reordering="true"
        :allow-column-resizing="true"

        @toolbar-preparing="toolbarPreparing($event)"
    >
      <DxColumn
          caption="Управление"
          type="buttons"
          cell-template="buttonControl"
          alignment="center"
          :width="90"
          :allow-resizing="false"
      />
      <DxColumn
          caption="Имя "
          data-field="fileName"
          cell-template="cellTemplate"
      />
      <DxColumn
          caption="Размер"
          data-field="size"
          data-type="string"
      />
      <DxColumn
          caption="Дата загрузки"
          data-field="uploadDate"
          data-type="datetime"
      />
      <DxPaging :page-size="10"/>
      <DxPager
          :visible="true"
          :allowed-page-sizes="[5, 10, 20,50,'all']"
          :show-page-size-selector="true"
          :show-info="true"
          :show-navigation-buttons="true"
          display-mode="full"
      />
      <DxColumnChooser :enabled="true" mode="select"/>
      <DxSearchPanel :visible="true"/>
      <DxHeaderFilter :visible="true" :allow-search="true"/>

      <template #cellTemplate="{data}">
        <div class="dx-command-edit dx-command-edit-with-icons">
          <a :class="getCssClasses(data.data)"></a><span class="filename">{{ data.data.fileName }}</span>
        </div>
      </template>

      <template #buttonControl="{data}">
        <div class="dx-command-edit dx-command-edit-with-icons">
          <a href="#"
             class="dx-link dx-icon-download dx-link-icon"
             title="Скачать файл"
             v-on:click="downloadFile(data.data)"
          ></a>
          <a href="#"
             class="dx-link dx-icon-trash dx-link-icon"
             title="Удалить файл"
             v-on:click="deleteFile(data.data)"
          ></a>
        </div>
      </template>
    </DxDataGrid>

    <UploadForm
        v-if="uploadFormVisible"
        :visible.sync="uploadFormVisible"
        @submit="uploadFormSubmit"
    />
  </div>
</template>

<script>
import DxDataGrid, {
  DxColumn,
  DxPaging,
  DxSearchPanel,
  DxHeaderFilter,
  DxPager,
  DxColumnChooser
} from "devextreme-vue/data-grid";

import {confirm} from 'devextreme/ui/dialog';

import UploadForm from "../components/forms/UploadForm";

import notify from 'devextreme/ui/notify';

import * as AspNetData from "devextreme-aspnet-data-nojquery";
import axios from 'axios';

const dataSource = AspNetData.createStore({
  key: 'id',
  loadUrl: `api/file/all`,
  onBeforeSend: (method, ajaxOptions) => {
    ajaxOptions.xhrFields = {withCredentials: true};
  },
});

export default {
  name: "Home",
  data() {
    return {
      dataSource,
      gridRefName: 'dataGrid',
      uploadFormVisible: false,
    };
  },
  computed: {},
  methods: {
    getCssClasses(row) {
      let extension = this.$enums.extensions.find(s => s.id === row.extension);
      if (extension)
        return extension.cssClasses;
      else
        return 'dx-link dx-icon-file dx-link-icon';
    },
    uploadFormSubmit() {
      this.refreshDataGrid();
    },
    openUploadForm() {
      this.uploadFormVisible = true;
    },
    deleteFile(data) {
      confirm(`Удалить файл?`, `Удаление`)
          .then((dialogResult) => {
            if (dialogResult) {
              axios.delete(`api/file/${data.id}`)
                  .then(() => {
                    this.refreshDataGrid();
                    notify("Файл удален", "success", 3000);
                  })
                  .catch((error) => {
                    notify(error.data.error, "error", 3000);
                  })
            }
          });
    },
    downloadFile(data) {
      axios({
        url: `api/file/${data.id}/download`,
        method: 'GET',
        responseType: 'blob'
      }).then(response => {
        let url = window.URL.createObjectURL(new Blob([response.data]));
        let link = document.createElement('a');
        link.href = url;
        link.setAttribute('download', data.fileName);
        document.body.appendChild(link);
        link.click();
        link.remove();
      }).catch(error => {
        notify(error, 'error', 2000);
      });
    },
    toolbarPreparing(e) {
      e.toolbarOptions.items.unshift(
          {
            location: 'before',
            widget: 'dxButton',
            locateInMenu: 'auto',
            showText: 'auto',
            options: {
              text: 'Загрузка файлов',
              hint: 'Загрузка файлов',
              type: 'default',
              stylingMode: 'contained',
              focusStateEnabled: false,
              onClick: () => {
                this.openUploadForm();
              }
            },
          },
          {
            location: 'after',
            widget: 'dxButton',
            locateInMenu: 'auto',
            showText: 'inMenu',
            options: {
              text: 'Обновить',
              hint: 'Обновить',
              icon: 'refresh',
              type: 'normal',
              stylingMode: 'contained',
              onClick: this.refreshDataGrid.bind(this)
            }
          },
      )
    },
    async refreshDataGrid() {
      this.$refs[this.gridRefName].instance.refresh();
    },
  },
  components: {
    UploadForm,
    DxDataGrid,
    DxColumn,
    DxPaging,
    DxSearchPanel,
    DxHeaderFilter,
    DxPager,
    DxColumnChooser
  }
}
</script>

<style lang="scss">
.truncate-two-lines {
  overflow: hidden;
  position: relative;
  line-height: 1.25em;
  max-height: 2.5em;
  word-break: keep-all;
  white-space: pre-line;
  text-overflow: ellipsis;
  display: -webkit-box;
  -webkit-box-orient: vertical;
  -webkit-line-clamp: 2;
}

.filename{
  margin-left: 10px;
}
</style>
