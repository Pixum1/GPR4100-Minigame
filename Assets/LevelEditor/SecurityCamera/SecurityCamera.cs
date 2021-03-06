using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [SerializeField]
    private Vector3 rotLeft;
    [SerializeField]
    private Vector3 rotRight;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float CameraSmooth;
    [SerializeField]
    private float alarmTimer = .5f;
    private string dir = "left";
    private bool inLOS = false;

    [SerializeField]
    private float visionRadius;

    [SerializeField]
    private GameObject radiusOrigin;

    private PlayerController player;

    [SerializeField]
    private Material[] mats;

    [SerializeField]
    private LineRenderer line;

    private bool alarmed = false;
    private bool alarmStarted = false;

    GameManager gm;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }
    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }
    private void Update()
    {
        if (dir == "left")
            RotateLeft();
        else if (dir == "right")
            RotateRight();
        else
            RotateLeft();

        CheckForPlayer();

        UpdateColor();
    }
    private void RotateLeft()
    {
        if (Vector3.Distance(transform.eulerAngles, rotLeft) > CameraSmooth)
            transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, rotLeft, speed * Time.deltaTime);
        else
            dir = "right";
    }
    private void RotateRight()
    {
        if (Vector3.Distance(transform.eulerAngles, rotRight) > CameraSmooth)
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, rotRight, speed * Time.deltaTime);
        else
            dir = "left";
    }

    private void FollowPlayer()
    {
        Vector3 tPos = new Vector3(player.transform.position.x,
                                    transform.position.y,
                                    player.transform.position.z);
        transform.LookAt(tPos);
    }

    private IEnumerator AlarmTimer(float _time)
    {
        if(inLOS)
        {
            alarmStarted = true;
            if (_time > 0)
            {
                yield return new WaitForSeconds(1);
                _time--;
                StartCoroutine(AlarmTimer(_time));
            }
            else
            {
                alarmed = true;
                foreach (GuardBehaviour guard in gm.Guards)
                {
                    if (guard.gameObject != null)
                    {
                        guard.AlarmGuards();
                    }
                }
            }
        }
        else
        {
            alarmed = false;
            StopAllCoroutines();
        }
    }
    private void UpdateColor()
    {
        if(player != null)
        {
            if (inLOS && !alarmed)
                line.material = mats[1];
            else if (alarmed && inLOS)
                line.material = mats[2];
            else if (!inLOS)
                line.material = mats[0];
        }
        else
            line.material=mats[0];
    }

    void CheckForPlayer()
    {
        if(player != null)
        {
            if (Vector3.Distance(radiusOrigin.transform.position, player.transform.position) <= visionRadius)
            {
                RaycastHit hit;
                Vector3 direction = player.transform.position - transform.position; //Vector from guard position to player position

                //cast ray towards player's position | collide with everything
                if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity))
                {
                    //if ray collides with player's collider
                    if (hit.collider.CompareTag("Player"))
                    {
                        inLOS = true;
                        dir = null;
                        FollowPlayer();
                        if(!alarmStarted)
                            StartCoroutine(AlarmTimer(alarmTimer));
                    }
                    else
                    {
                        inLOS = false; 
                        alarmed = false;
                        StopAllCoroutines();
                        alarmStarted = false;
                    }
                }
            }
            else
            {
                inLOS = false; 
                alarmed = false;
                StopAllCoroutines();
                alarmStarted=false;
            }
        }
    }

    public Vector3 GetRotation()
    {
         return transform.eulerAngles;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(radiusOrigin.transform.position, visionRadius);
    }
}
