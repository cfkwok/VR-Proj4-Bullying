using UnityEngine;
using System.Collections;

public class BullyTriggerA : MonoBehaviour {
    public GameObject triggerHolder;

    void OnTriggerEnter()
    {
        triggerHolder.GetComponent<SOneMovementA>().reachedBully = true;
    }
}
