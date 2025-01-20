using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaFuego : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float impulsoDisparo;
    [SerializeField] private float danhoDisparo;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * impulsoDisparo, ForceMode2D.Impulse);
        StartCoroutine(timeToLive());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator timeToLive()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerHitbox")) 
        { 
            SistemaVidas sistemaVidas = collision.gameObject.GetComponent<SistemaVidas>();
            sistemaVidas.RecibirDanho(danhoDisparo);
            AudioManager.Instace.PlaySfx("hit");
            Destroy(gameObject, AudioManager.Instace.GetSfxClip("hit").length);
        } 
    }
}
