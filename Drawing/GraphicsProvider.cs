using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exfal.Drawing
{
    public class GraphicsProvider 
    {
        public SpriteBatch SpriteBatch { get; }
        public GraphicsDevice Device { get; }
        public GraphicsDeviceManager Manager { get; }
        public Texture2D Pixel { get; }

        public GraphicsProvider(SpriteBatch batch, GraphicsDeviceManager manager)
        {
            SpriteBatch = batch;
            Manager = manager;
            Device = manager.GraphicsDevice;
        }
    }
}