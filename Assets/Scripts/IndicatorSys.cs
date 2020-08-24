using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorSys : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("References")]
    [SerializeField] private Indicator indicatorPrefab = null;
    [SerializeField] private RectTransform holder = null;
    [SerializeField] private new Camera camera = null;
    [SerializeField] private Transform player = null;

    private Dictionary<Transform, Indicator> Indicators = new Dictionary<Transform, Indicator>();

    #region Delegates
    public static Action<Transform,Transform> CreateIndicator = delegate { };
    public static Func<Transform, bool> CheckIfObjectInSight = null;
    #endregion

    private void OnEnable()
    {
        CreateIndicator += Create;
        CheckIfObjectInSight += Insight;
    }

    /*private void OnDisable()
    {
        CreateIndicator -= Create;
        CheckIfObjectInSight -= Insight;
    }*/

    void Create(Transform target,Transform playerr)
    {
        if (Indicators.ContainsKey(target))
        {
            Indicators[target].Restart();
            return;
        }

        Indicator newIndicator = Instantiate(indicatorPrefab, holder);
        newIndicator.Register(target, playerr, new Action(() => {  }));//Indicators.Remove(target);

        Indicators.Add(target, newIndicator);
    }

    bool Insight(Transform t)
    {
        Vector3 screenPoint = camera.WorldToViewportPoint(t.position);
        return (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1);
    }
}
