using System.Collections;
using UnityEngine;

public class TimeScaleForTest : MonoBehaviour
{
    private bool isSlowed = false;
    private void Start()
    {
        Time.timeScale = 1f;
    }
    private void Update()
    {
        //Debug.Log(transform.position);
        if (Input.GetKeyDown(KeyCode.F))
        {
            isSlowed = !isSlowed;
            Time.timeScale = isSlowed ? 0.0f : 1f;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Time.timeScale = 1;
            StartCoroutine(PassFrameNum());
        }
    }
    IEnumerator PassFrameNum()
    {
        yield return new WaitForEndOfFrame();
        Time.timeScale = 0;
    }
}
