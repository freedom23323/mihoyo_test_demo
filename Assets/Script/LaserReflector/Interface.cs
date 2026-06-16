using UnityEngine;
public interface IDestructible
{
    void OnLaserHit(Vector3 hitPoint);
}
public interface IHittable
{
    void OnLaserHit(Vector3 hitPoint);
}