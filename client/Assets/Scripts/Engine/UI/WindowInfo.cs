using UnityEngine;
using System.Collections;

public class WindowInfo : MonoBehaviour {

    public WindowType windowType = WindowType.Normal;
    public OpenAnimType animType = OpenAnimType.None;
    public float animTime = 0.3f;
    public bool closeOnEmpty = false;
    public bool mask = false;
    public Vector3 defaultPos = Vector3.zero;
    public Vector3 openPos = Vector3.zero;

    void Update() {
//#if UNITY_EDITOR
//        if (Application.isPlaying) {
//            return;
//        }
//        if (this.transform.parent == null && !UnityEditor.EditorApplication.isPlaying) {
//            GameObject uiRoot = GameObject.Find("UIRoot2D");
//            if (uiRoot == null) {
//                GameObject uiRoot2D = MonoBehaviour.Instantiate(LocalAssetMgr.Instance.Load_UI("UIRoot2D")) as GameObject;
//                uiRoot2D.name = "UIRoot2D";
//            }
//            this.transform.SetParent(GameObject.Find("UIRoot2D").transform.Find("NormalPanel"));
//            RectTransform rectTrans = gameObject.GetComponent<RectTransform>();
//            rectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0f, 0f);
//            rectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0f, 0f);
//            rectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0f, 0f);
//            rectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0f, 0f);
//            rectTrans.anchorMin = new Vector2(0f, 0f);
//            rectTrans.anchorMax = new Vector2(1f, 1f);
//            rectTrans.anchoredPosition3D = Vector3.zero;
//            rectTrans.localScale = new Vector3(1f, 1f, 1f);
//        }
//#endif
    }
}
