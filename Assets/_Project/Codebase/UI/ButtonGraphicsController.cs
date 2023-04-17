using UnityEngine;

namespace CHR.UI
{
    public abstract class ButtonGraphicsController : MonoBehaviour
    {
        protected CustomButton button;
        public void LinkButton(CustomButton targetButton) => this.button = targetButton;
        public abstract void OnStartHover();
        public abstract void OnEndHover();
        public abstract void OnPress();
        public abstract void OnRelease();
        public abstract void OnButtonEnable();
        public abstract void OnButtonDisable();
        public abstract void OnInteractableStateChange(bool state);
    }
}