using UnityEngine;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    public Transform[] cars; // List of all car Transforms
    public int currentCarIndex = 0; // Start from Car 1 (Index 0)
    private bool isTopDown = true;
    private bool isSearching = false;

    private Transform topDownView; // ✅ Dynamically assigned
    private Transform thirdPersonView; // ✅ Dynamically assigned

    private void Start()
    {
        FindCars(); // ✅ Ensure cars are found at start
        SetInitialCar(); // ✅ Automatically go to Car 1
    }

    private void Update()
    {
        if (cars == null || cars.Length == 0 || cars[currentCarIndex] == null || !cars[currentCarIndex].gameObject.activeInHierarchy)
        {
            FindNextAvailableCar(); // ✅ Keep looking for a car
        }

        HandleCameraSwitching();
    }

    private void HandleCameraSwitching()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isTopDown = !isTopDown;
        }

        if (cars.Length == 0) return; // ✅ Prevent errors if no cars exist

        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Keypad0 + i))
            {
                SwitchCar(i - 1); // ✅ 1-9 keys switch cars, 0 switches to car 10
            }
        }
        if (Input.GetKeyDown(KeyCode.Keypad0)) { SwitchCar(9); }

        if (cars[currentCarIndex] != null)
        {
            Transform targetView = isTopDown ? topDownView : thirdPersonView;
            if (targetView != null) // ✅ Ensure camera views are assigned
            {
                transform.position = Vector3.Lerp(transform.position, targetView.position, Time.deltaTime * 5);
                transform.LookAt(cars[currentCarIndex].position);
            }
        }
    }

    private void SwitchCar(int index)
    {
        if (cars.Length == 0) return;

        if (index < cars.Length && cars[index] != null && cars[index].gameObject.activeInHierarchy)
        {
            currentCarIndex = index;
            AssignCameraViews(cars[currentCarIndex]); // ✅ Update camera views
        }
        else
        {
            Debug.LogWarning($"Car {index + 1} is missing. Finding next available car...");
            FindNextAvailableCar();
        }
    }

    private void FindNextAvailableCar()
    {
        if (isSearching) return;
        isSearching = true;

        for (int i = 0; i < cars.Length; i++)
        {
            int nextIndex = (currentCarIndex + i) % cars.Length;
            if (cars[nextIndex] != null && cars[nextIndex].gameObject.activeInHierarchy)
            {
                currentCarIndex = nextIndex;
                AssignCameraViews(cars[currentCarIndex]); // ✅ Assign camera views
                isSearching = false;
                return;
            }
        }

        Debug.LogWarning("No available cars found! Searching every second...");
        Invoke(nameof(FindCars), 1f); // ✅ Keep searching
        isSearching = false;
    }

    private void FindCars()
    {
        GameObject[] carObjects = GameObject.FindGameObjectsWithTag("Car"); // ✅ Ensure cars have the "Car" tag
        List<Transform> carList = new List<Transform>();

        foreach (GameObject car in carObjects)
        {
            carList.Add(car.transform);
        }

        cars = carList.ToArray();

        if (cars.Length > 0)
        {
            SetInitialCar(); // ✅ Set camera to first car found
        }
        else
        {
            Debug.LogWarning("No cars found. Searching again...");
            Invoke(nameof(FindCars), 1f); // ✅ Keep searching every second
        }
    }

    private void SetInitialCar()
    {
        if (cars.Length == 0) return;

        if (cars[0] != null && cars[0].gameObject.activeInHierarchy)
        {
            currentCarIndex = 0;
            AssignCameraViews(cars[currentCarIndex]); // ✅ Assign camera views
        }
        else
        {
            FindNextAvailableCar(); // ✅ If Car 1 isn't found, look for the next available car
        }
    }

    private void AssignCameraViews(Transform car)
    {
        if (car == null) return;

        // ✅ Find camera positions dynamically from the car
        Transform topDown = car.Find("TopDownView");
        Transform thirdPerson = car.Find("ThirdPersonView");

        if (topDown != null && thirdPerson != null)
        {
            topDownView = topDown;
            thirdPersonView = thirdPerson;
            Debug.Log($"Camera views assigned for {car.name}");
        }
        else
        {
            Debug.LogWarning($"Car {car.name} is missing camera views! Ensure 'TopDownView' and 'ThirdPersonView' are child objects.");
        }
    }
}
