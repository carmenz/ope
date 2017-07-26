using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FerryController : MonoBehaviour {

	[SerializeField]
	GameObject player;
	[SerializeField]
	GameObject score;
	[SerializeField]
	GameObject popup;
	[SerializeField]
	GameObject ticketsPanel;
	[SerializeField]
	GameObject unClickableTicketsPanel;
	[SerializeField]
	Camera playerCamera;
	[SerializeField]
	List<GameObject> uis;
	GameManager gm;

	float ZOOM_OUT_ANGLE = 100f;
	float ZOOM_IN_ANGLE = 59f;
	int TICKET_PRICE = 100;
	float ANIM_DURATION = 3f;

	Animator _animator;
	bool _curValue = false;
	NewPlayer _playerController;

	void Awake () {
		_animator = gameObject.GetComponent<Animator>();
		_animator.SetBool("move", _curValue);
		_playerController = player.GetComponent<NewPlayer>();
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	public void Toggle() {
		_curValue = !_curValue;
		_animator.SetBool("move", _curValue);
	}

	public void CloseTicketsPanel() {
		ticketsPanel.SetActive(false);
	}

	public void OpenTicketsPanel() {
		AudioSource audio = GameObject.Find("AudioFerry").GetComponent<AudioSource>();
		audio.Play();
		score.GetComponent<Text>().text = _playerController.GetTotalScore().ToString();
		if (gm.currentIsland == "A") {
			ticketsPanel.SetActive(true);
		} else {
			unClickableTicketsPanel.SetActive(true);
		}
	}

	public void GetTicket() {
		var total = _playerController.GetTotalScore();
		if (total >= TICKET_PRICE) {
			_playerController.AddTotalScore(-TICKET_PRICE);
			popup.SetActive(true);
		} else {
			Debug.Log("points not enough");
		}
	}

	public void Comfirmed() {
		CloseTicketsPanel();
		var ferry = GameObject.Find("Ferry");
		// move character to the ferry
		player.transform.DOMove(new Vector3(-479.2394f, 48f, 0f), ANIM_DURATION).OnComplete(() => {
			player.transform.parent = ferry.transform;
			player.GetComponent<SpriteRenderer>().enabled = false;

			// Zoom out camera view
			playerCamera.GetComponent<Camera>().DOFieldOfView(ZOOM_OUT_ANGLE, ANIM_DURATION);
			
			var seq = DOTween.Sequence ();  
			// disable score, mission list button
			HideUIs();
			// move ferry 
			seq.Append(ferry.GetComponent<DOTweenPath>().tween)
				.OnComplete(() => {
					// move character to the target island
					player.GetComponent<SpriteRenderer>().enabled = true;
					player.transform.parent = null;
					// Zoom in camera view
					playerCamera.GetComponent<Camera>().DOFieldOfView(ZOOM_IN_ANGLE, ANIM_DURATION);
					player.transform.DOMove(new Vector3(830f, -30f, 0f), ANIM_DURATION).OnComplete(() => {
						ShowUIs();
						_playerController.SaveCurrentIsland("B");
						_playerController.SetUpMiniMap();
						_playerController.SetUpMissionList();
						_playerController.SaveCoordinate();
						_playerController.PlayBGM();
					});
				});
		});
	}

	public void NotComfirmed() {
		popup.SetActive(false);
	}

	void ShowUIs() {
		for (var i=0;i<uis.Count;i++) {
			uis[i].SetActive(true);
		}
	}

	void HideUIs() {
		for (var i=0;i<uis.Count;i++) {
			uis[i].SetActive(false);
		}
	}
}
