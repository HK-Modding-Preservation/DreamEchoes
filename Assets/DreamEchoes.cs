using System.Collections.Generic;
using WeaverCore;
using UnityEngine;

namespace DreamEchoes
{
    public class DreamEchoes : WeaverMod
    {
        public DreamEchoes() : base("DreamEchoes") { }

        public override string GetVersion()
        {
            return "1.0.0.0";
        }
        public override List<(string, string)> GetPreloadNames()
        {
            return Preloader.GetPreloadNames();
        }
        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Preloader.Initialize(preloadedObjects);
        }
    }
}
