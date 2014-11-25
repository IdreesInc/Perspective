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

namespace Perspective_Library
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //This is the List that will contain every object in the game
        List<Object> objects = new List<Object>();
        //Use this variable to record time passed. Remember, the update loop runs 60 times a second
        int ticks;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            //Create the background object
            Object background = new Object();
            //Set the object's type
            background.type = "BACKGROUND";
            //Set the object's image
            background.texture = Content.Load<Texture2D>("Forest");
            //Set the bounds of the object to match the image
            background.fitCollisionRectangle();
            //Add it to the objects list
            objects.Add(background);
            //Create our first bird. I have named it Bob. Don't touch Bob.
            createBird();
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
            //The first thing to do is increase the ticks
            ticks++;

            //To start, we are going to create a bird object 6 times a second
            if (ticks % 10 == 0)
                createBird();
            //Now, we are going to update all the objects in our "objects" List
            foreach (Object obj in objects)
            {
                //We will now update each object depending on what "type" of object it is
                //This is where the AI and animation for each object is updated
                switch (obj.type)
                {
                    case "BIRD":
                        //Here we will move the birds. Since objects should appear to move slower the farther back 
                        //they are, we multiply the movement by the scale, divided by the maximum scale
                        obj.moveHorizontal((int)(-25 * (float)(obj.scale / obj.maxScale)));
                        //Now, we update the animation for the object
                        if (ticks % obj.animationChangeSpeed == 0)
                        {
                            obj.changeAnimation();
                        }
                        break;
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Creates a bird at a random y coordinate on the screen
        /// </summary>
        public void createBird()
        {
            //Create a random coordinate for the bird to be spawned in
            Random rand = new Random();
            int x = 800;
            int y = rand.Next(0, 480);
            //Create a new object called bird and initialize it
            Object bird = new Object();
            //Set the "type" of the object to "BIRD"
            bird.type = "BIRD";
            //Add the animations to the "animations" list contained within the object 
            //The animations are added in the order of which they are meant to appear
            bird.animations.Add(Content.Load<Texture2D>("BrownBird - 1"));
            bird.animations.Add(Content.Load<Texture2D>("BrownBird - 2"));
            bird.animations.Add(Content.Load<Texture2D>("BrownBird - 3"));
            bird.animations.Add(Content.Load<Texture2D>("BrownBird - 2"));
            //This is the default texture the bird will start with
            bird.texture = Content.Load<Texture2D>("BrownBird - 1");
            //Make the collision rectangle's bounds the same as the texture's dimensions
            bird.fitCollisionRectangle();
            //Allow the object to be affected by perspective
            bird.allowPerspective = true;
            //Set the maximum scale of the object
            bird.maxScale = 3f;
            //Set the minimum scale of the object
            bird.startScale = .5f;
            //Set the speed in which the animations should change
            bird.animationChangeSpeed = 5;
            //Set the default location of the bird
            bird.setLocation(x, y);
            //Add the bird to the objects list
            objects.Add(bird);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            //Now we must draw each object in the "objects" list onto the screen
            foreach (Object obj in objects)
            {
                drawObject(obj);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Draws a certain object onto the screen.
        /// </summary>
        /// <param name="obj">The object to be drawn</param>
        public void drawObject(Object obj)
        {
            spriteBatch.Draw(obj.texture, obj.rectangle, Color.White * obj.transparency);
        }
    }
}
