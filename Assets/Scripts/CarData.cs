using UnityEngine;
using System.Collections.Generic;

[System.Serializable]

public class CarData
{
    public float topSpeed;
    public float acceleration;
    public float downforce;
    public float fuel;
    public float tyreGrip;
    public float aeroEfficiency;
    public float pitStopTime;


    public CarData(float topSpeed, float acceleration, float downforce, float fuel, float tyreGrip, float aeroEfficiency, float pitStopTime, string weather)
    {
        this.topSpeed = topSpeed;
        this.acceleration = acceleration;
        this.downforce = downforce;
        this.fuel = fuel;
        this.tyreGrip = tyreGrip;
        this.aeroEfficiency = aeroEfficiency;
        this.pitStopTime = pitStopTime;

    }

    [System.Serializable]
    public class CarDataList
    {
        public List<CarData> cars;
        public CarDataList(List<CarData> cars)
        {
            this.cars = cars;
        }
    }
}
