using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EscenaIntroduccion : MonoBehaviour
{
    [SerializeField] private VideoPlayer player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isPaused || Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Juego");
            GameObject gameMusic = AudioManager.Instance.transform.GetChild(1).gameObject;
            gameMusic.SetActive(true);
        }
    }
}
