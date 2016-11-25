using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanDoProxy : MonoBehaviour {
    public Text txtTitle;

    private RefAction action;

    public void SetData(RefAction refAction) {
        action = refAction;
        txtTitle.setText(action.Action + "Title");
        gameObject.SetActive(true);
    }

    public void ClearData() {
        action = null;
        gameObject.SetActive(false);
    }

    public void OnDoClick() {
        ActionInfoWindow.Instance.OpenWindow( new ActionInfo(action) );
    }
}
