using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;

public class InventoryManager : MonoBehaviour
{
    private List<ItemEntry> listEntries;
    public RectTransform ScrollViewContent;
    public RectTransform StartingPos;
    public float verticalOffset;
    private DialogueManager DialogueManager;
    private static GameObject entryPrefab;

    private void Awake() {
        DialogueManager = Managers.GetDialogueManager();
        entryPrefab = Resources.Load<GameObject>(Paths.ItemEntry);
        if (entryPrefab == null) {
            Debug.Log("Entry Prefab could not be found! " + Paths.ItemEntry);
        }
    }

    public void ClearInventory () {
        if (listEntries != null) {
            listEntries.Clear();
        } else {
            listEntries = new List<ItemEntry>();
        }
    }

    public void AddItem(Collectable item) {
        if (entryPrefab == null) {
            Debug.Log("Entry Prefab is null and could not be added! " + Paths.ItemEntry);
            return;
        }

        GameObject entryObj = Instantiate(entryPrefab);
        Managers.GetSceneController().MoveGameObjectToManagers(entryObj);
        entryObj.transform.SetParent(ScrollViewContent);

        ItemEntry entry = entryObj.GetComponent<ItemEntry>();
        entry.Set(item, 1);
        
        if (!listEntries.Contains(entry)) {
            listEntries.Add(entry);
            UpdateUI();
        } else {
            int i = listEntries.IndexOf(entry);
            listEntries[i].Set(listEntries[i].GetCount() + 1);
        }
    }

    public void RemoveItem (Collectable item) {
        for (int i = 0; i < listEntries.Count; i++) {
            if (!listEntries[i].GetItem().Equals(item)) {
                continue;
            } else {
                listEntries[i].Set(listEntries[i].GetCount() - 1);
            }

            if (listEntries[i].GetCount() <= 0) {
                Destroy(listEntries[i].gameObject);
                listEntries.RemoveAt(i);
                UpdateUI();
            }
        }
    }

    private void UpdateUI() {
        float height = entryPrefab.GetComponent<RectTransform>().rect.height;
        for (int i = 0; i < listEntries.Count; i++) {
            ItemEntry entry = listEntries[i];
            entry.SetPosition(new Vector2(0, StartingPos.anchoredPosition.y - (verticalOffset * i)));
            height += entry.GetComponent<RectTransform>().rect.height;
        }
        ScrollViewContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }

    private void OnDrawGizmos() {
        if (entryPrefab == null)
            entryPrefab = Resources.Load<GameObject>(Paths.ItemEntry);

        Vector2 size = new Vector2(entryPrefab.GetComponent<RectTransform>().rect.width, entryPrefab.GetComponent<RectTransform>().rect.height);
        
        Gizmos.DrawCube(StartingPos.position, size);
    }
}
