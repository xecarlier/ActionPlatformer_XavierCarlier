using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Sistema de movimiento")]
    [SerializeField]
    private float velocidadMovimiento;
    [SerializeField]
    private float fuerzaSalto;
    [SerializeField]
    private float distanciaDeteccionSuelo;
    [SerializeField]
    private Transform pies;
    [SerializeField]
    private LayerMask queEsSaltable;
    private Rigidbody2D rb;
    private float inputH;
    private Animator anim;
    [Header("Sistema de combate")]
    [SerializeField]
    private Transform puntoAtaque;
    [SerializeField]
    private float radioAtaque;
    [SerializeField]
    private float danhoAtaque;
    [SerializeField]
    private LayerMask queEsDanhable;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movimiento();

        Saltar();

        LanzarAtaque();
    }

    private void LanzarAtaque()
    {
        if(Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("attack");
        }
    }

    //Se ejecuta en evento de animacion
    private void Ataque()
    {
        Collider2D[] colliderTocados = Physics2D.OverlapCircleAll(puntoAtaque.position, radioAtaque, queEsDanhable);
        for (int i = 0; i < colliderTocados.Length; i++)
        {
            SistemaVidas sistemaVidas = colliderTocados[i].gameObject.GetComponent<SistemaVidas>();
            sistemaVidas.RecibirDanho(danhoAtaque);
        }
    }

    private void Saltar()
    {
        if(Input.GetKeyDown(KeyCode.Space) && EstoyEnSuelo())
        {
            rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            anim.SetTrigger("saltar");
        }
    }

    private bool EstoyEnSuelo()
    {
        Debug.DrawRay(pies.position, Vector3.down, Color.red, 0.3f);
        return Physics2D.Raycast(pies.position, Vector3.down, distanciaDeteccionSuelo, queEsSaltable);
    }

    private void Movimiento()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(inputH * velocidadMovimiento, rb.velocity.y);
        if(inputH != 0)
        {
            anim.SetBool("running", true);
            if(inputH > 0)
            {
                transform.eulerAngles = Vector3.zero;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
        else
        {
            anim.SetBool("running", false);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(puntoAtaque.position, radioAtaque);
    }
}
