using System.Collections;
using UnityEngine;

// NOTA: Este script no necesita la librería TMPro, solo se enfoca en la entrada (Input).

public class DialogueStarter : MonoBehaviour
{
    [Header("Referencias del Sistema")]
    // Referencia al script que controla toda la caja de diálogo
    public DialogueManager dialogueManager;

    [Header("Datos de la Conversación")]
    // Los datos que serán pasados al DialogueManager
    // ESTOS DEBEN SER LOS MISMOS DATOS QUE DEFINISTE EN EL DialogueManager
    public DialogueLine[] conversationData;
    public DialogueLine[] laserCargado;

    [Header("Configuración de Entrada")]
    public KeyCode continueKey = KeyCode.Alpha1; // Tecla para iniciar/continuar (Tecla '1')

    // Variable de estado para controlar si ya se inició la conversación
    private bool dialogueHasStarted = false;


    private void Start()
    {
        if (!dialogueHasStarted)
        {
            StartCoroutine(DialogoInicio());
        }
    }

    private void Update()
    {
        //if (Input.GetKeyDown(continueKey))
        //{
        //    if (!dialogueHasStarted)
        //    {
        //        StartCoroutine(DialogoLaserCargado());
        //    }
        //}
    }
    public void LaserCargado()
    {
        StartCoroutine(DialogoLaserCargado());
    }

    // Opcional: Función para resetear el estado y poder iniciar el diálogo de nuevo

    public IEnumerator DialogoInicio()
    {
        if (!dialogueHasStarted)
        {
            // Primera pulsación: Inicia el diálogo (llama a Abrir)
            if (dialogueManager != null && conversationData != null && conversationData.Length > 0)
            {
                dialogueManager.StartDialogue(conversationData);
                dialogueHasStarted = true;
            }
            else
            {
                Debug.LogError("ERROR: DialogueStarter no tiene referencias al DialogueManager o no hay datos de conversación.");
            }
        }
        while (dialogueManager.isTyping)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2.5f);

        while (dialogueManager.currentLineIndex < dialogueManager._conversationData.Length - 1)
        {

            dialogueManager.currentLineIndex++;
            dialogueManager.ProcessNextLine();
            while (dialogueManager.isTyping)
            {
                yield return null;
            }
            yield return new WaitForSeconds(1.0f);
        }

        dialogueManager.EndDialogue();
        dialogueHasStarted = false;
        dialogueManager.currentLineIndex = 0;

    }

    public IEnumerator DialogoLaserCargado()
    {
        if (!dialogueHasStarted)
        {
            // Primera pulsación: Inicia el diálogo (llama a Abrir)
            if (dialogueManager != null && laserCargado != null && laserCargado.Length > 0)
            {
                dialogueManager.StartDialogue(laserCargado);
                dialogueHasStarted = true;
                
            }
            else
            {
                Debug.LogError("ERROR: DialogueStarter no tiene referencias al DialogueManager o no hay datos de conversación.");
            }
        }
        while (dialogueManager.isTyping)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2.0f);

        while (dialogueManager.currentLineIndex < dialogueManager._conversationData.Length - 1)
        {

            dialogueManager.currentLineIndex++;
            dialogueManager.ProcessNextLine();
            while (dialogueManager.isTyping)
            {
                yield return null;
            }
            yield return new WaitForSeconds(1.0f);
        }

        dialogueManager.EndDialogue();
        dialogueHasStarted = false;
        dialogueManager.currentLineIndex = 0;


    }


    public void ResetStarter()
    {
        dialogueHasStarted = false;
    }
}