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
          capption=""
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
      <DxSearchPanel :visible="true"/>
      <DxHeaderFilter :visible="true" :allow-search="true"/>

      <template #cellTemplate="{data}">
        <div class="row">
          <div>
            <audio v-if="data.data.extension === $enums.extensions.mp3" class="file-icon"></audio>
            <csv v-else-if="data.data.extension === $enums.extensions.csv" class="file-icon"></csv>
            <doc v-else-if="data.data.extension === $enums.extensions.doc" class="file-icon"></doc>
            <docx v-else-if="data.data.extension === $enums.extensions.docx" class="file-icon"></docx>
            <epub v-else-if="data.data.extension === $enums.extensions.epub" class="file-icon"></epub>
            <pdf v-else-if="data.data.extension === $enums.extensions.pdf" class="file-icon"></pdf>
            <ppt v-else-if="data.data.extension === $enums.extensions.ppt" class="file-icon"></ppt>
            <pptx v-else-if="data.data.extension === $enums.extensions.pptx" class="file-icon"></pptx>
            <rtf v-else-if="data.data.extension === $enums.extensions.rtf" class="file-icon"></rtf>
            <txt v-else-if="data.data.extension === $enums.extensions.txt" class="file-icon"></txt>
            <xls v-else-if="data.data.extension === $enums.extensions.xls" class="file-icon"></xls>
            <xlsx v-else-if="data.data.extension === $enums.extensions.xlsx" class="file-icon"></xlsx>
            <file v-else class="file-icon"></file>
          </div>
          <div>
            <span>{{ data.data.fileName }}</span><br>
            <span>{{ data.data.size }}</span>
          </div>
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
             class="dx-link dx-icon-close dx-link-icon"
             title="Удалить файл"
             v-on:click="deleteFile(data.data)"
          ></a>
        </div>
      </template>
    </DxDataGrid>

    <UploadForm
        v-if="uploadFormVisible"
        :visible.sync="uploadFormVisible"
    />
  </div>
</template>

<script>
import DxDataGrid, {
  DxColumn,
  DxPaging,
  DxSearchPanel,
  DxHeaderFilter,
  DxPager
} from "devextreme-vue/data-grid";
import DxButton from 'devextreme-vue/button';

import {confirm} from 'devextreme/ui/dialog';

import UploadForm from "../components/forms/UploadForm";

import notify from 'devextreme/ui/notify';

import * as AspNetData from "devextreme-aspnet-data-nojquery";
import axios from 'axios';

import csv from '../svg/csv.svg';
import doc from '../svg/doc.svg';
import docx from '../svg/docx.svg';
import epub from '../svg/epub.svg';
import file from '../svg/file.svg';
import pdf from '../svg/pdf.svg';
import ppt from '../svg/ppt.svg';
import pptx from '../svg/pptx.svg';
import rtf from '../svg/rtf.svg';
import txt from '../svg/txt.svg';
import xls from '../svg/xls.svg';
import xlsx from '../svg/xlsx.svg';

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
  methods: {
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
              elementAttr: {id: 'upload-button'},
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
    file,
    pptx,
    doc,
    docx,
    rtf,
    pdf,
    csv,
    epub,
    ppt,
    txt,
    xls,
    xlsx,
    UploadForm,
    DxButton,
    DxDataGrid,
    DxColumn,
    DxPaging,
    DxSearchPanel,
    DxHeaderFilter,
    DxPager
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

.file-icon {
  width: 40px;
  height: 40px;
  margin-left: 10px;
  margin-right: 5px;
}

@import "../themes/generated/variables.base.scss";
@import "../dx-styles.scss";

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
}

.dx-datagrid-search-panel {
  margin: 0 10px 0 10px !important;
}

#upload-button {
  margin-left: 10px;
}

#file-img {
  padding: 5px 5px 5px 5px;
}

#toolbar {
  padding-right: 10px;
  padding-left: 10px;
  border-bottom: 1px outset rgb(255, 255, 255);
}
</style>
