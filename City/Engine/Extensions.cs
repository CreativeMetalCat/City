using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace City.Engine
{
    public static class Extensions
    {
        public static Box2DX.Common.Vec2 ToBox2DVector(this Microsoft.Xna.Framework.Vector2 vector)
        {
            return new Box2DX.Common.Vec2(vector.X, vector.Y);
        }

        public static Microsoft.Xna.Framework.Vector2 ToVector(this Box2DX.Common.Vec2 vector)
        {
            return new Microsoft.Xna.Framework.Vector2(vector.X, vector.Y);
        }
    }
}
