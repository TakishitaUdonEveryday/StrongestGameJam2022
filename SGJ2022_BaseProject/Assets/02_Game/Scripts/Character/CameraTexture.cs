using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SGJ
{

	public class CameraTexture : MonoBehaviour
	{
		private Animator m_animator = null;

		private readonly int ANIM_PRM_ENTER = Animator.StringToHash("Enter");
		private readonly int ANIM_PRM_SHUTTER = Animator.StringToHash("Shutter");
		private readonly int ANIM_PRM_LEAVE = Animator.StringToHash("Leave");


		private static CameraTexture ms_instance = null;

		public enum Status
		{
			Outside,
			Enter,
			Hold,
			TakePicture,
			Leave,
		};

		private Status m_status = Status.Outside;


		public Status	GetStatus()
		{
			return m_status;
		}


		private void Awake()
		{
			ms_instance = this;
			m_animator = GetComponent<Animator>();
		}

		public static CameraTexture	Instance
		{
			get { return ms_instance; }
		}


		// Start is called before the first frame update
		void Start()
		{
		}



		/// <summary>
		/// ì¸èÍ 
		/// </summary>
		public void ActionEnter()
		{
			if (m_status == Status.Outside)
			{
				m_animator.SetTrigger(ANIM_PRM_ENTER);
				m_status = Status.Enter;
			}
		}

		public void ActionShutter()
		{
			if (m_status == Status.Hold)
			{
				m_animator.SetTrigger(ANIM_PRM_SHUTTER);
				m_status = Status.TakePicture;
			}
		}

		public void ActionLeave()
		{
			if (m_status == Status.Hold)
			{
				m_animator.SetTrigger(ANIM_PRM_LEAVE);
				m_status = Status.Leave;
			}
		}


		/////////////////////////////////////////////////////////////////////



		public void AnimEvent_Entered()
		{
			m_status = Status.Hold;
		}


		public void AnimEvent_Leaved()
		{
			m_status = Status.Outside;
		}

		public void AnimEvent_Shuttered()
		{
			m_status = Status.Leave;
		}

	}

}

