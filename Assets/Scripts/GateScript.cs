using TMPro;
using UnityEngine;

public class GateScript : MonoBehaviour
{
    public TextMeshProUGUI text;
    public int brickValue;

    private void OnValidate()
    {
        text.text = brickValue.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            BrickStacker.Instance.Stack(brickValue);
            Destroy(gameObject);
        }
    }
}
