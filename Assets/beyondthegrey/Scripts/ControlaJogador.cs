using UnityEngine;
using UnityEngine.InputSystem;

public class ControlaJogador_ : MonoBehaviour
{
    public float velocidade = 5.5f;
    public GameObject Sprite; 
    private float ultimaDirecao = 1;

    public AudioClip somMoeda;
    public AudioClip somSalto;
    public AudioClip somDisparo;

    public GameObject balaPrefab;   
    public Transform pontoDisparo;  
    public float intervaloTiros = 0.5f;
    private float tempoProximoTiro = 0f;
    
    private bool aDisparar = false;

    void Update()
    {
        float moverHorizontal = 0;

        if (this.aDisparar == false)
        {
            if (Keyboard.current.aKey.isPressed)
            {
                moverHorizontal = -1;
                this.ultimaDirecao = -1;
                this.Sprite.GetComponent<SpriteRenderer>().flipX = true;
                this.Sprite.GetComponent<Animator>().Play("jogador_andar");
            }
            else if (Keyboard.current.dKey.isPressed)
            {
                moverHorizontal = 1;
                this.ultimaDirecao = 1;
                this.Sprite.GetComponent<SpriteRenderer>().flipX = false;
                this.Sprite.GetComponent<Animator>().Play("jogador_andar");
            }
            else
            {
                this.Sprite.GetComponent<Animator>().Play("jogador_idle");
                
                if (this.ultimaDirecao == -1)
                {
                    this.Sprite.GetComponent<SpriteRenderer>().flipX = true;
                }
                else
                {
                    this.Sprite.GetComponent<SpriteRenderer>().flipX = false;
                }
            }
        }
        else
        {
            if (Keyboard.current.aKey.isPressed) moverHorizontal = -1;
            else if (Keyboard.current.dKey.isPressed) moverHorizontal = 1;
        }

        this.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(moverHorizontal * this.velocidade, this.GetComponent<Rigidbody2D>().linearVelocity.y);

        if (Keyboard.current.spaceKey.wasPressedThisFrame && Mathf.Abs(this.GetComponent<Rigidbody2D>().linearVelocity.y) < 0.1f)
        {
            this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1100f));
            this.GetComponent<AudioSource>().PlayOneShot(this.somSalto);
        }

        if (Mouse.current.leftButton.wasPressedThisFrame && Time.time >= this.tempoProximoTiro)
        {
            this.Disparar();
            this.GetComponent<AudioSource>().PlayOneShot(this.somDisparo);
            this.tempoProximoTiro = Time.time + this.intervaloTiros;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Moeda"))
        {
            this.GetComponent<AudioSource>().PlayOneShot(this.somMoeda);
            GameObject.Find("GestorDeJogo").GetComponent<GestorDeJogo>().AddCoin(1);
            Destroy(col.gameObject);
        }

        if (col.gameObject.CompareTag("Vida"))
        {
            int vidasAtuais = GameObject.Find("GestorDeJogo").GetComponent<GestorDeJogo>().vidas;

            if (vidasAtuais < 3)
            {
                col.gameObject.SetActive(false); 
                this.GetComponent<AudioSource>().PlayOneShot(this.somMoeda);
                GameObject.Find("GestorDeJogo").GetComponent<GestorDeJogo>().GanharVida();
                Destroy(col.gameObject);
            }
        }
    }

    void Disparar()
    {
        GameObject novaBala = Instantiate(this.balaPrefab, this.pontoDisparo.position, Quaternion.identity);
        
        this.aDisparar = true;

        this.Sprite.GetComponent<Animator>().Play("jogador_disparo"); 

        this.CancelInvoke("TerminarDisparo");
        this.Invoke("TerminarDisparo", 0.3f);

        float direcaoBala = 15f; 
        if (this.ultimaDirecao == -1) 
        {
            direcaoBala = -15f;
            this.Sprite.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            this.Sprite.GetComponent<SpriteRenderer>().flipX = false;
        }

        novaBala.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(direcaoBala, 0);
        Destroy(novaBala, 2f);
    }

    void TerminarDisparo()
    {
        this.aDisparar = false;
        
        if (this.ultimaDirecao == -1)
        {
            this.Sprite.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            this.Sprite.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
}
