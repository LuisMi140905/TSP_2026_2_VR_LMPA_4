using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;

public class BD : MonoBehaviour
{
    public DatabaseReference reference;
    [SerializeField]
    TMP_InputField textonombre;
    [SerializeField]
    TMP_InputField textoedad;
    public bool registroBooleano = true;

    private void Awake()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void Booleano(bool toggleB)
    {
        registroBooleano = toggleB;
    }

    public void Registro()
    {
        //Generar clave para registro tipo string
        string key = reference.Child("Nombre").Push().Key;
        reference.Child("Nombre").Child(key).SetValueAsync(textonombre.text);

        //Clave única para datos individuales tipo int
        reference.Child("Edad").SetValueAsync(int.Parse(textoedad.text));

        //Clave para tipo booleano

        reference.Child("Booleano").SetValueAsync(registroBooleano);

        //Clave para registro de objeto tipo usuario

        Usuario usuario = new Usuario("LuisAcolt", "lperezacolt@gmail");
        string json = JsonUtility.ToJson(usuario);

        reference.Child("Usuario").SetRawJsonValueAsync(json);

        //Datos a actualizar
        Debug.Log("Dato Escuela anterior: UNAM");
        reference.Child("Escuela").SetValueAsync("UNAM");
        Debug.Log("Dato Escuela actual: POLI");
        reference.Child("Escuela").SetValueAsync("POLI");

        Debug.Log("Dato Año anterior: 2025");
        reference.Child("Año").SetValueAsync(2025);
        Debug.Log("Dato Año actual: 2026");
        reference.Child("Año").SetValueAsync(2026);
    }


    public void CargaBD()
    {
        //Obteniendo registro Año
        reference.Child("Año").GetValueAsync().ContinueWithOnMainThread(TaskExtension =>
        {
            if (TaskExtension.IsFaulted)
            {
                Debug.Log("Error al obtener datos " + TaskExtension.Exception);
            }
            else if (TaskExtension.IsCompleted)
            {
                DataSnapshot snapshot = TaskExtension.Result;
                string value = snapshot.Value.ToString();
                Debug.Log("Tipo de valor obtenido " + snapshot.Value.GetType());
                Debug.Log("Valor: " + value);
            }
            else
            {
                Debug.Log("Registro con error");
            }
        });

        //Carga de valores anidados con clave única

        reference.Child("Nombre").GetValueAsync().ContinueWithOnMainThread
        (
            TaskExtension =>
            {
                if (TaskExtension.IsFaulted)
                {
                    Debug.Log("Error al obtener datos " + TaskExtension.Exception);
                }
                else if (TaskExtension.IsCompleted)
                {
                    DataSnapshot snapshot2 = TaskExtension.Result;
                    //Recorrer todos los hijos de Registro Nombre y obtener los valores
                    foreach (DataSnapshot childSnapshot in snapshot2.Children)
                    {
                        string value2 = childSnapshot.Value.ToString();
                        Debug.Log("Tipo de valor obtenido " + snapshot2.Value.GetType());
                        Debug.Log("Valor: " + value2);
                    }

                }
                else
                {
                    Debug.Log("Registro con error");
                }
            }
            );

        //Carga tipo JSON
        reference.Child("Usuario").GetValueAsync().ContinueWithOnMainThread
            (
                TaskExtension =>
                {
                    if (TaskExtension.IsFaulted)
                    {
                        Debug.Log("Error al obtener datos " + TaskExtension.Exception);
                    }
                    else if (TaskExtension.IsCompleted)
                    {
                        DataSnapshot snapshot3 = TaskExtension.Result;
                        //Convertir el JSON a un diccionario

                        Dictionary<string, object> userData = JsonConvert.DeserializeObject<Dictionary<string, object>>(snapshot3.GetRawJsonValue());
                        Debug.Log("Tipo de valor obtenido " + userData.GetType());
                        string nombre = (string)userData["UserName"];
                        string email = (string)userData["Email"];

                        Debug.Log($"Nombre de usuario {nombre}, correo: {email}");

                    }
                    else
                    {
                        Debug.Log("Registro con error");
                    }
                }
            );
    }
}


public class Usuario
{
    public string UserName;
    public string Email;

    public Usuario(string UserName, string Email)
    {
        this.UserName = UserName;
        this.Email = Email;
    }
}

