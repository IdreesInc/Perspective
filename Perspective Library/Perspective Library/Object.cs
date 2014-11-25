//Copyright 2015 Idrees Hassan. All Rights Reserved

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace Perspective_Library
{
    public class Object
    {
        //Private variables for getters and setters [Do not touch.]
        private Rectangle _rectangle;
        //The object's texture
        public Texture2D texture;
        //Object's type, for use when updating the object [I always make the type completely capitalized]
        public String type;
        //Rotation of object's texture [Does not affect collision]
        public float rotation;
        //Scale of the object
        public float scale = 1;
        //Whether the object should show perspective or not
        public Boolean allowPerspective = false;
        //The scale of the object when it is closest
        public float maxScale = 1;
        //The minimum scale of the object
        public float startScale = 0;
        //List containing the animations of the object, in order
        public List<Texture2D> animations = new List<Texture2D>();
        //Current animation of the object
        public int currentAnimation;
        //Speed at which the animation changes
        public int animationChangeSpeed;
        //Height of the window
        public float heightBounds = 480;
        //Transparency of the object's texture
        public float transparency = 1f;
        //Collision rectangle [Scaled, use for collision and drawing]
        public Rectangle rectangle;
        //Default rectangle [Unscaled, do not touch unless explicitly necessary]
        private Rectangle unscaledRectangle
        {
            get
            {
                return _rectangle;
            }
            set
            {
                this._rectangle = value;
                rectangle = value;
                rectangle.Width = (int)(rectangle.Width * scale);
                rectangle.Height = (int)(rectangle.Height * scale);
            }
        }

        /// <summary>
        /// Fits the collision rectangle to the texture's default width and height
        /// [I recommend using this whenever you create an object, unless you wish to change the dimensions]
        /// </summary>
        public void fitCollisionRectangle()
        {
            Rectangle rectangle = Rectangle.Empty;
            rectangle.Width = texture.Width;
            rectangle.Height = texture.Height;
            this.unscaledRectangle = rectangle;
        }

        /// <summary>
        /// This returns the location of the object
        /// [Use this by default]
        /// </summary>
        /// <returns>Returns the location in Vector2 form</returns>
        public Vector2 getLocation()
        {
            Vector2 loc = new Vector2(unscaledRectangle.X, unscaledRectangle.Y);
            return loc;
        }

        /// <summary>
        /// This translates the object's vertical position by a number of steps.
        /// This method changes the scale of the object so as to make it look like the farther up the screen the object is, the farther away it seems
        /// [Move up: negative number, down: positive]
        /// </summary>
        /// <param name="steps">The amount to move the object by</param>
        public void moveVertical(int steps)
        {
            this.unscaledRectangle = new Rectangle(unscaledRectangle.X, unscaledRectangle.Y + steps, unscaledRectangle.Width, unscaledRectangle.Height);
            updatePerspective();
        }

        /// <summary>
        /// This translates the object's horizontal translation by a number of steps
        /// [Move left: negative number, right: positive]
        /// </summary>
        /// <param name="steps">The amount to move the object by</param>
        public void moveHorizontal(int steps)
        {
            this.unscaledRectangle = new Rectangle(unscaledRectangle.X + steps, unscaledRectangle.Y, unscaledRectangle.Width, unscaledRectangle.Height);
            updatePerspective();
        }

        /// <summary>
        /// Updates the scale of the objects to create perspective
        /// </summary>
        public void updatePerspective()
        {
            if (allowPerspective)
                scale = (unscaledRectangle.Y / heightBounds) * maxScale + startScale;
        }

        /// <summary>
        /// Sets the location of the object to a certain x and y coordinate
        /// [I recommend you use this method by default to move the object]
        /// </summary>
        /// <param name="x">X-Coordinate</param>
        /// <param name="y">Y-Coordinate</param>
        public void setLocation(float x, float y)
        {
            Vector2 loc = new Vector2(x, y);
            this.unscaledRectangle = new Rectangle((int)loc.X, (int)loc.Y, this.unscaledRectangle.Width, this.unscaledRectangle.Height);
            updatePerspective();
        }

        /// <summary>
        /// Change the object's animation frame
        /// </summary>
        public void changeAnimation()
        {
            currentAnimation++;
            if (currentAnimation >= animations.Count)
                currentAnimation = 0;
            texture = (Texture2D)animations[currentAnimation];
        }
    }
}
