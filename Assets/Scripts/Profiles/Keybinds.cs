
using System;
using System.IO;
using System.Linq;
using UnityEngine;

public class Keybinds
{
    // DEFAULTS
    public static readonly KeyCode D_ACCELERATE = KeyCode.W;
    public static readonly KeyCode D_LEFT = KeyCode.A;
    public static readonly KeyCode D_BRAKE = KeyCode.S;
    public static readonly KeyCode D_RIGHT = KeyCode.D;
    private readonly int keybindsCount = 4;

    private FileInfo file;

    private KeyCode Accelerate; // Movimiento hacia adelante, hacer aceleracion de acuerdo al cambio del carro
    private KeyCode Left; // Movimiento hacia la izquierda
    private KeyCode Brake; // Movimiento hacia atrás, frenar el carro
    private KeyCode Right; // Movimiento hacia la derecha

    public Keybinds(FileInfo file)
    {
        this.file = file;
        this.readFile();
    }

    private void readFile()
    {
        StreamReader reader = new StreamReader(this.file.FullName);

        if(File.ReadLines(this.file.FullName).Count() != keybindsCount)
        {
            reader.Close();

            this.setDefaults();
            this.saveFile();
        } else
        {
            this.Accelerate = parseKeyCode(reader);
            this.Left = parseKeyCode(reader);
            this.Brake = parseKeyCode(reader);
            this.Right = parseKeyCode(reader);
            
            reader.Close();
        }
    }

    private void setDefaults()
    {
        this.Accelerate = D_ACCELERATE;
        this.Left = D_LEFT;
        this.Brake = D_BRAKE;
        this.Right = D_RIGHT;
    }

    private void saveFile()
    {
        StreamWriter writer = new StreamWriter(this.file.FullName, false);
        writer.WriteLine(this.Accelerate);
        writer.WriteLine(this.Left);
        writer.WriteLine(this.Brake);
        writer.WriteLine(this.Right);
        writer.Close();
    }

    private KeyCode parseKeyCode(StreamReader r)
    {
        return (KeyCode) Enum.Parse(typeof(KeyCode), r.ReadLine());
    }
}
