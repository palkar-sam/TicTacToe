using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstLauncher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LeadScene());
    }

    public IEnumerator LeadScene()
    {
        AsyncOperation asyncOperationStadium = null;
        yield return new WaitForSecondsRealtime(0.5f);
        asyncOperationStadium = SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
        while (!asyncOperationStadium.isDone)
        {
            //Debug.Log(OnLoadLevelAsync ::  while (!asyncOperationStadium.isDone) 111");
            yield return new WaitForEndOfFrame();
        }
    }
}
