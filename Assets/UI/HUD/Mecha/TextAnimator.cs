using UnityEngine;
using TMPro;
using System.Collections;

public class MegaAnimator : MonoBehaviour
{
    // Las 10 opciones de control (Puedes crear 10 más con esta base si lo deseas)
    public enum TextEffect
    {
        None, Glitch, Wave, Shake,
        Fade, Rain, Bounce, Rainbow,
        Rotate, Pulsate
    };

    public TextMeshProUGUI tmpComponent;

    // --- 1. CONTROLES COMPARTIDOS ---
    [Header("--- 1. Shared Controls ---")]
    public float animationSpeed = 0.02f; // Velocidad de refresco de la animación

    // --- 2. EFECTOS COMBINABLES (Activación/Desactivación) ---
    [Header("--- 2. Combined Effects (Choose up to 2) ---")]
    // Efectos de POSICIÓN
    public bool effect_Wave = false;
    public bool effect_Bounce = false;
    public bool effect_Shake = false;
    public bool effect_Rain = false;
    public bool effect_Typing = false;

    // Efectos de COLOR y ESCALA/ROTACIÓN
    public bool effect_Rainbow = false;
    public bool effect_Pulsate = false;
    public bool effect_Glitch = false;
    public bool effect_Rotation = false;
    public bool effect_Fade = false;


    // --- 3. CONFIGURACIÓN DE PARÁMETROS ---

    [Header("--- Glitch Settings ---")]
    [Range(0.01f, 0.5f)]
    public float glitchFrequency = 0.05f;
    public float maxDisplacement = 5.0f;
    public float maxAlpha = 0.5f;

    [Header("--- Fade Settings ---")]
    public float fadeTime = 1.0f;

    [Header("--- Wave Settings ---")]
    public float waveSpeed = 2.0f;
    public float waveAmplitude = 10.0f;

    [Header("--- Shake Settings ---")]
    public float shakeSpeed = 20f;
    public float shakeAmount = 2.0f;

    [Header("--- Rain Settings ---")]
    public float rainSpeed = 10f;
    public float rainAmplitude = 300f;

    [Header("--- Bounce Settings ---")]
    public float bounceSpeed = 10f;
    public float bounceHeight = 15f;

    [Header("--- Rainbow Settings ---")]
    public float colorSpeed = 1f;

    [Header("--- Rotate Settings ---")]
    public float rotationSpeed = 15f;

    [Header("--- Pulsate Settings ---")]
    public float pulsateSpeed = 4f;
    public float maxScale = 1.1f;

    // VARIABLES PRIVADAS DE DATOS ORIGINALES
    private Color32[] originalColors;
    private Vector3[] originalVertices;
    private float fadeTimer = 0f;
    private bool isFadingIn = true;
    private int maxVisibleCharacters = 0; // Para el efecto Typing

    void Start()
    {
        StoreOriginalMeshInfo();
        StartCoroutine(AnimateText());
    }

    void StoreOriginalMeshInfo()
    {
        tmpComponent.ForceMeshUpdate();

        int maxVertexCount = tmpComponent.textInfo.characterCount * 4;
        originalColors = new Color32[maxVertexCount];
        originalVertices = new Vector3[maxVertexCount];

        for (int i = 0; i < tmpComponent.textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = tmpComponent.textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int vertexIndex = charInfo.vertexIndex;
            int materialIndex = charInfo.materialReferenceIndex;

            Vector3[] vertices = tmpComponent.textInfo.meshInfo[materialIndex].vertices;
            Color32[] colors = tmpComponent.textInfo.meshInfo[materialIndex].colors32;

            for (int j = 0; j < 4; j++)
            {
                int currentVertex = vertexIndex + j;
                if (currentVertex < originalVertices.Length)
                {
                    originalColors[currentVertex] = colors[currentVertex];
                    originalVertices[currentVertex] = vertices[currentVertex];
                }
            }
        }
    }

    IEnumerator AnimateText()
    {
        while (true)
        {
            // Solo animamos si hay algún efecto activo, sino, simplemente refresca
            if (effect_Wave || effect_Bounce || effect_Shake || effect_Rain || effect_Typing ||
                effect_Rainbow || effect_Pulsate || effect_Glitch || effect_Rotation || effect_Fade)
            {
                tmpComponent.ForceMeshUpdate();

                // Lógica de Typing (Gestión global de visibilidad)
                if (effect_Typing)
                {
                    if (maxVisibleCharacters < tmpComponent.textInfo.characterCount)
                        maxVisibleCharacters++;
                    tmpComponent.maxVisibleCharacters = maxVisibleCharacters;
                }
                else
                {
                    tmpComponent.maxVisibleCharacters = 9999; // Muestra todo el texto si Typing está desactivado
                }

                // Lógica de Fade (Gestión global de transparencia)
                if (effect_Fade)
                {
                    if (isFadingIn) fadeTimer += animationSpeed;
                    else fadeTimer -= animationSpeed;
                    fadeTimer = Mathf.Clamp(fadeTimer, 0f, fadeTime);
                    if (fadeTimer >= fadeTime) isFadingIn = false;
                    if (fadeTimer <= 0f) isFadingIn = true;
                }

                for (int i = 0; i < tmpComponent.textInfo.characterCount; i++)
                {
                    TMP_CharacterInfo charInfo = tmpComponent.textInfo.characterInfo[i];
                    if (!charInfo.isVisible) continue;

                    int vertexIndex = charInfo.vertexIndex;
                    int materialIndex = charInfo.materialReferenceIndex;

                    Vector3[] vertices = tmpComponent.textInfo.meshInfo[materialIndex].vertices;
                    Color32[] colors = tmpComponent.textInfo.meshInfo[materialIndex].colors32;

                    Vector3 charCenter = (originalVertices[vertexIndex] + originalVertices[vertexIndex + 3]) / 2f;
                    Matrix4x4 matrix = Matrix4x4.identity;

                    // 1. RESTABLECER a la POSICIÓN y COLOR ORIGINALES antes de transformar
                    for (int j = 0; j < 4; j++)
                    {
                        vertices[vertexIndex + j] = originalVertices[vertexIndex + j];
                        colors[vertexIndex + j] = originalColors[vertexIndex + j];
                    }

                    // --- 2. APLICAR TRANSFORMACIONES COMBINADAS (Posición) ---

                    Vector3 positionOffset = Vector3.zero;

                    if (effect_Wave)
                        positionOffset.y += Mathf.Sin(Time.time * waveSpeed + (i * 0.5f)) * waveAmplitude;

                    if (effect_Bounce)
                        positionOffset.y += Mathf.Abs(Mathf.Sin(Time.time * bounceSpeed + (i * 0.5f))) * bounceHeight;

                    if (effect_Shake)
                    {
                        // CORRECCIÓN CS0104: Usar UnityEngine.Random
                        float shakeX = (Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) * 2 - 1) * shakeAmount;
                        float shakeY = (Mathf.PerlinNoise(0f, Time.time * shakeSpeed) * 2 - 1) * shakeAmount;
                        positionOffset += new Vector3(shakeX, shakeY, 0);
                    }

                    if (effect_Rain)
                        positionOffset.y += Mathf.Cos(Time.time * rainSpeed - (i * 0.5f)) * rainAmplitude;

                    // Aplica el Offset de Posición
                    for (int j = 0; j < 4; j++)
                        vertices[vertexIndex + j] += positionOffset;


                    // --- 3. APLICAR TRANSFORMACIONES COMBINADAS (Matriz - Rotación/Escala) ---

                    if (effect_Rotation)
                    {
                        float angle = Mathf.Sin(Time.time * rotationSpeed + (i * 0.2f)) * 30f;
                        matrix = Matrix4x4.TRS(charCenter, Quaternion.Euler(0, 0, angle), Vector3.one);
                    }
                    else if (effect_Pulsate)
                    {
                        float scale = Mathf.Lerp(1f, maxScale, Mathf.Abs(Mathf.Sin(Time.time * pulsateSpeed)));
                        matrix = Matrix4x4.TRS(charCenter, Quaternion.identity, Vector3.one * scale);
                    }

                    // Aplica la Matriz (Rotación/Escala)
                    if (effect_Rotation || effect_Pulsate)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            // Multiplica el vértice (que ya tiene el offset de posición) por la matriz
                            vertices[vertexIndex + j] = matrix.MultiplyPoint3x4(vertices[vertexIndex + j]);
                        }
                    }


                    // --- 4. APLICAR TRANSFORMACIONES COMBINADAS (Color/Alpha) ---

                    // A) Color (Arcoíris)
                    if (effect_Rainbow)
                    {
                        Color rainbowColor = Color.HSVToRGB((Time.time * colorSpeed + (i * 0.1f)) % 1f, 1f, 1f);
                        for (int j = 0; j < 4; j++)
                            colors[vertexIndex + j] = rainbowColor;
                    }

                    // B) Glitch (Modifica Alpha y Posición X de forma destructiva)
                    if (effect_Glitch)
                    {
                        // CORRECCIÓN CS0104: Usar UnityEngine.Random
                        if (UnityEngine.Random.value < glitchFrequency)
                        {
                            float glitchX = UnityEngine.Random.Range(-maxDisplacement, maxDisplacement);
                            byte alpha = (byte)UnityEngine.Random.Range((int)(255 * (1.0f - maxAlpha)), 255);

                            for (int j = 0; j < 4; j++)
                            {
                                vertices[vertexIndex + j].x += glitchX; // Añadir desplazamiento extra al Glitch
                                colors[vertexIndex + j].a = alpha; // Aplicar alpha del Glitch
                            }
                        }
                    }

                    // C) Fade (Modifica Alpha Global - siempre va al final para sobrescribir)
                    if (effect_Fade)
                    {
                        byte alphaValue = (byte)((fadeTimer / fadeTime) * 255);
                        for (int j = 0; j < 4; j++)
                            colors[vertexIndex + j].a = alphaValue;
                    }
                }

                tmpComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
            }
            else // Si no hay efectos activos, asegúrate de que el texto esté estático
            {
                // Si el efecto cambia a 'None', se refresca el texto a su estado estático
                tmpComponent.ForceMeshUpdate();
                tmpComponent.maxVisibleCharacters = 9999;
            }

            yield return new WaitForSeconds(animationSpeed);
        }
    }
}