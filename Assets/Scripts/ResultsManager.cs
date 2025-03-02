using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResultsManager : MonoBehaviour
{
    public GameObject resultsPanel;
    public Transform tabContainer;
    public GameObject tabButtonTemplate;
    public Transform statsContainer;
    public GameObject statsTemplate;
    public TMP_Text generalRaceStats;
    public TMP_Text leaderboardText;

    private Dictionary<GameObject, CarStats> carResults = new Dictionary<GameObject, CarStats>();

    public void ShowResults(Dictionary<GameObject, CarStats> raceResults)
    {
        carResults = raceResults;
        resultsPanel.SetActive(true);

        foreach (Transform child in tabContainer) Destroy(child.gameObject);
        foreach (Transform child in statsContainer) Destroy(child.gameObject);

        int position = 1;
        leaderboardText.text = "Final Leaderboard:\n";

        foreach (var entry in carResults)
        {
            GameObject car = entry.Key;
            CarStats stats = entry.Value;

            // Add to leaderboard
            leaderboardText.text += $"{position}. Car {car.name} - {stats.fastestLap}s\n";
            position++;

            // Create tab button
            GameObject tabButtonObj = Instantiate(tabButtonTemplate, tabContainer);
            Button tabButton = tabButtonObj.GetComponent<Button>();
            tabButton.GetComponentInChildren<TMP_Text>().text = car.name;
            tabButton.onClick.AddListener(() => ShowCarStats(car));
        }
    }

    private void ShowCarStats(GameObject car)
    {
        foreach (Transform child in statsContainer) Destroy(child.gameObject);

        CarStats stats = carResults[car];
        GameObject statObj = Instantiate(statsTemplate, statsContainer);
        statObj.transform.Find("CarTitle").GetComponent<TMP_Text>().text = car.name;
        statObj.transform.Find("FastestLap").GetComponent<TMP_Text>().text = $"Fastest Lap: {stats.fastestLap}s";
        statObj.transform.Find("TopSpeed").GetComponent<TMP_Text>().text = $"Top Speed: {stats.topSpeed} km/h";
        statObj.transform.Find("TotalCollisions").GetComponent<TMP_Text>().text = $"Collisions: {stats.totalCollisions}";
    }
}
