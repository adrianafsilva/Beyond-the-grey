using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public GameObject textoNoCanvas; 

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GameObject.Find("GestorDeJogo").GetComponent<GestorDeJogo>().AtualizarRespawn(col.transform.position);
            
            if (this.textoNoCanvas != null)
            {
                this.textoNoCanvas.SetActive(true);
                this.CancelInvoke("EsconderTexto");
                this.Invoke("EsconderTexto", 2.0f);
            }
        }
    }

    void EsconderTexto()
    {
        if (this.textoNoCanvas != null)
        {
            this.textoNoCanvas.SetActive(false);
        }
    }
}
