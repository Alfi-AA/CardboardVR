using System.Collections;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    // Kita ganti bukan input koordinat, tapi "Jarak Geser"
    // Contoh: Jika ingin geser ke kanan (sumbu X) sejauh 0.8 meter, isi X = 0.8 di Inspector
    public Vector3 slideOffset = new Vector3(0.8f, 0, 0); 
    public float speed = 2.0f;
    
    private Vector3 closedPos; // Posisi tertutup (local)
    private Vector3 openPos;   // Posisi terbuka (local)
    
    private bool isOpen = false;
    private bool isMoving = false;

    void Start()
    {
        // KUNCI: Kita gunakan localPosition, bukan position biasa
        closedPos = transform.localPosition;
        openPos = closedPos + slideOffset; 
    }

    public void ToggleDoor()
    {
        if (isMoving) return;

        if (isOpen)
            StartCoroutine(MoveDoor(closedPos)); // Tutup
        else
            StartCoroutine(MoveDoor(openPos)); // Buka
    }

    IEnumerator MoveDoor(Vector3 target)
    {
        isMoving = true;
        while (Vector3.Distance(transform.localPosition, target) > 0.001f)
        {
            // Perhatikan penggunaan localPosition disini
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, speed * Time.deltaTime);
            yield return null;
        }
        transform.localPosition = target;
        isOpen = !isOpen;
        isMoving = false;
    }
}