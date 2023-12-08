using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenPlankGeneratorScript : MonoBehaviour
{
    [SerializeField] private GameObject plankOjb;
    [SerializeField] private float plankGenerationInterval;
    [SerializeField] private float generationAreaWidth;
    
    private void Start()
    {
        StartCoroutine(GenerateWoodenPlank());
    }

    IEnumerator GenerateWoodenPlank()
    {
        while (true)
        {
            if(MiniGamePlayerController.Instance.getWin()){
                break;
            }
            
            yield return new WaitForSeconds(plankGenerationInterval);
            var posX = Random.Range(-generationAreaWidth, generationAreaWidth);

            Vector3 generationPos = new Vector3(transform.position.x + posX, transform.position.y, transform.position.z);
            Instantiate(plankOjb, generationPos, Quaternion.identity);
        }
    }
}
