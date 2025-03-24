using System.Collections.Generic;

namespace TeoEngine.Private
{
    static class Application
    {
        public static List<WithParents> Parents = [];
        public static List<Element> Objects = [];
        public static List<GameObject> GameObjects = [];
        public static List<UIElement> UIElements = [];


        public static void Update()
        {
            foreach (WithParents e in Parents)
            {
                e.Update();
            }       
        }

        public static void SortParents()
        {
            Parents.Sort(delegate (WithParents x, WithParents y)
            {
                return x.ParentOrder.CompareTo(y.ParentOrder);
            });
        }

        public static void SortObjects()
        {
            Objects.Sort(delegate (Element x, Element y)
            {
                return x.Transform.ZIndex.CompareTo(y.Transform.ZIndex);
            });
        }

        public static void SortGameObjects()
        {
            GameObjects.Sort(delegate (GameObject x, GameObject y)
            {
                return x.Transform.ZIndex.CompareTo(y.Transform.ZIndex);
            });
        }

        public static void SortUIElements()
        {
            UIElements.Sort(delegate (UIElement x, UIElement y)
            {
                return x.Transform.ZIndex.CompareTo(y.Transform.ZIndex);
            });
        }
    }
}
