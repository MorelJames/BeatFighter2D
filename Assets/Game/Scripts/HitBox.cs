using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Enemy"))
        {
            _playerController.TouchEnemy(transform.position);
        }
    }
    
}
