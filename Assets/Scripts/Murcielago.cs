using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Murcielago : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float velocidadPatrulla;
    [SerializeField] private float danhoAtaque;
    private GameObject target;
    private Vector3 destinoActual;
    private int indiceActual = 0;
    // Start is called before the first frame update
    void Start()
    {
        destinoActual = waypoints[indiceActual].position;
        StartCoroutine(Patrulla());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Patrulla()
    {
        while(true)
        {
            while(transform.position != destinoActual)
            {
                transform.position = Vector3.MoveTowards(transform.position, destinoActual, velocidadPatrulla * Time.deltaTime);
                yield return null;
            }
            DefinirNuevoDestino();
        }
    }

    private void DefinirNuevoDestino()
    {
        if(target == null)
        {
            indiceActual++;
            if(indiceActual >= waypoints.Length)
            {
                indiceActual = 0;
            }
            destinoActual = waypoints[indiceActual].position;
        }
        else
        {
            destinoActual = target.transform.position;
        }
        EnfocarDestino();
    }

    private void EnfocarDestino()
    {
        if(destinoActual.x > transform.position.x)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if(elOtro.gameObject.CompareTag("DeteccionPlayer"))
        {
            target = elOtro.gameObject;
        }
        else if(elOtro.gameObject.CompareTag("PlayerHitbox"))
        {
            SistemaVidas sistemaVidas = elOtro.gameObject.GetComponent<SistemaVidas>();
            sistemaVidas.RecibirDanho(danhoAtaque);
        }
    }

    private void OnTriggerExit2D(Collider2D elOtro)
    {
        if(elOtro.gameObject.CompareTag("DeteccionPlayer"))
        {
            target = null;
        }
    }
}
