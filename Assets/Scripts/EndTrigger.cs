using System.Collections;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    [SerializeField] private GameObject[] confettiFx;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player")) 
        {
            Debug.Log("Win");
            foreach (GameObject gO in confettiFx) 
            {
                gO.SetActive(true);
                StartCoroutine(NewGame());
            }
        }
    }

    private IEnumerator NewGame() 
    {
        yield return new WaitForSeconds(2.5f);

        foreach(var gO in confettiFx) 
        {
            gO.SetActive(false);
        }
        player.transform.position = spawnPoint.position;
        player.transform.rotation = spawnPoint.rotation;

    }
}
