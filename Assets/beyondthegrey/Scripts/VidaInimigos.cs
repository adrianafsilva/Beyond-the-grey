using UnityEngine;
using Unity.Cinemachine;

public class VidaInimigos : MonoBehaviour
{
    public int vidaAtual = 3;
    public GameObject[] barreiras;
    public CinemachineCamera vcam; 
    public Transform jogador;

    private bool aPiscar = false;

    public void ReceberDano(int quantidade)
    {
        this.vidaAtual = this.vidaAtual - quantidade;

        this.aPiscar = true;
        this.EfeitoPiscarInimigo();
        this.Invoke("PararPiscarInimigo", 0.3f);

        if (this.vidaAtual <= 0) 
        {
            this.Morrer();
        }
    }

    void EfeitoPiscarInimigo()
    {
        if (this.aPiscar == true)
        {
            bool estado = this.GetComponentInChildren<SpriteRenderer>().enabled;
            this.GetComponentInChildren<SpriteRenderer>().enabled = !estado;
            this.Invoke("EfeitoPiscarInimigo", 0.08f);
        }
    }

    void PararPiscarInimigo()
    {
        this.aPiscar = false;
        this.GetComponentInChildren<SpriteRenderer>().enabled = true;
        this.CancelInvoke("EfeitoPiscarInimigo");
    }

    void Morrer()
    {
        for (int i = 0; i < this.barreiras.Length; i++)
        {
            if (this.barreiras[i] != null)
            {
                this.barreiras[i].SetActive(false);
            }
        }

        if (this.vcam != null)
        {
            this.vcam.Follow = this.jogador;
        }

        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "ProjetilJogador" || col.gameObject.name == "ProjetilJogador(Clone)")
        {
            this.ReceberDano(1);
            Destroy(col.gameObject);
        }
    }
}
