using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DoingPorxy : MonoBehaviour {
    public Text txtWhat;
    public Text txtWho;
    public Slider sldProgress;

    public ActionInfo actionInfo;

    public void SetData(ActionInfo _info) {
        actionInfo = _info;
        txtWho.setText(_info.action.Action + LangType.Title.ToString());
        txtWhat.setText(_info.executor.roleName);
        sldProgress.value = _info.GetProgress();
        gameObject.SetActive(true);
    }

    public void OnClickHandle() {
        if (ActionInfoWindow.Instance.actionInfo != null && ActionInfoWindow.Instance.actionInfo.actionID == actionInfo.actionID) {
            return;
        }

        ActionInfoWindow.Instance.OpenWindow(actionInfo);
    }

    public void ClearData() {
        actionInfo = null;
        gameObject.SetActive(false);
    }

    public void Update() {
        if (actionInfo == null)
            return;
        sldProgress.value = actionInfo.GetProgress();
    }

}
