using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG2 {
	public class InputHandler : MonoBehaviour {
		#region VAR
		float vertical;
		float horizontal;
		bool b_input;
		bool x_input;
		bool a_input;
		bool y_input;
		bool rt_input;
		bool rb_input;
		bool lb_input;
		bool lt_input;
		float rt_axis;
		float lt_axis;
		float b_timer;
		float delta;
		public StateManager state;
		public CameraManager cameraManager;
		public GamePhase currentPhase;
		Transform camPos;
		#endregion
		#region INIT
		void Start() {
			InitializeInGame();
		}

		public void InitializeInGame() {
			state.Initialize();
			cameraManager.Initialize(state);
			camPos = cameraManager.mTransform;
		}
		#endregion
		#region FIXEDUPDATE

		private void FixedUpdate() {
			delta += Time.deltaTime;
			GetInput_FixedUpdate();
			switch (currentPhase) {
				case GamePhase.inGame:
					InGameStates_FixedUpdate();
					state.Fixed_Tick(delta);
					cameraManager.Tick(delta);
					break;
				case GamePhase.inMenu:
					break;
				case GamePhase.inInventory:
					break;
				default:
					break;
			}
		}

		void GetInput_FixedUpdate() {
			vertical = Input.GetAxis(StaticStrings.Vertical);
			horizontal = Input.GetAxis(StaticStrings.Horizontal);
		}

		void InGameStates_FixedUpdate() {
			state.input.vertical = vertical;
			state.input.horizontal = horizontal;
			state.input.mCount = Mathf.Clamp01((Mathf.Abs(vertical) + Mathf.Abs(horizontal)));

			Vector3 moveDir = camPos.forward * vertical;
			moveDir += camPos.right * horizontal;
			moveDir.Normalize();
			state.input.moveDir = moveDir;
		}
		#endregion
		#region UPDATE
		private void Update() {
			delta += Time.deltaTime;
			GetInput_Update();
			switch (currentPhase) {
				case GamePhase.inGame:
					InGameState_Update();
					state.Tick(delta);
					break;
				case GamePhase.inMenu:
					break;
				case GamePhase.inInventory:
					break;
				default:
					break;
			}
		}

		void GetInput_Update() {
			b_input = Input.GetButton(StaticStrings.B);
			a_input = Input.GetButton(StaticStrings.A);
			y_input = Input.GetButton(StaticStrings.Y);
			x_input = Input.GetButton(StaticStrings.X);
			rt_input = Input.GetButton(StaticStrings.RT);
			lt_input = Input.GetButton(StaticStrings.LT);
			rt_axis = Input.GetAxis(StaticStrings.RT);
			if (rt_axis != 0)
				rt_input = true;
			lt_axis = Input.GetAxis(StaticStrings.LT);
			if (lt_axis != 0)
				lt_input = true;
			rb_input = Input.GetButton(StaticStrings.RB);
			lb_input = Input.GetButton(StaticStrings.LB);
			if (b_input)
				b_timer += delta;
		}

		void InGameState_Update() {
			state.input.rb = rb_input;
			state.input.rt = rt_input;
			state.input.lb = lb_input;
			state.input.lt = lt_input;
		}
		#endregion
	}
	public enum InputType {
		rt,
		lt,
		rb,
		lb
	}
	public enum GamePhase {
		inGame,
		inMenu,
		inInventory
	}
}
