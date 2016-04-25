using UnityEngine;
using System.Collections;

public class BullyTrigger : MonoBehaviour {
    public GameObject triggerHolder;

    void OnTriggerEnter()
    {
        triggerHolder.GetComponent<SOneMovement>().reachedBully = true;
    }
}
