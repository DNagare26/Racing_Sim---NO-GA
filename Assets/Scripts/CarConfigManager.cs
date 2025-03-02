using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class CarConfigManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField numberOfCarsInput; // Input field for selecting number of cars
    public GameObject carConfigTemplate; // Prefab for individual car settings
    public Transform configPanelContainer; // Parent object for all car panels
    public Transform tabContainer; // Parent object for tab buttons
    public GameObject tabButtonTemplate; // Prefab for tab buttons
    public GameObject saveButton; // Save button for finalizing settings

    private List<CarData> carConfigurations = new List<CarData>(); // Stores car settings
    private List<GameObject> carPanels = new List<GameObject>(); // Stores UI panels for each car
    private List<Button> tabButtons = new List<Button>(); // Stores buttons for switching tabs

    private string savePath;

    private void Start()
    {
        saveButton.SetActive(false);
        savePath = Path.Combine(Application.persistentDataPath, "CarConfigs.json");

        LoadConfigurations(); // Load previously saved configurations if available
    }

    public void GenerateCarConfigurations()
    {
        foreach (Transform child in configPanelContainer) Destroy(child.gameObject);
        foreach (Transform child in tabContainer) Destroy(child.gameObject);
        carConfigurations.Clear();
        carPanels.Clear();
        tabButtons.Clear();

        if (!int.TryParse(numberOfCarsInput.text, out int carCount) || carCount <= 0 || carCount > 10)
        {
            Debug.Log("Invalid number of cars entered! Must be between 1 and 10.");
            carCount = 5;
            return;
        }

        for (int i = 0; i < carCount; i++)
        {
            // Generate realistic car values
            float topSpeed = Mathf.Round(Random.Range(20f, 60f) * 100f) / 100f;
            float acceleration = Mathf.Round(Random.Range(1.5f, 5.5f) * 100f) / 100f;
            float downforce = Mathf.Round(Random.Range(50f, 200f) * 100f) / 100f;
            float fuel = Mathf.Round(Random.Range(30f, 100f) * 100f) / 100f;
            float tyreGrip = Mathf.Round(Random.Range(0.7f, 1.3f) * 100f) / 100f;
            float aeroEfficiency = Mathf.Round(Random.Range(0.75f, 0.95f) * 100f) / 100f;
            float pitStopTime = Mathf.Round(Random.Range(2.5f, 6f) * 100f) / 100f;

            CarData newCar = new CarData(topSpeed, acceleration, downforce, fuel, tyreGrip, aeroEfficiency, pitStopTime, "Sunny");
            carConfigurations.Add(newCar);

            GameObject panel = Instantiate(carConfigTemplate, configPanelContainer);
            panel.name = $"Car {i + 1} Config";
            panel.SetActive(i == 0);
            carPanels.Add(panel);
            SetupCarPanel(panel, i);

            GameObject tabButtonObj = Instantiate(tabButtonTemplate, tabContainer);
            Button tabButton = tabButtonObj.GetComponent<Button>();
            tabButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Car {i + 1}";
            int index = i;
            tabButton.onClick.AddListener(() => SwitchToCar(index));
            tabButtons.Add(tabButton);
        }

        saveButton.gameObject.SetActive(true);
    }

    private void SetupCarPanel(GameObject panel, int index)
    {
        panel.transform.Find("CarTitle").GetComponent<TextMeshProUGUI>().text = $"Car {index + 1}";

        string[] fields = { "TopSpeedInput", "AccelerationInput", "DownforceInput", "FuelInput", "TyreGripInput", "AeroEfficiencyInput", "PitStopTimeInput" };
        float[] values = { carConfigurations[index].topSpeed, carConfigurations[index].acceleration, carConfigurations[index].downforce,
                           carConfigurations[index].fuel, carConfigurations[index].tyreGrip, carConfigurations[index].aeroEfficiency, carConfigurations[index].pitStopTime };

        for (int i = 0; i < fields.Length; i++)
        {
            TMP_InputField inputField = panel.transform.Find(fields[i]).GetComponent<TMP_InputField>();
            inputField.text = values[i].ToString();
            int fieldIndex = i;
            inputField.onEndEdit.AddListener(value => UpdateCarConfig(index, fieldIndex, value));
        }
        
    }

    private void UpdateCarConfig(int carIndex, int fieldIndex, string value)
    {
        if (float.TryParse(value, out float newValue))
        {
            switch (fieldIndex)
            {
                case 0: carConfigurations[carIndex].topSpeed = newValue; break;
                case 1: carConfigurations[carIndex].acceleration = newValue; break;
                case 2: carConfigurations[carIndex].downforce = newValue; break;
                case 3: carConfigurations[carIndex].fuel = newValue; break;
                case 4: carConfigurations[carIndex].tyreGrip = newValue; break;
                case 5: carConfigurations[carIndex].aeroEfficiency = newValue; break;
                case 6: carConfigurations[carIndex].pitStopTime = newValue; break;
            }
        }
    }

    private void SwitchToCar(int index)
    {
        for (int i = 0; i < carPanels.Count; i++)
        {
            carPanels[i].SetActive(i == index);
        }
    }

    public void SaveConfigurations()
    {
        if (carConfigurations.Count == 0)
        {
            Debug.LogError("No car configurations to save!");
            return;
        }

        CarData.CarDataList carDataList = new CarData.CarDataList(carConfigurations);
        string json = JsonUtility.ToJson(carDataList, true);
        File.WriteAllText(savePath, json);

        Debug.Log($"Saved {carConfigurations.Count} Car Configurations: {savePath}");

        UnityEngine.SceneManagement.SceneManager.LoadScene("Training");
    }

    public void LoadConfigurations()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            CarData.CarDataList loadedData = JsonUtility.FromJson<CarData.CarDataList>(json);
            carConfigurations = loadedData.cars;

            Debug.Log($"Loaded {carConfigurations.Count} Car Configurations from {savePath}");
        }
        else
        {
            Debug.LogWarning("No saved car configurations found.");
        }
    }
    public List<CarData> GetCarConfigurations()
    {
        return carConfigurations;
    }

}
