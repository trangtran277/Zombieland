using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour, IInteractable
{
    public Notes note;
    public void Interact()
    {
        NotesManager.instance.AddNote(note);
        Destroy(gameObject);
    }
}
