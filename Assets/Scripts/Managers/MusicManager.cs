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
    private AudioController audioController;

    private void Awake() {
        audioController = GetComponent<AudioController>();
    }

    public void SetMusic(Enums.Music type) {
        AudioClip clip = null;
        
        switch (type) {
            case Enums.Music.MainMenu:
                clip = MainMenu;
                break;
            case Enums.Music.DockingBay3:
                clip = DockingBay3;
                break;
            case Enums.Music.Lobby:
                clip = Lobby;
                break;
            case Enums.Music.Tram:
                clip = Tram;
                break;
            case Enums.Music.Office:
                clip = Office;
                break;
            case Enums.Music.Credits:
                clip = Credits;
                break;
            default:
                break;
        }

        if (clip == null)
            return;

        audioController.PlayMusic(clip);
    }

    public AudioController GetAudioController() => audioController;
}
