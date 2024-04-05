using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MultipleCamerasController : Singleton<MultipleCamerasController>
{
    [SerializeField] private CameraUI _cameraUIPrefab;

    private List<CameraForServer> cams = new List<CameraForServer>();

    private List<CameraUI> _uiObjects = new List<CameraUI>();

    [SerializeField] private CameraManager _cameraManager;


    private void Start()
    {
        cams = _cameraManager.Cams;
        CalculateCamsRect();
    }

    private void OnEnable()
    {
        _cameraManager.OnCamsCountChanged += CalculateCamsRect;
    }

    private void OnDisable()
    {
        _cameraManager.OnCamsCountChanged -= CalculateCamsRect;
    }

    public void CalculateCamsRect()
    {
        print("calc cams rect");
        CreateButtons();
        
        int rows = Mathf.RoundToInt(Mathf.Sqrt(cams.Count));
        int columns = Mathf.CeilToInt((float)cams.Count / rows);
        float cellWidth = 1.0f / columns;
        float cellHeight = ServerUICanvas.Instance.MultipleCamsCanvas.GetComponent<RectTransform>().rect.height / Screen.height / rows;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int index = (row * columns) + col;
                if(index>=cams.Count)
                    continue;
                float x = col * cellWidth;
                float y = row * cellHeight;
                float width = cellWidth;
                float height = cellHeight;

                var camera = cams[index].GetComponent<Camera>();
                camera.rect = new Rect(x,y,width,height);
                camera.gameObject.SetActive(true);
            }
        }
    }

    private void CreateButtons()
    {
        _uiObjects.ForEach((obj => Destroy(obj.gameObject)));
        _uiObjects.Clear();
        
        int rows = Mathf.RoundToInt(Mathf.Sqrt(cams.Count));
        int columns = Mathf.CeilToInt((float)cams.Count / rows);
        float cellWidth = ServerUICanvas.Instance.MultipleCamsCanvas.GetComponent<RectTransform>().rect.width / columns;
        float cellHeight = ServerUICanvas.Instance.MultipleCamsCanvas.GetComponent<RectTransform>().rect.height / rows;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int index = (row * columns) + col;
                if(index>=cams.Count)
                    continue;
                float x = col * cellWidth;
                float y = row * cellHeight;
                float width = cellWidth;
                float height = cellHeight;

                CameraUI cameraUIObj = Instantiate(_cameraUIPrefab,ServerUICanvas.Instance.MultipleCamsCanvas.transform);
                
                var uiRect = cameraUIObj.GetComponent<RectTransform>();
                uiRect.pivot = Vector2.zero;
                uiRect.anchorMin = Vector2.zero;
                uiRect.anchorMax = Vector2.zero;
                uiRect.anchoredPosition = new Vector3(x, y, 0);
                uiRect.sizeDelta = new Vector2(width, height);
                uiRect.localScale = Vector3.one;

                // Добавляем обработчик события на нажатие кнопки
                cameraUIObj.AddButtonListener(() => ClickOnCamera(cams[index].GetComponent<Camera>()));
                cameraUIObj.SetName(_cameraManager.GetStudentByCam(cams[index]).Name);
                
                _uiObjects.Add(cameraUIObj);
            }
        }
    }

    private void ClickOnCamera(Camera camera)
    {
        cams.ForEach((cam => cam.gameObject.SetActive(false)));
        camera.gameObject.SetActive(true);
        camera.rect = new Rect(0, 0, 1, 1);
        ServerUICanvas.Instance.ShowMultipleButton();
    }
}
