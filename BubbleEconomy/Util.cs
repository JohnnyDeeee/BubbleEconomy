using Microsoft.Xna.Framework;
using System;

namespace BubbleEconomy {
    public static class Util {
        public static Random rand = new Random();

        public static Vector2 RandomVector2() {
            int x = rand.Next(0, GraphicsDeviceManager.DefaultBackBufferWidth);
            int y = rand.Next(0, GraphicsDeviceManager.DefaultBackBufferHeight);

            return new Vector2(x, y);
        }
    }
}
