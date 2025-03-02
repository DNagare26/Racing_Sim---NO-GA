using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TMP_Text carNameText;
    public TMP_Text downforceText;
    public TMP_Text accelerationText;
    public TMP_Text speedText;
    public TMP_Text fastestLapText;
    public TMP_Text lapNumberText;
    public TMP_Text positionText;

    private CameraController cameraController;
    private CarController currentCar;
    private RaceManager raceManager;

    private void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        raceManager = FindObjectOfType<RaceManager>();
    }

    private void Update()
    {
        if (cameraController == null || cameraController.cars.Length == 0) return;

        currentCar = cameraController.cars[cameraController.currentCarIndex].GetComponent<CarController>();
        if (currentCar == null) return;

        carNameText.text = $"Car {cameraController.currentCarIndex + 1}";
        downforceText.text = $"Downforce: {currentCar.GetDownforce()} N";
        accelerationText.text = $"Acceleration: {currentCar.GetCurrentAcceleration()} m/s²";
        speedText.text = $"Speed: {currentCar.GetCurrentSpeed()} km/h";
        fastestLapText.text = $"Fastest Lap: {raceManager.GetFastestLapTime(currentCar)} s";
        lapNumberText.text = $"Lap: {raceManager.GetCurrentLap(currentCar)}";
        positionText.text = $"Position: {raceManager.GetCarPosition(currentCar)}";
    }

}