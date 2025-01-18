using UnityEngine;
using UnityEngine.SceneManagement;

public class Obstacle : MonoBehaviour
{
    private Transform _target;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float destroyDelay = 1f;

    // Start is called before the first frame update
    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("chosen").transform;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_target) return;

        Vector3 position = Vector3.Lerp(transform.position, _target.position, moveSpeed * Time.deltaTime);
        transform.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("chosen"))
        {
            Destroy(other.gameObject);
            SceneManager.LoadScene("Game");
        }
    }

    private void OnParticleTriggerEnter(GameObject other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
