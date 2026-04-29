using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Threading;
using Unity.VisualScripting;
using System.IO;

public class FlightThreadSinc : MonoBehaviour
{
    public float speed = 50f;
    public float rotationSpeed = 100f;
    public Transform cameraTransform;
    public Vector2 movementInput;

    //Control de iteraciones
    public int turbulenceIterations = 1000; // N

    //Lista de vectores de posición calculados
    private List<Vector3> turbulenceForces = new List<Vector3>();

    //Variables para manipular el hilo secundario

    private Thread turbulenceThread; //Instancia del hilo secundario
    private bool isTurbulenceRunning = false; //Bandera para saber si sigue el cálculo
    private bool stopTurbulenceThread = false; //Bandera para saber si el hilo terminó
    public float capturedTime; //Variable para almacenar el tiempo transcurrido

    //Bandera de control sobre lectura
    public bool read = false;
    public bool write = false;
    private object filelock = new object();
    //Ruta de almacenamiento de archivo
    string filepath;

    //Método para mover la nave
    public void OnMovement(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    void Start()
    {
        filepath = Application.dataPath + "/TurbulenceData.txt";
        Debug.Log("Ruta al archivo: " + filepath);
    }
    
    void Update()
    {
        if (cameraTransform == null)
        {
            Debug.LogError("No hay cámara asignada");
            return;
        }

        //Tiempo transcurrido
        capturedTime = Time.time;

        //Proceso pesado en hilo secundario
        if (!isTurbulenceRunning)
        {
            isTurbulenceRunning = true;
            stopTurbulenceThread = false;

            turbulenceThread = new Thread(() => SimulateTurbulence(capturedTime));
            turbulenceThread.Start();
        }

        //Mover la nave de manera lineal

        Vector3 moveDirection = cameraTransform.forward * movementInput.y * speed * Time.deltaTime;
        this.transform.position += moveDirection;

        //Mover la nave en rotación
        float yaw = movementInput.x * rotationSpeed * Time.deltaTime;
        this.transform.Rotate(0, yaw, 0);

        //Activiadd 3: Siniccronizar hilos
        if (write && !read)
        {
            TryReadFile();
            read = true;
        }

    }

    public void SimulateTurbulence(float time)
    {
        turbulenceForces.Clear();

        //Repeticiones

        for (int i = 0; i < turbulenceIterations; i++)
        {
            //Verificar si se debe detener el hilo

            if (stopTurbulenceThread) { break; }

            Vector3 force = new Vector3(
                    Mathf.PerlinNoise(i * 0.001f, time) * 2 - 1,
                    Mathf.PerlinNoise(i * 0.002f, time) * 2 - 1,
                    Mathf.PerlinNoise(i * 0.003f, time) * 2 - 1
                    );

            turbulenceForces.Add(force);
        }

        //Señal en consola de inicio de hilo

        Debug.Log("Iniciando simulación de turbulencia");


        //ACTIVIDAD 3: Método para escritura del archivo

        lock (filelock)
        {
            //Escritura del archivo
            using (StreamWriter writter = new StreamWriter(filepath, false))
            {
                foreach (var force in turbulenceForces)
                {
                    writter.WriteLine(force.ToString());
                }
                writter.Flush();
            }

            Debug.Log("Archivo escrito");

            //Simulación completa
            isTurbulenceRunning = false;
            write = true;

        }
    }
        void TryReadFile()
        {
            try
            {
                lock (filelock)
                {
                    if (File.Exists(filepath))
                    {
                        string content = File.ReadAllText(filepath);
                        Debug.Log("Archivo Leido" + content);
                    }
                    else
                    {
                        Debug.LogError("Ocurrió un problema");
                    }
                }

            }
            catch (IOException ex)
            {
                Debug.LogError("Error de acceso al archivo" + ex.Message);
            }
        }
    

    private void OnDestroy()
    {
        //Indicar el cierre del hilo secundario
        stopTurbulenceThread = true;

        //Verificar si el hilo existe y se está ejecutando
        if (turbulenceThread != null && turbulenceThread.IsAlive)
        {
            //Lo unimos al hilo principal y cerramos ejecución
            turbulenceThread.Join();
        }
    }
}

