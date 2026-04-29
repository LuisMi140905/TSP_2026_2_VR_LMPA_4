using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class AIManager : MonoBehaviour
{
    [SerializeField]
    public GameObject player;
    [SerializeField]
    public Transform entrance;
    [SerializeField]
    public Transform exit;

    public float detectionRange = 2f;
    public float exitRange = 2f;
    public float minDistanceFromEntrance = 15f;

    List<NavMeshAgent> agents = new ();


    NavMeshTriangulation triangulation;
    Vector3 entrancePos;

    float detencionRangeSqr;
    float exitRangesqr;
    float minDistanceSqr;

    bool gameWon = false;

    System.Random random = new();
        
    void Start()
    {
        entrancePos = entrance.position;
        triangulation = NavMesh.CalculateTriangulation();

        detencionRangeSqr = detectionRange*detectionRange;
        exitRangesqr = Mathf.Pow(exitRange,2);
        minDistanceSqr = Mathf.Pow(minDistanceFromEntrance,2);

        FindAllnemies();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = player.transform.position;

        //Detección de enemigos en posición de jugador

        bool playercaught = false;

        foreach (var agent in agents)
        {
            if (!agent.enabled) continue;

            if ((agent.transform.position - playerPos).sqrMagnitude < detencionRangeSqr)
            {
                playercaught = true;
                break;
            }
            
        }

        //Si el jugador es atrapado
        if (playercaught)
        {
            TeleportPlayerToEntrance();
            RelocateAllNPC();

            return;
        }

        //Jugador en la Salida

        if ((playerPos - exit.position).sqrMagnitude < exitRangesqr)
        {
            gameWon = true;
            Debug.Log("Felicidades ganaste");
        }

        //Persecución

        foreach (var agent in agents) 
        {
            if (agent.enabled && !agent.isStopped) 
            {
                agent.SetDestination(playerPos);
            }
        }
    }


    //Método para llevar a Player a la entrada

    void TeleportPlayerToEntrance() 
    {
        var cc = player.GetComponent<NavMeshAgent>();

        if (cc != null)
        {
            cc.enabled = false;
        }
        player.transform.position = entrancePos;
        if (cc != null)
        {
            cc.enabled = true;
        }

        Debug.Log("Teletransport: " + entrancePos);
    }

    //Métodos para posicionar a los enemigos

    void RelocateAllNPC()
    {
        if (triangulation.vertices.Length == 0) return;

        foreach (var agent in agents)
        {
            agent.enabled = false;
            agent.transform.position = GetValidRandomPosition();
            agent.enabled =true;
        }
    }

    //Método para calcular posiciones válidas
    Vector3 GetValidRandomPosition()
    {
        Vector3 pos;
        do
        {
            int i = random.Next(0,triangulation.indices.Length/3)*3;

            Vector3 v1 = triangulation.vertices[triangulation.indices[i]];
            Vector3 v2 = triangulation.vertices[triangulation.indices[i+1]];
            Vector3 v3 = triangulation.vertices[triangulation.indices[i+2]];

            float r1 = (float)random.NextDouble();
            float r2 = (float)random.NextDouble();

            if (r1 + r2 > 1f)
            {
                r1 = 1f - r1;
                r2 = 1f - r2;
            }

            pos = v1 + r1 * (v2 - v1) + r2 * (v3-v1);

        } while ((pos-entrancePos).sqrMagnitude < minDistanceSqr);
        return pos;
    }

    //Método para ubicar los objetos enemigo

    void FindAllnemies()
    {
        agents.Clear();
        foreach (var agent in FindObjectsByType<NavMeshAgent>(FindObjectsSortMode.None))
        {
            if (agent.CompareTag("Enemy"))
            {
                agents.Add(agent);
            }
        }
    }
}
