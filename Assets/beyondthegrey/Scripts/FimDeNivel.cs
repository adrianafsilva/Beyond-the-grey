using UnityEngine;

public class FimDeNivel : MonoBehaviour
{
    private bool jaAtivou = false;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && this.jaAtivou == false)
        {
            this.jaAtivou = true;

            GestorDeJogo gestorJogo = GameObject.FindFirstObjectByType<GestorDeJogo>();
            if (gestorJogo != null)
            {
                if (gestorJogo.displayCoracoes != null) gestorJogo.displayCoracoes.gameObject.SetActive(false);
                if (gestorJogo.textoMoedas != null) gestorJogo.textoMoedas.gameObject.SetActive(false);
                if (gestorJogo.iconeMoedas != null) gestorJogo.iconeMoedas.gameObject.SetActive(false);
                
                if (gestorJogo.musicaDoNivel != null)
                {
                    gestorJogo.musicaDoNivel.Stop();
                }
            }

            GameObject gestor = GameObject.Find("GestorCutscene");
            if (gestor != null)
            {
                gestor.GetComponent<Cutscenes>().AtivarCutsceneComFade(col.gameObject);
            }
        }
    }
}
