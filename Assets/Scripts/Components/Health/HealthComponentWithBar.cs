//using Arena.UI.Health;
//using Arena.UI.Screens;
//using UISystem.Screens;
//using UnityEngine;

//namespace Arena.Components.Health
//{
//    public class HealthComponentWithBar : HealthComponent
//    {
//        [SerializeField] private bool visibleWhenFull;
//        [SerializeField] private bool isFollower;
//        [SerializeField] private AttachedHealthBar healthBarPrefab;
//        [SerializeField] private ScreenManager screenManager;
        
//        private AttachedHealthBar _healthBar;

//        public bool IsInitialized { get; private set; }

//        public override void Init(int maxHealth)
//        {
//            base.Init(maxHealth);

//            if (_healthBar == null)
//            {
//                Transform parent = screenManager.GetScreen<GameScreen>().WorldComponents;
//                _healthBar = Instantiate(healthBarPrefab, parent);
//                _healthBar.name += $" {name}";
//            }

//            _healthBar.Init(transform);

//            if (visibleWhenFull) ShowBar();
//            else HideBar();

//            UpdateBar();

//            IsInitialized = true;
//        }

//        public void SetHPBarParams(Vector3 offset)
//        {
//            _healthBar?.SetParams(offset);
//        }

//        private void LateUpdate()
//        {
//            if (isFollower)
//                _healthBar?.UpdateMovement();
//        }

//        public void ShowBar()
//        {
//            _healthBar?.gameObject.SetActive(true);
//        }

//        public void HideBar()
//        {
//            _healthBar?.gameObject.SetActive(false);
//        }

//        public override void ChangeHealth(int delta)
//        {
//            base.ChangeHealth(delta);

//            if (IsDead)
//            {
//                _healthBar?.gameObject.SetActive(false);
//                IsInitialized = false;
//            }
//            else
//                UpdateBar();
//        }

//        public override void SetMaxHealth(int maxHealth)
//        {
//            base.SetMaxHealth(maxHealth);
//            UpdateBar();
//        }

//        private void UpdateBar()
//        {
//            float fillAmount = Health / MaxHealth;

//            _healthBar.UpdateBar(fillAmount);

//            if (visibleWhenFull)
//                return;

//            if (Health < MaxHealth) ShowBar();
//            else HideBar();
//        }
//    }
//}