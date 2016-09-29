using UnityEngine;
using System.Collections.Generic;

public class BulletsPool
{
    private List<Bullet> _bullets = null;

    public BulletsPool()
    {
        _bullets = new List<Bullet>();
    }

    private Bullet CreateNewBullet()
    {
        Object loadedObj = Resources.Load("Prefabs/Bullet");
        GameObject instBulletObj = Object.Instantiate(loadedObj) as GameObject;

        return instBulletObj.GetComponent<Bullet>();
    }

    public Bullet Get()
    {
        if (_bullets.Count > 0)
        {
            Bullet b = _bullets[0];
            b.Use();

            _bullets.RemoveAt(0);

            return b;
        }
        else
        {
            Bullet newBullet = CreateNewBullet();
            newBullet.Use();

            return newBullet;
        }
    }

    public void Return(Bullet bullet)
    {
        bullet.Reset();
        _bullets.Add(bullet);
    }
}