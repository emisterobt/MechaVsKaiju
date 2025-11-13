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

    [Header("Configuración de Entrada")]
    public KeyCode continueKey = KeyCode.Alpha1; // Tecla para iniciar/continuar (Tecla '1')

    // Variable de estado para controlar si ya se inició la conversación
    private bool dialogueHasStarted = false;

    void Update()
    {
        // 1. Detectar si se presiona la tecla configurada (por defecto, la tecla '1')
        if (Input.GetKeyDown(continueKey))
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
            else
            {
                // Pulsaciones siguientes: Avanza la línea (hace el fade y el texto)
                if (dialogueManager != null)
                {
                    dialogueManager.OnContinueClicked();
                }
            }
        }
    }

    // Opcional: Función para resetear el estado y poder iniciar el diálogo de nuevo
    public void ResetStarter()
    {
        dialogueHasStarted = false;
    }
}