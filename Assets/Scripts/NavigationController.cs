using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationController : MonoBehaviour
{
    public Transform pointer;
    private List<Transform> markers;


    // Start is called before the first frame update

    private void Start()
    {
        markers = new List<Transform>(GameObject.Find("Navigation Markers").GetComponentsInChildren<Transform>());
        markers.RemoveAt(0);
        SetMarkerIndex(0);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += ResetMarkers;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= ResetMarkers;
    }

    private void ResetMarkers(Scene scene, LoadSceneMode mode)
    {
        markers = new List<Transform>(GameObject.Find("Navigation Markers").GetComponentsInChildren<Transform>());
        markers.RemoveAt(0);
        SetMarkerIndex(0);
    }

    // Update is called once per frame
    void Update()
    {
        // Why isn't LookAt2D a thing???
        // rotates arrow to face current objective
        if (markers != null && markers.Count > 0)
        {
            gameObject.SetActive(true);
            pointer.localEulerAngles = LookAt2D(markers[GetMarkerIndex].position);
        }
        else
            gameObject.SetActive(false);
    }

    public void SetMarkerIndex(int i) => GetMarkerIndex = i;
    public int GetMarkerIndex { get; private set; }

    public Vector3 LookAt2D(Vector3 pos)
    {
        Vector3 target = new Vector3(pos.x - pointer.position.x, pos.y - pointer.position.y);
        float rotation = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        target = new Vector3(0f, 0f, rotation - 90f);
        // Debug.Log(string.Format("Pointer should face {0}, {1} at {2} degrees", pos.x, pos.y, target.z));
        return target;
    }
}
