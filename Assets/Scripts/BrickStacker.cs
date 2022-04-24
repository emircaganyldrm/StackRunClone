using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BrickStacker : MonoBehaviour
{
    private List<GameObject> rows = new List<GameObject>();
    [Header("Row To Instantiate")]
    public GameObject rowPrefab;
    [Space]
    public Transform rowPosition;
    public Transform bricksParent;
    public float brickHeight = 0.1f;
    public float stackSpeed = 0.001f;
    public int totalBricks;
    private int rowsHeight = 0;
    
    
    //coroutine queue can be used to execute multiple coroutine calls
    //private Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();
    //removed because it is unnecessary

    public static BrickStacker Instance;

    private void Awake()
    {
        Instance = this;
        
        //something like object pooling
        AddNewRow(150);
    }

    public void Stack(int amount)
    {
        StartCoroutine(StackEnum(amount));
        ArrangeHeight();
        //coroutineQueue.Enqueue(StackEnum(amount));
    }

    public void DeStack(int amount)
    {
        StartCoroutine(DeStackEnum(amount));
        StartCoroutine(ArrangeHeightEnum());
        //coroutineQueue.Enqueue(DeStackEnum(amount));
    }
    
    void AddNewRow(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var newRow = Instantiate(rowPrefab, rowPosition.position, Quaternion.identity);
            rows.Add(newRow);
            newRow.transform.SetParent(bricksParent);
        }
    }

    private IEnumerator StackEnum(int amount)
    {
        foreach (var row in rows.ToList())
        {
            for (int i = 0; i < row.transform.childCount; i++)
            {
                if (!row.transform.GetChild(i).gameObject.activeSelf && amount > 0)
                {
                    yield return new WaitForSeconds(stackSpeed);
                    row.transform.GetChild(i).gameObject.SetActive(true);
                    totalBricks++;
                    amount--;
                }
            }
        }
    }

    private IEnumerator DeStackEnum(int amount)
    {
        for (int i = rows.ToList().Count - 1; i >= 0; i--)
        {
            for (int j = rows[i].transform.childCount - 1; j >= 0; j--)
            {
                if (rows[i].transform.GetChild(j).gameObject.activeSelf && amount > 0)
                {
                    rows[i].transform.GetChild(j).gameObject.SetActive(false);
                    totalBricks--;
                    amount--;
                    yield return new WaitForSeconds(stackSpeed);
                }
            }
        }
    }

    /*private IEnumerator CoroutineManager()
    {
        while (true)
        {
            while (coroutineQueue.Count > 0)
            {
                yield return StartCoroutine(coroutineQueue.Dequeue());
            }

            yield return null;
        }
    }*/
    
    private void ArrangeHeight()
    {
        rowsHeight = 0;
        foreach (var row in rows.ToList())
        {
            if (row.transform.childCount == 0)
            {
                rows.Remove(row);
            }
            
            row.transform.localPosition = rowPosition.localPosition + Vector3.up * brickHeight * rowsHeight;
            rowsHeight++;
        }
    }

    public IEnumerator ArrangeHeightEnum()
    {
        //used enum to avoid arranging constantly
        yield return new WaitForSeconds(1f);
        ArrangeHeight();
    }
}
