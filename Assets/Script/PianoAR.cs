using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoAR : MonoBehaviour
{
    public AudioClip[] clips;
    public AudioSource audioSource;
    string btnName;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
        
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray =Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                btnName = hit.transform.name;

                switch (btnName)
                {
                    case "Do":
                        audioSource.clip = clips[0];
                        audioSource.Play();
                        break;
                    case "Re":
                        audioSource.clip = clips[1];
                        audioSource.Play();
                        break;
                    case "Mi":
                        audioSource.clip = clips[2];
                        audioSource.Play();
                        break;
                    case "Fa":
                        audioSource.clip = clips[3];
                        audioSource.Play();
                        break;
                    case "Sol":
                        audioSource.clip = clips[4];
                        audioSource.Play();
                        break;
                    case "La":
                        audioSource.clip = clips[5];
                        audioSource.Play();
                        break;
                    case "Si":
                        audioSource.clip = clips[6];
                        audioSource.Play();
                        break;
                    default:
                        break;
                }
            }
        }

        else if (Input.touchCount >0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                btnName = hit.transform.name;

                switch (btnName)
                {
                    case "Do":
                        audioSource.clip = clips[0];
                        audioSource.Play();
                        break;
                    case "Re":
                        audioSource.clip = clips[1];
                        audioSource.Play();
                        break;
                    case "Mi":
                        audioSource.clip = clips[2];
                        audioSource.Play();
                        break;
                    case "Fa":
                        audioSource.clip = clips[3];
                        audioSource.Play();
                        break;
                    case "Sol":
                        audioSource.clip = clips[4];
                        audioSource.Play();
                        break;
                    case "La":
                        audioSource.clip = clips[5];
                        audioSource.Play();
                        break;
                    case "Si":
                        audioSource.clip = clips[6];
                        audioSource.Play();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
