using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerTrigger : MonoBehaviour
{
    public Transform nextMarker;
    public bool shouldDisable;

    private void OnTriggerEnter2D(Collider2D collision) {
        SetMarker(collision.GetComponent<NavigationController>());
    }

    public void GenericTrigger(NavigationController nav) {
        SetMarker(nav);
    }

    private void SetMarker(NavigationController nav) {
        if (nav != null) {
            nav.SetTarget(nextMarker);

            if (shouldDisable) {
                gameObject.SetActive(false);
            }
        }
    }
}
