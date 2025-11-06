using UnityEngine;
using System;

public class Fruit : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private FruitType _fruitType;

    [Header("Action")]
    public static Action<Fruit, Fruit> OnCollsionWithFruit;

    [Header("Element")]
    [SerializeField] private Rigidbody2D _rigidbody2D;

    public void MoveTo(Vector2 targetPosition)
    {
        transform.position = targetPosition;
    }

    public void EnableFruit()
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Fruit otherfruit))
        {
            if (otherfruit.GetFruitType() != _fruitType)
                return;
    
                OnCollsionWithFruit?.Invoke(this, otherfruit);
        }
    }

    public FruitType GetFruitType()
    {
    return _fruitType;
    }

}
