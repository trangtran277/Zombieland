using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteButton : MonoBehaviour
{
    public Notes note;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        GameObject noteReadingUI = NotesUIManager.instance.noteReadingUI;
        Text readingContent = NotesUIManager.instance.readingContent;
        noteReadingUI.SetActive(true);
        noteReadingUI.GetComponentInChildren<Text>().text = note.title;
        readingContent.text = note.content;
    }
}
