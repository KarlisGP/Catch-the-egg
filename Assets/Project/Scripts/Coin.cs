using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class Coin : MonoBehaviour
{
    private void OnMouseDown() // peles kliksis
    {
        GameManager.instance.AddScore();
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Lava")) // CompareTag, ne ==
        {
            GameManager.instance.RemoveScore();
            Destroy(gameObject);
        }
    }
}