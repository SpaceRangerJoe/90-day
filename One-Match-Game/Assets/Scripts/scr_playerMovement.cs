using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class scr_playerMovement : MonoBehaviour
{

    [SerializeField]
    private Rigidbody rigidbody;
    [SerializeField]
    private NetworkedObject networkedObject;

    private float moveSpeed = 3f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(NetworkingManager.Singleton.IsServer)
        {
            // Are we the server ? 
            if(networkedObject.IsLocalPlayer)
            {
                // Do we own the player object, or we want something executed only locally
                // Then we have authority to execute the movement ourselves
                MovePlayer();
            }
        }
        else if(NetworkingManager.Singleton.IsClient)
        {
            // Are we the client?
            if(networkedObject.IsLocalPlayer)
            {
                // Then we request the server who has authority to execute our movement
                RequestServerToMovePlayer();
            }
        }
    }

    private void MovePlayer()
    {
        float xMove = Input.GetAxisRaw("Horizontal") * moveSpeed;
        float zMove = Input.GetAxisRaw("Vertical") * moveSpeed;

        Vector3 movePosition = transform.right * xMove + transform.forward * zMove;
        Vector3 nextPostition = new Vector3(movePosition.x, rigidbody.velocity.y, movePosition.z);

        rigidbody.velocity = nextPostition;
    }


    [ServerRPC]
    private void RequestServerToMovePlayer()
    {
        MovePlayer();
    }
}
