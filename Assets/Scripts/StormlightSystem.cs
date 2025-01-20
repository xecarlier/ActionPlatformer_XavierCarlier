using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class StormlightSystem : MonoBehaviour
{
    [SerializeField] private float maxStormlight;
    [SerializeField] private float currentStormlight;
    [SerializeField]
    private Image stormlightBar;
    [SerializeField] private Light2D stormlightGlow;
    [SerializeField] private float stormlightSfxThreshhold;
    private bool isSfxActive;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        stormlightBar.fillAmount = Mathf.MoveTowards(stormlightBar.fillAmount, currentStormlight/maxStormlight, Time.deltaTime);
        stormlightGlow.intensity = Mathf.MoveTowards(stormlightGlow.intensity, currentStormlight, Time.deltaTime * 100);

        if (!isSfxActive && HasEnoughStormlight(stormlightSfxThreshhold))
        {
            AudioManager.Instace.PlayStormlight("stormlight");
            isSfxActive = true;
        }
        else if (isSfxActive && !HasEnoughStormlight(stormlightSfxThreshhold))
        {
            AudioManager.Instace.StopAudioStormlight();
            isSfxActive = false;
        }
    }

    public void RecibirStormlight(float stormlightRecibido)
    {
        currentStormlight += stormlightRecibido;
        if(currentStormlight > maxStormlight)
        {
            currentStormlight = maxStormlight;
        }
    }

    public void UsarStormlight(float stormlightRecibido)
    {
        currentStormlight -= stormlightRecibido;
        if(currentStormlight < 0)
        {
            currentStormlight = 0;
        }
    }

    public bool HasEnoughStormlight(float stormlightRequired)
    {
        return currentStormlight >= stormlightRequired;
    }
}
