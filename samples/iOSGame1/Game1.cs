using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using SDL2;

namespace iOSGame1;

public class Game1 : Game
{
	public Game1()
	{
		GraphicsDeviceManager gdm = new GraphicsDeviceManager(this);

		// Typically you would load a config here...
		gdm.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
		gdm.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
		gdm.IsFullScreen = true;
		gdm.SynchronizeWithVerticalRetrace = true;

		IsMouseVisible = true;

		// All content loaded will be in a "Content" folder
		Content.RootDirectory = "Content";
	}

	protected override void Initialize()
	{
		/* This is a nice place to start up the engine, after
		 * loading configuration stuff in the constructor
		 */
		base.Initialize();

		Window.PointerDown += (o, e) =>
		{
			System.Console.WriteLine($"PointerDown - PointerId: {e.PointerId}, Position: {e.Position}, Button: {e.Button}");
		};
		Window.PointerMove += (o, e) =>
		{
			System.Console.WriteLine($"PointerMove - PointerId: {e.PointerId}, Position: {e.Position}, Delta: {e.Delta}");
		};
		Window.PointerUp += (o, e) =>
		{
			System.Console.WriteLine($"PointerUp - PointerId: {e.PointerId}, Position: {e.Position}");
		};
	}

	protected override void LoadContent()
	{
		// Create the batch...
		_batch = new SpriteBatch(GraphicsDevice);

		// ... then load a texture from ./Content/FNATexture.png
		_texture1 = Content.Load<Texture2D>("Image1");

		var w = _texture1.Width;
		var h = _texture1.Height;

		w = w + (4 - w % 4) % 4;
		h = h + (4 - h % 4) % 4;

		var blocksPerRow = (w + 3) / 4;
		var bytesPerRow = blocksPerRow * 8;

		_etc2Texture = new Texture2D(GraphicsDevice, w, h, false, SurfaceFormat.Rgb8A1Etc2);
		byte[] data = new byte[bytesPerRow * h];
		System.Random.Shared.NextBytes(data);
		_etc2Texture.SetData(data);

		base.LoadContent();
	}

	protected override void UnloadContent()
	{
		// Clean up after yourself!
		base.UnloadContent();
	}

	private SpriteBatch _batch;
	private Texture2D _texture1;
	private Texture2D _etc2Texture;

	protected override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.CornflowerBlue);

		// Draw the texture to the corner of the screen
		_batch.Begin();
		_batch.Draw(_texture1, new Vector2((GraphicsDevice.DisplayMode.Width - _texture1.Width)/2, 0), Color.White);
		_batch.Draw(_etc2Texture, new Vector2((GraphicsDevice.DisplayMode.Width - _etc2Texture.Width)/2, _texture1.Height), Color.White);
		_batch.End();

		base.Draw(gameTime);
	}
}
