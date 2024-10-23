using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton

    public static PlayerManager Instance;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    #endregion

    public GameObject player;
}
