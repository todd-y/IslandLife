public enum WindowType {
    Normal = 0,                 //--普通窗体，总是在最下层
    Modal = 1,                  //--模态窗体，总是处于普通窗体之上，提示窗体之下
    Tips = 2,                   //--提示窗体，总是处于系统窗体之下
    System = 3,                 //--系统窗体，总是在最上层
}

public enum OpenAnimType {
    None,                       //--无
    Position,                   //--位置
    Scale,                      //--缩放
    Alpha,                      //--渐变
    ScaleAndAlpha,              //--缩放和渐变 
    Custom,                     //自定义
}