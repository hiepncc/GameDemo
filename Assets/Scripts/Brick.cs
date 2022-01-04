using UnityEngine;

public class Brick : MonoBehaviour
{
    public int hits = 1;
    public int points = 50;
    public Vector3 rotator;
    public Material hitMaterial;

    Material _orgMaterial;
    Renderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(rotator * (transform.position.x + transform.position.y) * 0.1f);
        _renderer = GetComponent<Renderer>();
        _orgMaterial = _renderer.sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotator * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        hits--;
        if (hits <= 0)
        {
            GameManager.Instance.Score += points;
            Destroy(gameObject);
        }
        _renderer.material.color = Color.red;
        Invoke("restoreMaterial", 0.08f);
    }
    void restoreMaterial()
    {
        _renderer.sharedMaterial = _orgMaterial;
    }
}
