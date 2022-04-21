using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationController : MonoBehaviour
{
    public Transform pointer;
    public List<Transform> markers;


    // Start is called before the first frame update

    private void Start()
    {
        if (markers != null)
        {
            SetMarkerIndex(0);
        }
        else
        {
            pointer.gameObject.SetActive(false);
        }
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
        if (markers != null)
        {
            SetMarkerIndex(0);
        }
        else
        {
            pointer.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Why isn't LookAt2D a thing???
        // rotates arrow to face current objective
        if (markers != null && markers.Count > 0)
        {
            if (!pointer.gameObject.activeInHierarchy)
                pointer.gameObject.SetActive(true);

            pointer.localEulerAngles = LookAt2D(markers[GetMarkerIndex].position);
        }
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
    }
}
