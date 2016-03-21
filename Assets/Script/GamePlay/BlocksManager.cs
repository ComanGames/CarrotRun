using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.ObjectPool;
using UnityEngine;
using Random = System.Random;

namespace Assets.Script.GamePlay
{
	public class BlocksManager : MonoBehaviour
	{
		#region Variables
		private LinkedList<GameObject> _movableBlocks;
		private List<GameObject> _movableProblems;
		private readonly Random _randome = new Random();
		public bool MoveGround = true;
		public float GroundMovingScale = 0.5f;
		public Transform StartPosition;
		public Transform EndPosition;
		public Transform GroundPool;
		public Vector3 DistanceBetweenBlocks;
		public Vector3 MovingDirection;
		private Vector3 _nextPosition = Vector3.zero;
		public BlockPrefabs Gp; //Prefabs Collection
		public int MinRandomNextProblme;
		public int MaxRandomNextProblem;
		private int _orderOfPlank;
		public int MaxOrderValue = 10000;
		public float Speed;
		private int _reSpawnCount;

		[Serializable]
		public class BlockPrefabs
		{
			#region  Variables

			public GameObject GroundPrefab;
			public int CountOfGround;
			public List<Level> Blocks;
			public int N;

			#endregion
		}
		#endregion

		public void UpdateSpeed(float speed)
		{
			Speed = speed;
			_nextPosition = MovingDirection * speed * -1;
		}
		public void Creator()
		{
			_movableBlocks = new LinkedList<GameObject>();
			_movableProblems = new List<GameObject>();
			SettingStartVeriables();
			CreateGroundPrefabs();
			StartCoroutine(ProblemCreator());
			_isPause = false;
		}

		private void SettingStartVeriables()
		{
			UpdateSpeed(GameManager.Instance.SpeedSetting.StartSpeed);
		}

		private void CreateGroundPrefabs()
		{
			var planks = new GameObject[Gp.CountOfGround];
			for (var i = 0; i < planks.Length; i++)
			{
				var position = ((DistanceBetweenBlocks) * (i + 1)) + EndPosition.position;
				var plank = Instantiate(Gp.GroundPrefab, position, Quaternion.identity) as GameObject;
				if (plank != null)
				{
					plank.transform.position = position;
					plank.transform.parent = GroundPool.transform;
					planks[i] = plank;
				}
			}
			foreach (GameObject t in planks)
				_movableBlocks.AddLast(t);
			ResortGround();
		}

		public void AddNewProblem()
		{
			if (Gp.Blocks.Count > Gp.N + 1)
				Gp.N++; //Making over problems harder	
		}

		private IEnumerator ProblemCreator()
		{
			CreateProblem();
			// ReSharper disable once MergeSequentialChecks
			while (true)
			{
				if (Gp.Blocks == null || Gp.Blocks[Gp.N] == null || Gp.Blocks[Gp.N].Count == 0)
					yield return new WaitForFixedUpdate();
				float time = (_randome.Next(MinRandomNextProblme, MaxRandomNextProblem) / (Time.deltaTime * 100) / Speed);
				yield return new WaitForSeconds(time);
				if (!_isPause)
					CreateProblem();
			}
			// ReSharper disable once FunctionNeverReturns
		}

		private void CreateProblem()
		{
			var problem = Gp.Blocks[Gp.N][_randome.Next(0, Gp.Blocks[Gp.N].Count)].Spawn();
			problem.transform.position = StartPosition.position;
			_movableProblems.Add(problem);
		}

		private void UpdateMovableBlocks()
		{
			var toRemove = new List<GameObject>();
			foreach (var o in _movableProblems)
			{
				o.transform.Translate(_nextPosition * Time.deltaTime);
				if (o.transform.position.x < EndPosition.position.x)
				{
					o.Recycle();
					toRemove.Add(o);
				}
			}
			foreach (var o in toRemove)
				_movableProblems.Remove(o);
		}

		private void UpdateGroundBlocks()
		{
			if (_movableBlocks == null || _movableBlocks.Count < 1)
				return;
			_reSpawnCount = 0;
			for (var listNode = _movableBlocks.First; listNode != null; listNode = listNode.Next)
			{
				GameObject value = listNode.Value;
				value.transform.Translate(_nextPosition * Time.deltaTime * GroundMovingScale);
				if (value.transform.position.x < EndPosition.position.x)
					_reSpawnCount++;
			}
		}

		public void FixedUpdate()
		{

			for (int i = 1; i < _reSpawnCount; i++)
				ReSpawnGround();
			_reSpawnCount = 0;
		}

		public void ReSpawnGround()
		{
			var groundBlock = _movableBlocks.First.Value;
			groundBlock.transform.position = _movableBlocks.Last.Value.transform.position + DistanceBetweenBlocks;
			groundBlock.GetComponentInChildren<SpriteRenderer>().sortingOrder = _orderOfPlank++;
			_movableBlocks.RemoveFirst();
			_movableBlocks.AddLast(groundBlock);
			if (_orderOfPlank > MaxOrderValue)
				ResortGround();
		}

		private void ResortGround()
		{
			_orderOfPlank = 0;
			foreach (GameObject movableBlock in _movableBlocks)
			{
				if (movableBlock != null)
					movableBlock.GetComponentInChildren<SpriteRenderer>().sortingOrder = _orderOfPlank++;
			}
		}

		public void ReSpawnBorder(GameObject item)
		{
			item.Recycle();
			_movableProblems.Remove(item);
		}

		public void BlocksUpdate()
		{
			if (MoveGround)
				UpdateGroundBlocks();


//                UpdateMovableBlocks();
		}

		public void RemoveCoin(GameObject item)
		{
			_movableProblems.Remove(item);
		}

		public void Reset()
		{
			SettingStartVeriables();
			StopAllCoroutines();
			ResortGround();
			StartCoroutine(ProblemCreator());
			ResumeMobebleBlocksAnimations();
		}

		public void RemoveMovebleProblems()
		{
			foreach (GameObject movableProblem in _movableProblems)
				movableProblem.Recycle();
			_movableProblems = new List<GameObject>();
		}

		public void StopAllAnimations()
		{
			if (_movableBlocks != null && _movableProblems != null)
			{
				foreach (GameObject movableBlock in _movableBlocks)
				{
					SAnimation.SAnimation componentInChildren = movableBlock.GetComponentInChildren<SAnimation.SAnimation>();
					componentInChildren?.StopAnimation();
				}
				foreach (GameObject movableProblem in _movableProblems)
				{
					SAnimation.SAnimation componentInChildren = movableProblem.GetComponentInChildren<SAnimation.SAnimation>();
					componentInChildren?.StopAnimation();
				}
			}
		}

		public void ResumeAllAnimations()
		{
			if (_movableBlocks != null && _movableProblems != null)
			{
				ResumeMobebleBlocksAnimations();
				foreach (GameObject movableProblem in _movableProblems)
				{
					SAnimation.SAnimation componentInChildren = movableProblem.GetComponentInChildren<SAnimation.SAnimation>();
					componentInChildren?.StartAnimation();
				}
			}
		}

		private void ResumeMobebleBlocksAnimations()
		{
			foreach (GameObject movableBlock in _movableBlocks)
			{
				SAnimation.SAnimation componentInChildren = movableBlock.GetComponentInChildren<SAnimation.SAnimation>();
				componentInChildren?.StartAnimation();
			}
		}

		public void Stop()
		{
			Speed = 0;
			Gp.N = 0;
			RemoveMovebleProblems();
			StopAllCoroutines();
			StopAllAnimations();
		}

		private float _lastSpeed;
		private bool _isPause;
		public void Pause()
		{
			_isPause = true;
			_lastSpeed = Speed;
			UpdateSpeed(0);
		}

		public void UnPause()
		{
			_isPause = false;
			UpdateSpeed(_lastSpeed);
		}

	}
}