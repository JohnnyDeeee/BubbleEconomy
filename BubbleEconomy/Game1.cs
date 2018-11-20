using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace BubbleEconomy {
    public class Game1 : Game {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private int nextTick = 0;
        private int maxTick = 60; // How many ticks in between the 'fixed ticks'
        private KeyboardState previousKeyboardState;
        private List<City> cities = new List<City>();

        public static SpriteFont font;
        public static bool debug = false;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
        }

        protected override void Initialize() {
            //for (int i = 0; i < 5; i++)
            //    cities.Add(new City(Util.RandomVector2(), 100f, 10f, 90f));
            cities.Add(new City(new Vector2(100, 200), 100f, 10f, 90f));
            cities.Add(new City(new Vector2(400, 200), 100f, 70f, 20f));
            cities.Add(new City(new Vector2(600, 400), 100f, 10f, 90f));
            cities.Add(new City(new Vector2(550, 150), 100f, 50f, 50f));

            base.Initialize();
        }
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("font");
        }

        protected override void UnloadContent() {
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Keyboard.GetState().IsKeyUp(Keys.Space) && previousKeyboardState.IsKeyDown(Keys.Space)) // Space - Toggle Debug mode
                debug = !debug;
            if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
                City.neighbourBoundaryRadius = City.neighbourBoundaryRadius > 0 ? City.neighbourBoundaryRadius -= 1 : 0;
            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
                City.neighbourBoundaryRadius += 1;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                maxTick = maxTick > 1 ? maxTick - 1 : 1;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                maxTick += 1;

            previousKeyboardState = Keyboard.GetState();

            // Fixed tick region
            if(nextTick == 0) {
                cities.ForEach(x => x.Update(cities));

                nextTick = maxTick;
            }

            nextTick -= 1;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(new Color(45, 52, 54));

            cities.ForEach(x => x.Draw(spriteBatch));

            spriteBatch.Begin();
            spriteBatch.DrawString(font, $"Press SPACE to toggle debug mode: {(debug ? "off" : "on")}", new Vector2(10, graphics.PreferredBackBufferHeight-20), Color.White);
            spriteBatch.DrawString(font, $"Press +/- to increase/decrease neighbour boundary: {City.neighbourBoundaryRadius}", new Vector2(10, graphics.PreferredBackBufferHeight-40), Color.White);
            spriteBatch.DrawString(font, $"Press UP/DOWN to increase/decrease tick rate: {maxTick}", new Vector2(10, graphics.PreferredBackBufferHeight-60), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
