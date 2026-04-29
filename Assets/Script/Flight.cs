using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Flight : MonoBehaviour
{
    public float speed = 50f;
    public float rotationSpeed = 100f;
    public Transform cameraTransform;
    public Vector2 movementInput;
    //Control de iteraciones
    public int turbulenceIterations = 100000;
    //Lista de vectores de posición calculados
    private List<Vector3>turbulenceForces = new List<Vector3>();
    //Método para mover la nave
    public void OnMovement(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }
    void Start()
    {
        
    }
    void Update()
    {
        if (cameraTransform == null)
        {
            Debug.LogError("No hay cámara asignada");
            return;
        }

        //ACTIVIDAD 1: Proceso pesado que consume recursos
        SimulateTurbulence();   
        
        //Mover la nave de manera lineal

        Vector3 moveDirection = cameraTransform.forward * movementInput.y * speed *Time.deltaTime;
        this.transform.position += moveDirection;

        //Mover la nave en rotación
        float yaw = movementInput.x * rotationSpeed * Time.deltaTime;
        this.transform.Rotate(0,yaw,0);

    }

    public void SimulateTurbulence()
    {
        turbulenceForces.Clear();

        //Repeticiones

        for (int i=0; i<turbulenceIterations;i++)
        {
            Vector3 force = new Vector3(
                    Mathf.PerlinNoise(i * 0.001f, Time.time)*2-1,
                    Mathf.PerlinNoise(i * 0.002f, Time.time)*2-1,
                    Mathf.PerlinNoise(i * 0.003f, Time.time)*2-1
                    );

            turbulenceForces.Add(force);
        }
    }
}
