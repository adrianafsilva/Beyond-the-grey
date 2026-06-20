using UnityEngine;

public class DanoJogador : MonoBehaviour
{
    public GameObject SpriteJogador;
    public bool estaInvencivel = false;
    public float tempoInvencivel = 2.0f;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Perigo"))
        {
            this.estaInvencivel = false;
            this.SpriteJogador.GetComponent<SpriteRenderer>().enabled = true;
            this.CancelInvoke("EfeitoPiscar");
            
            GestorDeJogo gm = GameObject.Find("GestorDeJogo").GetComponent<GestorDeJogo>();
            gm.vidas = 0; 
            gm.PerderVida(); 
        }

        if (col.gameObject.CompareTag("Inimigo") && this.estaInvencivel == false)
        {
            GameObject.Find("GestorDeJogo").GetComponent<GestorDeJogo>().PerderVida();
            this.estaInvencivel = true;
            this.EfeitoPiscar();
            this.Invoke("TerminarInvencibilidade", this.tempoInvencivel);
        }
    }

    void EfeitoPiscar()
    {
        if (this.estaInvencivel == true)
        {
            bool estado = this.SpriteJogador.GetComponent<SpriteRenderer>().enabled;
            this.SpriteJogador.GetComponent<SpriteRenderer>().enabled = !estado;
            this.Invoke("EfeitoPiscar", 0.1f);
        }
    }

    void TerminarInvencibilidade()
    {
        this.estaInvencivel = false;
        this.SpriteJogador.GetComponent<SpriteRenderer>().enabled = true;
        this.CancelInvoke("EfeitoPiscar");
    }
}
