using UnityEngine;

public class ProjetilBoss : MonoBehaviour 
{
    public float velocidade = 10f;

    void Start() 
    {
        GameObject jogador = GameObject.Find("Jogador");

        // Definir direção (Recorri ao Google/Youtube)
        float direcaoX = 1f;
        if (jogador.transform.position.x < this.transform.position.x)
        {
            direcaoX = -1f;
        }

        this.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(direcaoX * this.velocidade, 0);
        Destroy(this.gameObject, 3f);
    }

    void OnTriggerEnter2D(Collider2D col) 
    {
        if (col.gameObject.name == "Jogador") 
        {
            Destroy(this.gameObject);
        }
    }
}
