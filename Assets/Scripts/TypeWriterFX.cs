using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;

public class TypeWriterFX : MonoBehaviour
{
    private Text stored;
    private char[] startTxt;
    [Tooltip("Reveal Speed / 100")]
    public float revealSpeed;
    public AudioClip typeSound;
    private int waitIndex;
    [HideInInspector] public bool completed;
    public bool running;

    public void Run(Text text) {
        completed = false;
        ClearText();
        // txt = gameObject.GetComponent<Text>();
        stored = text;
        startTxt = new char[stored.text.Length];
        text.text = "";

        if (!running) {
            StartCoroutine(
                StartType(stored)
            );
        }
    }

    public void ClearText() {
        // index = 0;
        stored.text = "";
        startTxt = new char[0];
    }

    public void Stop() {
        StopCoroutine("StartType");
        running = false;
    }

    IEnumerator StartType(Text txt) {
        running = true;
        for (int i = 0; i < stored.text.Length; i++) {
            if (stored.text[i] == '\\' ) {
                waitIndex = int.Parse(stored.text[i + 1].ToString() + stored.text[i + 2].ToString());
                i += 2;
                for (int j = 0; j < waitIndex; j++) {
                    yield return new WaitForSecondsRealtime(revealSpeed / 100);
                }
                continue;
            }
            if (stored.text[i] != ' ') {
                Managers.GetMusic().GetAudioController().PlayClip(typeSound);
            }
            txt.text += stored.text[i];
            yield return new WaitForSecondsRealtime(revealSpeed / 100);
        }
        completed = true;
        running = false;
        yield return null;
    }
}
