using UnityEngine;
using UnityEngine.Events;

namespace _Project.Codebase.UI
{
    public class ToggleButton : CustomButton
    {
        public bool Toggled { get; protected set; }
        [SerializeField] protected bool allowUntoggle;

        public UnityAction onUntoggle;
        public UnityAction<bool> onToggleOrUntoggle;

        protected override void Awake()
        {
            base.Awake();
            onToggleOrUntoggle ??= toggleState => { };
            onUntoggle ??= () => { };
        }

        public virtual void ForceSetToggleState(bool state, bool changeGraphic = true)
        {
            Toggled = state;
            
            if (changeGraphic)
            {
                if (Toggled)
                    graphicsController.OnPress();
                else
                    graphicsController.OnRelease();
            }
        }

        public override void OnPointerEnter()
        {
            if (!Interactable) return;

            onHoverEvent.Invoke();
            onHover.Invoke();
            if (!PressStarted && !Toggled && graphicsController != null)
                graphicsController.OnStartHover();
        }

        public override void OnPointerExit()
        {
            if (!Interactable) return;

            if (!PressStarted && !Toggled && graphicsController != null)
                graphicsController.OnEndHover();
        }

        public override void OnPointerDown()
        {
            if (!Interactable) return;

            if (!allowUntoggle && Toggled) return;

            PressStarted = true;
            if (!Toggled)
                graphicsController.OnPress();
            else
                graphicsController.OnRelease();
        }

        public override void OnPointerUp()
        {
            if (!Interactable) return;

            if (!allowUntoggle && Toggled) return;
            
            PressStarted = false;

            if (!PointerInside)
            {
                if (!Toggled)
                    graphicsController.OnRelease();
                else
                    graphicsController.OnPress();
            }
            else
            {
                Toggled = !Toggled;
                onToggleOrUntoggle.Invoke(Toggled);
                
                if (Toggled)
                {
                    onClick.Invoke();
                    inspectorOnClickEvent.Invoke();
                }
                else
                    onUntoggle.Invoke();
            }
        }

        protected override void OnEnable()
        {
            graphicsController.OnButtonEnable();
        }

        protected override void OnDisable()
        {
            graphicsController.OnButtonDisable();
            PressStarted = false;
            PointerInside = false;
        }
    }
}