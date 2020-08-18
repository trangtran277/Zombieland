using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{
    public LightingManager lighting;
    public CanvasGroup canvasGroup;
    public void Interact()
    {
        canvasGroup.gameObject.SetActive(true);
        StartCoroutine(FadeUI(0, 1));
    }

    IEnumerator FadeUI(float start, float end, float lerpTime = 2f)
    {
        float timeStartLerping = Time.time;
        float timeSinceStarted = Time.time - timeStartLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while(percentageComplete < 1)
        {
            timeSinceStarted = Time.time - timeStartLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);
            canvasGroup.alpha = currentValue;
            yield return new WaitForEndOfFrame();
        }
        lighting.TimeOfDay = 5f;

        timeStartLerping = Time.time;
        timeSinceStarted = Time.time - timeStartLerping;
        percentageComplete = timeSinceStarted / lerpTime;
        while (percentageComplete < 1)
        {
            timeSinceStarted = Time.time - timeStartLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(end, start, percentageComplete);
            canvasGroup.alpha = currentValue;
            yield return new WaitForEndOfFrame();
        }
        canvasGroup.gameObject.SetActive(false);

    }
}
