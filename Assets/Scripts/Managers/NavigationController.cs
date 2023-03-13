using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationController : MonoBehaviour
{
    public Transform pointer;
    private Transform target;


    // Start is called before the first frame update

    private void Start() {
        if (target == null) {
            pointer.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update() {
        // Why isn't LookAt2D a thing???
        // rotates arrow to face current objective
        if (target != null)
        {
            if (!pointer.gameObject.activeInHierarchy)
                pointer.gameObject.SetActive(true);

            pointer.localEulerAngles = VectorMath.LookAt2D(transform.position, target.position);
        }
    }

    public void SetTarget (Transform target) {
        this.target = target;
        gameObject.SetActive(true);
    }
}
