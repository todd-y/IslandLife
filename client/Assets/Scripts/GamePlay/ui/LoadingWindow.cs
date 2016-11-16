using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingWindow : BaseWindow {
    public Image imgTest;
    public Button btnTest;

    public override void InitMsg() {
        base.InitMsg();

        btnTest.onClick.AddListener(OnClickTest);
    }

    public override void ClearMsg() {
        base.ClearMsg();

        btnTest.onClick.RemoveAllListeners();
    }

    void OnClickTest() {
        RefIcon.SetSprite(imgTest, "Icon1");
    }
}
