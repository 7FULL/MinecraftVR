using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "noiseSettings", menuName = "Data/NoiseSettings")]
public class NoiseSettings : ScriptableObject
{
    [Header("Que tan cerca queremos ver el perlin")]
    public float noiseZoom;
    [Header("Cantidad de capas a juntar para generar el noise (cuanto menos mas plano 'Menos detalle')")]
    public int octaves;
    [Header("Donde empezamos a utilizar el perlin (si empezamos en el 0,0 se repetira en las 2 direcciones)")]
    public Vector2Int offest;
    [Header("Concepto como la semilla del mundo")]
    public Vector2Int worldOffset;
    [Header("Irregularidad/Suavidad del terreno")]
    public float persistance;
    [Space]
    [Header("Estos valores no pertenecen a la generacion del noise si no a como se aplica luego")]
    [Space]
    [Header("Como de distribuidos son los cambio entre valles y colinas y que tan exagerados son las conexiones entre ellos")]
    public float redistributionModifier;
    public float exponent;
}
