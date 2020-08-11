using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesManager : MonoBehaviour
{
    #region Singleton
    public static NotesManager instance;
    private void Awake()
    {
        if (instance != null) return;
        instance = this;
    }
    #endregion
    public List<Notes> notesList = new List<Notes>();
    public void AddNote(Notes newNote)
    {
        notesList.Add(newNote);
        NotesUIManager.instance.UpdateUI();
    }
}
