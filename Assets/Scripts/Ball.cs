using UnityEngine;

public class Ball : MonoBehaviour
{
    public float _speed = 30f;
    Rigidbody _rigidbody;
    Vector3 _velocity;
    Renderer _renderer;


    public void changeSpeed(float speed)
    {
        _speed = speed;
    }
    // Start is called before the first frame update
    void Start()
    {

        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
        Invoke("Launch", 0.05f);

    }

    void Launch()
    {
        _rigidbody.velocity = Vector3.up * _speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_renderer.isVisible)
        {
            GameManager.Instance.Balls--;
            Destroy(gameObject);
        }
        _rigidbody.velocity = _rigidbody.velocity.normalized * _speed;
        _velocity = _rigidbody.velocity;
    }
    private void OnCollisionEnter(Collision collision)
    {
        _rigidbody.velocity = Vector3.Reflect(_velocity, collision.contacts[0].normal);
    }
}
