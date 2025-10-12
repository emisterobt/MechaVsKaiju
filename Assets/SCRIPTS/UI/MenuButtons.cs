using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("Juego");
    }

    public void Creditos()
    {
        Debug.Log("Abriendo Creditos");
    }

    public void Configuracion()
    {
        Debug.Log("Abriendo Configuracion");
    }

    public void Salir()
    {
        Application.Quit();
    }
}
