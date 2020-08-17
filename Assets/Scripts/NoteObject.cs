using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteObject : MonoBehaviour, IInteractable
{
    public Notes note;
    public void Interact()
    {
        NotesManager.instance.AddNote(note);
        Destroy(gameObject);

        NotesUIManager.instance.ToggleNoteUI();
        GameObject noteReadingUI = NotesUIManager.instance.noteReadingUI;
        Text readingContent = NotesUIManager.instance.readingContent;
        noteReadingUI.SetActive(true);
        noteReadingUI.GetComponentInChildren<Text>().text = note.title;
        readingContent.text = note.content;
    }
}
