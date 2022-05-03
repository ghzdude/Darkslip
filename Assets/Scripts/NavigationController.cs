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

            pointer.localEulerAngles = LookAt2D(target.position);
        }
    }

    // public void SetMarkerIndex(int i) => GetMarkerIndex = i;
    // public int GetMarkerIndex { get; private set; }

    public Vector3 LookAt2D(Vector3 pos) {
        Vector3 target = new Vector3(pos.x - pointer.position.x, pos.y - pointer.position.y);
        float rotation = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        target = new Vector3(0f, 0f, rotation - 90f);
        // Debug.Log(string.Format("Pointer should face {0}, {1} at {2} degrees", pos.x, pos.y, target.z));
        return target;
    }
    /*
    public void AddMarker(Transform marker)
    {
        markers.Add(marker);
        SetMarkerIndex(markers.Count - 1);
    }

    public void RemoveMarker(Transform marker)
    {
        markers.Remove(marker);
        if (markers.Count > 1)
            SetMarkerIndex(markers.Count - 2);
        else
            SetMarkerIndex(0);
    }*/

    public void SetTarget (Transform target) {
        this.target = target;
        gameObject.SetActive(true);
    }
}
