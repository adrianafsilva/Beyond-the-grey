using UnityEngine;
using Unity.Cinemachine;

public class AtivadorBoss : MonoBehaviour
{
    public GameObject boss;
    public GameObject[] barreiras;
    
    public CinemachineCamera vcam; 
    public Transform centroDaArena; 

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Jogador")
        {
            this.boss.SetActive(true);

            // ATIVAR BARREIRAS (Recorri ao Google Gemini)
            for (int i = 0; i < this.barreiras.Length; i++)
            {
                this.barreiras[i].SetActive(true);
            }

            this.vcam.Follow = this.centroDaArena;
            Destroy(this.gameObject);
        }
    }
}
