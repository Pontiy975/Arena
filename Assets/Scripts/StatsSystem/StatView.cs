//using TMPro;
//using UnityEngine;

//namespace Arena.StatsSystem
//{
//    public class StatView : MonoBehaviour
//    {
//        [SerializeField] private TMP_Text statText;

//        public void SetValue(int stat, StatType type)
//        {
//            string tag = "";
//            switch (type)
//            {
//                case StatType.Health:
//                    tag = "<sprite name=\"icons_1\">";
//                    break;
//                case StatType.Damage:
//                    tag = "<sprite name=\"icons_0\">";
//                    break;
//            }

//            statText.text = $"{stat} {tag}";
//        }
//    }
//}
