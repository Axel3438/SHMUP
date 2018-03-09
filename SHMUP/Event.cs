using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SHMUP
{
    class Hero_Shot
    {
        public Vector2 Location = new Vector2();
        public Rectangle Hitbox = new Rectangle(0,0,50,50);
        public bool has_hit=false;

        public Hero_Shot(Vector2 Start)
        {
            Location = Start;
            Hitbox.Y = (int)Location.Y;
            Hitbox.X = (int)Location.X;
        }

        public void Move(){
            Location.Y-=5;
            Hitbox.Y-=5;
        }

        public bool isActive()
        {
            if (Location.Y < 0)
                return false;
            return true;
        }

        public bool A_Hit( Straight_Enemy target){
            if(Hitbox.Intersects(target.Hitbox)){
                return true;
            }
            return false;
        }

        public bool hit_used()
        {
            return has_hit;
        }

        public void Has_Hit()
        {
            has_hit = true;
        }
    }

    class Straight_Enemy
    {
        public Vector2 Location = Vector2.Zero;
        public Rectangle Hitbox = new Rectangle(0, 0, 50, 50);
        public bool was_hit = false;

        public void Move()
        {
            Location.Y+=5;
            Hitbox.Y+=5;
        }

        public bool isActive()
        {
            if (Location.Y > 600)
                return false;
            return true;
        }

        public void hit()
        {
            was_hit = true;
        }

        public bool Check_Hit()
        {
            return was_hit;
        }

    }

    class Hero
    {
        public Vector2 location =Vector2.Zero;

        public Rectangle Hitbox = new Rectangle(0,0, 50, 50);

        public void doStuffs(KeyboardState state){
            if (state.IsKeyDown(Keys.A))
            {
                location.X--;
                Hitbox.X--;
            }
            if (state.IsKeyDown(Keys.D))
            {
                location.X++;
                Hitbox.X++;
            }
            if (state.IsKeyDown(Keys.W))
            {
                location.Y--;
                Hitbox.Y--;
            }
            if (state.IsKeyDown(Keys.S))
            {
                location.Y++;
                Hitbox.Y++;
            }  
        }

    }
}
