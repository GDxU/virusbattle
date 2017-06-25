using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameSystem : MonoBehaviour
{
	public static GameObject currentGame = null;

    public static GameSystem instance = null;

    public static GameSystem Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        instance = this;
    }
    
	void Start()
    {
        Object origin = Resources.Load("Prefab/TipSystem");
        GameObject gameObject = GameObject.Instantiate(origin) as GameObject;
        Canvas canvas = gameObject.GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;

        origin = Resources.Load("UI/Login");
        currentGame = GameObject.Instantiate(origin, this.transform) as GameObject;

        currentGame.transform.localPosition = Vector3.zero;
        currentGame.transform.localScale = new Vector3(1, 1, 1);

    }
	
	void Update()
    {
	    if (Input.GetKeyDown(KeyCode.Return))
        {
            GameSystem.StartGame();
        }


	}

	public void LoadGame()
	{
		instance.StartCoroutine(instance.LoadingGame());
	}

    public static void StartGame()
    {
		GameObject.Destroy(currentGame);
		instance.StartCoroutine(instance.LoadingGame());
    }
	
	private void SetProgress(int progress)
	{
		// **** 指针动画(加载界面用) ****
		GameObject.Find ("Point").transform.Rotate (3.6f * Vector3.back);
		GameObject.Find ("Progress").GetComponent<Text> ().text = "加载中(" + progress.ToString () + "%)";
	}

	IEnumerator LoadingGame()
	{
		this.transform.Find ("Login(Clone)/Login").gameObject.SetActive (false);
		this.transform.Find ("Login(Clone)/Loading").gameObject.SetActive (true);

		int targerProgress = 0;
        int displayProgress = 0;
		
		// 加载进度页面
		ResourceRequest resourceRequest = Resources.LoadAsync("UI/Game");
        resourceRequest.allowSceneActivation = false;
		
		while (!resourceRequest.isDone)
        {
            targerProgress = (int)(resourceRequest.progress * 100);
            while (displayProgress < targerProgress)
            {
                this.SetProgress(++displayProgress);
                yield return null;
            }
            yield return null;
        }
		
		targerProgress = 100;
        while (displayProgress < targerProgress)
        {
            this.SetProgress(++displayProgress);
            yield return null;
        }

		yield return new WaitForEndOfFrame();
		resourceRequest.allowSceneActivation = true;

		instance.StartCoroutine(instance.ReSetGame(resourceRequest.asset));
	}

    IEnumerator ReSetGame(Object origin)
    {
        currentGame = GameObject.Instantiate(origin, this.transform) as GameObject;

        currentGame.transform.localPosition = Vector3.zero;
        currentGame.transform.localScale = new Vector3(1, 1, 1);
        yield return null;
    }
}
