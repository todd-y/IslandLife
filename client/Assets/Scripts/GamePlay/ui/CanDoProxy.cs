using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanDoProxy : MonoBehaviour {
    public Text txtTitle;
    public Button btnDo;

    private RefAction action;

    void Start() {
        btnDo.onClick.AddListener(OnDoClick);
    }

    void Destory() {
        btnDo.onClick.RemoveListener(OnDoClick);
    }

    public void SetData(RefAction refAction) {
        action = refAction;
        txtTitle.setText(action.Action + "Title");
        gameObject.SetActive(true);
    }

    public void ClearData() {
        action = null;
        gameObject.SetActive(false);
    }

    private void OnDoClick() {
        ActionInfoWindow.Instance.OpenWindow( new ActionInfo(action) );
    }
}
