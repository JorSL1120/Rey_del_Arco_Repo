using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonIniciarJuego : MonoBehaviour
{
    public GameObject PanelSelectorNivel;

    private void Start()
    {
        PanelSelectorNivel.SetActive(false);
    }

    public void ActivarPanel()
    {
        PanelSelectorNivel.SetActive(true);
    }
}
