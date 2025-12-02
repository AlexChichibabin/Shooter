using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : SingletonBase<Player>
{
    [SerializeField] private UIPlayerNotic m_UIPlayerNotic;

    private int pursuersNumber;

    public void StartPursuit()
    {
        pursuersNumber++;
        m_UIPlayerNotic.Show();
    }
    public void StopPursuit()
    {
        pursuersNumber--;

        if(pursuersNumber == 0) m_UIPlayerNotic.Hide();
    }
    public void RestartScene()
    {
        StartCoroutine(RestartSceneNumerator());
    }
    private IEnumerator RestartSceneNumerator()
    {
        yield return new WaitForSeconds(5);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
