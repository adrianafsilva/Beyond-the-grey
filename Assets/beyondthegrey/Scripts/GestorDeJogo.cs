using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GestorDeJogo : MonoBehaviour
{
    public int moedas = 0;
    public int vidas = 3;
    public TextMeshProUGUI textoMoedas;
    public Image iconeMoedas;

    public Image displayCoracoes;
    public Sprite[] spritesCoracoes;

    public Volume volumePostProcess;
    public GameObject Jogador;
    private ColorAdjustments cores;
    private float velocidadOriginal;
    private Vector3 pontoRespawn;

    public AudioSource musicaDoNivel;

    void Start()
    {
        // (Da linha 30 à linha 111, recorri ao Gemini para saber como criar uma dinâmica de menu gameover)

        // Verifica se o jogador já tinha tocado num checkpoint antes de morrer
        if (PlayerPrefs.HasKey("CheckpointX"))
        {
            float posX = PlayerPrefs.GetFloat("CheckpointX");
            float posY = PlayerPrefs.GetFloat("CheckpointY");
            float posZ = PlayerPrefs.GetFloat("CheckpointZ");
            
            this.pontoRespawn = new Vector3(posX, posY, posZ);
            this.Jogador.transform.position = this.pontoRespawn;
        }
        else
        {
            // Se for a primeira vez a jogar o nível, a posição inicial passa a ser o respawn padrão
            this.pontoRespawn = this.Jogador.transform.position;
        }

        this.velocidadOriginal = this.Jogador.GetComponent<ControlaJogador_>().velocidade;
        
        // Proteção caso o volume não esteja arrastado no Inspector
        if (this.volumePostProcess == null)
        {
            this.volumePostProcess = GameObject.FindFirstObjectByType<Volume>();
        }

        if (this.volumePostProcess != null)
        {
            this.volumePostProcess.profile.TryGet(out this.cores);
        }
        
        this.AtualizarEstado();
    }

    public void AddCoin(int valor)
    {
        this.moedas = this.moedas + valor;
        this.AtualizarInterface();
    }

    public void PerderVida()
    {
        this.vidas = this.vidas - 1;
        this.AtualizarEstado();

        if (this.vidas <= 0)
        {
            this.GameOverReset();
        }
    }

    void GameOverReset()
    {
        this.vidas = 3;
        this.moedas = 0;

        // Desliga a música em loop antes de ir para o GameOver
        if (this.musicaDoNivel != null)
        {
            this.musicaDoNivel.Stop();
        }

        SceneManager.LoadScene("gameover"); 
    }

    public void GanharVida()
    {
        if (this.vidas < 3)
        {
            this.vidas = this.vidas + 1;
            this.AtualizarEstado();
        }
    }

    public void AtualizarRespawn(Vector3 novaPosicao)
    {
        this.pontoRespawn = novaPosicao;

        // Gravar o checkpoint na memória
        PlayerPrefs.SetFloat("CheckpointX", novaPosicao.x);
        PlayerPrefs.SetFloat("CheckpointY", novaPosicao.y);
        PlayerPrefs.SetFloat("CheckpointZ", novaPosicao.z);
        PlayerPrefs.Save();
    }

    void AtualizarInterface()
    {
        this.textoMoedas.text = ":" + this.moedas;

        if (this.displayCoracoes != null && this.spritesCoracoes.Length >= 3)
        {
            if (this.vidas == 3)
            {
                this.displayCoracoes.enabled = true;
                this.displayCoracoes.sprite = this.spritesCoracoes[0];
            }
            else if (this.vidas == 2)
            {
                this.displayCoracoes.enabled = true;
                this.displayCoracoes.sprite = this.spritesCoracoes[1];
            }
            else if (this.vidas == 1)
            {
                this.displayCoracoes.enabled = true;
                this.displayCoracoes.sprite = this.spritesCoracoes[2];
            }
            else if (this.vidas <= 0)
            {
                this.displayCoracoes.enabled = false;
                this.textoMoedas.enabled = false;
                if (this.iconeMoedas != null) 
                {
                    this.iconeMoedas.enabled = false; 
                }
            }
        }
    }

    public void AtualizarEstado()
    {
        this.AtualizarInterface();

        if (this.cores != null)
        {
            float perda = (this.vidas - 3) * 35f;
            this.cores.saturation.value = perda;
        }

        float novoCalculo = 0.60f + (this.vidas * 0.13f);
        this.Jogador.GetComponent<ControlaJogador_>().velocidade = this.velocidadOriginal * novoCalculo;
    }
}
