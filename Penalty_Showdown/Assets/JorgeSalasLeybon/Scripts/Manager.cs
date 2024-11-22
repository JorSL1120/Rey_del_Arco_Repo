using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public Button[] BotonSeccion; //Lista para asignar botones
    public GameObject[] panels; // Array para los paneles
    private bool panelsActive = true;

    public Button[] buttons;
    private bool[] isMouseOverButton;
    private float[] hoverTime;
    private float hoverThreshold = 2.0f;
    private bool[] excludeButton;


    private int playerSeleccion;
    private int maquinaSeleccion;
    public int marcadorPlayer;
    public int marcadorMaquina;

    public float Dificultad;

    private int contadorPenales;

    public GameObject PrimerPenal;
    public GameObject SegundoPenal;
    public GameObject TercerPenal;
    public GameObject CuartoPenal;
    public GameObject QuintoPenal;

    public GameObject TextoInicial1;
    public GameObject TextoInicial2;
    public GameObject TextoInicial3;
    public GameObject FondoNegro;
    public GameObject PanelEvitarTirar;

    public GameObject ParadaText;
    public GameObject GolText;

    public GameObject Balon1;
    public GameObject Balon2;
    public GameObject Balon3;
    public GameObject Balon4;
    public GameObject Balon5;
    public GameObject Balon6;

    public GameObject Guantes1;
    public GameObject Guantes2;
    public GameObject Guantes3;
    public GameObject Guantes4;
    public GameObject Guantes5;
    public GameObject Guantes6;

    public GameObject GuantesMov;

    public TextMeshProUGUI MarcadorPortero;
    public TextMeshProUGUI MarcadorDelantero;
    public TextMeshProUGUI TextoContPenales;
    public int ActMarcadorPortero;
    public int ActMarcadorMaquina;
    public int ActContPenales;

    public AudioSource Abucheos;
    public AudioSource Publico;
    public AudioSource Festejo;

    void Start()
    {
        Abucheos.Stop();
        Publico.Play();
        Festejo.Stop();

        TextoInicial1.SetActive(true);
        FondoNegro.SetActive(true);
        TextoInicial2.SetActive(false);
        TextoInicial3.SetActive(false);
        PanelEvitarTirar.SetActive(false);

        StartCoroutine(TextInicial());

        ParadaText.SetActive(false);
        GolText.SetActive(false);

        Balon1.SetActive(false);
        Balon2.SetActive(false);
        Balon3.SetActive(false);
        Balon4.SetActive(false);
        Balon5.SetActive(false);
        Balon6.SetActive(false);

        Guantes1.SetActive(false);
        Guantes2.SetActive(false);
        Guantes3.SetActive(false);
        Guantes4.SetActive(false);
        Guantes5.SetActive(false);
        Guantes6.SetActive(false);

        PrimerPenal.SetActive(false);
        SegundoPenal.SetActive(false);
        TercerPenal.SetActive(false);
        CuartoPenal.SetActive(false);
        QuintoPenal.SetActive(false);

        ActMarcadorPortero = 0;
        ActMarcadorMaquina = 0;
        ActContPenales = 0;

        isMouseOverButton = new bool[buttons.Length];
        hoverTime = new float[buttons.Length];
        excludeButton = new bool[buttons.Length];

        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].gameObject.AddComponent<EventTrigger>();
            EventTrigger trigger = buttons[i].GetComponent<EventTrigger>();

            EventTrigger.Entry entryEnter = new EventTrigger.Entry();
            entryEnter.eventID = EventTriggerType.PointerEnter;
            entryEnter.callback.AddListener((eventData) => { StartHover(index); });
            trigger.triggers.Add(entryEnter);

            EventTrigger.Entry entryExit = new EventTrigger.Entry();
            entryExit.eventID = EventTriggerType.PointerExit;
            entryExit.callback.AddListener((eventData) => { StopHover(index); });
            trigger.triggers.Add(entryExit);
        }

        StartCoroutine(UpdateRandomSelection());


        //Se usa bucle "foreach" que recorre cada uno de los elementos que hay en una lista o arreglo
        foreach (Button botonSeleccion in BotonSeccion)
        {
            botonSeleccion.onClick.AddListener(() => BotonSeleccionado(botonSeleccion));//Se le asigna un valor a la variable "botonSeleccion" y se le manda como argumento a la funcion "BotonSeleccionado"
        }
    }

    void Update()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (isMouseOverButton[i])
            {
                hoverTime[i] += Time.deltaTime;
                if (hoverTime[i] >= hoverThreshold)
                {
                    excludeButton[i] = true;
                }
            }
            else
            {
                hoverTime[i] = 0;
            }
        }
    }

    void StartHover(int index)
    {
        isMouseOverButton[index] = true;
    }

    void StopHover(int index)
    {
        isMouseOverButton[index] = false;
        hoverTime[index] = 0;
        excludeButton[index] = false;
    }

    IEnumerator UpdateRandomSelection() //Cambia el numero Random
    {
        while (true)
        {
            if (panelsActive)
            {
                do
                {
                    maquinaSeleccion = Random.Range(0, 6);
                } while (excludeButton[maquinaSeleccion]);

                UpdatePanels();
            }
            yield return new WaitForSeconds(Dificultad);
        }
    }

    void UpdatePanels()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == maquinaSeleccion); //Actualiza el numero de panel que se debe de activar
        }
    }

    public void BotonSeleccionado(Button button)
    {
        playerSeleccion = System.Array.IndexOf(BotonSeccion, button);

        if (playerSeleccion == maquinaSeleccion)
        {
            StartCoroutine(AppearAndDisappearTextoParada());
            marcadorPlayer++;
            ActMarcadorPortero = marcadorPlayer;
            ActMarcadorMaquina = marcadorMaquina;
            ActualizarMarcadorDelantero();
            ActualizarMarcadorPortero();
            StartCoroutine(PlayStopFestejos());
        }
        else
        {
            StartCoroutine(AppearAndDisappearTextoGol());
            marcadorMaquina++;
            ActMarcadorPortero = marcadorPlayer;
            ActMarcadorMaquina = marcadorMaquina;
            ActualizarMarcadorPortero();
            ActualizarMarcadorDelantero();
            StartCoroutine(PlayStopAbucheos());
        }

        contadorPenales++;
        if (contadorPenales >= 5)
        {
            FinJuego();
        }

        ActualizarNumeroPenales();
        StartCoroutine(DeactivatePanelsTemporarily());

        switch (maquinaSeleccion)
        {
            case 0: StartCoroutine(AppearAndDisappearBalon1()); break;
            case 1: StartCoroutine(AppearAndDisappearBalon2()); break;
            case 2: StartCoroutine(AppearAndDisappearBalon3()); break;
            case 3: StartCoroutine(AppearAndDisappearBalon4()); break;
            case 4: StartCoroutine(AppearAndDisappearBalon5()); break;
            case 5: StartCoroutine(AppearAndDisappearBalon6()); break;
            default: Debug.Log("Ninguno de los numeros sale"); break;
        }

        switch (playerSeleccion)
        {
            case 0: StartCoroutine(AppearAndDisappearGuantes1()); break;
            case 1: StartCoroutine(AppearAndDisappearGuantes2()); break;
            case 2: StartCoroutine(AppearAndDisappearGuantes3()); break;
            case 3: StartCoroutine(AppearAndDisappearGuantes4()); break;
            case 4: StartCoroutine(AppearAndDisappearGuantes5()); break;
            case 5: StartCoroutine(AppearAndDisappearGuantes6()); break;
            default: Debug.Log("Ninguno de los numeros sale"); break;
        }
    }

    IEnumerator DeactivatePanelsTemporarily()
    {
        panelsActive = false;
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
        yield return new WaitForSeconds(3f);
        panelsActive = true;
    }

    void ActualizarMarcadorPortero()
    {
        if (MarcadorPortero != null)
        {
            MarcadorPortero.text = ActMarcadorPortero.ToString(); //Convierte los numeros en texto para que se puedan actualizar en un TextMeshPro
        }
        else
        {
            Debug.LogWarning("No se asignó el componente TextMeshPro en el Inspector.");
        }
    }

    void ActualizarMarcadorDelantero()
    {
        if (MarcadorDelantero != null)
        {
            MarcadorDelantero.text = ActMarcadorMaquina.ToString(); //Convierte los numeros en texto para que se puedan actualizar en un TextMeshPro
        }
        else
        {
            Debug.LogWarning("No se asignó el componente TextMeshPro en el Inspector.");
        }
    }

    void ActualizarNumeroPenales()
    {
        ActContPenales = ActMarcadorMaquina + ActMarcadorPortero;

        if (ActContPenales == 1)
        {
            PrimerPenal.SetActive(true);
        }
        else if (ActContPenales == 2)
        {
            SegundoPenal.SetActive(true);
        }
        else if (ActContPenales == 3)
        {
            TercerPenal.SetActive(true);
        }
        else if (ActContPenales == 4)
        {
            CuartoPenal.SetActive(true);
        }
        else if (ActContPenales == 5)
        {
            QuintoPenal.SetActive(true);
        }
    }

    void FinJuego()
    {
        if (marcadorPlayer > marcadorMaquina)
        {
            StartCoroutine(CambiarAGanaste());
        }
        else
        {
            StartCoroutine(CambiarAPerdiste());
        }
    }


    IEnumerator TextInicial()
    {
        GuantesMov.SetActive(false);

        yield return new WaitForSeconds(5f);

        TextoInicial1.SetActive(false);
        TextoInicial2.SetActive(true);

        yield return new WaitForSeconds(5f);

        TextoInicial2.SetActive(false);
        TextoInicial3.SetActive(true);

        yield return new WaitForSeconds(5f);

        TextoInicial3.SetActive(false);
        FondoNegro.SetActive(false);
        GuantesMov.SetActive(true);
    }

    IEnumerator CambiarAGanaste()
    {
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("Ganaste");
    }

    IEnumerator CambiarAPerdiste()
    {
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("Perdiste");
    }


    IEnumerator AppearAndDisappearTextoParada()
    {
        ParadaText.SetActive(true);

        yield return new WaitForSeconds(3f);

        ParadaText.SetActive(false);
    }

    IEnumerator AppearAndDisappearTextoGol()
    {
        GolText.SetActive(true);

        yield return new WaitForSeconds(3f);

        GolText.SetActive(false);
    }

    IEnumerator AppearAndDisappearBalon1()
    {
        Balon1.SetActive(true);

        yield return new WaitForSeconds(3f);

        Balon1.SetActive(false);
    }

    IEnumerator AppearAndDisappearBalon2()
    {
        Balon2.SetActive(true);

        yield return new WaitForSeconds(3f);

        Balon2.SetActive(false);
    }

    IEnumerator AppearAndDisappearBalon3()
    {
        Balon3.SetActive(true);

        yield return new WaitForSeconds(3f);

        Balon3.SetActive(false);
    }

    IEnumerator AppearAndDisappearBalon4()
    {
        Balon4.SetActive(true);

        yield return new WaitForSeconds(3f);

        Balon4.SetActive(false);
    }

    IEnumerator AppearAndDisappearBalon5()
    {
        Balon5.SetActive(true);

        yield return new WaitForSeconds(3f);

        Balon5.SetActive(false);
    }

    IEnumerator AppearAndDisappearBalon6()
    {
        Balon6.SetActive(true);

        yield return new WaitForSeconds(3f);

        Balon6.SetActive(false);
    }

    IEnumerator AppearAndDisappearGuantes1()
    {
        Guantes1.SetActive(true);
        GuantesMov.SetActive(false);
        PanelEvitarTirar.SetActive(true);

        yield return new WaitForSeconds(3f);

        Guantes1.SetActive(false);
        GuantesMov.SetActive(true);
        PanelEvitarTirar.SetActive(false);
    }

    IEnumerator AppearAndDisappearGuantes2()
    {
        Guantes2.SetActive(true);
        GuantesMov.SetActive(false);
        PanelEvitarTirar.SetActive(true);

        yield return new WaitForSeconds(3f);

        Guantes2.SetActive(false);
        GuantesMov.SetActive(true);
        PanelEvitarTirar.SetActive(false);
    }

    IEnumerator AppearAndDisappearGuantes3()
    {
        Guantes3.SetActive(true);
        GuantesMov.SetActive(false);
        PanelEvitarTirar.SetActive(true);

        yield return new WaitForSeconds(3f);

        Guantes3.SetActive(false);
        GuantesMov.SetActive(true);
        PanelEvitarTirar.SetActive(false);
    }

    IEnumerator AppearAndDisappearGuantes4()
    {
        Guantes4.SetActive(true);
        GuantesMov.SetActive(false);
        PanelEvitarTirar.SetActive(true);

        yield return new WaitForSeconds(3f);

        Guantes4.SetActive(false);
        GuantesMov.SetActive(true);
        PanelEvitarTirar.SetActive(false);
    }

    IEnumerator AppearAndDisappearGuantes5()
    {
        Guantes5.SetActive(true);
        GuantesMov.SetActive(false);
        PanelEvitarTirar.SetActive(true);

        yield return new WaitForSeconds(3f);

        Guantes5.SetActive(false);
        GuantesMov.SetActive(true);
        PanelEvitarTirar.SetActive(false);
    }

    IEnumerator AppearAndDisappearGuantes6()
    {
        Guantes6.SetActive(true);
        GuantesMov.SetActive(false);
        PanelEvitarTirar.SetActive(true);

        yield return new WaitForSeconds(3f);

        Guantes6.SetActive(false);
        GuantesMov.SetActive(true);
        PanelEvitarTirar.SetActive(false);
    }

    IEnumerator PlayStopAbucheos()
    {
        Abucheos.Play();
        Publico.Stop();

        yield return new WaitForSeconds(3f);

        Abucheos.Stop();
        Publico.Play();
    }

    IEnumerator PlayStopFestejos()
    {
        Festejo.Play();
        Publico.Stop();

        yield return new WaitForSeconds(3f);

        Festejo.Stop();
        Publico.Play();
    }
}
