using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Arcade;
namespace SG
{
    [RequireComponent(typeof(LoopScrollRect))]
    [DisallowMultipleComponent]
    public class InitOnStart : MonoBehaviour
    {
        public int totalCount = -1;
        void Start()
        {
            LoopScrollRect ls = GetComponent<LoopScrollRect>();
            ls.totalCount = 0;
            ls.RefillCells();
        }
    }
}