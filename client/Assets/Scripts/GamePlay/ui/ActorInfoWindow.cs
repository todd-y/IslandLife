using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActorInfoWindow : BaseWindowWrapper<ActorInfoWindow> {
    public Image imgHeadIcon;
    public Text txtName;
    public Text txtOfficial;
    public Text txtAge;
    public Text txtLoyalty;
    public Text txtCharacteristic;
    public Text txtCachet;
    public Text txtAbility;

    private Actor actor;

    public void OpenWindow(Actor _actor) {
        actor = _actor;
        if (hasOpen) {
            RefreshWindow();
        }
        else {
            WindowMgr.Instance.OpenWindow<ActorInfoWindow>();
        }
    }
    
    protected override void InitCtrl() {
    }

    protected override void OnPreOpen() {
        RefreshWindow();
    }

    protected override void OnOpen() {
    }

    protected override void InitMsg() {
    }

    protected override void ClearMsg() {
    }

    private void RefreshWindow() {
        RefIcon.SetSprite(imgHeadIcon, actor.headIconID);
        txtName.setText(actor.name);
        txtOfficial.setText(actor.roleName);
        txtAge.setText("AgeDesc", actor.GetAge());
        txtCachet.setText("CachetDesc", (int)actor.cachet);
        txtAbility.setText("AbilityDesc", (int)actor.ability);
        txtLoyalty.setText("LoyaltyDesc", (int)actor.loyalty);
        txtCharacteristic.setText("CharacteristicDesc");
    }
}
