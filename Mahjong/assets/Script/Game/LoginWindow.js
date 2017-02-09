cc.Class({
    extends: cc.Component,

    properties: {

    },

    // use this for initialization
    onLoad: function () {

    },
    
    onStartClick: function () {
        cc.Game.uiMgr.openWindow('MainWindow');
        cc.Game.uiMgr.closeWindow(this);
    },
});
