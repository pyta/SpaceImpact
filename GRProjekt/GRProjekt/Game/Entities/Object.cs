using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GRProjekt.Game.Entities
{
    public abstract class Object
    {
        #region Members

        protected Model objectModel;
        protected Vector3 objectSpherePosition;
        protected Matrix[] objectBoneTransforms;
        protected Vector3 cameraTarget;
        protected BoundingSphere objectSphereBounding;

        #endregion

        #region Constructor

        public Object()
        {
            cameraTarget = Vector3.Zero;
        }

        #endregion

        public BoundingSphere ObjectSphereBounding
        {
            get { return objectSphereBounding; }
        }

        #region Abstract Methods

        public abstract void LoadContent(ContentManager content);
        public abstract void Draw(GameTime gameTime);
        public abstract void Update();

        #endregion
    }
}
