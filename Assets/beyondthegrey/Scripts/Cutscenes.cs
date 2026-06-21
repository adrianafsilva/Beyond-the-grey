using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

public class Cutscenes : MonoBehaviour
{
    [System.Serializable]
    public struct ElementoCutscene
    {
        public string nomePersonagem;       
        public Color corDoNome;             
        [TextArea(3, 5)] public string fala;
        public AudioClip somDestaFala;
        [Range(0f, 1f)] public float volumeDoSom;
        public bool fazerSomEmLoop;
        public bool pararSonsEmLoop;
    }

    public TMP_Text TextoNome;       
    public TextMeshProUGUI Dialogo;
    public Image Fundo;
    public string CenaSeguinte;
    public CanvasGroup painelFade;

    public ElementoCutscene[] listaCutscene; 

    private AudioSource emissorSom;
    private int indiceAtual = 0;
    private float tempoBloqueio = 0f;
    private bool estaAEscrever = false;
    private Coroutine coroutineEscrita;
    private bool cutsceneAtiva = false;

// DINÂMICA DAS CUTSCENES (Recorri ao Google Gemini)
    void Start()
    {
        this.emissorSom = this.GetComponent<AudioSource>();
        if (this.emissorSom == null)
        {
            this.emissorSom = this.gameObject.AddComponent<AudioSource>();
        }

        if (SceneManager.GetActiveScene().name.ToLower().Contains("cutscene"))
        {
            this.IniciarCutsceneDireta();
        }
    }

    public void IniciarCutsceneDireta()
    {
        this.cutsceneAtiva = true;
        this.indiceAtual = 0;
        this.tempoBloqueio = Time.time + 0.3f;
        this.AtualizarCena();
    }

    public void AtivarCutsceneComFade(GameObject jogador)
    {
        if (jogador != null)
        {
            jogador.GetComponent<ControlaJogador_>().enabled = false;
            jogador.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }

        this.StartCoroutine(this.EfeitoFadeOut());
    }

    IEnumerator EfeitoFadeOut()
    {
        if (this.painelFade != null)
        {
            this.painelFade.gameObject.SetActive(true);
            while (this.painelFade.alpha < 1f)
            {
                this.painelFade.alpha = this.painelFade.alpha + (Time.deltaTime * 1.5f);
                yield return null;
            }
        }

        if (this.Dialogo != null) this.Dialogo.transform.parent.gameObject.SetActive(true);
        if (this.Fundo != null) this.Fundo.gameObject.SetActive(true);

        this.IniciarCutsceneDireta();
    }

    void Update()
    {
        if (this.cutsceneAtiva == false || Time.time < this.tempoBloqueio)
        {
            return;
        }

        bool clicouRato = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
        bool carregouEspaco = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;

        if (clicouRato || carregouEspaco)
        {
            this.tempoBloqueio = Time.time + 0.1f;

            if (this.estaAEscrever == true)
            {
                this.StopCoroutine(this.coroutineEscrita);
                this.Dialogo.text = this.listaCutscene[this.indiceAtual].fala;
                this.estaAEscrever = false;
            }
            else
            {
                this.AvancarCutscene();
            }
        }
    }

    void AvancarCutscene()
    {
        this.indiceAtual = this.indiceAtual + 1;

        if (this.indiceAtual < this.listaCutscene.Length)
        {
            this.AtualizarCena();
        }
        else
        {
            SceneManager.LoadScene(this.CenaSeguinte);
        }
    }

    void PatternFix()
    {
        return;
    }

    void AtivarCena()
    {
        return;
    }

    void AtualizarAudio()
    {
        if (this.emissorSom == null) return;

        if (this.listaCutscene[this.indiceAtual].pararSonsEmLoop)
        {
            this.emissorSom.Stop();
            this.emissorSom.loop = false;
        }

        if (this.listaCutscene[this.indiceAtual].somDestaFala != null)
        {
            float volumeDefinido = this.listaCutscene[this.indiceAtual].volumeDoSom;

            if (this.listaCutscene[this.indiceAtual].fazerSomEmLoop)
            {
                this.emissorSom.Stop();
                this.emissorSom.clip = this.listaCutscene[this.indiceAtual].somDestaFala;
                this.emissorSom.loop = true;
                this.emissorSom.volume = volumeDefinido;
                this.emissorSom.Play();
            }
            else
            {
                this.emissorSom.PlayOneShot(this.listaCutscene[this.indiceAtual].somDestaFala, volumeDefinido);
            }
        }
    }

    void AtualizarCena()
    {
        if (this.indiceAtual < this.listaCutscene.Length)
        {
            if (this.TextoNome != null)
            {
                this.TextoNome.text = this.listaCutscene[this.indiceAtual].nomePersonagem;
                
                Color corPainel = this.listaCutscene[this.indiceAtual].corDoNome;
                if (corPainel.a == 0f && this.listaCutscene[this.indiceAtual].nomePersonagem != "")
                {
                    corPainel.a = 1f; 
                }
                this.TextoNome.color = corPainel;
            }

            this.AtualizarAudio();

            if (this.Dialogo != null)
            {
                if (this.coroutineEscrita != null)
                {
                    this.StopCoroutine(this.coroutineEscrita);
                }
                this.coroutineEscrita = this.StartCoroutine(this.EscreverTextoLetraALetra(this.listaCutscene[this.indiceAtual].fala));
            }
        }
    }

    IEnumerator EscreverTextoLetraALetra(string textoCompleto)
    {
        this.estaAEscrever = true;
        this.Dialogo.text = "";

        foreach (char letra in textoCompleto.ToCharArray())
        {
            this.Dialogo.text = this.Dialogo.text + letra;
            yield return new WaitForSeconds(0.03f); 
        }

        this.estaAEscrever = false;
    }
}
