using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Parsers
{
    public static KeyCode parseKeyCode(StreamReader r)
    {
        return (KeyCode)Enum.Parse(typeof(KeyCode), r.ReadLine());
    }

    public static int parseInt(StreamReader r)
    {
        return int.Parse(r.ReadLine());
    }

    public static float parseFloat(StreamReader r)
    {
        return float.Parse(r.ReadLine(), CultureInfo.InvariantCulture.NumberFormat);
    }

    public static bool parseBool(StreamReader r)
    {
        return bool.Parse(r.ReadLine());
    }
}
