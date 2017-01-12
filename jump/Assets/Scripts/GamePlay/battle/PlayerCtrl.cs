using UnityEngine;
using System.Collections;

public class PlayerCtrl : MonoBehaviour {
    private Rigidbody2D body;
    private Collider2D collider;
    private const float size = 40;
    private const float MoveLimit = 300;
    private float addSpeedX = 2000;
    private float reduceSpeedX = 1000;
    private float maxSpeedX = 300;
    private float curSpeedX = 0;
    private float moveFactor = 0;
    private float lastSpeedX;
    private float lastPosY;

    void Awake() {
        InitCtrl();
    }

    void OnTriggerEnter2D(Collider2D coll) {
        switch (coll.gameObject.tag) {
            case "Gold":
                BattleMgr.Instance.playerInfo.Gold++;
                RoomCreatMgr.Instance.RemoveGameObject(coll.gameObject);
                break;
            case "Item":
                ItemProxy itemProxy = coll.gameObject.GetComponent<ItemProxy>();
                if (itemProxy != null) {
                    itemProxy.GetItem();
                }
                else {
                    Debug.LogError("itemProxy is null");
                }
                break;
            default:
                Debug.LogError("dont handle :" + coll.gameObject.tag);
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D coll) {
        switch(coll.gameObject.tag){
            case "Ground":
                RoomCreatMgr.Instance.RemoveGameObject(coll.gameObject);
                break;
            case "Enemy":
                EnemyProxy enemyProxy = coll.gameObject.GetComponent<EnemyProxy>();
                if(enemyProxy != null){
                    enemyProxy.DoAtk();
                }
                else{
                    Debug.LogError("enemyProxy is null");
                }
                break;
            case "Wall":
                // do nothing
                break;
            default:
                Debug.LogError("dont handle :" + coll.gameObject.tag);
                break;
        }
    }

    public void FixedUpdateHandle() {
        //HitCheck();
        //SpeedUpdate();
        //PosXUpdate();
    }

    public void UpdateHandle() {
        InputHandle();
        PosYUpdate();
    }

    private void HitCheck() {
        RaycastHit2D leftHit2D = Physics2D.Raycast(collider.bounds.min + Vector3.down * 0.01f, Vector2.down, 0.01f);
        RaycastHit2D rightHit2D = Physics2D.Raycast(collider.bounds.min + new Vector3(collider.bounds.size.x, 0, 0) + Vector3.down * 0.01f, Vector2.down, 0.01f);
        Debug.DrawRay(collider.bounds.min + Vector3.down * 0.01f, Vector2.down * 0.01f, Color.white);
        Debug.DrawRay(collider.bounds.min + new Vector3(collider.bounds.size.x, 0, 0) + Vector3.down * 0.01f, Vector2.down * 0.01f, Color.white);

        if (leftHit2D.collider != null || rightHit2D.collider != null) {
            body.AddForce(Vector2.up * 300);
            //Debug.LogError("hit");
        }
    }

    private void SpeedUpdate() {
        if (moveFactor != 0) {
            curSpeedX += moveFactor * addSpeedX * Time.fixedDeltaTime;
            curSpeedX = Mathf.Clamp(curSpeedX, -maxSpeedX, maxSpeedX);
            lastSpeedX = curSpeedX;
        }
        else if (curSpeedX != 0) {
            curSpeedX = (curSpeedX > 0 ? -reduceSpeedX : reduceSpeedX) * Time.fixedDeltaTime + curSpeedX;
            if (lastSpeedX * curSpeedX <= 0) {
                curSpeedX = 0;
            }
        }
    }

    private void PosXUpdate() {
        if (curSpeedX == 0)
            return;
        float newX = transform.localPosition.x + curSpeedX * Time.fixedDeltaTime;
        newX = Mathf.Clamp(newX, -MoveLimit, MoveLimit);
        transform.localPosition = new Vector3(newX, transform.localPosition.y, transform.localPosition.z);
    }

    private void PosYUpdate() {
        float curPosY = transform.localPosition.y;
        if (curPosY - lastPosY != 0) {
            Send.SendMsg(SendType.PlayerYMove, (curPosY - lastPosY));
            lastPosY = curPosY;
        }
    }

    private void InitCtrl() {
        body = gameObject.GetComponent<Rigidbody2D>();
        collider = gameObject.GetComponent<Collider2D>();
        lastPosY = transform.localPosition.y;
    }

    private void InputHandle() {
        //move
        moveFactor = 0;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            body.velocity = new Vector2(-5, body.velocity.y);
            moveFactor = -1;
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            body.velocity = new Vector2(5, body.velocity.y);
            moveFactor = 1;
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Space)) {
        }
        //skill
        if (Input.GetKeyDown(KeyCode.Q)) {
        }
        if (Input.GetKeyDown(KeyCode.W)) {
        }
        if (Input.GetKeyDown(KeyCode.E)) {
        }
        if (Input.GetKeyDown(KeyCode.R)) {
        }

        //item
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {

        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {

        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {

        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) {

        }
    }
}
