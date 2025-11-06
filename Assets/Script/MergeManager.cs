using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class MergeManager : MonoBehaviour
{

    [Header("Actions")]
    public Action <FruitType, Vector2> OnFruitMergeProcessed;
    
    [Header("Settings")]
    Fruit lastFruitSender;
    
    void Start()
    {
        Fruit.OnCollsionWithFruit += CollisionBetweenFruitsCallBack;
    }

    private void CollisionBetweenFruitsCallBack(Fruit sender, Fruit otherFruit)
    {
        if (lastFruitSender != null)
            return;

        lastFruitSender = sender;

        ProcessMerge(sender, otherFruit);

        Debug.Log("Столкновение" + sender.name);
    }

    private void ProcessMerge(Fruit sender, Fruit otherFruit)
    {
        FruitType margeFruitType = sender.GetFruitType();
        margeFruitType += 1;

        Vector2 margePosition = (sender.transform.position + otherFruit.transform.position) / 2f;
        sender.MoveTo(margePosition);

        Destroy(sender.gameObject);
        Destroy(otherFruit.gameObject);
        StartCoroutine(RestartLastSenderCorroutine());

        OnFruitMergeProcessed?.Invoke(margeFruitType, margePosition);   

    }
    IEnumerator RestartLastSenderCorroutine()
    {
        yield return new WaitForEndOfFrame(); //подожди кадр
        lastFruitSender = null;
    }
}
