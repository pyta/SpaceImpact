using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GRProjekt.Game.Entities
{
    public sealed class  Network : GRProjekt.Game.Entities.Object
    {
        #region Memebers

        private bool networkVisible;

        #endregion

        #region Methods

        public sealed override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            networkVisible = true;
            this.objectModel = content.Load<Model>("Ground");
            this.objectBoneTransforms = new Matrix[objectModel.Bones.Count];
        }

        public bool NetworkVisible
        {
            get { return this.networkVisible; }
            set { this.networkVisible = value; }
        }

        public sealed override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.networkVisible == true)
            {
                this.objectModel.CopyAbsoluteBoneTransformsTo(this.objectBoneTransforms);

                foreach (ModelMesh mesh in this.objectModel.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.TextureEnabled = true;
                        effect.World = this.objectBoneTransforms[mesh.ParentBone.Index];

                        effect.View = Matrix.CreateLookAt(World.Current.cameraPosition, World.Current.shipPosition, Vector3.Up);
                        effect.Projection = World.Current.projectionMatrix;
                    }
                    mesh.Draw();
                }
            }
        }

        public sealed override void Update()
        { }

        #endregion
    }
}
