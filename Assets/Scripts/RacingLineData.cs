using UnityEngine;
using System.Collections.Generic;

public class RacingLineData : MonoBehaviour
{
    public static RacingLineData Instance;
    private List<Vector3> waypoints = new List<Vector3>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetWaypoints(List<Vector3> newWaypoints)
    {
        waypoints = new List<Vector3>(newWaypoints);
    }

    public List<Vector3> GetWaypoints()
    {
        return waypoints;
    }
}