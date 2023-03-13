using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemEntry : MonoBehaviour
{
    public Image Icon;
    public Text Label;
    private int Count;
    private Collectable Item;
    const string format = "{0}x {1}";

    public void Set(Collectable item, int count) {
        Item = item;
        Count = count;
        UpdateUI();
    }

    public void Set(int count) {
        Count = count;
    }

    public int GetCount() {
        return Count;
    }

    public Collectable GetItem() {
        return Item;
    }

    public void UpdateUI() {
        Label.text = string.Format(format, Item.internalName, Count);
        Icon.sprite = Item.icon;
    }

    public void SetPosition(Vector2 position) {
        GetComponent<RectTransform>().localPosition = position;
    }
}
