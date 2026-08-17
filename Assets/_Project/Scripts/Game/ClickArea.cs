using UnityEngine;
using UnityEngine.UI;

public class ClickArea : MonoBehaviour
{
    [SerializeField] int index;

    void Start()
    {
        if (BoardManager.instance != null)
        {
            GetComponentInParent<Button>().onClick.AddListener(() => BoardManager.instance.OnButtonClick(index));
        }
    }
}
