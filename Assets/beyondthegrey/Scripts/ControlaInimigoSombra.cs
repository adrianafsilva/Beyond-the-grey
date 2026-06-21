using UnityEngine;

public class InimigoSimples : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidade = 2f;
    public float tempoDeCaminhada = 2f;

    private Rigidbody2D rb;
    private bool movendoParaDireita = true;
    private float cronometro;

// PATRULHA DOS INIMIGOS (Recorri ao Google Gemini)
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        cronometro = tempoDeCaminhada;
    }

    void Update()
    {
        cronometro -= Time.deltaTime;

        if (cronometro <= 0)
        {
            InverterDirecao();
            cronometro = tempoDeCaminhada;
        }
    }

    void FixedUpdate()
    {
        float velocidadeAtual = movendoParaDireita ? velocidade : -velocidade;
        rb.linearVelocity = new Vector2(velocidadeAtual, rb.linearVelocity.y);
    }

    void InverterDirecao()
    {
        movendoParaDireita = !movendoParaDireita;

        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }
}
