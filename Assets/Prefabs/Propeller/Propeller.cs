using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Propeller : MonoBehaviour
{
    [SerializeField] Vector3 upPosition;
    [SerializeField] Vector3 downPosition;
    [SerializeField] float speed = 20f;
    Animator animator;

    Transform player;     // Player referansı
    Vector3 lastPos;      // Propeller’in bir önceki frame’deki pozisyonu
    bool moving = false;
    void Start()
    {
        animator = GetComponent<Animator>();
        lastPos = transform.position;
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !moving)
        {
            player = other.transform;
            animator.enabled = false;
            moving = true;
            transform.DOMove(upPosition, speed).SetEase(Ease.OutSine).onComplete = () =>
            {
                StartCoroutine(WaitAndMoveDown());
            };
        }

        lastPos = transform.position;
    }
    IEnumerator WaitAndMoveDown()
    {
        yield return new WaitForSeconds(3f);
        transform.DOMove(downPosition, speed)
                .SetEase(Ease.InSine).onComplete = () =>
                {
                    animator.enabled = true;
                    moving = false;
                    player = null;
                };
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(upPosition, 0.1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(downPosition, 0.1f);
    }
}
