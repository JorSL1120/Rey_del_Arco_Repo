using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectorNivel : MonoBehaviour
{
    public void CambiarNivelFacil()
    {
        SceneManager.LoadScene("NivelFacil");
    }

    public void CambiarNivelIntermedio()
    {
        SceneManager.LoadScene("NivelIntermedio");
    }

    public void CambiarNivelDificil()
    {
        SceneManager.LoadScene("NivelDificil");
    }
}
