using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardHide : MonoBehaviour
{
	void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnHide);
    }
	
    public void OnHide()
    {
        this.transform.parent.gameObject.SetActive(false);
        GameObject.Destroy(this.gameObject);
    }
}
