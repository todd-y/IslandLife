using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MiniMapGrid : MonoBehaviour {

    public RoomInfo roomInfo;
    public Image imgState;
    public bool open = false;

    public void SetState(State state) {
        switch (state){
            case State.Hide:
                imgState.gameObject.SetActive(false);
                break;
            case State.Show:
                imgState.gameObject.SetActive(true);
                imgState.color = open ? Color.white : Color.grey;
                break;
            case State.InRoom:
                open = true;
                imgState.gameObject.SetActive(true);
                imgState.color = Color.red;
                break;
        }
    }

    public enum State {
        Hide, 
        Show, 
        InRoom,
    }
}
