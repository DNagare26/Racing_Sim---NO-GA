using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    public int totalLaps = 3;
    private Dictionary<CarController, float> fastestLapTimes = new Dictionary<CarController, float>();
    private Dictionary<CarController, int> lapCounts = new Dictionary<CarController, int>();
    private List<CarController> racePositions = new List<CarController>();
    private bool raceEnd = false;

    private void Update()
    {
        if (raceEnd)
        {
            Debug.Log("Race has ended! No more laps will be counted.");
        }
    }

    public void RegisterCar(CarController car)
    {
        if (car == null)
        {
            Debug.LogError("Attempted to register a null car in RaceManager!");
            return;
        }

        if (!lapCounts.ContainsKey(car))
        {
            lapCounts.Add(car, 0);
        }

        if (!fastestLapTimes.ContainsKey(car))
        {
            fastestLapTimes.Add(car, float.MaxValue);
        }

        if (!racePositions.Contains(car))
        {
            racePositions.Add(car);
        }
    }

    public float GetFastestLapTime(CarController car)
    {
        return fastestLapTimes.ContainsKey(car) ? fastestLapTimes[car] : 0f;
    }

    public int GetCurrentLap(CarController car)
    {
        return lapCounts.ContainsKey(car) ? lapCounts[car] : 0;
    }

    public int GetCarPosition(CarController car)
    {
        return racePositions.IndexOf(car) + 1;
    }

    public bool CompleteLap(CarController car)
    {
        if (!lapCounts.ContainsKey(car))
        {
            lapCounts[car] = 0;
        }

        lapCounts[car]++;

        if (lapCounts[car] >= totalLaps)
        {
            raceEnd = true;
            EndRace(car.gameObject);
            return true;
        }

        return false;
    }

    public float GetPitStopTime()
    {
        return 5f;
    }

    public void RespawnCar(CarController car)
    {
        if (car == null)
        {
            Debug.LogError("Attempted to respawn a null car.");
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        car.transform.position = spawnPoint.position;
        car.transform.rotation = spawnPoint.rotation;
        car.GetComponent<Rigidbody>().velocity = Vector3.zero;

        if (lapCounts.ContainsKey(car))
        {
            lapCounts[car] = Mathf.Max(0, lapCounts[car] - 1); // Lose progress but keep waypoints
        }

        CarAI carAI = car.GetComponent<CarAI>();
        if (carAI != null)
        {
            carAI.Evolve(); // AI evolves upon respawning
        }
    }

    private void EndRace(GameObject winner)
    {
        CarController winnerCar = winner.GetComponent<CarController>();

        if (winnerCar != null && CompleteLap(winnerCar))
        {
            Debug.Log($"{winner.name} wins!");
            Invoke(nameof(ShowResults), 10f);
        }
    }

    private void ShowResults()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Results");
    }
}
