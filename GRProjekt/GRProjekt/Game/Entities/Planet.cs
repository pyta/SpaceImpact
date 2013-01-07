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

    public enum MaterialType
    {
        food,
        water,
        fuel,
        crystal,
        steel,
        garbage
    }

    public class Planet : GRProjekt.Game.Entities.Object
    {
        private Vector3 planetPosition;
        private string path;
        private PlanetType planetType;
        private float rotation = 0.0f;
        private double angle = 0;
        private double angularSpeed = 1;
        private double distance;
        private bool docked = false;

        #region PlanetParameters

        private List<Material> materials = new List<Material>();

        private int population;
        private int maxPopulation;
        private float prosperity;
        private float exchanage;

        #endregion

        public Planet(string path, Vector3 planetPosition, PlanetType planetType, double distance, double angularSpeed, int population, int maxpopulation, float prosperity,float exchanage)
        {
            this.path = path;
            this.planetPosition = planetPosition;
            this.planetType = planetType;
            this.objectSpherePosition = planetPosition;
            this.distance = distance;
            this.angularSpeed = angularSpeed;

            this.maxPopulation = maxpopulation;
            this.prosperity = prosperity;
            this.exchanage = exchanage;

            if (planetType == PlanetType.LavaPlanet)
            {
                materials.Add(new Material(MaterialType.crystal, 5000, 800000, 5, 10, 3, 4.8f, 10, -30));
                materials.Add(new Material(MaterialType.food, 300000, 1000000, 1, 10, 1, 0.5f, 10, 150));
                materials.Add(new Material(MaterialType.fuel, 10000, 500000, 8, 30, 8, 7, 30, -40));
                materials.Add(new Material(MaterialType.garbage, 500000, 1200000, -5, -15, -5, 10, 15, 160));
                materials.Add(new Material(MaterialType.steel, 500000, 2000000, 4, 25, 4, 3, 25, 170));
                materials.Add(new Material(MaterialType.water, 600000, 1300000, 10, 35, 10, 9, 35, -100));
            }
            else if (planetType == PlanetType.GasPlanet)
            {
                materials.Add(new Material(MaterialType.crystal, 15000, 900000, 7, 20, 7, 6.5f, 20, -50));
                materials.Add(new Material(MaterialType.food, 500000, 1000000, 4, 12, 4, 10.5f, 10, -30));
                materials.Add(new Material(MaterialType.fuel, 400000, 5000000, 2, 15, 2, 1.4f, 15, 180));
                materials.Add(new Material(MaterialType.garbage, 100000, 6200000, 2, 8, 2, 10, 15, -200));
                materials.Add(new Material(MaterialType.steel, 500000, 2000000, 10, 25, 10, 9, 25, -40));
                materials.Add(new Material(MaterialType.water, 800000, 1500000, 5, 15, 5, 3, 15, -15));            
            }
            else if (planetType == PlanetType.RingPlanet)
            {
                materials.Add(new Material(MaterialType.crystal, 3000000, 5000000, 3, 13, 3, 2.5f, 13, 170));
                materials.Add(new Material(MaterialType.food, 90000, 700000, 8, 24, 8, 6.5f, 24, -40));
                materials.Add(new Material(MaterialType.fuel, 40000, 500000, 4, 15, 4, 3.3f, 15, -20));
                materials.Add(new Material(MaterialType.garbage, 100000, 620000, -3, -5, -3, 10, 15, 80));
                materials.Add(new Material(MaterialType.steel, 500000, 2000000, 10, 25, 10, 9, 25, -40));
                materials.Add(new Material(MaterialType.water, 1000000, 2500000, 2.5f, 15, 2.5f, 2, 15, 150));    
            }
        }

        public void UpdatePlanetState()
        {
            Random rand = new Random();
            this.prosperity = rand.Next(5, 100);
            population += rand.Next(-2, 4);
            if (population > maxPopulation)
            {
                population = maxPopulation;
            }
            else if(population <= 0)
            {
                population = 10;
            }
            exchanage += 100;
            foreach (Material material in materials)
            {
                material.UpdateMaterial(population, prosperity);
            }
        }

        public void PlanetDelivery()
        {
            foreach (Material material in materials)
            {
                exchanage += material.radnomDelivery();
            }
        }

        public Vector3 GetPlanetPositon()
        {
            if (planetType == PlanetType.LavaPlanet)
            {
                return planetPosition + new Vector3(500, 1350, -410);
            }
            else if (planetType == PlanetType.RingPlanet)
            {
                return planetPosition + new Vector3(100, 900, -210);
            }
            else if (planetType == PlanetType.GasPlanet)
            {
                return planetPosition + new Vector3(100, 1200, -210);
            }
            return planetPosition;
        }

        public void Dock()
        {
            this.docked = true;
        }

        public void UnDock()
        {
            this.docked = false;
        }

        public bool IsDocked()
        {
            return docked;
        }

        public void UpdatePosition()
        {
            Vector3 VecPosition = planetPosition;
            angle += angularSpeed / 10000;
            double x = distance * Math.Cos(angle);
            double z = distance * Math.Sin(angle);

            VecPosition.X = (float)x;
            VecPosition.Z = (float)z;

            planetPosition = VecPosition;
            this.objectSpherePosition = VecPosition;


            this.objectSphereBounding.Center = VecPosition;
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
            //rotation += 0.001f;
      
           // this.objectSphereBounding = objectSphereBounding.Transform(Matrix.CreateScale(444f) * Matrix.CreateTranslation(objectSpherePosition) * Matrix.CreateRotationY(rotation));
        }

    }

    public class Planets : List<Planet>
    {
        public Planets()
        {
            //Add(new Planet("RingPlanet2/planet 2274-J", new Vector3(-6000, 5000, -2000), PlanetType.LavaPlanet));
            //Add(new Planet("RingPlanet2/planet 2274-J", new Vector3(-10000, 7000, -9000), PlanetType.LavaPlanet));
            //Add(new Planet("RingPlanet2/planet 2274-J", new Vector3(-42000, 2000, -1000), PlanetType.LavaPlanet));

            //Add(new Planet("Star/star", new Vector3(6000, 5000, -20000), PlanetType.Star));
            //Add(new Planet("Star/star", new Vector3(-10000, 7000, 9000), PlanetType.Star));
            //Add(new Planet("Star/star", new Vector3(-42000, 2000, 50000), PlanetType.Star));

            //Add(new Planet("GasPlanet/Untitled(9)", new Vector3(9000, 4000, -7000), PlanetType.GasPlanet));
            //Add(new Planet("GasPlanet/Untitled(9)", new Vector3(-40000, 7000, -11000), PlanetType.GasPlanet));
            //Add(new Planet("GasPlanet/Untitled(9)", new Vector3(-2000, 2000, -40000), PlanetType.GasPlanet));

            //Add(new Planet("RingPlanet/ring", new Vector3(20000, 6000, 7000), PlanetType.RingPlanet));
            //Add(new Planet("RingPlanet/ring", new Vector3(-6000, 3000, -40000), PlanetType.RingPlanet));
            //Add(new Planet("RingPlanet/ring", new Vector3(-10000, 10000, -12000), PlanetType.RingPlanet));
            float scale = 10000;
            Add(new Planet("Star/star", new Vector3(0, 0, 0), PlanetType.Star,0,0,0,0,0,0));

            Add(new Planet("RingPlanet2/planet 2274-J", new Vector3(2 * scale, 0, 0), PlanetType.LavaPlanet, 2 * scale,1.2,120000,1000000,100,9000000));
            Add(new Planet("RingPlanet2/planet 2274-J", new Vector3(4.5f * scale, 0, 0), PlanetType.LavaPlanet, 4.5f * scale,1,130000,1200000,100,15000000));

            Add(new Planet("RingPlanet/ring", new Vector3(10 * scale, 0, 0), PlanetType.RingPlanet, 10 * scale, 0.6,90000,200000,100,90000000));
            Add(new Planet("RingPlanet/ring", new Vector3(15 * scale, 0, 0), PlanetType.RingPlanet, 15 * scale, 0.4, 85000, 200000, 100, 90000000));

            Add(new Planet("GasPlanet/Untitled(9)", new Vector3(25 * scale, 0, 0), PlanetType.GasPlanet, 25 * scale, 0.2, 50000, 1000000, 100, 9000000));
            Add(new Planet("GasPlanet/Untitled(9)", new Vector3(30 * scale, 0, 0), PlanetType.GasPlanet, 30 * scale, 0.1, 65000, 1000000, 100, 10000000));
        }
    }
}
