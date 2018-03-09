using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SHMUP
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Hero hero = new Hero();
        List<Hero_Shot> test= new List<Hero_Shot>();
        List<Straight_Enemy> enemy= new List<Straight_Enemy>();

        int shot_offscreen = -1;
        int Enemy_offscreen = 0;
        int shot_timer = 0;
        int Set_timer = 15;
        int Enemy_spawn = 15;

        int[] Enemy_hit = new int[10];
        int[] shot_hit = new int[10];
        int shotVal=-1;
        int enemVal = 1;
        int hitcount = 0;

        bool Kill_Enemy = false;
        bool remove_shot = false;



        SpriteFont TestFont;
        Texture2D HorrifyingBloodSmile;
        Texture2D TestShot;
        Texture2D Trip_Universe;

        String ButtonTest = "HI ALEX, THIS IS YOUR GAME SPEAKING, TRIPPY AS FUCK!!";



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            HorrifyingBloodSmile = Content.Load<Texture2D>("Smile!!");
            Trip_Universe = Content.Load<Texture2D>("Universe");

            TestShot = Content.Load<Texture2D>("Boom");

            TestFont = Content.Load<SpriteFont>("TestFont");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            KeyboardState KeyboardState = Keyboard.GetState();


            hero.doStuffs(KeyboardState);
            if (KeyboardState.IsKeyDown(Keys.Space))
            {
                if (shot_timer == 0)
                {
                    test.Add(new Hero_Shot(hero.location));
                    shot_timer = Set_timer;
                }
            }


            if (shot_timer != 0)
                shot_timer--;


            //code to remove enemies by death
            foreach (Hero_Shot shot in test)
            {
                enemVal = 1;
                shotVal++;
                foreach (Straight_Enemy target in enemy)
                {
                    //overbuilt for multiple shots hitting things at once and one shot hitting multiple things.
                    if (!shot.hit_used())
                    {
                        if (shot.A_Hit(target))
                        {
                            if (!Enemy_hit.Contains(enemVal))
                            {
                                Enemy_hit[hitcount] = enemVal;
                                shot_hit[hitcount] = shotVal;
                                hitcount++;
                                shot.Has_Hit();
                            }
                        }
                    }
                    enemVal++;
                }
            }


            //remove killed enemies. THis lacks an updated feature for the new enemy number. Fixed later
            if (hitcount > 0)
            {
                for (int i = 0; i < hitcount; i++)
                {
                     enemy.RemoveAt(Enemy_hit[i]-1);
                }
                for (int i = 0; i < hitcount; i++)
                {
                    test.RemoveAt(shot_hit[i]);
                }
                Array.Clear(Enemy_hit, 0, 10);
                Array.Clear(shot_hit, 0, 10);
            }
            hitcount = 0;
            shotVal = -1;

            //code to remove a single shot when it reaches the end of the screen.
            //need to only remove one because the speed is constant, and character speed,
            //as long as it is less than the shot, will
            // ensure there will not be two reaching the boundry at the same time.

            //werite later to allow for multiple enemies exiting at once, paths can change this.
            foreach (Hero_Shot shot in test)
            {
                shot.Move();
                if (!remove_shot)
                {
                    shot_offscreen++;
                    if (!shot.isActive())
                    {
                        remove_shot = true;
                    }
                }
            }
            if (remove_shot && test.Count > 0)
                test.RemoveAt(shot_offscreen);
            shot_offscreen = -1;
            remove_shot = false;

            //Creating a Line of straight enemies for testing purposes.
            if (Enemy_spawn == 0)
            {
                enemy.Add(new Straight_Enemy());
                Enemy_spawn = 20;
            }

            Enemy_spawn--;


            //code again to remove enemies that reached the bottome of the screen, as compared to shots hitting the top.
            foreach (Straight_Enemy straight in enemy)
            {
                straight.Move();
                if (!Kill_Enemy)
                {
                    Enemy_offscreen++;
                    if (!straight.isActive())
                    {
                        Kill_Enemy = true;
                    }
                }
            }
            if (Kill_Enemy && enemy.Count>=0)
                enemy.RemoveAt(Enemy_offscreen);

            Enemy_offscreen=-1;
            Kill_Enemy=false;

            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.HotPink);

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            spriteBatch.Draw(HorrifyingBloodSmile, hero.location, Color.White);

            foreach (Hero_Shot shot in test)
            {
                spriteBatch.Draw(TestShot, shot.Location, Color.White);
            }
            foreach (Straight_Enemy straight in enemy)
            {
                spriteBatch.Draw(Trip_Universe, straight.Location, Color.White);
            }
            spriteBatch.DrawString(TestFont, ButtonTest, Vector2.Zero, Color.Red);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
