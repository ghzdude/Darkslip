using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public class InventoryManager : MonoBehaviour
{
    // private Dictionary<Collectable, int> items;
    // private List<Collectable> itemList;
    // private List<int> itemListCount;
    private List<ItemEntry> listEntries;
    public RectTransform ScrollViewContent;
    public RectTransform StartingPos;
    // public GameObject listEntry;
    public float verticalOffset;
    // private int count = 1;
    private DialogueManager DialogueManager;
    private static GameObject entryPrefab;
    const string format = "{0}x {1}";

    private void Start() {
        DialogueManager = Managers.GetDialogueManager();
        entryPrefab = Resources.Load<GameObject>(Paths.ItemEntry);
        // InventoryView.
    }

    public void ClearInventory () {
        listEntries = new List<ItemEntry>();
        
        /*
        // Set Variables
        items= new Dictionary<Collectable, int>();
        itemList = new List<Collectable>();
        itemListCount = new List<int>();
        entries = new List<GameObject>();
        for (int i = 0; i < InventoryPanel.GetComponentsInChildren<RectTransform>().Length; i++) {
            RectTransform entry = InventoryPanel.GetComponentsInChildren<RectTransform>()[i];
            if (entry.CompareTag(Tags.ItemEntry)) {
                Destroy(entry.gameObject);
            }
        }
        */
    }

    public void AddItem(Collectable item) {
        ItemEntry entry = Instantiate(entryPrefab).GetComponent<ItemEntry>();
        entry.Set(item, 1);
        if (!listEntries.Contains(entry)) {
            listEntries.Add(entry);
            UpdateUI();
        } else {
            int i = listEntries.IndexOf(entry);
            listEntries[i].Set(listEntries[i].GetCount() + 1);
        }
        
        /*
        // Check if itemList is not null and has something
        if (itemList != null && itemListCount != null && entries != null) {
            // Debug.Log("item list, count, and entries exists");

            // Check if item exists
            // itemList.Exists(x => item.internalName == Enums.inventoryType.Clipboard);
            for (int i = 0; i < itemList.Count; i++) {
                if (itemList[i].internalName == item.internalName) {
                    // Debug.Log("matching item detected");
                    itemListCount[i]++;
                    InventoryPanel.GetChild(i).GetComponentInChildren<Text>().text = string.Format(format, itemListCount[i], item.internalName);

                    // Debug.Log(string.Format("{0} has been added at the item slot {1} with the amount of {2}", itemList[i].name, i, itemListCount[i]));
                    return;
                }
            }
            count = 1;
            itemList.Add(item);
            itemListCount.Add(count);
            entries.Add(Instantiate(listEntry, InventoryPanel));
            int j = entries.Count - 1;
            entries[j].transform.GetChild(0).GetComponent<Image>().sprite = item.icon;
            entries[j].GetComponentInChildren<Text>().text = string.Format(format, count, item.internalName);
            entries[j].GetComponent<RectTransform>().anchoredPosition = new Vector2(
                entries[j].GetComponent<RectTransform>().anchoredPosition.x, 
                entries[j].GetComponent<RectTransform>().anchoredPosition.y - (verticalOffset * j)
            );
            
            entries[j].transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => RetrieveDialouge(j, itemList[j].characterResponse));
        }
        */
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
        
        /*
        int index = itemList.IndexOf(item);
        itemList.Remove(item);
        itemListCount.RemoveAt(index);
        entries.RemoveAt(index);
        */
    }

    private void UpdateUI() {
        for (int i = 0; i < listEntries.Count; i++) {
            ItemEntry entry = listEntries[i];
            entry.SetPosition(new Vector2(0, StartingPos.anchoredPosition.y + (verticalOffset * i)));
            /*
            entry.EntryUI = Instantiate(entryPrefab);
            entry.EntryUI.transform.parent = ScrollViewContent;
            entry.EntryUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, StartingPos.anchoredPosition.y + (verticalOffset * i));
            */
        }
    }

    public void RetrieveDialouge (int index, bool character) {
        if (!DialogueManager.IsActive() && character)
            DialogueManager.EnableDialogueBox(listEntries[index].GetItem().TextOnPickup, Enums.Character.Sean);
        else if (!DialogueManager.IsActive())
            DialogueManager.EnableDialogueBox(listEntries[index].GetItem().TextOnPickup);
    }

    private void OnDrawGizmos() {
        if (entryPrefab == null)
            entryPrefab = Resources.Load<GameObject>(Paths.ItemEntry);

        Vector2 size = new Vector2(entryPrefab.GetComponent<RectTransform>().rect.width, entryPrefab.GetComponent<RectTransform>().rect.height);
        
        Gizmos.DrawCube(StartingPos.position, size);
    }

    private class Entry {
        public Collectable Collectable;
        public int count;
        public ItemEntry EntryUI;

        public Entry (Collectable collectable, ItemEntry entryUI) {
            Collectable = collectable;
            count = 1;
            EntryUI = entryUI;
        }

        public override string ToString() {
            return string.Format(format, Collectable.name, count);
        }
    }
}
