using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeWriterFX : MonoBehaviour
{
    private string stored;
    private char[] startTxt;
    [Tooltip("Reveal Speed / 100")]
    public float revealSpeed;
    public AudioClip typeSound;
    private AudioSource src;
    private int waitIndex;
    [HideInInspector] public bool completed;
    private DialogueManager DialogueManager;
    public bool running;

    private void Awake() {
        DialogueManager = GetComponent<DialogueManager>();
    }

    public void TypeWriter(Text txt, AudioSource src) {
        this.src = src;
        completed = false;
        ClearText();
        // txt = gameObject.GetComponent<Text>();
        stored = txt.text;
        startTxt = new char[stored.Length];
        txt.text = "";
        if (!running) {
            StartCoroutine(StartType(txt));
        }
    }

    public void ClearText() {
        // index = 0;
        stored = "";
        startTxt = new char[0];
    }

    public void Stop() {
        StopCoroutine("StartType");
        running = false;
    }

    IEnumerator StartType(Text txt) {
        running = true;
        for (int i = 0; i < stored.Length; i++) {
            if (stored[i] == '\\' ) {
                waitIndex = int.Parse(stored[i + 1].ToString() + stored[i + 2].ToString());
                i += 2;
                for (int j = 0; j < waitIndex; j++) {
                    yield return new WaitForSecondsRealtime(revealSpeed / 100);
                }
                continue;
            }
            if (stored[i] != ' ') {
                src.PlayOneShot(typeSound, Managers.GetDialogueManager().GetSFXSliderValue() / 3);
            }
            txt.text += stored[i];
            yield return new WaitForSecondsRealtime(revealSpeed / 100);
        }
        /*
        while (txt.text.Length < stored.Length) {
            startTxt[index] = stored[index];
            txt.text += startTxt[index];
            index++;
            yield return new WaitForSecondsRealtime(revealSpeed / 100);
        }*/
        completed = true;
        running = false;
        yield return null;
    }
}
