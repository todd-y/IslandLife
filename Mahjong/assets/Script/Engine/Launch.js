cc.Class({
    extends: cc.Component,

    properties: {
        
    },

    // use this for initialization
    onLoad: function () {
        this.init();
        
        this.uiMgr.openWindow("LoginWindow");
    },
    
    init: function(){
        cc.log("start launch init");
        cc.Game = this;
        this.uiMgr = require('UIMgr')();
        this.uiMgr.initUI();
    },
});
