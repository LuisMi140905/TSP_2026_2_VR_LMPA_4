using UnityEngine;

public class 
    NewMonoBehaviourScript: MonoBehaviour
{
    // Atributos / variables de clase

    public Transform vrCamera;
    public float angulo = 30.0f;
    public float speed = 3.0f;
    public bool move;

    private CharacterController controller;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (vrCamera.eulerAngles.x >= angulo && vrCamera.eulerAngles.x < 60.0f)
        {
            move = true;
        }
        else
        {
            move= false;
        }
        if (move )
        {
            Vector3 direccion = vrCamera.TransformDirection(Vector3.forward);
            controller.SimpleMove(direccion*speed);
        }

    }
}
