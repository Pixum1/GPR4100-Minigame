using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Movement variables
    [SerializeField]
    private float m_moveSpeed = 5f; //player's movementspeed
    [SerializeField]
    private float m_sprintSpeed = 10f;
    private float maxSpeed; //player's maximum allowed movementspeed
    private float speedSave;
    private Vector3 currMoveDir; //direction that the player moves in
    private Rigidbody rb;
    #endregion
    [SerializeField]
    private Transform m_playerSprites;
    Endurance endur;
    private HPData hp;
    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private GameObject idleAnim;
    [SerializeField]
    private GameObject walkAnim;


    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        hp = GetComponent<HPData>();
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        speedSave = m_moveSpeed;
        endur = GetComponent<Endurance>();
        uiManager.ResetScreens();
    }
    private void Update()
    {
        maxSpeed = m_moveSpeed / 12.5f; //set the maximum speed of the player according to its initial speed

        #region Movement Direction
        currMoveDir = Vector3.zero;
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");


        #region Normal Movement
        if (x != 0)
        {
            //if +, move to lower right || if -, move to upper left (- x - = +)
            currMoveDir += new Vector3(x, 0, 0);
        }
        if (z != 0)
        {
            //if +, move to upper right || if -, move to lower left
            currMoveDir += new Vector3(0, 0, z);
        }

        if(x != 0 || z != 0) {
            walkAnim.SetActive(true);
            idleAnim.SetActive(false);
        }
        else if( x == 0 && z == 0) {
            walkAnim.SetActive(false);
            idleAnim.SetActive(true);
        }
        #endregion

        #endregion

        #region Sprint
        if (Input.GetKey(KeyCode.LeftShift) && endur.AllowSprint)
        {
            endur.Excercising = true;
            m_moveSpeed = m_sprintSpeed;
        }
        else
        {
            endur.Excercising = false;
            m_moveSpeed = speedSave;
        }
        #endregion

        #region RotatePlayerSprites
        //if (Time.timeScale > 0)
        //{
        //    if (currMoveDir == new Vector3(0, 0, -2)) //move lowerright
        //        m_playerSprites.rotation = Quaternion.Euler(new Vector3(0, -180, m_playerSprites.rotation.z));

        //    if (currMoveDir == new Vector3(-1, 0, -1)) //move down
        //        m_playerSprites.rotation = Quaternion.Euler(new Vector3(0, -135, m_playerSprites.rotation.z));

        //    if (currMoveDir == new Vector3(-2, 0, 0)) //move lowerlwft
        //        m_playerSprites.rotation = Quaternion.Euler(new Vector3(0, -90, m_playerSprites.rotation.z));

        //    if (currMoveDir == new Vector3(-1, 0, 1)) //move left
        //        m_playerSprites.rotation = Quaternion.Euler(new Vector3(0, -45, m_playerSprites.rotation.z));

        //    if (currMoveDir == new Vector3(0, 0, 2)) //move upperleft
        //        m_playerSprites.rotation = Quaternion.Euler(new Vector3(0, 0, m_playerSprites.rotation.z));

        //    if (currMoveDir == new Vector3(1, 0, 1)) //move up
        //        m_playerSprites.rotation = Quaternion.Euler(new Vector3(0, 45, m_playerSprites.rotation.z));

        //    if (currMoveDir == new Vector3(2, 0, 0)) //move upperright
        //        m_playerSprites.rotation = Quaternion.Euler(new Vector3(0, 90, m_playerSprites.rotation.z));

        //    if (currMoveDir == new Vector3(1, 0, -1)) //move right
        //        m_playerSprites.rotation = Quaternion.Euler(new Vector3(0, 135, m_playerSprites.rotation.z));
        //}
        #endregion
    }
    private void FixedUpdate()
    {
        #region Rigidbody Movement
        rb.AddForce(currMoveDir * m_moveSpeed);

        #region limit speed
        //if player gets too fast
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed; //limit its speed
        }
        #endregion

        #endregion
    }
    private void OnDestroy()
    {
        uiManager.GameOverScreen();
    }
}
