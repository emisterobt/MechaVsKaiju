using System.Collections;
using UnityEngine;

public class SmoothBlink : MonoBehaviour
{
    private MeshRenderer lidRenderer;
    private const int FRAME_COUNT = 4; // Índices: 0, 1, 2, 3

    // --- VARIABLES PÚBLICAS CONTROLABLES EN UNITY ---
    [Range(1f, 10f)]
    public float minWaitTime = 5.0f;
    [Range(2f, 15f)]
    public float maxWaitTime = 10.0f;

    // La velocidad a la que pasa de un frame al siguiente (para la fluidez del movimiento)
    [Range(0.01f, 0.2f)]
    public float frameSpeed = 0.04f;

    // --------------------------------------------------

    void Awake()
    {
        lidRenderer = GetComponent<MeshRenderer>();
        // Aseguramos que el ojo inicie en el estado abierto (Frame 3)
        SetNewFrame(FRAME_COUNT - 1);
    }

    void Start()
    {
        StartCoroutine(BlinkRoutine());
    }

    IEnumerator BlinkRoutine()
    {
        // El bucle principal asegura que el ojo se mantenga ABIERTO la mayor parte del tiempo.
        while (gameObject.activeInHierarchy)
        {
            // 1. ESPERAR (Tiempo aleatorio entre parpadeos, el ojo está en Frame 3/Abierto)
            float randomWait = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(randomWait);

            // 2. CERRAR EL OJO: Va del índice 3 (Abierto) al 0 (Cerrado)
            // Recorre hacia atrás desde el penúltimo índice (3) hasta 0.
            for (int i = FRAME_COUNT - 1; i >= 0; i--)
            {
                SetNewFrame(i);
                yield return new WaitForSeconds(frameSpeed);
            }

            // 3. ESPERAR CERRADO (Mantiene el Frame 0 por un instante)
            yield return new WaitForSeconds(frameSpeed * 3f); // Pequeña pausa en el punto más cerrado

            // 4. ABRIR EL OJO: Vuelve del índice 1 al 3 (Abierto)
            // Recorre hacia adelante desde el índice 1 hasta el último índice (3).
            for (int i = 1; i < FRAME_COUNT; i++)
            {
                SetNewFrame(i);
                yield return new WaitForSeconds(frameSpeed);
            }

            // Al salir del bucle, el ojo está de vuelta en el Frame 3 (Abierto), 
            // y el ciclo comienza de nuevo con la larga espera.
        }
    }

    void SetNewFrame(int frameIndex)
    {
        // Calcula el offset y lo aplica al material
        float xOffset = frameIndex * (1f / FRAME_COUNT);
        Vector2 offset = new Vector2(xOffset, 0f);
        lidRenderer.material.SetTextureOffset("_MainTex", offset);
    }
}