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
            yield return new WaitForSeconds(plankGenerationInterval);
            var posX = Random.Range(-generationAreaWidth, generationAreaWidth);

            Vector3 generationPos = new Vector3(posX, transform.position.y, transform.position.z);
            GameObject.Instantiate(plankOjb, generationPos, Quaternion.identity);
        }
    }
}
