using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SelectActorProxy : MonoBehaviour {
    public Image imgHeadIcon;
    public Text txtName;
    public Text txtOfficial;
    public Text txtAge;
    public Text txtLoyalty;
    public Text txtCharacteristic;
    public Text txtCachet;
    public Text txtAbility;

    public Actor actor;
    private Action<SelectActorProxy> callBack;

    public void SetData(Actor _actor) {
        actor = _actor;
        RefIcon.SetSprite(imgHeadIcon, actor.headIconID);
        txtName.setText(actor.name);
        txtOfficial.setText(actor.roleName);
        txtAge.setText("AgeDesc", actor.GetAge());
        txtCachet.setText("CachetDesc", (int)actor.cachet);
        txtAbility.setText("AbilityDesc", (int)actor.ability);
        txtLoyalty.setText("LoyaltyDesc", (int)actor.loyalty);
        txtCharacteristic.setText("CharacteristicDesc");
        gameObject.SetActive(true);
    }

    public void ClearData() {
        actor = null;
        callBack = null;
        gameObject.SetActive(false);
    }

    public void SetCallBack( Action<SelectActorProxy> _callBack ) {
        callBack = _callBack;
    }

    public void OnClickHandle() {
        if (callBack != null) {
            callBack(this);
        }
        else {
            Debug.LogError("call back is null");
        }
    }
}
