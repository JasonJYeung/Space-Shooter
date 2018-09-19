using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;

}

public class PlayerController : MonoBehaviour {

    public float speed;
    public Boundary boundary;
    public float tilt;
    public GameObject shot;
    public Transform shotSpawn;
    public float weaponRange;
    public float fireRate;
    public int rayDamage;
    public float timeRate = 5f;
    public float slowDuration = 2f;
    public float slowFactor = 0.05f;

    private Rigidbody rb;
    private float nextFire;
    private AudioSource audio;
    private WaitForSeconds shotDuration = new WaitForSeconds(.07f);
    private LineRenderer laserLine;
    private float nextTimePause = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        laserLine = GetComponentInChildren<LineRenderer>();
        laserLine.enabled = false;
    }

    private void Update()
    {
        Time.timeScale += (1f / slowDuration) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0, 1);
        if (Input.GetButton("Fire1") && Time.time > nextFire) {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            audio.Play();
        } else if (Input.GetButton("Jump") && Time.time > nextFire) {
            nextFire = Time.time + fireRate;
            StartCoroutine(ShotEffect());
            laserLine.SetPosition(0, shotSpawn.position);

            RaycastHit hit;
            Vector3 forwardPosition = new Vector3(shotSpawn.position.x,
                                                  shotSpawn.position.y,
                                                  shotSpawn.position.z + weaponRange);
            if (Physics.Raycast(
                shotSpawn.position, forwardPosition, out hit, weaponRange)) {
                laserLine.SetPosition(1, hit.point);
                DestroyByContact dbc =  hit.collider.GetComponent<DestroyByContact>();
                dbc.Damage(rayDamage);
            } else {
                laserLine.SetPosition(1, forwardPosition);
            }
        }
        if (Input.GetButton("Fire3") && Time.time > nextTimePause) {
            nextTimePause = Time.time + timeRate;
            BulletTime();
        }
    }

    private IEnumerator ShotEffect() {
        laserLine.enabled = true;
        audio.Play();
        yield return shotDuration;
        laserLine.enabled = false;
    }



    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.velocity = movement * speed;

        rb.position = new Vector3(
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
        );

        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }

    public void BulletTime()
    {
        Time.timeScale = slowFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }
}
