using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GRProjekt.Game.Entities
{
    public enum PlanetType
    {
        IcePlanet,
        RockPlanet,
        LavaPlanet,
        RingPlanet,
        Star,
        GasPlanet
    }

    public class Planet : GRProjekt.Game.Entities.Object
    {
        private Vector3 planetPosition;
        private string path;
        private PlanetType planetType;
        private float rotation = 0.0f;

        public Planet(string path, Vector3 planetPosition, PlanetType planetType)
        {
            this.path = path;
            this.planetPosition = planetPosition;
            this.planetType = planetType;
            this.objectSpherePosition = planetPosition;
        }

        public PlanetType GetPlanetType
        {
            get { return this.planetType; }
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            this.objectModel = content.Load<Model>(String.Format("Models/Planets/{0}",this.path));
            objectBoneTransforms = new Matrix[objectModel.Bones.Count];

            foreach (ModelMesh mesh in objectModel.Meshes)
            {
                this.objectSphereBounding = BoundingSphere.CreateMerged(this.objectSphereBounding, mesh.BoundingSphere);
            }

            this.objectSphereBounding = objectSphereBounding.Transform(Matrix.CreateScale(444f) * Matrix.CreateTranslation(objectSpherePosition));
        }
        Matrix asd;

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            objectModel.CopyAbsoluteBoneTransformsTo(objectBoneTransforms);
            foreach (ModelMesh mesh in objectModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World =asd= objectBoneTransforms[mesh.ParentBone.Index] * Matrix.CreateScale(444f) * Matrix.CreateTranslation(planetPosition);

                    effect.View = Matrix.CreateLookAt(World.Current.cameraPosition,World.Current.shipPosition, Vector3.Up);
                    effect.Projection = World.Current.projectionMatrix;
                }
                mesh.Draw();
            }
        }

        public override void Update()
        {
            if (rotation == 360f) rotation = 0.0f;
           // rotation += 0.001f;

      
           // this.objectSphereBounding = objectSphereBounding.Transform(Matrix.CreateScale(444f) * Matrix.CreateTranslation(objectSpherePosition) * Matrix.CreateRotationY(rotation));
        }

    }

    public class Planets : List<Planet>
    {
        public Planets()
        {
            Add(new Planet("RingPlanet2/planet 2274-J", new Vector3(-6000, 5000, -2000), PlanetType.LavaPlanet));
            Add(new Planet("RingPlanet2/planet 2274-J", new Vector3(-10000, 7000, -9000), PlanetType.LavaPlanet));
            Add(new Planet("RingPlanet2/planet 2274-J", new Vector3(-42000, 2000, -1000), PlanetType.LavaPlanet));

            Add(new Planet("Star/star", new Vector3(6000, 5000, -20000), PlanetType.Star));
            Add(new Planet("Star/star", new Vector3(-10000, 7000, 9000), PlanetType.Star));
            Add(new Planet("Star/star", new Vector3(-42000, 2000, 50000), PlanetType.Star));

            Add(new Planet("GasPlanet/Untitled(9)", new Vector3(9000, 4000, -7000), PlanetType.GasPlanet));
            Add(new Planet("GasPlanet/Untitled(9)", new Vector3(-40000, 7000, -11000), PlanetType.GasPlanet));
            Add(new Planet("GasPlanet/Untitled(9)", new Vector3(-2000, 2000, -40000), PlanetType.GasPlanet));

            Add(new Planet("RingPlanet/ring", new Vector3(20000, 6000, 7000), PlanetType.RingPlanet));
            Add(new Planet("RingPlanet/ring", new Vector3(-6000, 3000, -40000), PlanetType.RingPlanet));
            Add(new Planet("RingPlanet/ring", new Vector3(-10000, 10000, -12000), PlanetType.RingPlanet));
        }
    }
}
