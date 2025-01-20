using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SistemaVidas : MonoBehaviour
{
    [SerializeField] private float maxVidas;
    [SerializeField] private float currentVidas;
    [SerializeField]
    private Image healthBar;

    void Update()
    {
        if(healthBar != null)
        {
            healthBar.fillAmount = Mathf.MoveTowards(healthBar.fillAmount, currentVidas/maxVidas, Time.deltaTime);
        }
    }

    public void RecibirDanho(float danhoRecibido)
    {
        currentVidas -= danhoRecibido;
        if(currentVidas <= 0)
        {
            Destroy(this.gameObject);
        }
        AudioManager.Instace.PlaySfx("hit");
    }

    public void CurarDanho(float vidaRecibida)
    {
        currentVidas += vidaRecibida;
        if(currentVidas >= maxVidas)
        {
            currentVidas = maxVidas;
        }
    }

    public bool isFullLife()
    {
        return currentVidas >= maxVidas;
    }
}
