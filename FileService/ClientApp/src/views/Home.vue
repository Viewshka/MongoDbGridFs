<template>
  <div>
    <div>
      <DxToolbar>
        <DxItem
            :options="prevButtonOptions"
            location="before"
            widget="dxButton"
        />
        <DxItem
            :options="nextButtonOptions"
            location="before"
            widget="dxButton"
        />
        <DxItem
            :options="addButtonOptions"
            location="after"
            locate-in-menu="auto"
            widget="dxButton"
        />
      </DxToolbar>
    </div>
    <div>
      <DxTileView
          height="auto"
          direction="vertical"
          :show-scrollbar="true"
          :data-source="dataSource"
          :base-item-height="250"
          :base-item-width="180"

          @item-click="itemClick"
      >
        <template #item="{ data }">
          <div>
            <div>
              <div>
                <audio v-if="data.extension === $enums.extensions.mp3"></audio>
                <csv v-else-if="data.extension === $enums.extensions.csv"></csv>
                <doc v-else-if="data.extension === $enums.extensions.doc"></doc>
                <docx v-else-if="data.extension === $enums.extensions.docx"></docx>
                <epub v-else-if="data.extension === $enums.extensions.epub"></epub>
                <pdf v-else-if="data.extension === $enums.extensions.pdf"></pdf>
                <ppt v-else-if="data.extension === $enums.extensions.ppt"></ppt>
                <pptx v-else-if="data.extension === $enums.extensions.pptx"></pptx>
                <rtf v-else-if="data.extension === $enums.extensions.rtf"></rtf>
                <txt v-else-if="data.extension === $enums.extensions.txt"></txt>
                <xls v-else-if="data.extension === $enums.extensions.xls"></xls>
                <xlsx v-else-if="data.extension === $enums.extensions.xlsx"></xlsx>
                <file v-else></file>
              </div>
              <div class="file-name text-ellipsis">{{ data.fileName }}</div>
              <div class="file-name">({{ data.size }})</div>
            </div>
          </div>
        </template>
      </DxTileView>
    </div>
  </div>
</template>

<script>
import DxButton from 'devextreme-vue/button';
import {DxTileView} from 'devextreme-vue/tile-view';
import DxToolbar, {DxItem} from 'devextreme-vue/toolbar'

import notify from 'devextreme/ui/notify';

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

export default {
  name: "Home",
  data() {
    return {
      dataSource: [],
      prevButtonOptions: {
        icon: 'chevronleft',
        onClick: () => {
          notify('Назад');
        }
      },
      nextButtonOptions: {
        icon: 'chevronright',
        onClick: () => {
          notify('Вперед');
        }
      },
      addButtonOptions: {
        icon: 'upload',
        onClick: () => {
          notify('Загрузить файл');
        }
      },
    };
  },
  async created() {
    await this.initFilesInfo(0, 50)
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
  methods: {
    async initFilesInfo(skip, take) {
      axios.get(`api/file/all?skip=${skip}&take=${take}`)
          .then(response => {
            this.dataSource = response.data;
          })
          .catch(error => {
            notify(error, 'error', 2000);
          });
    },
    itemClick(item) {
      axios({
        url: `api/file/${item.itemData.id}/download`,
        method: 'GET',
        responseType: 'blob'
      }).then(response => {
        let url = window.URL.createObjectURL(new Blob([response.data]));
        let link = document.createElement('a');
        link.href = url;
        link.setAttribute('download', item.itemData.fileName);
        document.body.appendChild(link);
        link.click();
        link.remove();
      }).catch(error => {
        notify(error, 'error', 2000);
      });
    }
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
    DxButton,
    DxToolbar,
    DxItem,
    DxTileView
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

.text-ellipsis {
  word-break: keep-all;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.file-name {
  text-align: center;
  padding-left: 5px;
  padding-right: 5px;
}

#file-img {
  padding: 5px 5px 5px 5px;
}
</style>
