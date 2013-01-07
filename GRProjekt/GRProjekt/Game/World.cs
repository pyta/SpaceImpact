using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GRProjekt.Game
{
    class World
    {
        #region Members

        public Matrix worldMatrix { get; set; }
        public Matrix viewMatrix { get; set; }
        public Matrix projectionMatrix { get; set; }
        public Vector3 cameraPosition { get; set; }
        public Vector3 shipPosition { get; set; }

        private static object _threadLock = new Object();
        private static World current = null;

        #endregion

        #region Constructor

        public World(float aspectRatio=0)
        {
            if (current == null)
            {
                this.cameraPosition = new Vector3(1000, 100.0f, 0);
                this.shipPosition = new Vector3(-100.0f,200.0f, -10000.0f);
                this.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(75.0f), aspectRatio, 1.0f, 1000000.0f);
            }
            current = this;
        }

        #endregion

        #region Propeteries

        public static World Current
        {
            get
            {
                lock (_threadLock)
                    if (current == null)
                        current = new World();

                return current;
            }
        }

        #endregion
    }
}
