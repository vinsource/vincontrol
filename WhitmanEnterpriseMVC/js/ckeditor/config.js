/*
Copyright (c) 2003-2010, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function (config) {
    config.removePlugins = 'elementspath';
    config.coreStyles_bold = { element: 'b', overrides: 'strong' };
    config.coreStyles_italic = { element: 'i', overrides: 'em' };

    config.resize_enabled = false;
    config.toolbarCanCollapse = true;
    config.startupShowBorders = false;
    config.enterMode = CKEDITOR.ENTER_DIV;

    config.scayt_srcUrl = "http://athenawebprod/spellcheck/lf/scayt/scayt.js";
    config.wsc_customLoaderScript = "http://athenawebprod/spellcheck/lf/22/js/wsc_fck2plugin.js";
    config.scayt_autoStartup = true;

    config.templates = 'custom';
    config.templates_replaceContent = false;

    config.fontSize_sizes = '8/8pt;9/9pt;10/10pt;11/11pt;12/12pt;13/13pt;14/14pt;15/15pt;16/16pt;17/17pt;18/18pt;19/19pt;20/20pt';
    config.font_defaultLabel = 'Calibri';
    config.fontSize_defaultLabel = '9pt';
    config.font_names = 'Arial;Calibri;Comic Sans MS;Courier New;Georgia;Lucida Sans Unicode;Tahoma;Trebuchet MS;Verdana';

    config.toolbar = 'BasicToolbar';
    config.toolbar_BasicToolbar =
    [
        ['Cut', 'Copy', 'Paste'],
        ['-', 'Undo', 'Redo', '-', 'Find', 'Replace', '-', 'SelectAll', '-'],
        ['Bold', 'Italic', 'Underline', 'Strike', '-'],
        ['Format', 'FontSize', '-'],
        ['Maximize', '-', 'SpellChecker', 'Scayt'],
        ['Source']
    ];

    config.toolbar_EnhancedToolbar =
    [
        ['Cut', 'Copy', 'Paste', 'PasteText'],
        ['Undo', 'Redo', '-', 'Find', 'Replace', '-', 'SelectAll', 'RemoveFormat'],
        ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', 'Blockquote', 'CreateDiv'],
        ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
        ['BidiLtr', 'BidiRtl'],
        ['Link', 'Unlink', 'Anchor'],
        ['Table', 'HorizontalRule', 'SpecialChar'],
        '/',
        ['Templates'],
        ['Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'],
        ['Styles', 'FontSize'],
        ['TextColor', 'BGColor'],
        ['Maximize', 'ShowBlocks', '-', 'SpellChecker', 'Scayt'],
        ['Source']
    ];

    config.extraPlugins = 'onchange';
    config.minimumChangeMilliseconds = 300; // 100 milliseconds (default value)


};
