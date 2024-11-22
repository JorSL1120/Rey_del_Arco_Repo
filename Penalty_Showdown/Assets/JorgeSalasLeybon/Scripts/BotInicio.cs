using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BotInicio : MonoBehaviour
{
    public void CambiarInicio()
    {
        SceneManager.LoadScene("Inicio");
    }
}
