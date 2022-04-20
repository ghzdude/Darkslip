using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private List<Collectable> itemList;
    private List<int> itemListCount;
    private List<GameObject> entries;
    public RectTransform InventoryPanel;
    public GameObject listEntry;
    public float verticalOffset;
    private int count = 1;
    private DialogueManager dialogueManager;
    private string format;

    private void Start()
    {
        dialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
    }

    public void ClearInventory()
    {
        // Set Variables
        itemList = new List<Collectable>();
        itemListCount = new List<int>();
        entries = new List<GameObject>();
        for (int i = 0; i < InventoryPanel.GetComponentsInChildren<RectTransform>().Length; i++)
        {
            RectTransform entry = InventoryPanel.GetComponentsInChildren<RectTransform>()[i];
            if (entry.CompareTag("Item Entry"))
            {
                Destroy(entry.gameObject);
            }
        }
    }

    public void AddItem(Collectable item)
    {
        format = "{0}x {1}";
        // Check if itemList is not null and has something
        if (itemList != null && itemListCount != null && entries != null)
        {
            // Debug.Log("item list, count, and entries exists");

            // Check if item exists
            // itemList.Exists(x => item.internalName == Enums.inventoryType.Clipboard);
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i].internalName == item.internalName)
                {
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
                entries[j].GetComponent<RectTransform>().anchoredPosition.y - (verticalOffset * j));

            entries[j].transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => RetrieveDialouge(j, itemList[j].characterResponse));
            // Debug.Log(string.Format("{0} has been added at the item slot {1} with the amount of {2}", itemList[j].name, j, itemListCount[j]));
        }
    }

    public void RemoveItem(Collectable item)
    {
        itemList.Remove(item);
    }

    public void RetrieveDialouge (int index, bool character)
    {
        if (!dialogueManager.dialogueActive && character)
            dialogueManager.EnableDialogueBox(itemList[index].TextOnPickup, Enums.character.Sean);
        else
            dialogueManager.EnableDialogueBox(itemList[index].TextOnPickup);
    }
}
