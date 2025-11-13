using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Estructura de datos para definir cada línea de diálogo
[System.Serializable]
public class DialogueLine
{
    public string characterName;
    public Sprite portrait;
    public string text;
    public bool isGirlA;
}

public class DialogueManager : MonoBehaviour
{
    [Header("Referencias de UI y Animator")]
    public Animator dialogueBoxAnimator;
    public TMPro.TextMeshProUGUI nameText;
    public TMPro.TextMeshProUGUI dialogueText;
    public GameObject nextIndicator;

    [Header("Retratos y Grupos (Para el Fade)")]
    public CanvasGroup girlACanvasGroup;
    public CanvasGroup girlBCanvasGroup;
    public Image girlAImage;
    public Image girlBImage;

    [Header("Configuración de Animación")]
    public float typingSpeed = 0.05f;
    public float fadeDuration = 0.3f;

    // --- VARIABLES INTERNAS (NO APARECEN EN EL INSPECTOR) ---
    private DialogueLine[] _conversationData; // Almacena la conversación activa
    private int currentLineIndex = 0;
    private bool isTyping = false;
    private bool dialogueIsActive = false;
    private CanvasGroup activeCharacterGroup;

    // =========================================================
    // MÉTODOS PÚBLICOS DE CONTROL
    // =========================================================

    public void StartDialogue(DialogueLine[] dialogueData)
    {
        if (dialogueIsActive) return;

        // El Manager almacena la conversación que le fue pasada
        _conversationData = dialogueData;
        currentLineIndex = 0;
        dialogueIsActive = true;
        nextIndicator.SetActive(false);

        dialogueBoxAnimator.SetTrigger("Abrir");

        Invoke("ProcessNextLine", 0.5f);
    }

    public void OnContinueClicked()
    {
        if (!dialogueIsActive) return;

        if (isTyping)
        {
            // Salta el efecto de escritura
            StopAllCoroutines();
            dialogueText.text = _conversationData[currentLineIndex].text;
            isTyping = false;
            nextIndicator.SetActive(true);
        }
        else
        {
            // Avanza a la siguiente línea o termina
            currentLineIndex++;
            if (currentLineIndex < _conversationData.Length) // Usa la data interna
            {
                ProcessNextLine();
            }
            else
            {
                EndDialogue();
            }
        }
    }

    private void EndDialogue()
    {
        dialogueBoxAnimator.SetTrigger("Cerrar");
        if (activeCharacterGroup != null) StartCoroutine(FadeCanvasGroup(activeCharacterGroup, 0f));
        dialogueIsActive = false;
    }

    // =========================================================
    // LÓGICA INTERNA DE FLUJO
    // =========================================================

    private void ProcessNextLine()
    {
        nextIndicator.SetActive(false);

        DialogueLine line = _conversationData[currentLineIndex]; // Usa la data interna

        UpdatePortraitAndFade(line.isGirlA, line.portrait);
        nameText.text = line.characterName;
        StartCoroutine(TypeSentence(line.text));
    }

    private void UpdatePortraitAndFade(bool isSpeakingGirlA, Sprite newPortrait)
    {
        CanvasGroup characterToFadeIn = isSpeakingGirlA ? girlACanvasGroup : girlBCanvasGroup;
        CanvasGroup characterToFadeOut = isSpeakingGirlA ? girlBCanvasGroup : girlACanvasGroup;
        Image imageToUpdate = isSpeakingGirlA ? girlAImage : girlBImage;

        if (activeCharacterGroup != characterToFadeIn)
        {
            if (activeCharacterGroup != null) StartCoroutine(FadeCanvasGroup(characterToFadeOut, 0f));
            StartCoroutine(FadeCanvasGroup(characterToFadeIn, 1f));
        }

        imageToUpdate.sprite = newPortrait;
        activeCharacterGroup = characterToFadeIn;
    }

    // =========================================================
    // COROUTINES (Animaciones por Código)
    // =========================================================

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
        nextIndicator.SetActive(true);
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float targetAlpha)
    {
        if (canvasGroup == null) yield break;
        if (targetAlpha > 0) canvasGroup.gameObject.SetActive(true);

        float startAlpha = canvasGroup.alpha;
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
        if (targetAlpha == 0) canvasGroup.gameObject.SetActive(false);
    }
}