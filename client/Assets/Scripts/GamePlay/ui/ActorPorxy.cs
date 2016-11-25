using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActorPorxy : MonoBehaviour {
    public Image imgHeadIcon;
    public Text txtName;
    public Text txtOffical;

    private Actor actor;

    public void SetData(Actor _actor) {
        actor = _actor;
        RefIcon.SetSprite(imgHeadIcon, actor.headIconID);
        txtName.setText(actor.name);
        txtOffical.setText(actor.roleName);
        gameObject.SetActive(true);
    }

    public void ClearData() {
        actor = null;
        gameObject.SetActive(false);
    }

    public void OnClickHandle() {
        ActorInfoWindow.Instance.OpenWindow(actor);
    }
    
}
