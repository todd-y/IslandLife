using UnityEngine;
using System.Collections;
using DunGen;

public class Transfer : MonoBehaviour {

    public Doorway doorWay;
    public GameObject transferPos;

    void Start() {
        doorWay.TransferPos = transferPos;
    }
}
