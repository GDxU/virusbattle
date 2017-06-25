using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TipSystem : MonoBehaviour
{
    private Image ImageTip { get; set; }
    private Text TextTip { get; set; }
    private Text _TextTip { get; set; }

    private float progressTime = 0;
    private float targetTime = 1f;

    private bool show = false;
    private bool hide = false;

    private bool self = false;

    void Awake()
    {
        ImageTip = transform.FindChild("Tip").GetComponent<Image>();
        TextTip = transform.FindChild("Tip/Text").GetComponent<Text>();
        _TextTip = transform.FindChild("Tip/_Text").GetComponent<Text>();
    }

    void OnDestroy()
    {
        EventManager.RemoveListener(DataType.ShowTip, ShowTip);
        EventManager.RemoveListener(DataType.ShowTimeTip, ShowTimeTip);
    }

	void Start()
    {
        EventManager.AddListener(DataType.ShowTip, ShowTip);
        EventManager.AddListener(DataType.ShowTimeTip, ShowTimeTip);
    }
	
	void Update()
    {
        if (show)
        {
            progressTime += Time.deltaTime;

            ImageTip.color = new Color(1, 1, 1, progressTime);
            TextTip.color = new Color(0, 0, 0, progressTime);
            _TextTip.color = new Color(0, 0, 0, progressTime);

            if (progressTime >= targetTime)
            {
                hide = true;
                show = false;
            }
        }

        if (hide)
        {
            progressTime -= Time.deltaTime;

            ImageTip.color = new Color(1, 1, 1, progressTime);
            TextTip.color = new Color(0, 0, 0, progressTime);
            _TextTip.color = new Color(0, 0, 0, progressTime);

            if (progressTime <= 0)
            {
                ImageTip.color = new Color(1, 1, 1, 0);
                TextTip.color = new Color(0, 0, 0, 0);
                TextTip.text = "";
                _TextTip.color = new Color(0, 0, 0, 0);
                _TextTip.text = "";
                progressTime = 0;
                hide = false;
            }
        }
    }

    public void ShowTip(object[] parameters)
    {
        self = (bool)parameters[1];
        string src = (string)parameters[0];
        if (self)
        {
            TextTip.text = src;
            _TextTip.text = "";
        }
        else
        {
            TextTip.text = "";
            _TextTip.text = src;
        }
        
        show = true;
        hide = false;
    }

    public void ShowTimeTip(object[] parameters)
    {
        string src = (string)parameters[0];
        self = (bool)parameters[2];
        ShowTip(new object[] { src, self });
        targetTime = (float)parameters[1];
    }

    public enum DataType
    {
        ShowTip,
        ShowTimeTip
    }
}
