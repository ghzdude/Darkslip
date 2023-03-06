using UnityEngine;
using Common;

public class MusicManager : MonoBehaviour
{
    public AudioClip MainMenu;
    public AudioClip DockingBay3;
    public AudioClip Lobby;
    public AudioClip Tram;
    public AudioClip Office;
    public AudioClip Credits;
    private AudioSource src;

    private void Awake() {
        src = GetComponent<AudioSource>();
    }

    public void SetMusic(Enums.Music type) {
        switch (type) {
            case Enums.Music.MainMenu:
                if (MainMenu != null) {
                    src.clip = MainMenu;
                }
                break;
            case Enums.Music.DockingBay3:
                if (DockingBay3 != null) {
                    src.clip = DockingBay3;
                }
                break;
            case Enums.Music.Lobby:
                if (Lobby != null) {
                    src.clip = Lobby;
                }
                break;
            case Enums.Music.Tram:
                if (Tram != null) {
                    src.clip = Tram;
                }
                break;
            case Enums.Music.Office:
                if (Office != null) {
                    src.clip = Office;
                }
                break;
            case Enums.Music.Credits:
                if (Credits != null) {
                    src.clip = Credits;
                }
                break;
            default:
                break;
        }

        if (!src.isPlaying) {
            src.Play();
        }
    }
}
