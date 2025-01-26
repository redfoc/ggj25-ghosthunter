using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public float speed = 6f;           // Kecepatan karakter
    public float jumpForce = 5f;       // Kekuatan lompat

    private Rigidbody rb;              // Menyimpan referensi ke Rigidbody
    private bool isGrounded;           // Mengecek apakah karakter berada di tanah

    // Ground check variables
    public Transform groundCheck;      // Posisi untuk memeriksa apakah karakter di tanah
    public float groundDistance = 0.4f;    // Jarak cek tanah
    public LayerMask groundMask;       // Layer tanah

    public int maxHealth;
    public int curHealth;
    private Animator anim;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Mendapatkan komponen Rigidbody dari player
        rb = GetComponent<Rigidbody>();
        curHealth = maxHealth;
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        // Mengambil input dari keyboard untuk pergerakan
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Membuat vektor gerak berdasarkan input
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Menggerakkan karakter dengan kecepatan yang ditentukan, hanya pada sumbu X dan Z
        rb.MovePosition(rb.position + move * speed * Time.deltaTime);
    }

    void Jump()
    {
        // Mengecek apakah karakter berada di tanah menggunakan Physics.CheckSphere
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Jika tombol lompat ditekan dan karakter berada di tanah, lakukan lompatan
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void TakeDamage(int damage)
    {
        anim.SetTrigger("Hit");
        AudioManager.instance.PlaySFX(0);
        curHealth -= damage;
        if(curHealth < 0){
            curHealth = 0;
        }
        UIScript.instance.UpdateHealthSliderValue(curHealth, maxHealth);
        if (curHealth <= 0)
        {
            // gameObject.SetActive(false);
            UIScript.instance.ShowPopupGameover();
            anim.SetTrigger("Dead");
            AudioManager.instance.PlaySFX(1);
            print("GameOver");
        } else {
            CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
            if (cameraFollow != null)
            {
                cameraFollow.StartShake();
            }
        }
    }
}
