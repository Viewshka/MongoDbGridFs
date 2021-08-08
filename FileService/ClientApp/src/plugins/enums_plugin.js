const extensions = Object.freeze([
    {
        id: 'pdf',
        cssClasses: 'dx-link dx-icon-pdffile dx-link-icon'
    },
    {
        id: 'jpg',
        cssClasses: 'dx-link dx-icon-image dx-link-icon'
    }, 
    {
        id: 'jpeg',
        cssClasses: 'dx-link dx-icon-image dx-link-icon'
    }, 
    {
        id: 'docx',
        cssClasses: 'dx-link dx-icon-docxfile dx-link-icon'
    },
    {
        id: 'doc',
        cssClasses: 'dx-link dx-icon-docfile dx-link-icon'
    },
    {
        id: 'txt',
        cssClasses: 'dx-link dx-icon-txtfile dx-link-icon'
    },
    {
        id: 'zip',
        cssClasses: 'dx-link dx-icon-box dx-link-icon'
    },
    {
        id: 'mp3',
        cssClasses: 'dx-link dx-icon-music dx-link-icon'
    },
    {
        id: 'csv',
        cssClasses: 'dx-link dx-icon-file dx-link-icon'
    },
    {
        id: 'epub',
        cssClasses: 'dx-link dx-icon-file dx-link-icon'
    },
    {
        id: 'ppt',
        cssClasses: 'dx-link dx-icon-pptfile dx-link-icon'
    },
    {
        id: 'pptx',
        cssClasses: 'dx-link dx-icon-pptxfile dx-link-icon'
    },
    {
        id: 'rtf',
        cssClasses: 'dx-link dx-icon-rtffile dx-link-icon'
    },
    {
        id: 'xls',
        cssClasses: 'dx-link dx-icon-xlsfile dx-link-icon'
    },
    {
        id: 'xlsx',
        cssClasses: 'dx-link dx-icon-xlsxfile dx-link-icon'
    },
    {
        id:'png',
        cssClasses: 'dx-link dx-icon-image dx-link-icon'
    }
]);

export default {
    install(Vue, options) {
        Vue.prototype.$enums = {
            extensions
        };
    }
}