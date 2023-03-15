using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class Enums
    {
        public enum Character { Sean, Doctor, Static }
        public enum Direction { Left, Right, Up, Down }
        public enum Music { MainMenu, DockingBay3, Lobby, Tram, Office, Credits }
    }

    public class Managers
    {
        private static RectTransform Canvas;
        public static SceneController GetSceneController() => GameObject.FindGameObjectWithTag(Tags.SceneManager).GetComponent<SceneController>();
        public static DialogueManager GetDialogueManager() => GameObject.FindGameObjectWithTag(Tags.DialogueManager).GetComponent<DialogueManager>();
        public static PlayerController GetPlayerController() => GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<PlayerController>();
        public static RectTransform GetCanvas() {
            if (Canvas == null)
                Canvas = GameObject.FindGameObjectWithTag(Tags.Canvas).GetComponent<RectTransform>();
            return Canvas;
        }
        public static CanvasManager GetCanvasManager() => GetCanvas().GetComponent<CanvasManager>();
        public static InventoryManager GetInventoryManager() => GetCanvasManager().InventoryManager;
        public static MusicManager GetMusic() => GameObject.FindGameObjectWithTag(Tags.Music).GetComponent<MusicManager>();
        public static Transform GetSpawnPoint() => GameObject.FindGameObjectWithTag(Tags.SpawnPoint).transform;
        public static Transform GetCamera() => GameObject.FindGameObjectWithTag(Tags.Camera).transform;
    }

    public class Scenes
    {
        public const string Manager = "Managers";
        public const string DockingBay3 = "dockingbay3";
        public const string Lobby = "lobby";
        public const string Tram = "tram";
        public const string Office = "office";
        public const string Labs = "labs";
        public const string DeepLabs = "deeplabs";
        public const string Darkslip = "darkslip";
        public const string Credits = "credits";
    }

    public class VectorMath
    {
        public static Vector2 CalculateVector(Vector2 a, Vector2 b) => b - a;

        public static Vector3 LookAt2D(Vector3 self, Vector3 lookAt)
        {
            Vector3 target = new Vector3(lookAt.x - self.x, lookAt.y - self.y);
            float rotation = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
            target = new Vector3(0f, 0f, rotation - 90f);
            return target;
        }
    }

    public static class Tags
    {
        public const string AudioController = "AudioController";
        public const string Destructible = "Destructible";
        public const string Interactable = "Interactable";
        public const string EndDemo = "EndDemo";
        public const string Dialogue = "Dialogue";
        public const string Wall = "Wall";
        public const string Door = "Door";
        public const string Collectable = "Collectable";
        public const string ItemEntry = "ItemEntry";
        public const string Canvas = "Canvas";
        public const string Music = "Music";
        public const string Tram = "Tram";
        public const string DialogueManager = "DialogueManager";
        public const string SceneManager = "SceneController";
        public const string InventoryManager = "InventoryManager";
        public const string Player = "Player";
        public const string Camera = "MainCamera";
        public const string SpawnPoint = "SpawnPoint";
    }

    public class SeanAnimationStates
    {
        public const string IdleDown = "seanIdleDown";
        public const string IdleUp = "seanIdleUp";
        public const string IdleRight = "seanIdleRight";
        public const string IdleLeft = "seanIdleLeft";
  
        public const string WalkDown = "seanWalkDown";
        public const string WalkUp = "seanWalkUp";
        public const string WalkRight = "seanWalkRight";
        public const string WalkLeft = "seanWalkLeft";

        public const string AttackDown = "seanAttackDown";
        public const string AttackUp = "seanAttackUp";
        public const string AttackRight = "seanAttackRight";
        public const string AttackLeft = "seanAttackLeft";

        public const string IdleDownAttack = "seanIdleDownAttack";
        public const string IdleRightAttack = "seanIdleRightAttack";
        public const string IdleLeftAttack = "seanIdleLeftAttack";

        public const string WalkDownAttack = "seanWalkDownAttack";
        public const string WalkRightAttack = "seanWalkRightAttack";
        public const string WalkLeftAttack = "seanWalkLeftAttack";
    }

    public class Paths {
        public const string Player = "Prefabs/Sean";
        public const string ItemEntry = "Prefabs/UI/Item Entry";
        public const string StaticAnimator = "Animations/staticPortrait1";
    }
}