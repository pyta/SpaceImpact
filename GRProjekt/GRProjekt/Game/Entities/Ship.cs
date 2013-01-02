using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GRProjekt.Game;
using GRProjekt.Game.Entities;

namespace GRProjekt.Ship
{
    public class Ship : GRProjekt.Game.Entities.Object
    {
        #region Members

        private Matrix shipOrientation;
        private float shipV, shipH;
        private  BoundingSphere SphereBoundingBuff;
        private Vector3 previousShipPosition;

        public float    CurrentSpeed    { get; set; }
        public int      MaxSpeed        { get; set; }
        public float    CurrentFuel     { get; set; }
        public int      MaxFuel         { get; set; }

        public ShipPanel Panel { get; set; }

        #endregion

        #region Constructor

        public  Ship(int MaxSpeed, int MaxFuel)
            :base()
        {
            shipV = shipH = 0;
            this.objectSpherePosition = new Vector3(300.0f, 300.0f, 300.0f);
            this.cameraTarget = new Vector3(1000, 0, 0);
            this.CurrentSpeed = 0.0f;
            this.MaxSpeed = MaxSpeed;
            this.MaxFuel = MaxFuel;
        }

        #endregion

        #region Methods 

        public override void LoadContent(ContentManager content)
        {
            this.objectModel = content.Load<Model>("Models/Ship/guard3");

            shipOrientation = Matrix.CreateRotationY(MathHelper.ToRadians(shipH));
            shipOrientation *= Matrix.CreateFromAxisAngle(shipOrientation.Left, MathHelper.ToRadians(shipV));

            World.Current.shipPosition = new Vector3(World.Current.shipPosition.X, World.Current.shipPosition.Y, World.Current.shipPosition.Z);
            World.Current.shipPosition += shipOrientation.Forward * 100.0f;

            objectBoneTransforms = new Matrix[objectModel.Bones.Count];

            foreach (ModelMesh mesh in this.objectModel.Meshes)
            {
                this.objectSphereBounding = BoundingSphere.CreateMerged(objectSphereBounding, mesh.BoundingSphere);
            }

            this.objectSphereBounding.Radius -= 500;
            this.SphereBoundingBuff = this.objectSphereBounding;
            this.previousShipPosition = World.Current.shipPosition;

            this.Panel = new ShipPanel(content.Load<Texture2D>("Graphics/Game/speed"), content.Load<Texture2D>("Graphics/Game/pointer"));
        }

        public override void Draw(GameTime gameTime)
        {
            objectModel.CopyAbsoluteBoneTransformsTo(objectBoneTransforms);

            foreach (ModelMesh mesh in objectModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = objectBoneTransforms[mesh.ParentBone.Index] * shipOrientation * Matrix.CreateTranslation(World.Current.shipPosition);

                    effect.View = Matrix.CreateLookAt(World.Current.cameraPosition, World.Current.shipPosition, Vector3.Up);
                    effect.Projection = World.Current.projectionMatrix;
                }
                mesh.Draw();
            }

            
        }

        public override void Update()
        {
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Left))
                shipH += 0.5f;
            if (ks.IsKeyDown(Keys.Right))
                shipH -= 0.5f;
            if (ks.IsKeyDown(Keys.Down) && shipV < 20)
                shipV += 0.5f;
            if (ks.IsKeyDown(Keys.Up) && shipV > -20)
                shipV -= 0.5f;

            if (ks.IsKeyDown(Keys.Space))
            {
                if (this.CurrentSpeed > 0)
                    this.CurrentSpeed -= 1;
            }

            this.previousShipPosition = World.Current.shipPosition;

            shipOrientation = Matrix.CreateRotationY(MathHelper.ToRadians(shipH));
            shipOrientation *= Matrix.CreateFromAxisAngle(shipOrientation.Left, MathHelper.ToRadians(shipV));

            World.Current.shipPosition = new Vector3(World.Current.shipPosition.X, World.Current.shipPosition.Y, World.Current.shipPosition.Z);
            World.Current.shipPosition += shipOrientation.Forward * this.CurrentSpeed;

            this.objectSphereBounding = this.SphereBoundingBuff.Transform(shipOrientation * Matrix.CreateTranslation(World.Current.shipPosition));

            World.Current.cameraPosition = World.Current.shipPosition + shipOrientation.Backward * 200.0f + shipOrientation.Up*40;

            if (World.Current.shipPosition.Z < -60000 && (this.previousShipPosition.Z > World.Current.shipPosition.Z))
            {
                shipH = 180;
            }
            if (World.Current.shipPosition.Z > 55000 && (this.previousShipPosition.Z < World.Current.shipPosition.Z))
            {
                shipH = 0;
            }
            if (World.Current.shipPosition.X < -60000 && (this.previousShipPosition.X > World.Current.shipPosition.X))
            {
                shipH = 260;
            }
            if (World.Current.shipPosition.X > 55000 && (this.previousShipPosition.X < World.Current.shipPosition.X))
            {
                shipH = 100;
            }

            if (this.CurrentSpeed < (this.MaxSpeed / 2))
                this.CurrentSpeed += 0.5f;
        }

        public void DrawPanel(SpriteBatch spriteBatch, Rectangle bounds)
        {
            this.Panel.Draw(spriteBatch, bounds, this.CurrentSpeed);
        }

        #endregion
    }
}
