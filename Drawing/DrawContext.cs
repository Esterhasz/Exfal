using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Exfal.Extensions;

namespace Exfal.Drawing
{
    public class DrawContext : IDisposable
    {
        public SpriteBatch SpriteBatch { get; }
        public Texture2D PixelTexture { get; }
        public Camera Camera { get; internal set; }
        public int Layer { get; internal set; }

        private bool _disposed = false;

        public DrawContext(GraphicsProvider graphics)
        {
            SpriteBatch = graphics.SpriteBatch;
            
            PixelTexture = new Texture2D(graphics.Device, 1, 1);
            PixelTexture.SetData(new[] { Color.White });
        }
        ~DrawContext()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    PixelTexture.Dispose();
                }

                _disposed = true;
            }
        }

        public void String(string str, SpriteFont font, in DrawOptions options)
        {
            String(
                str,
                font,
                options.position,
                options.color,
                options.scale,
                options.origin,
                options.rotationRad,
                options.depth);
        }
        public void String(string str, SpriteFont font, Vector2 position, Color color, float depth = 0)
        {
            String(
                str, 
                font, 
                position, 
                color, 
                Vector2.One, 
                Vector2.Zero, 
                depth);
        }
        public void String(string str, SpriteFont font, Vector2 position, Color color, Vector2 scale, Vector2 origin, float rotation = 0, float depth = 0)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;

            if (scale.X < 0)
            {
                spriteEffects |= SpriteEffects.FlipHorizontally;
                scale.X = -scale.X;
            }

            if (scale.Y < 0)
            {
                spriteEffects |= SpriteEffects.FlipVertically;
                scale.Y = -scale.Y;
            }

            SpriteBatch.DrawString(
                font, 
                str, 
                position, 
                color, 
                rotation, 
                origin, 
                scale, 
                spriteEffects, 
                depth);
        }

        public void Texture(Texture2D texture, in DrawOptions options)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            Vector2 scale = options.scale;
            Vector2 origin = options.origin;

            if (scale.X < 0)
            {
                spriteEffects |= SpriteEffects.FlipHorizontally;
                scale.X = -scale.X;
                origin.X = texture.Width - origin.X - 1;
            }

            if (scale.Y < 0)
            {
                spriteEffects |= SpriteEffects.FlipVertically;
                scale.Y = -scale.Y;
                origin.Y = texture.Height - origin.Y - 1;
            }

            SpriteBatch.Draw(
                texture,
                options.position,
                null,
                options.color,
                options.rotationRad,
                origin,
                scale,
                spriteEffects,
                options.depth);
        }
        public void Texture(Texture2D texture, Vector2 position, Color color, float depth = 0)
        {
            Texture(texture, new DrawOptions
            {
                position = position,
                color = color,
                scale = Vector2.One,
                origin = Vector2.Zero,
                rotationRad = 0,
                depth = depth
            });
        }
        public void Texture(Texture2D texture, Vector2 position, Color color, Vector2 scale, Vector2 origin, float rotation = 0, float depth = 0)
        {
            Texture(texture, new DrawOptions
            {
                position = position,
                color = color,
                scale = scale,
                origin = origin,
                rotationRad = rotation,
                depth = depth
            });
        }

        public void Rectangle(Rectangle rect, Color color)
        {
            var size = rect.Size;
            rect.Size = new(size.X + 1, size.Y + 1);
            SpriteBatch.Draw(PixelTexture, rect, color);
        }
        public void Rectangle(Vector2 position, Vector2 size, Color color, float rotation = 0, float depth = 0)
        {
            SpriteBatch.Draw(
                PixelTexture,
                position,
                null,
                color,
                rotation,
                Vector2.Zero,
                size,
                SpriteEffects.None,
                depth);
        }
        public void Rectangle(Vector2 position, Vector2 size, Color color, Vector2 origin, float rotation = 0, float depth = 0)
        {
            SpriteBatch.Draw(
                PixelTexture,
                position,
                null,
                color,
                rotation,
                origin,
                size,
                SpriteEffects.None,
                depth);
        }

        public void Polygon(List<Vector2> vertices, Color color, float boundThickness = 1f, float depth = 0)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                var current = vertices[i];
                var next = vertices[(i + 1) % vertices.Count];

                Line(current, next, color, Vector2.Zero, boundThickness, depth);
            }
        }
        
        public void Line(Vector2 start, Vector2 end, Color color, float thickness = 1f)
        {
            Line(start, end, color, Vector2.Zero, thickness, 0);
        }
        public void Line(Vector2 start, Vector2 end, Color color, Vector2 origin, float thickness = 1f, float depth = 0)
        {
            Vector2 edge = end - start;
            float angle = MathF.Atan2(edge.Y, edge.X);
            float length = edge.Length();

            SpriteBatch.Draw(
                PixelTexture,
                start,
                null,
                color,
                angle,
                origin,
                new Vector2(length, thickness),
                SpriteEffects.None,
                depth);
        }

        public void Pixel(Vector2 position, Color color)
        {
            SpriteBatch.Draw(PixelTexture, position, color);
        }
        public void Pixel(Vector2 position, Color color, float depth)
        {
            SpriteBatch.Draw(
                PixelTexture,
                position,
                null,
                color,
                0,
                Vector2.Zero,
                Vector2.One,
                SpriteEffects.None,
                depth);
        }
    }
}