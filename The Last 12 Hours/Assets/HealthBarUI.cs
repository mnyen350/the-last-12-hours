using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Player player => Player.Instance;

    private Image[] hearts;

    [SerializeField]
    public Sprite fullHealthSprite;
    
    [SerializeField]
    public Sprite halfHealthSprite;
    
    [SerializeField] 
    public Sprite noHealthSprite;

    // Start is called before the first frame update
    void Start()
    {
        player.OnAttacked += UpdateHealth;
        UpdateHealth(null, 0);
    }

    void OnDestroy()
    {
        player.OnAttacked -= UpdateHealth;
    }

    void UpdateHealth(Entity e, int d)
    {
        hearts = new[]
{
            GameObject.Find("2")?.GetComponent<Image>(),
            GameObject.Find("4")?.GetComponent<Image>(),
            GameObject.Find("6")?.GetComponent<Image>(),
            GameObject.Find("8")?.GetComponent<Image>(),
            GameObject.Find("10")?.GetComponent<Image>(),
        };

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] == null)
                continue;

            int health = (i + 1) * 2;
            if (player.health >= health)
                hearts[i].sprite = fullHealthSprite;
            else if (player.health >= health - 1)
                hearts[i].sprite = halfHealthSprite;
            else
                hearts[i].sprite = noHealthSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
