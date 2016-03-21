using System.Collections;
using Assets.Script.Menu.DataManagment;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.DataManagment
{
	public class LoadingManager : MonoBehaviour
	{
		#region Variables
		public Image TransactionImage;
		public Color TargetColor = Color.white;
		public GameObject LoadingContainer;
		private bool _isLoadingEnable;
		public float ColorStep = 0.1f;
		public float TimeOut = 1.0f;
		public delegate void TransactionAction();
		public TransactionAction TranactionInEnd;
		public  TransactionAction TranactionOutEnd;

		#endregion
		public void Awake()
		{
			if (SceneManager.CurrentLoadingManager == null)
			{
				SceneManager.PreloadGame();
				SceneManager.CurrentLoadingManager = this;
				Application.targetFrameRate = 120;
				TransactionImage.gameObject.SetActive(true);
				Invoke("StartTransactionIn",0.05f);
			}
			else
			{
				Destroy(gameObject);
			}
		}


		public void StartTransactionIn()
		{
			StopAllCoroutines();
			TransactionImage.color = TargetColor;
			_isClean = false;
			TransactionImage.gameObject.SetActive(true);
			StartCoroutine(TransactionIn());
		}

		public void StartTransactionOut()
		{
			StartTransactionOut(true);
		}

		public void StartTransactionOut(bool isLoading)
		{
			_isLoadingEnable = isLoading;
			StopAllCoroutines();
			TransactionImage.color = Color.clear;
			_isTarget = false;
			TransactionImage.gameObject.SetActive(true);
			StartCoroutine(TransactionOut());
		}

		private bool _isTarget;
		private bool _isClean;
		private IEnumerator TransactionOut()
		{
			yield return new WaitForEndOfFrame();
			while (true)
			{
				TransactionImage.color = new Color(TargetColor.r, TargetColor.g, TargetColor.b, TransactionImage.color.a + ColorStep);
				if (TransactionImage.color.a >= TargetColor.a - 0.1f && !_isTarget)
				{
					_isTarget = true;
					TransactionImage.color = TargetColor;
					if (_isLoadingEnable)
						LoadingContainer.gameObject.SetActive(true);
					yield return new WaitForEndOfFrame();
				}
				if (_isTarget)
				{
					if (TranactionOutEnd != null)
					{
						TranactionOutEnd();
						TranactionOutEnd = null;
					}
					yield break;
				}
				yield return new WaitForEndOfFrame();
			}
		}

		private IEnumerator TransactionIn()
		{
			LoadingContainer.gameObject.SetActive(false);
			while (true)
			{
				TransactionImage.color = new Color(TargetColor.r, TargetColor.g, TargetColor.b, TransactionImage.color.a - ColorStep);
				if (TransactionImage.color.a < 0.1f && !_isClean)
				{
					_isClean = true;
					TransactionImage.gameObject.SetActive(false);
					yield return new WaitForEndOfFrame();
				}
				if (_isClean)
				{
					if (TranactionInEnd != null)
					{
						TranactionInEnd();
						TranactionInEnd = null;
					}
					yield break;
				}
				yield return new WaitForEndOfFrame();
			}
		}

	}
}