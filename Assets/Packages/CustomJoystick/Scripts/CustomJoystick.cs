using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomJoystick : MonoBehaviour
{
    public static CustomJoystick Instance;

    private enum TouchPointMode
    {
        Static,
        Dynamic
    }

    [SerializeField] private Image backImage;
    [SerializeField] private Image middleImage;

    [SerializeField] private RectTransform middle;

    [SerializeField] private TouchPointMode touchPointModeMode = TouchPointMode.Static;
    //[SerializeField] private ScreenManager screenManager;

    private RectTransform _rect;

    private Vector3 _touchPosition;
    private bool _isTouched = false;

    private Vector3 _directionInPixels;
    private Vector3 _direction;

    private float _directionMagnitude;

    private RectTransform _canvasRect;

    protected static readonly Quaternion CAMERA_ROTATION_FIX = Quaternion.Euler(0f, 30f, 0f);

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _rect = GetComponent<RectTransform>();
        _canvasRect = transform.parent.GetComponent<RectTransform>();
    }

    private void ShowJoy()
    {
        backImage.gameObject.SetActive(true);
        middleImage.gameObject.SetActive(true);

        StartCoroutine(FadeImage(backImage, new Color(1f, 1f, 1f, 0.3f), 0.2f));
        StartCoroutine(FadeImage(middleImage, new Color(1f, 1f, 1f, 0.3f), 0.2f));
    }

    private void HideJoy()
    {
        StartCoroutine(FadeImage(backImage, new Color(1f, 1f, 1f, 0f), 0.2f));
        StartCoroutine(FadeImage(middleImage, new Color(1f, 1f, 1f, 0f), 0.2f));
    }

    private IEnumerator FadeImage(Image image, Color targetColor, float duration)
    {
        Color startColor = image.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            image.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        image.color = targetColor;

        if (targetColor.a == 0f)
        {
            image.gameObject.SetActive(false);
        }
    }

    private bool IsCanStartTouch()
    {
#if UNITY_EDITOR
        return Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject();
#else
        return Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId);
#endif
    }
    
    private bool IsCanContinueTouch()
    {
#if UNITY_EDITOR
        return Input.GetMouseButton(0);
#else
        return Input.touchCount > 0;
#endif
    }

    void Update()
    {
        //if (screenManager.CurrentScreen is not GameScreen)
        //{
        //    if (_isTouched)
        //        EndTouch();
            
        //    return;
        //}

        if(!_isTouched && IsCanStartTouch())
            StartTouch();

        if (_isTouched && !IsCanContinueTouch())
        {
            EndTouch();
            return;
        }
        
        _directionInPixels = Input.mousePosition - _touchPosition;
        _direction = _directionInPixels * (1536f / Screen.width);

        _directionMagnitude = _direction.magnitude;

        if (_directionMagnitude > 180f)
        {
            if (touchPointModeMode == TouchPointMode.Dynamic && !Application.isEditor)
            {
                _touchPosition += _direction - _direction.normalized * 180f;

                RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, _touchPosition, null,
                    out Vector2 screenPos);
                _rect.anchoredPosition = screenPos;
            }

            _direction *= 180f / _directionMagnitude;
        }
        
        middle.anchoredPosition = _direction;
        
        _direction /= 180f;
        _directionMagnitude = _direction.magnitude;
    }

    private void StartTouch()
    {
        if(_isTouched)
            return;
        
        ShowJoy();

        _isTouched = true;
        _touchPosition = Input.mousePosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, _touchPosition, null, out Vector2 screenPos);
        _rect.anchoredPosition = screenPos;
    }

    private void EndTouch()
    {
        if(!_isTouched)
            return;
        
        HideJoy();
        _isTouched = false;
        _directionMagnitude = 0;
    }

    public bool HasInput => _isTouched && DirectionXZ != Vector3.zero;

    public bool IsTouched => _isTouched;

    public Vector3 DirectionXZ => new Vector3(_direction.x, 0f, _direction.y);
    public float DirectionMagnitude => _directionMagnitude;
}
