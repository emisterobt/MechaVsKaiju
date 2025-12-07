using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool canFinish;// Temporal para probar
    public bool isInPause;

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
            AudioManager.Instance.Stop("MechaWalk");
            AudioManager.Instance.Stop("KidsScream");
            AudioManager.Instance.Stop("CarAlarm");
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
            AudioManager.Instance.Stop("MechaWalk");
            AudioManager.Instance.Stop("KidsScream");
            AudioManager.Instance.Stop("CarAlarm");
            SceneManager.LoadScene("EscenaVictoria");
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public IEnumerator NuclearLoss()
    {
        if (canFinish == false)
        {
            yield return null;
        }
        else
        {
            AudioManager.Instance.Stop("MechaWalk");
            AudioManager.Instance.Stop("KidsScream");
            AudioManager.Instance.Stop("CarAlarm");
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("NuclearLOSE");
            Cursor.lockState = CursorLockMode.None;
        }
    }

}
