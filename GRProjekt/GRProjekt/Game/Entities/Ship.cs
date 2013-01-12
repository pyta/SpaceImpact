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
using GRProjekt.Exceptions; 

namespace GRProjekt.Ship
{
    public class Ship : GRProjekt.Game.Entities.Object
    {
        #region Members

        private Matrix          shipOrientation;
        private float           shipV, shipH;
        private BoundingSphere  SphereBoundingBuff;
        private Vector3         previousShipPosition;
        private float           shipTurn;
        private int             shipPos;

        /// <summary>
        /// Aktualna prędkość
        /// </summary>
        private float _speed = 1;
        public float Speed
        {
            get { return this._speed; }
            set { this._speed = value; }
        }

        /// <summary>
        /// Przyśpieszenie
        /// </summary>
        private float _speeding = 0.5f;
        public float Speeding
        {
            get { return this._speeding; }
            set { this._speeding = value; }
        }

        /// <summary>
        /// Aktualny stan paliwa
        /// </summary>
        private float _fuel = 70.0f;
        public float Fuel
        {
            get { return this._fuel; }
            set { this._fuel = value; }
        }

        /// <summary>
        /// Spalanie statku
        /// </summary>
        private float _combustion = 0.02f;
        public float Combustion
        {
            get { return this._combustion; }
            set { this._combustion = value; }
        }

        /// <summary>
        /// Maksymalna prędkość 0 - 225 (Wartość wynikająca z kąta)
        /// </summary>
        private float _maxSpeed = 150.0f;
        public float MaxSpeed
        {
            get { return this._maxSpeed; }
            set { this._maxSpeed = value; }
        }

        // Wartości stałe wynikające z kąta pomiędzy mminimalym, a maksymanlnym wychyleniem wskazówki licznika
        private const float _maxFuel = 70;      // Wartość zawsze stała - zmiany tylko spalaniem

        #endregion

        #region Constructor

        public  Ship()
            :base()
        {
            shipV = shipH = 0;
            this.objectSpherePosition = new Vector3(300.0f, 300.0f, 300.0f);
            this.cameraTarget = new Vector3(1000, 0, 0);
        }

        #endregion

        #region Methods 
        
        public void PlanetCollision()
        {
            this._speed = 0;
            shipV = 0;
            shipH = 0;
            this.shipTurn = 0;
            this.shipPos = 0;
        }

        public void ShipStart()
        {
            this._speed = 1;
            shipV = 0;
            shipH = 80;
        }

        public override void LoadContent(ContentManager content)
        {
            this.objectModel = content.Load<Model>("Models/Ship/guard3");

            shipOrientation = Matrix.CreateRotationY(MathHelper.ToRadians(shipH));
            shipOrientation *= Matrix.CreateFromAxisAngle(shipOrientation.Left, MathHelper.ToRadians(shipV));

            World.Current.shipPosition = new Vector3(World.Current.shipPosition.X, World.Current.shipPosition.Y, World.Current.shipPosition.Z);
            World.Current.shipPosition += shipOrientation.Forward * this._speed;

            objectBoneTransforms = new Matrix[objectModel.Bones.Count];

            foreach (ModelMesh mesh in this.objectModel.Meshes)
            {
                this.objectSphereBounding = BoundingSphere.CreateMerged(objectSphereBounding, mesh.BoundingSphere);
            }

            this.objectSphereBounding.Radius -= 500;
            this.SphereBoundingBuff = this.objectSphereBounding;
            this.previousShipPosition = World.Current.shipPosition;
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

        private void DecrementSpeed()
        {
            if (this._speed > 50)
            {
                this._speed -= this._speeding;
            }
        }

        public override void Update()
        {
            KeyboardState ks = Keyboard.GetState();
            this.shipPos = 0;
         

            // Zmiana kierunku możliwa tylko jeśli jest statek w ruchu 
            if (this._speed > 0)
            {
                if (ks.IsKeyDown(Keys.Left))
                {
                    shipH += 1.2f;
                    if(shipTurn < 1) shipTurn += 0.01f;
                    shipPos = 1;
                }
                if (ks.IsKeyDown(Keys.Right))
                {
                    shipH -= 1.2f;
                    if (shipTurn > -1) shipTurn -= 0.01f;
                    shipPos = -1;
                }
                if (ks.IsKeyDown(Keys.Up) && shipV > -40)
                    shipV -= 0.2f;
                if (ks.IsKeyDown(Keys.Down) && shipV < 40)
                    shipV += 0.2f;
            }
            // Zmiana prędkości i stanu paliwa
            if (this._speed > 0)
            {
                // Jeśli paliwo jest w bezpiecznym stanie to prędkość rośnie normalnie do maksymalnej dostępnej
                if (this._fuel > 0)
                {
                    if (this._speed < _maxSpeed)
                        this._speed += this._speeding;
                }
                // Jeśli nie prędkość to zaczyna spadać do 0
                else
                {
                    this._speed -= this._speeding;
                }

                // Spalanie
                if (this._fuel > 0)
                    this._fuel -= this._combustion;
            }

            if (shipTurn > 0 && shipPos==0) shipTurn -= 0.1f;
            if (shipTurn < 0 && shipPos==0) shipTurn += 0.1f;


            // Wyjątek spowodowany wyczerpaniem się paliwa
            if (this._speed <= 0 && this._fuel <= 0)
                throw new RunOutFuelException();

            this.previousShipPosition = World.Current.shipPosition;

            shipOrientation = Matrix.CreateRotationY(MathHelper.ToRadians(shipH));
            shipOrientation *= Matrix.CreateFromAxisAngle(shipOrientation.Left, MathHelper.ToRadians(shipV));

            World.Current.shipPosition = new Vector3(World.Current.shipPosition.X, World.Current.shipPosition.Y, World.Current.shipPosition.Z);
            World.Current.shipPosition += shipOrientation.Forward * this._speed;

            //World.Current.shipPosition = Vector3.Transform(World.Current.shipPosition, Matrix.CreateRotationY(MathHelper.ToRadians(0.40f)));

            this.objectSphereBounding = this.SphereBoundingBuff.Transform(shipOrientation * Matrix.CreateTranslation(World.Current.shipPosition));

            World.Current.cameraPosition = World.Current.shipPosition + shipOrientation.Backward * 200.0f + shipOrientation.Up*40;

            //if (World.Current.shipPosition.Z < -60000 && (this.previousShipPosition.Z > World.Current.shipPosition.Z))
            //{
            //    shipH = 180;
            //}
            //if (World.Current.shipPosition.Z > 55000 && (this.previousShipPosition.Z < World.Current.shipPosition.Z))
            //{
            //    shipH = 0;
            //}
            //if (World.Current.shipPosition.X < -60000 && (this.previousShipPosition.X > World.Current.shipPosition.X))
            //{
            //    shipH = 260;
            //}
            //if (World.Current.shipPosition.X > 55000 && (this.previousShipPosition.X < World.Current.shipPosition.X))
            //{
            //    shipH = 100;
            //}
        }

        #endregion
    }
}
