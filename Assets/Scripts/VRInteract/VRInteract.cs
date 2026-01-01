using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRInteract : MonoBehaviour {
    public Image imgGaze; 
    public float totalTime = 2; 
    bool gvrStatus; 
    float gvrTimer = 0; 
    
    public int distanceOfRay = 20; 
    private RaycastHit _hit; 
    
    // VARIABLE BARU: Mencegah eksekusi berulang kali saat menatap terus menerus
    private bool hasInteracted = false; 

    void Start(){
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void Update(){
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        // --- INTERAKSI CLICK (DESTROY) ---
        if(Physics.Raycast(ray, out _hit, distanceOfRay)){
            if(Input.GetButtonDown("Fire1") && _hit.transform.CompareTag("Enemy")){
                Destroy(_hit.transform.gameObject);
                // Reset Gaze jika musuh hancur agar UI tidak nyangkut
                GVROff(); 
            }
        }
        
        // --- LOGIKA GAZE (TIMER) ---
        if(gvrStatus){
            gvrTimer += Time.deltaTime;
            imgGaze.fillAmount = gvrTimer/totalTime;
            
            // Cek apakah loading penuh DAN belum pernah dieksekusi sebelumnya
            if(imgGaze.fillAmount >= 1 && !hasInteracted){
                
                // Lakukan Raycast lagi untuk memastikan objek masih ada di depan mata
                if(Physics.Raycast(ray, out _hit, distanceOfRay)){
                    
                    // 1. Logika Teleport
                    if(_hit.transform.CompareTag("Teleport")){
                        _hit.transform.gameObject.GetComponent<Teleport>().TeleportPlayer();
                        hasInteracted = true; // Kunci agar tidak teleport berulang-ulang
                        GVROff(); // Reset UI setelah teleport
                    }
                    
                    // 2. Logika Pintu Geser (NEW)
                    else if (_hit.transform.CompareTag("PintuGeser")){
                        _hit.transform.gameObject.GetComponent<SlidingDoor>().ToggleDoor();
                        hasInteracted = true; // Kunci agar pintu tidak buka-tutup berulang-ulang secepat kilat
                        // Kita tidak panggil GVROff disini agar user sadar action sudah terjadi, 
                        // user harus membuang muka dulu untuk reset.
                    }
                }
            }
        }
    }
    
    // Dipasang pada EventTrigger (Pointer Enter)
    public void GVROn(){
        gvrStatus = true;
        hasInteracted = false; // Reset status interaksi saat mulai menatap objek baru
    }
    
    // Dipasang pada EventTrigger (Pointer Exit)
    public void GVROff(){
        gvrStatus = false;
        gvrTimer = 0;
        imgGaze.fillAmount = 0;
        hasInteracted = false; // Reset status interaksi
    }
}