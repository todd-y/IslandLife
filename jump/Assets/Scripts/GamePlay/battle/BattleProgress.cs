using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleProgress : MonoBehaviour {
    private const int tipNum = 3;
    private float totalSldWidth;
    private Slider sldProgress;
    private Image [] arrImgTip;

	void Start () {
        InitCtrl();
        InitMsg();
	}

    void Destory() {
        ClearMsg();
    }

    private void InitCtrl() {
        sldProgress = gameObject.GetChildControl<Slider>("SldProgress");
        RectTransform rectTransform = gameObject.GetChildControl<RectTransform>("SldProgress");
        totalSldWidth = rectTransform.rect.width;

        arrImgTip = new Image[tipNum];
        for (int index = 1; index <= tipNum; index++ ) {
            Image img = gameObject.GetChildControl<Image>("SldProgress/imgProgress" + index);
            arrImgTip[index - 1] = img;
        }

        int []arrProgress = BattleMgr.Instance.arrProgress;

        if (arrProgress.Length > tipNum) {
            Debug.LogError("progress is larger than tip:" + arrProgress.Length + "/" + tipNum);
        }

        int length = Mathf.Min(arrProgress.Length, tipNum);
        for (int index = 0; index < length; index++ ) {
            float per = (float)arrProgress[index] / BattleMgr.Instance.totalProgress;
            arrImgTip[index].transform.localPosition = new Vector3(per * totalSldWidth - totalSldWidth / 2, 0, 0);
        }

        OnProgressChange();
    }

    private void InitMsg() {
        Send.RegisterMsg(SendType.ProgressChange, OnProgressChange);
    }

    private void ClearMsg() {
        Send.UnregisterMsg(SendType.ProgressChange, OnProgressChange);
    }

    private void OnProgressChange(params object[] objs) {
        sldProgress.value = BattleMgr.Instance.CurProgress / BattleMgr.Instance.totalProgress;
    }
}
