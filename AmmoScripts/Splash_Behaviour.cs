using UnityEngine;

public class Splash_Behaviour : MonoBehaviour
{
    public GameObject waterTrail;
    WaterTrail_Behaviour trail;

   public void Splash(Vector2 position)
    {
        gameObject.transform.position = position;
    }

    private void Update()
    {
        if (Time.timeScale != 0)
        {
            if (gameObject.GetComponent<SpriteRenderer>().sprite.name == "WaterSplash_11")
            {
                trail = Instantiate(waterTrail).GetComponent<WaterTrail_Behaviour>();
                trail.CreatePuddle(gameObject.transform.position, 1, 1, gameObject.transform.localScale.x * 3.8f);
                Destroy(gameObject);
            }
        }
    }
}
