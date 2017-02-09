cc.Class({
    extends: cc.Component,

    properties: {
        level:0,
    },

    onLoad: function () {
        this.init();
    },
    
    init: function () {
        this.hasOpen = false;
    },
    
    doOpen: function () {
        this.hasOpen = true;
        this.node.active = true;
    },
    
    doClose: function () {
        this.hasOpen = false;
        this.node.active = false;
    },
});
