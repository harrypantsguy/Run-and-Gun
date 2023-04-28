using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Codebase.UI
{
    public interface IGraphicTarget
    {
        public GraphicRaycaster Raycaster { get; set; }
        public List<GameObject> GraphicsTargets { get; set; }
    }
}