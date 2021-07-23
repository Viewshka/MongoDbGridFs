const extensions = Object.freeze({
    pdf: 'pdf',
    jpg: 'jpg',
    jpeg: 'jpeg',
    docx: 'docx',
    doc: 'doc',
    txt: 'txt',
    zip: 'zip',
    mp3: 'mp3',
    csv: 'csv',
    epub: 'epub',
    ppt: 'ppt',
    pptx: 'pptx',
    rtf: 'rtf',
    xls: 'xls',
    xlsx: 'xlsx'
});

export default {
    install(Vue, options) {
        Vue.prototype.$enums = {
            extensions
        };
    }
}