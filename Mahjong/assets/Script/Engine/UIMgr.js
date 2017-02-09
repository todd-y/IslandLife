var UIMgr = function () {
    this.allWindow = {};
    this.uiRoot = null;
    this.arrLevel = [];
    this.totalLevel = 3;
}

UIMgr.prototype.initUI = function () {
    if( !!this.uiRoot ){
        cc.error("uiRoot is not null");
        return;
    }
    
    this.uiRoot = cc.find('Canvas/uiRoot');
    if( !this.uiRoot ){
        cc.error("uiRoot is null");
        return;
    }
    
    for(var i = 0; i < this.totalLevel; i ++){
        this.arrLevel[i] = cc.find('level' + i, this.uiRoot);
        
        if( !this.arrLevel[i] ){
            cc.error("level is null " + i);
        }
    }
}

UIMgr.prototype.openWindow = function (uiName, cb) {
    cc.log('open ui: ', uiName);

    this.getWindow(uiName, function (baseUI) {
        if(!baseUI){
            cc.error("baseUI is null:" + uiName);
            return;
        }
        if(baseUI.hasOpen)
            return;
        
        baseUI.doOpen();

        if (!!cb)
            cb(baseUI);
    });
}

UIMgr.prototype.closeWindow = function (uiName) {
    var window = this.allWindow[uiName];
    if(!!window){
        window.doClose();
    }
    else{
        cc.error("close window is fail :" + uiName);
    }
}

UIMgr.prototype.closeWindow = function (window) {
    var baseWindow = window.node.getComponent('BaseWindow');
    if(!baseWindow){
        cc.error("close fail:" + window);
        return;
    }
    
    baseWindow.doClose();
}

UIMgr.prototype.getWindow = function (uiName, cb) {
    var window = null;
    window = this.allWindow[uiName];
    if(window){
        cb(window);
        return;
    }
    
    var self = this;
    cc.loader.loadRes("Prefab/UI/" + uiName, function (err, prefab) {
        if (err) {
            cc.error('loadRes Prefab/UI/' + uiName, err);
            cb(null);
        }
        else {
            var ui = cc.instantiate(prefab);
            ui.active = false;
            
            var baseWindow = ui.getComponent('BaseWindow');
            if(baseWindow){
                ui.parent = self.arrLevel[baseWindow.level];
                self.allWindow[uiName] = ui;
                cb(baseWindow);
            }
            else{
                cc.error("baseWindow is null : " + uiName);
                cb(null);
            }
        }
    });
}

module.exports = function () {
    return new UIMgr();
};