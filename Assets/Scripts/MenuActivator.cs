using UnityEngine;
using UnityEngine.UI;
using Vrs.Internal;

public class MenuActivator : MonoBehaviour
{
	[SerializeField] GameObject menu;
	[SerializeField] Image fillImage;
	private float timeToHold = 1.5f;
	private GameObject fillImageParent;
	private float holdTime = 0f;
	private float delayBeforeFill = 0.2f;
	private float delayTimer = 0f;
	private bool isDelayPassed = false;

	private bool buttonPressed = false;
	private float ipd;

	private void Start()
	{
		InitIPD();
		fillImage.fillAmount = 0;
		fillImageParent = fillImage.transform.parent.gameObject;
		fillImageParent.SetActive(false);
		//menu.SetActive(false); //Временно меняем стратегию, на очках не работает удержание кнопки Fire1 и нет кнопки Cancel
	}

	private void Update()
	{
        /*if (Input.GetButtonDown("Fire1"))
		{
			buttonPressed = true; // надо добавить проверку на то что курсор не смотрит на интерактивный объект

			delayTimer = 0f;
			isDelayPassed = false;
			return;
		}

		if (!buttonPressed) return;

		if (Input.GetButtonUp("Fire1"))
		{
			fillImageParent.SetActive(false);
			fillImage.fillAmount = 0;
			holdTime = 0;
			delayTimer = 0f;
			isDelayPassed = false;
			buttonPressed = false;
			return;
		}


		if (Input.GetButton("Fire1"))
		{
			delayTimer += Time.deltaTime;

			if (delayTimer >= delayBeforeFill && !isDelayPassed)
			{
				fillImageParent.SetActive(true);
				isDelayPassed = true;
			}

			if (isDelayPassed)
			{
				holdTime += Time.deltaTime;
				fillImage.fillAmount = holdTime / timeToHold;

				if (holdTime >= timeToHold)
				{
					if (menu.activeInHierarchy) CloseMenu();
					ActivateMenu();
					fillImageParent.SetActive(false);
					fillImage.fillAmount = 0;
					holdTime = 0;
				}
			}
		}*/
        if (Input.GetButton("Cancel"))
		{
            if (menu.activeInHierarchy) CloseMenu();
            ActivateMenu();
        }


    }

	public void ActivateMenu()
	{
		menu.transform.SetParent(null);
		menu.SetActive(true);
	}

	public void CloseMenu()
	{
		menu.transform.SetParent(transform);
		menu.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
		menu.SetActive(false);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	private void InitIPD()
	{
		if (PlayerPrefs.HasKey("IPD"))
		{
			ipd = PlayerPrefs.GetFloat("IPD");
		}
		else
		{
			ipd = 58;
		}
		ChangeIPD(0);
	}

	public void ChangeIPD(int value)
	{
		ipd = PlayerPrefs.GetFloat("IPD");
		if (ipd == 0)
		{
			ipd = 58;
		}
		if ((ipd + value > 70) || (ipd + value <= 50))
			return;
		ipd += value;
		PlayerPrefs.SetFloat("IPD", ipd);
		var dist = ipd / 1000.0f;
		VrsViewer.Instance.SetIpd(dist);
	}

}
