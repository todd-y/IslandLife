using UnityEngine;
using System.Collections;

public class LimitProxy : MonoBehaviour {
    public float speed = 100;

    public void UpdateHandle() {
        transform.localPosition -= new Vector3(0, Time.deltaTime * speed, 0);
	}

    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.tag == "Player") {
            Debug.LogError("end");
        }
    }

    //void OnTriggerEnter2D(Collider2D other) {
    //}
}
