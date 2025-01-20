using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class StormlightSource : MonoBehaviour
{
    [SerializeField] private float maxStormlight;
    [SerializeField] private float currentStormlight;
    [SerializeField] private Light2D stormlightGlow;
    [SerializeField] private Color32[] stormlightColors;

    void Start()
    {
        int index = Random.Range( 0, stormlightColors.Length);
        stormlightGlow.color = stormlightColors[index];
    }

    // Update is called once per frame
    void Update()
    {
        stormlightGlow.intensity = Mathf.MoveTowards(stormlightGlow.intensity, currentStormlight, Time.deltaTime * 100);
        if(currentStormlight < maxStormlight)
        {
            currentStormlight = Mathf.MoveTowards(currentStormlight, maxStormlight, Time.deltaTime);
        }
    }

    public float DrainStormlight()
    {
        float stormlight = currentStormlight;
        currentStormlight = 0;
        return stormlight;
    }
}
