using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : Photon.MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;
    public float gunRotateSpeed;
    public GameObject gunRotator;
    public GameObject ammo;
    public GameObject ammoSpawn;
    public Camera myCam;

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.isMine)
        {
            myCam.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.isMine) { 
            //Pelaajan liike
            float xMovement = Input.GetAxis("Horizontal");
            float zMovement = Input.GetAxis("Vertical");
            transform.Translate(xMovement * moveSpeed * Time.deltaTime, 0, zMovement * moveSpeed * Time.deltaTime);

            //Pelaajan kääntö
            float xMouse = Input.GetAxis("Mouse X");
            Vector3 lookAt = new Vector3(0, xMouse * rotateSpeed * Time.deltaTime, 0);
            transform.Rotate(lookAt);

            //Gunin kääntö ylös alas
            float yMouse = Input.GetAxis("Mouse Y");
            Vector3 aimAt = new Vector3(yMouse * gunRotateSpeed * Time.deltaTime * -1, 0, 0);
            gunRotator.transform.Rotate(aimAt);

            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
                photonView.RPC("Shoot", PhotonTargets.Others);
            }
        } //photonView.isMine
    }
    [PunRPC]
    public void Shoot()
    {
        GameObject ammoInstance = Instantiate(ammo, ammoSpawn.transform.position,
            Quaternion.identity);
        ammoInstance.GetComponent<Rigidbody>().AddForce(gunRotator.transform.forward * 20,
            ForceMode.Impulse);
    }

    /*
     -Tehtävä: Tee jokaiselle pelaajalle health-arvo. Kun pelaaja osuu toisen pelaajan palloon (ei omaansa),
        vähennä healthiä esim 10.
     -Laita jokaisen pelaajan päälle tekstikenttä, jossa näkyy tämänhetkinen health.
     -Laita tykki eli gunRotator kääntymään myös muilla pelaajilla.
    */
}
