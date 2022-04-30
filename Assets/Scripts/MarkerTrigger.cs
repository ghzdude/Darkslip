using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerTrigger : MonoBehaviour
{
    public Transform nextMarker;

    private void OnTriggerEnter2D(Collider2D collision) {
        NavigationController nav;
        if (collision.GetComponent<NavigationController>() != null) {
            nav = collision.GetComponent<NavigationController>();
            nav.SetTarget(nextMarker);
        }
    }

    public void GenericTrigger(NavigationController nav) {
        if (nav != null) {
            nav.SetTarget(nextMarker);
        }
    }
}
