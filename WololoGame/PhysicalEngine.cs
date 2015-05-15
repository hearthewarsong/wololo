using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace WololoGame
{
    public interface ILogicObjectForPhysics
    {
        void CollidedWith(ILogicObjectForPhysics otherObject);
    }

    public interface IPhysicsObject
    {
        double X { get; set; }
        double Y { get; set; }
        double Height { get; set; }
        double Width { get; set; }
        bool AffectedByGravitation { get; set; }
        bool AffectedByFriction { get; set; }
        bool HasPhysicalBody { get; set; }

        double MoveIntentionX { get; set; }
        double MoveIntentionY { get; set; }

        bool CantJump { get; set; }
    }
    class PhysicsObject : IPhysicsObject
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public bool AffectedByGravitation { get; set; }
        public bool AffectedByFriction { get; set; }
        public bool HasPhysicalBody { get; set; }

        public double MoveIntentionX { get; set; }
        public double MoveIntentionY { get; set; }
        public bool CantJump { get { return StandingOn == null; } set { PVY = 0; } }

        private ILogicObjectForPhysics logicObject = null;
        public PhysicsObject(double X, double Y, double Height, double Width, ILogicObjectForPhysics logicObject = null)
        {
            this.logicObject = logicObject;
            this.X = X;
            this.Y = Y;
            this.Height = Height;
            this.Width = Width;
        }

        public void CollidedWith(PhysicsObject pO)
        {
            if (this.logicObject != null)
            {
                this.logicObject.CollidedWith(pO.logicObject);
            }
        }

        public double VX { get { return PVX + MoveIntentionX; } }
        public double VY { get { return PVY + MoveIntentionY; } }
        public double PVX { get; set; }
        public double PVY { get; set; }

        public PhysicsObject StandingOn = null;
    }
    class PhysicalEngine : GameComponent
    {
        public double Gravity { get; set; } = 2 / 9.0; // 2 / t^2
        HashSet<PhysicsObject> objects = new HashSet<PhysicsObject>();
        public PhysicalEngine(Game game) : base(game)
        {

        }

        private bool Between(double a, double b, double c)
        {
            return a <= b && a >= c || a <= c && a >= b;
        }
        public override void Update(GameTime gameTime)
        {
            foreach (var item in objects)
            {
                if (item.AffectedByFriction || item.AffectedByGravitation || item.VY != 0 || item.VX != 0)
                {
                    if (item.AffectedByGravitation)
                    {
                        item.PVY -= gameTime.ElapsedGameTime.TotalMilliseconds * Gravity / 1000.0;
                    }
                    if (item.AffectedByFriction && item.StandingOn != null && objects.Contains(item.StandingOn))
                    {
                        item.MoveIntentionX += item.StandingOn.VX;
                    }
                    double newX = item.X + gameTime.ElapsedGameTime.TotalMilliseconds * item.VX;
                    double newY = item.Y + gameTime.ElapsedGameTime.TotalMilliseconds * item.VY;

                    double left = Math.Min(item.X, newX);
                    double right = Math.Max(item.X, newX) + item.Width;
                    double bottom = Math.Min(item.Y, newY);
                    double top = Math.Max(item.Y, newY) + item.Height;
                    PhysicsObject collX = null;
                    PhysicsObject collY = null;
                    if (item.HasPhysicalBody)
                    {
                        foreach (var item2 in objects)
                        {
                            if (item != item2 && item2.HasPhysicalBody)
                            {
                                double X2 = newX < item.X ? item2.X + item2.Width : item2.X;
                                double Y2 = newY < item.Y ? item2.Y + item2.Height : item2.Y;
                                if (Between(X2, left, right) && item.Y <= top && item.Y + item.Height >= bottom)
                                {
                                    newX = X2;
                                    left = Math.Min(item.X, newX);
                                    right = Math.Max(item.X, newX) + item.Width;
                                    item.PVX = 0;
                                    collX = item2;
                                }

                                if (Between(Y2, top, bottom) && item.X <= right && item.X + item.Width >= left)
                                {
                                    newY = Y2;
                                    bottom = Math.Min(item.Y, newY);
                                    top = Math.Max(item.Y, newY) + item.Height;
                                    item.PVY = 0;
                                    collY = item2;
                                }
                            }
                        }
                        if (collY != null)
                        {
                            item.CollidedWith(collY);
                        }
                        if (collY != null)
                        {
                            item.CollidedWith(collX);
                        }
                        item.StandingOn = collY;
                    }
                    foreach (var item2 in objects)
                    {
                        if (item != item2 && (!item2.HasPhysicalBody || !item.HasPhysicalBody))
                        {
                            if ((Between(item2.Y, top, bottom) || Between(item2.Y + item2.Height, top, bottom))
                                && (Between(item2.X, left, right) || Between(item2.X + item2.Width, left, right))
                                )
                            {
                                item.CollidedWith(item2);
                            }
                        }
                    }
                }
            }
            foreach (var item in objects)
            {
                item.MoveIntentionX = 0;
                item.MoveIntentionY = 0;
            }
            base.Update(gameTime);
        }
        public IPhysicsObject AbbObject(double X, double Y, double Width, double Height
            , bool AffectedByGravitation = false, bool AffectedByFriction = false,
            bool HasPhysicalBody = true, ILogicObjectForPhysics logic = null)
        {
            PhysicsObject obj = new PhysicsObject(X, Y, Height, Width, logic);
            obj.AffectedByFriction = AffectedByFriction;
            obj.AffectedByGravitation = AffectedByGravitation;
            obj.HasPhysicalBody = HasPhysicalBody;
            objects.Add(obj);
            return obj;
        }
        public void RemoveObject(IPhysicsObject obj)
        {
            if (objects.Contains((PhysicsObject)obj))
            {
                objects.Remove((PhysicsObject)obj);
            }
            else
            {
                Logger.Get().Log("Physics", LogLevel.error, "Trying to remove ");
            }
        }
    }
}
