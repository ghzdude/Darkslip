using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Enums {
    public enum Character {Sean, Doctor, Static}
    public enum Direction {Left, Right, Up, Down}
    public enum InventoryType {TextLog, Clipboard, Plant, Other}
    public enum Music {MainMenu, DockingBay3, Lobby, Tram, Office, Credits}
}

public static class Managers {
    private static readonly string DialogueManager = "DialogueManager";
    private static readonly string SceneManager = "SceneController";
    private static readonly string InventoryManager = "InventoryManager";
    private static readonly string Player = "Player";
    private static readonly string Camera = "MainCamera";

    public static DialogueManager GetDialogueManager() => GameObject.FindGameObjectWithTag(DialogueManager).GetComponent<DialogueManager>();
    public static SceneController GetSceneController() => GameObject.FindGameObjectWithTag(SceneManager).GetComponent<SceneController>();
    public static InventoryManager GetInventoryManager() => GameObject.FindGameObjectWithTag(InventoryManager).GetComponent<InventoryManager>();
    public static PlayerController GetPlayerController() => GameObject.FindGameObjectWithTag(Player).GetComponent<PlayerController>();
    public static Transform GetCamera() => GameObject.FindGameObjectWithTag(Camera).transform;
}

public static class Scenes {
    public static readonly string Manager = "Managers";
    public static readonly string DockingBay3 = "dockingbay3";
    public static readonly string Lobby = "lobby";
    public static readonly string Tram = "tram";
    public static readonly string Office = "office";
    public static readonly string Labs = "labs";
    public static readonly string DeepLabs = "deeplabs";
    public static readonly string Darkslip = "darkslip";
    public static readonly string Credits = "credits";
}

public static class VectorMath {
    public static Vector2 CalculateVector(Vector2 a, Vector2 b) => b - a;

    public static Vector3 LookAt2D(Vector3 pointer, Vector3 pos) {
        Vector3 target = new Vector3(pos.x - pointer.x, pos.y - pointer.y);
        float rotation = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        target = new Vector3(0f, 0f, rotation - 90f);
        return target;
    }
}

public static class Tags {
    public static readonly string Destructible = "Destructible";
    public static readonly string Interactable = "Interactable";
    public static readonly string EndDemo = "EndDemo";
    public static readonly string Dialogue = "Dialogue";
    public static readonly string Wall = "Wall";
    public static readonly string Door = "Door";
    public static readonly string Collectable = "Collectable";
    public static readonly string ItemEntry = "ItemEntry";
    public static readonly string Canvas = "Canvas";
    public static readonly string Music = "Music";
    public static readonly string Tram = "Tram";
    public static readonly string DialogueManager = "DialogueManager";
    public static readonly string SceneManager = "SceneController";
    public static readonly string InventoryManager = "InventoryManager";
    public static readonly string Player = "Player";
    public static readonly string Camera = "MainCamera";
}