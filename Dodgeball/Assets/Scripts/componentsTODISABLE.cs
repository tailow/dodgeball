using UnityEngine;
using UnityEngine.Networking;

public class componentsTODISABLE : NetworkBehaviour {


    public Behaviour[] componentsToDisable;

    public SkinnedMeshRenderer playerMesh;

    Camera sceneCamera;

    public override void OnStartLocalPlayer()
    {
        // Hide player model
        playerMesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
    }

    void Start()
    {
        sceneCamera = Camera.main;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (!isLocalPlayer)
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }

        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        if (sceneCamera != null)
        {
            if (sceneCamera.gameObject.activeInHierarchy == false)
            {
                sceneCamera.gameObject.SetActive(true);
            }
        }

        gameObject.SetActive(false);
    }
}