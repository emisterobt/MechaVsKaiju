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
        SceneManager.LoadScene("Creditos");
    }

    public void Configuracion()
    {
        SceneManager.LoadScene("Configuracion");
    }

    public void Salir()
    {
        Application.Quit();
    }

    public void ComoJugar()
    {
        SceneManager.LoadScene("ComoJugar");
    }

    public void IrAMenu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}
