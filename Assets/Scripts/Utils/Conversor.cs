using System;
using UnityEngine;

public class Conversor : MonoBehaviour
{
    public static float UnitsToMeters(float units)
    {
        // Tomando que 20 unidades de Unity equivalen a 5 metros (con referencia al carro)
        return units * 5 / 20;
    }

    public static float MetersToUnits(float meters)
    {
        return meters * 20 / 5; 
    }

    public static float MetersToKilometers(float meters)
    {
        return meters / 1000;
    }

    public static float KilometersToMeters(float kilometers)
    {
        return kilometers * 1000;
    }

    public static float KilometersToUnits(float kilometers)
    {
        return MetersToUnits(KilometersToMeters(kilometers));
    }

    public static float KilometersPerHourToUnitsSecond(float kmh)
    {
        return KilometersToUnits(kmh) / 3600;
    }

    public static float UnitsSecondToKilometersPerHour(float ups)
    {
        return UnitsToKilometers(ups) * 3600;
    }

    public static float UnitsToKilometers(float units)
    {
        return MetersToKilometers(UnitsToMeters(units));
    }
}
