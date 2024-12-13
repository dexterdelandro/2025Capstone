using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Sprite : MonoBehaviour
{
    Canvas popUpCanvas;
    public ItemCollector player;
    public bool inContact = false;

    
    public bool isZone1Item = true;

    void Start()
    {
        player = GameObject.FindWithTag("Player")?.GetComponent<ItemCollector>();
        if (player == null)
        {
            Debug.LogError("Player or ItemCollector script not found. Make sure the player object has the correct tag and script.");
        }

        popUpCanvas = GetComponentInChildren<Canvas>();
        if (popUpCanvas == null)
        {
            Debug.LogError("Canvas component not found. Ensure the object has a child with a Canvas component.");
        }
    }

    void Update()
    {
        if (!inContact) return;

        if (popUpCanvas != null)
            popUpCanvas.transform.LookAt(Camera.main.transform);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isZone1Item)
            {
                player.CollectItemForZone1(); 
            }
            else
            {
                player.CollectItemForZone2(); 
            }
            StartCoroutine(Collected());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inContact = true;
            if (popUpCanvas != null) popUpCanvas.enabled = true;
        }
    }

    private IEnumerator Collected()
    {
        if (popUpCanvas != null) popUpCanvas.enabled = false;
        yield return new WaitForSeconds(4.5f);
        Destroy(gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inContact = false;
            if (popUpCanvas != null) popUpCanvas.enabled = false;
        }
    }
}
