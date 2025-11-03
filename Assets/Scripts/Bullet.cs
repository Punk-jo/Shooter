using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 5f;
    [SerializeField] private Rigidbody _rigidbody;
    public void Init(Vector3 velocity)
    {
        _rigidbody.linearVelocity = velocity;

        StartCoroutine(DelayDestroy());
    }

    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(_lifeTime);
        Destroy();
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy();
    }
}
