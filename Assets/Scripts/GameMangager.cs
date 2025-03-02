using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public GameObject carPrefab;
    public Transform[] spawnPoints;
    private List<CarController> cars = new List<CarController>();
    public RaceManager raceManager;
    private List<CarData> carDataList;

    private void Start()
    {
        LoadCarData();
        SpawnCars();
    }

    private void LoadCarData()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "CarConfigs.json");

        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            CarData.CarDataList loadedData = JsonUtility.FromJson<CarData.CarDataList>(json);
            carDataList = loadedData.cars;
            Debug.Log($"Loaded {carDataList.Count} car configurations from CarData.");
        }
        else
        {
            Debug.LogError("Car data file not found! Ensure configurations are saved before running.");
            carDataList = new List<CarData>();
        }
    }

    private void SpawnCars()
    {
        if (carDataList == null || carDataList.Count == 0)
        {
            Debug.LogError("No car configurations found!");
            return;
        }

        for (int i = 0; i < carDataList.Count; i++)
        {
            Transform spawnPoint = spawnPoints[i % spawnPoints.Length];
            GameObject carObj = Instantiate(carPrefab, spawnPoint.position, spawnPoint.rotation);
            CarController carController = carObj.GetComponent<CarController>();
            CarAI carAI = carObj.GetComponent<CarAI>();

            if (carController == null || carAI == null)
            {
                Debug.LogError($"CarController or CarAI missing on {carObj.name}");
                continue;
            }

            // Assign car attributes from CarData
            CarData carData = carDataList[i];
            carController.SetAttributes(carData);

            // Assign unique Neural Network to each car
            carAI.SetNeuralNetwork(new NeuralNetwork(12, 6, 2));

            // Debug Log Car Configurations
            Debug.Log($"Spawned Car {i + 1}: " +
                      $"TopSpeed={carData.topSpeed} km/h, " +
                      $"Acceleration={carData.acceleration} m/s², " +
                      $"Downforce={carData.downforce} N, " +
                      $"Fuel={carData.fuel} L, " +
                      $"TyreGrip={carData.tyreGrip}, " +
                      $"AeroEfficiency={carData.aeroEfficiency}, " +
                      $"PitStopTime={carData.pitStopTime} sec.");

            // Ensure RaceManager is assigned
            if (raceManager == null)
            {
                raceManager = FindObjectOfType<RaceManager>();
                if (raceManager == null)
                {
                    Debug.LogError("RaceManager not found in the scene!");
                    return;
                }
            }

            raceManager.RegisterCar(carController);
            cars.Add(carController);
        }
    }
}
