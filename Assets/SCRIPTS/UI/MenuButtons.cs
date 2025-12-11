using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public GameObject configMenu;


    public void Jugar()
    {
        SceneManager.LoadScene("EscenaIntroduccion");
        Time.timeScale = 1.0f;
        GameManager.Instance.isInPause = false;
        GameObject menuMusic = AudioManager.Instance.transform.GetChild(0).gameObject;
        menuMusic.SetActive(false);
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
        GameObject menuMusic = AudioManager.Instance.transform.GetChild(0).gameObject;
        GameObject gameMusic = AudioManager.Instance.transform.GetChild(1).gameObject;
        menuMusic.SetActive(true);
        gameMusic.SetActive(false);
    }

    public void PlayCursorSound()
    {
        AudioManager.Instance.Play("Cursor");
    }
    public void CloseConfg()
    {
        configMenu.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Juego");
        Time.timeScale = 1.0f;
        GameManager.Instance.isInPause = false;
        GameObject menuMusic = AudioManager.Instance.transform.GetChild(0).gameObject;
        menuMusic.SetActive(false);
    }
}
