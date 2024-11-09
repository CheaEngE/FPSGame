using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    public float hp = 10;
    public IHealthListener healthListener;
    public float invincibleTime;
    public AudioClip hitSound;
    public AudioClip dieSound;

    public Image hpGauge;

    float maxHp;
    float lastAttackedTime;

    // Start is called before the first frame update
    void Start()
    {
        maxHp = hp;
        healthListener = GetComponent<Health.IHealthListener>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float damage)
    {
        if (hp > 0 && lastAttackedTime + invincibleTime < Time.time)
        {
            hp -= damage;
            if (hpGauge != null)
            {
                hpGauge.fillAmount = hp / maxHp;
            }
            
            lastAttackedTime = Time .time;

            if (hp <= 0)
            {
                if (dieSound != null)
                {
                    GetComponent<AudioSource>().PlayOneShot(dieSound);
                }
                if(healthListener != null)
                {
                    healthListener.Die();
                }
                else
                {
                    if (hitSound != null)
                    {
                        GetComponent<AudioSource>().PlayOneShot(hitSound);
                    }
                    
                }
            }
        }
    }

    public interface IHealthListener
    {
        void Die();
    }
}
