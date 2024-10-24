using UnityEngine;
using System.Collections;

public class Alphabetrot : MonoBehaviour
{
  //  public Sprite letters;
    public Vector3 rotationSpeed = new Vector3(0, 45, 0); // Adjust the speed as needed
    public Sprite s;
    private void Start()
    {

        StartCoroutine(ContinuousRotate());
    }
    private IEnumerator ContinuousRotate()
    {
        while (true) // Keep rotating indefinitely
        {
            // Rotate the GameObject by the specified speed
            transform.Rotate(5 * rotationSpeed * Time.deltaTime);
            yield return null; // Wait for one frame
        }
    }
}
