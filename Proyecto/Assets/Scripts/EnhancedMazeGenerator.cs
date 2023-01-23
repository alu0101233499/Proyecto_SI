using UnityEngine;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// 	Clase para generar laberintos de forma procedural con las
///     características:
///     - Entrada y salida garantizadas con una conexión segura entre ellas
///     en un 85% aprox de seeguridad.
///     - Generación mediante el uso de heurísticas y procedimientos estocásticos.
/// </summary>
public class EnhancedMazeGenerator : MonoBehaviour
{
    /// <summary>
    /// 	Altura del laberinto.
    /// </summary>
    public int height;

    /// <summary>
    /// 	Ancho del laberinto.
    /// </summary>
    public int width;

    /// <summary>
    /// 	Muros del laberinto.
    /// </summary>
    public GameObject wall;

    /// <summary>
    /// 	Variable para definir la probabilidad de generar un muro.
    /// </summary>
    public float wallMinProbability;

    /// <summary>
    /// 	Variable que define una malla sobre la que se situarán los
    ///     identificadores de las distintas zonas.
    /// </summary>
    private string[,] mazeStructure;
    
    /// <summary>
    /// 	Configuraciones iniciales y generación del laberinto.
    /// </summary>
    void Start()
    {
        wallMinProbability = 0.6000f;
        height = 100;
        width = 100;
        mazeStructure = new string[height, width];
        
        GenerateLevel();
    }

    void Update()
    {}

    /// <summary>
    /// 	Método para generar el laberinto.
    /// </summary>
    void GenerateLevel()
    {
        // Se coloca la salida de forma aleatoria en un lado.
        int randomStartX = 0;
        int randomStartY = Mathf.RoundToInt(UnityEngine.Random.value * (height - 1) / 1f);
        mazeStructure[randomStartX, randomStartY] = "Start";

        // Se coloca la meta de forma aleatoria en el lado opuesto.
        int randomEndX = width - 1;
        int randomEndY = Mathf.RoundToInt(UnityEngine.Random.value * (height - 1) / 1f);
        mazeStructure[randomEndX, randomEndY] = "End";

        // Variable que define que se ha llegado a la meta.
        bool isReached = false;

        // Malla generada para colocar el valor de las heurísticas en cada casilla.
        float[,] heuristics = new float[height, width];

        // Generación de 2 tipos de ruido y la variable aleatoria de selección.
        double heuristicNoiseOne = GetRandomNumber(0.7d, 0.8d);
        double heuristicNoiseTwo = GetRandomNumber(0.2d, 0.3d);
        double heuristicRandomValue = UnityEngine.Random.value;

        // Se rellena la malla con las heurísticas.
        for (int x = 0; x < width; x += 1)
        {
            for (int y = 0; y < height; y += 1)
            {
                // Mediante un proceso estocástico, se selecciona la heurística a usar:
                // - Distancia de Manhattan.
                // - Distancia de Chebyshov.
                // - Distancia de Euclídes.
                if (heuristicRandomValue >= heuristicNoiseOne) heuristics[x, y] = (randomEndX - x) + (randomEndY - y);
                else if (heuristicRandomValue <= heuristicNoiseTwo) heuristics[x, y] = Mathf.Max((randomEndX - x), (randomEndY - y));
                else heuristics[x, y] = Mathf.Sqrt(Mathf.Pow((randomEndX - x), 2) + Mathf.Pow((randomEndY - y), 2));
            }
        }

        // Se coloca la posición actual como la inicial.
        int[] actualPosition = {randomStartX, randomStartY}; 

        // Se inicializa un conteo de las iteraciones.
        int counter = 0;

        // Mientras no se haya conseguido alcanzar a la salida.
        while (!isReached)
        {
            // Mallas con los vecinos de una celda y sus pesos.
            List<int[]> possibleNeighbours = new List<int[]>();
            List<float> weightedNeighbours = new List<float>();

            // Exploración de los vecinos superior e inferior.
            for (int x = -1; x <= 1; x += 2)
            {
                if (((actualPosition[0] + x) >= height) || ((actualPosition[0] + x) < 0)) continue;
                if ((mazeStructure[actualPosition[0] + x, actualPosition[1]] == "Path") || 
                    (mazeStructure[actualPosition[0] + x, actualPosition[1]] == "Start") ||
                    (mazeStructure[actualPosition[0] + x, actualPosition[1]] == "End")) continue;
                possibleNeighbours.Add(new int[]{actualPosition[0] + x, actualPosition[1]});
                weightedNeighbours.Add(heuristics[actualPosition[0] + x, actualPosition[1]]);
            }

            // Exploración de los vecinos izquierdo y derecho.
            for (int x = -1; x <= 1; x += 2)
            {
                if (((actualPosition[1] + x) >= width) || ((actualPosition[1] + x) < 0)) continue;
                if ((mazeStructure[actualPosition[0], actualPosition[1] + x] == "Path") || 
                    (mazeStructure[actualPosition[0], actualPosition[1] + x] == "Start") ||
                    (mazeStructure[actualPosition[0], actualPosition[1] + x] == "End")) continue;
                possibleNeighbours.Add(new int[]{actualPosition[0], actualPosition[1] + x});
                weightedNeighbours.Add(heuristics[actualPosition[0], actualPosition[1] + x]);
            }

            // Si no quedan vecinos, se corta el bucle.
            if (weightedNeighbours.Count == 0) break;
            
            // Mejor valor entre las heurísticas.
            float minValue = weightedNeighbours.Min();
            int minIndex = weightedNeighbours.IndexOf(minValue);

            // Generación de 4 tipos de ruido y una variable aleatoria de elección.
            double noiseOne = GetRandomNumber(0.85d, 0.9d);
            double noiseTwo1 = GetRandomNumber(0.7d, 0.8d);
            double noiseTwo2 = GetRandomNumber(noiseTwo1, 0.85d);
            double noiseThree = GetRandomNumber(0.1d, 0.2d);
            double randomValue = UnityEngine.Random.value;

            // Si la variable aleatoria cae en el primer umbral, se aplica
            // la peor elección de entre las heurísticas.
            if (randomValue >= noiseOne)
            {
                minValue = weightedNeighbours.Max();
                minIndex = weightedNeighbours.IndexOf(minValue);
            }
            // Si la variable aleatoria cae en el segundo umbral, se aplica una
            // elección aleatoria.
            else if ((randomValue >= noiseTwo1) && (randomValue <= noiseTwo2))
            {
                System.Random rand = new System.Random();
                int index = rand.Next(0, weightedNeighbours.Count);
                actualPosition = possibleNeighbours[index];
            }
            // Si la variable cae en el tercer umbral, se aplica la segunda
            // mejor elección.
            else if (randomValue <= noiseThree)
            {
                if (weightedNeighbours.Count > 1)
                {
                    weightedNeighbours.RemoveAt(minIndex);
                    possibleNeighbours.RemoveAt(minIndex);

                    minValue = weightedNeighbours.Min();
                    minIndex = weightedNeighbours.IndexOf(minValue);
                }
            }
            // Si la variable no cae en ningún umbral, se mantiene la
            // elección previa.

            // Se actualiza la posición actual
            actualPosition = possibleNeighbours[minIndex];

            // Si estamos en la posición final, se coloca como isReached, si no
            // se coloca la posición como navegable.
            if (mazeStructure[actualPosition[0], actualPosition[1]] == "End") isReached = true;
            else mazeStructure[actualPosition[0], actualPosition[1]] = "Path";

            // Se aumentan las iteraciones.
            counter++;

            // Si las iteraciones superan un máximo, se rompe el bucle.
            if (counter >= (width * height) * 2) break;
        }

        // Estructura inicial del laberinto, se colocan las paredes y los bordes.
        for (int x = 0; x < width; x += 1)
        {
            for (int y = 0; y < height; y += 1)
            {
                // Bordes del laberinto.
                if(x == (width - 1) || x == 0 || y == 0 || y == (height - 1))
                {
                    if (((x != randomStartX) || (y != randomStartY)) && ((x != randomEndX) || (y != randomEndY)))
                    {
                        Vector3 pos = new Vector3(x - (width - 1) / 2f, 1f, y - (height - 1) / 2f);
                        Instantiate(wall, pos, Quaternion.identity, transform);
                    }
                    continue;
                }

                // Si se supera un umbral, se coloca un muro.
                if (UnityEngine.Random.value > wallMinProbability)
                {
                    if ((mazeStructure[x, y] != "Path") && (mazeStructure[x, y] != "Start") &&
                        (mazeStructure[x, y] != "End"))
                        {
                            Vector3 pos = new Vector3(x - width / 2f, 1f, y - height / 2f);
                            Instantiate(wall, pos, Quaternion.identity, transform);
                        }
                }
            }
        }
    }

    /// <summary>
    /// 	Función para obtener un número aleatorio entre dos umbrales.
    /// </summary>
    double GetRandomNumber(double minimum, double maximum)
    { 
        System.Random random = new System.Random();
        return random.NextDouble() * (maximum - minimum) + minimum;
    }
}
