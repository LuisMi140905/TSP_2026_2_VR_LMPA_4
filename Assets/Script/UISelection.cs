//Obligatorios
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
//Optativos
using System.Collections.Generic;
using System;

public class UISelection : MonoBehaviour
{
    public static bool gazedAt;
    [SerializeField]
    float fillTime = 5f;
    public Image radialImage;
    public UnityEvent onFillComplete; //Evento genérico que se asigna al terminar la carga

    //Proceso asíncrono
    private Coroutine fillCoroutine;


    void Start()
    {
        gazedAt = false;
        radialImage.fillAmount = 0;
    }

    public void OnPointerEnter ()
    {
        gazedAt = true;

        if(fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
        }
        fillCoroutine = StartCoroutine(FillRadial());
    }

    public void OnPointerExit()
    {
        gazedAt = false;

        if(fillCoroutine !=null)
        {
            StopCoroutine(fillCoroutine);
            fillCoroutine = null;
        }
        radialImage.fillAmount = 0f; //Reinicia el llenado a 0
    }
    private IEnumerator FillRadial() 
    {
        float elapasedTime = 0f;

        while(elapasedTime < fillTime)
        {
            if(!gazedAt) //Dejamos de ver el botón
            {
                yield break;
            }

            elapasedTime += Time.deltaTime;
            radialImage.fillAmount = Mathf.Clamp01(elapasedTime/fillTime);

            yield return null;

        }

        //El evento a ejcutar
        onFillComplete?.Invoke();

        Console.WriteLine(gazedAt ? "Verdadero" : "Falso");
    }
    void Update()
    {
        
    }
}
