using UnityEngine;
using UnityEngine.SceneManagement;

public class MudarCena : MonoBehaviour
{

    public void MudarCenaParaCutscene0(string cutscene0)
    {
        SceneManager.LoadScene("cutscene0");
    }

    public void MudarCenaParaLevel1(string level1)
    {
        SceneManager.LoadScene("level1");
    }

    public void MudarCenaParaHowToPlay(string howtoplay)
    {
        SceneManager.LoadScene("howtoplay");
    }

    public void MudarCenaParaMainMenu(string mainmenu)
    {
        SceneManager.LoadScene("mainmenu");
    }

    
}
