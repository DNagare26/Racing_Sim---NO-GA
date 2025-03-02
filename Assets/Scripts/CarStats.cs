using UnityEngine;

public class CarStats
{
    public float fastestLap;
    public float topSpeed;
    public int totalCollisions;

    public CarStats(float fastestLap, float topSpeed, int totalCollisions)
    {
        this.fastestLap = fastestLap;
        this.topSpeed = topSpeed;
        this.totalCollisions = totalCollisions;
    }
}