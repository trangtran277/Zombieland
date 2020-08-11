using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotesUIManager : MonoBehaviour
{
    #region Singleton
    public static NotesUIManager instance;
    private void Awake()
    {
        if (instance != null) return;
        instance = this;
    }
    #endregion
    public GameObject noteReadingUI;
    public Text readingContent;
    public LayoutGroup noteListUI;
    public GameObject noteButton;
    NotesManager notesManager;
    int curCount = -1;

    private void Start()
    {
        notesManager = NotesManager.instance;
    }
    public void UpdateUI()
    {
        for(int i = 0; i <= notesManager.notesList.Count - 1; i++)
        {
            if(i > curCount)
            {
                GameObject newButton = Instantiate(noteButton, noteListUI.gameObject.transform);
                newButton.transform.SetAsFirstSibling();
                newButton.GetComponentInChildren<Text>().text = notesManager.notesList[i].title;
                newButton.GetComponent<NoteButton>().note = notesManager.notesList[i];
                curCount++;
            }
        }
    }
}
