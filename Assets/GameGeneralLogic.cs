using UnityEngine;

public class GameGeneralLogic : MonoBehaviour
{
    public static GameGeneralLogic instance;
    public int coin = 0;
    private int[] upgradeCost = { 30, 60, 12000 };
    public ProjectileShooter projectileShooter;
    public int shooterLevel = 1;
    private GameObject playerArrow;

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

    private void Start()
    {
        UIScript.instance.UpdateText(coin + " / " + upgradeCost[shooterLevel - 1]);
        UIScript.instance.UpdateSliderValue(coin, upgradeCost[shooterLevel - 1]);
        UIScript.instance.UpdateHealthSliderValue(Player.instance.maxHealth, Player.instance.maxHealth);
        playerArrow = GameObject.Find("arrowPlayer").gameObject;
    }

    private void Update()
    {
        if (playerArrow != null && Player.instance != null)
        {
            // Follow position
            Vector3 newPosition = Player.instance.transform.position;
            playerArrow.transform.position = newPosition;

            // Follow Y rotation with 90 degree offset
            Vector3 currentRotation = playerArrow.transform.eulerAngles;
            playerArrow.transform.rotation = Quaternion.Euler(currentRotation.x, Player.instance.transform.eulerAngles.y + 90f, currentRotation.z);
        }
    }

    public void AddCoin(int amount)
    {
        coin += amount;
        if (shooterLevel < 3)
        {
            if (coin >= upgradeCost[shooterLevel - 1])
            {
                Upgrade();
            }
        }
        UIScript.instance.UpdateText(coin + " / " + upgradeCost[shooterLevel - 1]);
        UIScript.instance.UpdateSliderValue(coin, upgradeCost[shooterLevel - 1]);
    }

    public void Upgrade()
    {
        if (shooterLevel < 3)
        {
            shooterLevel++;
            Debug.Log("P" + shooterLevel);
            UIScript.instance.ShowLevelUpText();
        }
    }
    public void Downgrade()
    {
        if (shooterLevel > 1)
        {
            shooterLevel--;
        }
    }
}