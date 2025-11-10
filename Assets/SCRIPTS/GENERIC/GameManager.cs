using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool canFinish;// Temporal para probar

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    public IEnumerator GameOver()
    {
        if (canFinish == false)
        {
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("EscenaDerrota");
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public IEnumerator Victory()
    {
        if (canFinish == false)
        {
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("EscenaVictoria");
            Cursor.lockState = CursorLockMode.None;
        }
    }

}
