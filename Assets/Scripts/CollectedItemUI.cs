using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectedItemUI : MonoBehaviour
{
    #region Singleton
    public static CollectedItemUI instance;
    private void Awake()
    {
        if (instance != null) return;
        instance = this;
    }
    #endregion
    public Queue<Collectibles> collectedItems = new Queue<Collectibles>();
    public Image collectedItemIcon;
    public Text collectedItemName;

    private void Update()
    {
        StartCoroutine(DisplayCollectedItem());
    }

    IEnumerator DisplayCollectedItem()
    {
        if (collectedItems.Count > 0 && !collectedItemIcon.transform.parent.gameObject.activeSelf)
        {
            Collectibles collected = collectedItems.Dequeue();
            collectedItemIcon.transform.parent.gameObject.SetActive(true);
            collectedItemIcon.sprite = collected.item.icon;
            collectedItemName.text = "+" + collected.number + " " + collected.item.name;
            collectedItemIcon.transform.parent.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            collectedItemIcon.transform.parent.gameObject.SetActive(false);
        }
        
    }

}
