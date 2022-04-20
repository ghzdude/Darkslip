using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeWriterFX : MonoBehaviour
{
    private string stored;
    private char[] startTxt;
    public float revealSpeed;
    private int index;
    [HideInInspector] public bool completed;


    public void TypeWriter(Text txt)
    {
        completed = false;
        ClearText();
        // txt = gameObject.GetComponent<Text>();
        stored = txt.text;
        startTxt = new char[txt.text.Length];
        txt.text = "";
        StartCoroutine(StartType(txt));
    }

    public void ClearText()
    {
        index = 0;
        stored = "";
        startTxt = new char[0];
    }

    public void Stop()
    {
        StopCoroutine("StartType");
    }

    IEnumerator StartType(Text txt)
    {
        while (txt.text.Length < stored.Length)
        {

            startTxt[index] = stored[index];
            txt.text += startTxt[index];
            index++;
            yield return new WaitForSecondsRealtime(revealSpeed/100);
        }
        completed = true;
        yield return null;
    }
}
