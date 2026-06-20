using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogo : MonoBehaviour
{
    public TextMeshProUGUI texto;
    public string[] lines;
    public float velocidadedotexto;

    private int index;
    
    void Start()
    {
        texto.text = string.Empty;
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (texto.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                texto.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            texto.text += c;
            yield return new WaitForSeconds(velocidadedotexto);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            texto.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}