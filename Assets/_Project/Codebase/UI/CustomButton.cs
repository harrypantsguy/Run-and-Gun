using System.Collections.Generic;
using NaughtyAttributes;
using UltEvents;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CHR.UI
{
    public class CustomButton : MonoBehaviour, IGraphicTarget
    {
        public UnityAction onClick;
        [Label("On Click")] public UltEvent inspectorOnClickEvent;

        public UnityAction onHover;
        [Label("On Hover")] public UltEvent onHoverEvent;
        
        [field: SerializeField, Required] public RectTransform RectTransform { get; private set; }
        [field: SerializeField] public GraphicRaycaster Raycaster { get; set; }
        [field: SerializeField] public List<GameObject> GraphicsTargets { get; set; }

        public bool Interactable => m_interactable;

        [OnValueChanged("UpdateColorOnInteractableStateChange")] [SerializeField]
        private bool m_interactable = true;

        protected ButtonGraphicsController graphicsController;

        public bool PointerInside { get; protected set; }
        public bool PressStarted { get; protected set; }
        
        private const string c_functionality_options = "Functionality Settings";

        protected virtual void Awake()
        {
            onClick ??= () => { };
            onHover ??= () => { };

            graphicsController = GetComponent<ButtonGraphicsController>();
            graphicsController.LinkButton(this);
        }
        
        protected virtual void Start() { }
        protected virtual void OnDestroy() { }

        protected virtual void Update()
        {
            bool mouseInside = RectTransformUtility.RectangleContainsScreenPoint(RectTransform, Input.mousePosition);

            bool mouseOnGraphic = GraphicTargetUtilities.IsGraphicInVision(Raycaster, GraphicsTargets, Input.mousePosition);
            
            if (mouseInside && mouseOnGraphic && !PointerInside)
            {
                PointerInside = true;
                OnPointerEnter();
            }
            else if ((!mouseInside || !mouseOnGraphic) && PointerInside)
            {
                PointerInside = false;
                OnPointerExit();
            }

            if (PointerInside && Input.GetKeyDown(KeyCode.Mouse0) && mouseOnGraphic)
            {
                OnPointerDown();
            }
            else if (PressStarted && Input.GetKeyUp(KeyCode.Mouse0))
            {
                OnPointerUp();
            }
        }

        private void UpdateColorOnInteractableStateChange()
        {
            graphicsController.OnInteractableStateChange(m_interactable);
        }

        public void SetEnabledState(bool state)
        {
            m_interactable = state;
            PressStarted = false;
            UpdateColorOnInteractableStateChange();
        }

        protected virtual void OnDisable()
        {
            PressStarted = false;
            PointerInside = false;
            if (graphicsController != null) 
                graphicsController.OnButtonDisable();
        }
        protected virtual void OnEnable()
        {
            if (graphicsController != null) 
                graphicsController.OnButtonEnable();
        }

        public virtual void OnPointerEnter()
        {
            if (!Interactable) return;

            onHoverEvent.Invoke();
            onHover.Invoke();
            
            if (!PressStarted && graphicsController != null)
                graphicsController.OnStartHover();
        }
        
        public virtual void OnPointerExit()
        {
            if (!Interactable) return;

            if (!PressStarted && graphicsController != null)
                graphicsController.OnEndHover();
        }

        public virtual void OnPointerDown()
        {
            if (!Interactable) return;
            
            PressStarted = true;
            
            if (graphicsController != null)
                graphicsController.OnPress();
        }

        public virtual void OnPointerUp()
        {
            if (!Interactable) return;
            
            PressStarted = false;
            
            if (PointerInside)
            {
                onClick.Invoke();
                inspectorOnClickEvent.Invoke();
            }

            if (graphicsController != null) 
                graphicsController.OnRelease();
        }

        GraphicRaycaster IGraphicTarget.Raycaster
        {
            get => Raycaster;
            set => Raycaster = value;
        }
    }
}