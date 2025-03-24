using System;
using System.Collections.Generic;
using TeoEngine.Transform;
using TeoEngine.Private;

namespace TeoEngine
{
    public abstract class Element
    {
        internal bool penabled = true;
        public bool Enabled { get; internal set; } = true;
        public ObjectTransform Transform = new();
        public Visibility View = new();
        internal ObjectSpace plocation = ObjectSpace.World;
        public int ParentOrder { get; internal set; } = 1;
        internal List<Element> Childrens = [];


        public void SetActive(bool activation) => penabled = activation;
        public int GetNumberOfChildrens() => Childrens.Count;
        public Element GetChild(int index) => Childrens[index];
        internal abstract void DrawOnBuffer(ObjectSpace where);
        public abstract void Dispose();
    }


    public abstract class WithParents : Element
    {
        internal Element parent = Game.Camera;

        internal void Update()
        {
            if (parent.Enabled)
                Enabled = penabled;
            else
                Enabled = false;
            Transform.GlobalPosition = Geometrics.RotatePoint(parent.Transform.GlobalPosition + Transform.Position, parent.Transform.GlobalPosition, parent.Transform.GlobalAngle);
            Transform.GlobalAngle = parent.Transform.GlobalAngle + Transform.Angle;
        }

        public Element GetParent() => parent;
        public void SetParent(Element ob)
        {
            //if (ob.plocation != this.plocation)
                //throw new Exception("Can not set a parent with different Location");

            parent = ob;
            ob.Childrens.Add(this);
            ParentOrder = ob.ParentOrder + 1;
            Application.SortParents();
        }
        public void RemoveParent()
        {
            if (parent != Game.Camera)
            {
                parent.Childrens.Remove(this);
                parent = Game.Camera;
                ParentOrder = 1;
                Application.SortParents();
            }
        }
    }


    public abstract class GameObject : WithParents
    {
        internal void OnCreate()
        {
            Application.Objects.Add(this);
            Application.GameObjects.Add(this);
            Application.Parents.Add(this);
            Application.SortObjects();
            Application.SortGameObjects();
            Application.SortParents();
        }


        public override void Dispose()
        {
            Application.Objects.Remove(this);
            Application.GameObjects.Remove(this);
            Application.Parents.Remove(this);
            GC.SuppressFinalize(this);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }


    public abstract class UIElement : WithParents
    {
        public ObjectSpace Location { get => plocation; set => plocation = value; }

        internal void OnCreate()
        {
            plocation = ObjectSpace.Screen;
            Application.Objects.Add(this);
            Application.UIElements.Add(this);
            Application.Parents.Add(this);
            Application.SortObjects();
            Application.SortUIElements();
            Application.SortParents();
        }

        public override void Dispose()
        {
            Application.Objects.Remove(this);
            Application.UIElements.Remove(this);
            Application.Parents.Remove(this);
            GC.SuppressFinalize(this);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /*private void OnObjectSpaceChange(ObjectSpace obsp)
        {
            if (plocation != obsp)
            {
                /*if (Location == ObjectSpace.World)
                    Transform.Position -= Game.Camera.Transform.Position;
                else
                    Transform.Position += Game.Camera.Transform.Position;
                plocation = obsp;
            }
        }*/
    }



    public class Point : Element
    {
        public Point()
        {
            Transform.Position = Vector2.Zero;
        }

        internal override void DrawOnBuffer(ObjectSpace where)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
