using UnityEngine;
using System.Collections;

namespace Script
{
    public class CoolingTankDoor : MonoBehaviour
    {
        [Header("旋转设置")] [SerializeField] private Transform doorPivot; // 门轴父物体
        [SerializeField] private Vector3 rotationAxis = Vector3.up; // 沿Y轴旋转
        [SerializeField] private float rotateAngle = -90f; // 旋转角度
        [SerializeField] private float duration = 0.8f; // 开门动画耗时

        private bool isOpened = false;
        private Quaternion targetRotation;

        void Start()
        {
            if (doorPivot == null && transform.parent != null)
            {
                doorPivot = transform.parent;
            }

            if (doorPivot != null)
            {
                targetRotation = doorPivot.localRotation;
            }
        }

        /// <summary>
        /// 核心公开方法：由降温槽脚本远程调用，玩家按F无效
        /// </summary>
        public void OpenDoorByTank()
        {
            // 确保只会被开启一次
            if (isOpened || doorPivot == null) return;

            isOpened = true;

            // 计算目标旋转角度
            targetRotation = targetRotation * Quaternion.AngleAxis(rotateAngle, rotationAxis);

            // 播放平滑开门动画
            StartCoroutine(AnimateOpen());
        }

        private IEnumerator AnimateOpen()
        {
            float elapsed = 0f;
            Quaternion startRotation = doorPivot.localRotation;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                t = Mathf.SmoothStep(0f, 1f, t); // 平滑减速起停

                doorPivot.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);
                yield return null;
            }

            doorPivot.localRotation = targetRotation;
            Debug.Log("特殊联动门已完全打开。");
        }
    }
}