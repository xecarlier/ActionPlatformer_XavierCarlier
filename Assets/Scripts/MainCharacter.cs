using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainCharacter : MonoBehaviour
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
    private float inputV;
    private float startingGravity;
    private bool inFlight = false;
    private Animator anim;
    [Header("Sistema de combate")]
    [SerializeField]
    private Transform puntoAtaque;
    [SerializeField]
    private float radioAtaque;
    [SerializeField]
    private float danhoAtaque;
    [SerializeField]
    private float stormlightFlightCost;
    [SerializeField]
    private float stormlightHealCost;
    [SerializeField]
    private float stormlightHealMultiplier;
    [SerializeField]
    private LayerMask queEsDanhable;
    [SerializeField]
    private LayerMask queEsInteractable;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        startingGravity = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(EstoyEnSuelo())
        {
            anim.SetBool("flying", false);
            rb.gravityScale = startingGravity;
            inFlight = false;
        }
        else if(inFlight)
        {
            StormlightSystem stormlightSystem = gameObject.GetComponent<StormlightSystem>();
            if(stormlightSystem.HasEnoughStormlight(stormlightFlightCost))
            {
                stormlightSystem.UsarStormlight(stormlightFlightCost);
            }
            else
            {
                rb.gravityScale = startingGravity;
                inFlight = false;
            }
        }
        Movimiento();

        SaltarVolar();

        LanzarAtaque();

        StartInteract();

        HealWithStormlight();
    }

    private void StartInteract()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            anim.SetTrigger("interact");
        }
    }

    private void HealWithStormlight()
    {
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            StormlightSystem stormlightSystem = gameObject.GetComponent<StormlightSystem>();
            SistemaVidas sistemaVidas = gameObject.GetComponent<SistemaVidas>();
            if(stormlightSystem.HasEnoughStormlight(stormlightHealCost) && !sistemaVidas.isFullLife())
            {
                stormlightSystem.UsarStormlight(stormlightHealCost);
                sistemaVidas.CurarDanho(stormlightHealCost * stormlightHealMultiplier);
            }
        }
    }

    private void LanzarAtaque()
    {
        if(Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("attack");
            AudioManager.Instace.PlaySfx("sword_atk");
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

    //Se ejecuta en evento de animacion
    private void Interact()
    {
        Collider2D[] colliderTocados = Physics2D.OverlapCircleAll(puntoAtaque.position, radioAtaque, queEsInteractable);
        for (int i = 0; i < colliderTocados.Length; i++)
        {
            if(colliderTocados[i].gameObject.CompareTag("StormlightSource"))
            {
                StormlightSource source = colliderTocados[i].gameObject.GetComponent<StormlightSource>();
                StormlightSystem sistemaStormlight = gameObject.GetComponent<StormlightSystem>();
                sistemaStormlight.RecibirStormlight(source.DrainStormlight());
            }
            else if(colliderTocados[i].gameObject.CompareTag("WinCon"))
            {
                GameManager.Instace.ChangeScene("SampleScene");
            }
        }
    }

    private void SaltarVolar()
    {
        if(Input.GetKeyDown(KeyCode.Space) && EstoyEnSuelo())
        {
            rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            anim.SetTrigger("saltar");
        }
        else if(Input.GetKeyDown(KeyCode.Space) && inFlight)
        {
            rb.gravityScale = startingGravity;
            inFlight = false;
        }
        else if(Input.GetKeyDown(KeyCode.Space) && !EstoyEnSuelo())
        {
            StormlightSystem stormlightSystem = gameObject.GetComponent<StormlightSystem>();
            if(stormlightSystem.HasEnoughStormlight(0.1f))
            {
                anim.SetBool("flying", true);
                rb.gravityScale = 0;
                inFlight = true;
            }
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

        if(inFlight)
        {
            inputV = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, inputV * velocidadMovimiento);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(puntoAtaque.position, radioAtaque);
    }
}
