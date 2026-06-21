using UnityEngine;
using System.Collections;

public class ControlaBoss : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform jogador;

    [Header("Configurações de Combate")]
    public GameObject projetilPrefab; 
    public Transform pontoDisparo;    
    public float forcaPulo = 12f;
    public float velocidadeTiro = 8f;
    public float intervaloAtaques = 3f;
    public float distanciaAtivacao = 25f;

// MECÂNICAS DO BOSS (Recorri ao Google Gemini)
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) jogador = playerObj.transform;
        StartCoroutine(RotinaCombate());
    }

    IEnumerator RotinaCombate()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervaloAtaques);

            if (jogador != null && Vector2.Distance(transform.position, jogador.position) < distanciaAtivacao)
            {

                int decisao = Random.Range(0, 3);

                if (decisao == 0) Saltar();
                else Atirar();
            }
        }
    }

    void Saltar()
    {
        if (jogador != null)
        {
            float direcao = (jogador.position.x > transform.position.x) ? 1 : -1;
            rb.linearVelocity = new Vector2(direcao * 5f, forcaPulo);
            InverterVisual(direcao);
        }
    }

    void Atirar()
    {
        if (projetilPrefab != null && pontoDisparo != null)
        {
            // Olhar na direção do jogador
            float direcaoParaJogador = (jogador.position.x > transform.position.x) ? 1 : -1;
            InverterVisual(direcaoParaJogador);

            GameObject tiro = Instantiate(projetilPrefab, pontoDisparo.position, Quaternion.identity);
            
            float direcaoBala = transform.localScale.x > 0 ? -1 : 1;
            
            Rigidbody2D rbTiro = tiro.GetComponent<Rigidbody2D>();
            if (rbTiro != null)
            {
                rbTiro.linearVelocity = new Vector2(direcaoBala * velocidadeTiro, 0);
            }

            Destroy(tiro, 1.5f); 
        }
    }

    void InverterVisual(float direcao)
    {
        float fatorX = Mathf.Abs(transform.localScale.x);

        if (direcao > 0) 
            transform.localScale = new Vector3(-fatorX, transform.localScale.y, transform.localScale.z);
        else 
            transform.localScale = new Vector3(fatorX, transform.localScale.y, transform.localScale.z);
    }
}