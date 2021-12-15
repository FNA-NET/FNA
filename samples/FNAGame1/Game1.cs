using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using SDL2;

namespace FNAGame1;

public class Game1 : Game
{
	public Game1()
	{
		GraphicsDeviceManager gdm = new GraphicsDeviceManager(this);

		// Typically you would load a config here...
		gdm.PreferredBackBufferWidth = 1280;
		gdm.PreferredBackBufferHeight = 720;
		gdm.IsFullScreen = false;
		gdm.SynchronizeWithVerticalRetrace = true;

		IsMouseVisible = true;

		// All content loaded will be in a "Content" folder
		Content.RootDirectory = "Content";
	}

	protected override void Initialize()
	{
		Console.WriteLine($"SysRendererType: {GraphicsDevice.SysRendererTypeEXT}");

		/* This is a nice place to start up the engine, after
		 * loading configuration stuff in the constructor
		 */
		base.Initialize();

		Window.KeyDown += (o, e) =>
		{
			if (e.Key == Keys.F1)
			{
				if (Window.ImmService.IsTextInputActive)
					Window.ImmService.StopTextInput();
				else
					Window.ImmService.StartTextInput();
			}
		};

		Window.ImmService.TextInput += (o, e) =>
		{
			Console.WriteLine($"[Text Input] {e.Character}");
		};

		Window.ImmService.TextComposition += (o, e) =>
		{
			var compStr = e.CompositionText.ToString();
			compStr = compStr.Insert(e.CursorPosition, "|");

			Console.WriteLine("--------[Text Composition]--------");
			Console.WriteLine($"CompString {compStr}");
			Console.WriteLine($"CompCursor {e.CursorPosition}");
		};
	}

	protected override void LoadContent()
	{
		// Create the batch...
		_batch = new SpriteBatch(GraphicsDevice);
		_sdfShapeBatch = new SdfShapeBatch(GraphicsDevice);

		// ... then load a texture from ./Content/FNATexture.png
		_texture1 = Content.Load<Texture2D>("Image1");
		_sound = Content.Load<SoundEffect>("Sound/120");
		_grayscaleEffect = Content.Load<Effect>("Effects/Grayscale");

		base.LoadContent();
	}

	protected override void UnloadContent()
	{
		// Clean up after yourself!
		base.UnloadContent();
	}

	private SpriteBatch _batch;
	private SdfShapeBatch _sdfShapeBatch;
	private Texture2D _texture1;
	private SoundEffect _sound;
	private KeyboardState _keyboardPrev = new KeyboardState();
	private Effect _grayscaleEffect;

	protected override void Update(GameTime gameTime)
	{
		KeyboardState keyboardCur = Keyboard.GetState();

		if (keyboardCur.IsKeyDown(Keys.Space) && _keyboardPrev.IsKeyUp(Keys.Space))
		{
			_sound.Play();
		}

		_keyboardPrev = keyboardCur;
		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.CornflowerBlue);

		// Draw the texture to the corner of the screen
		_batch.Begin(sortMode: SpriteSortMode.Deferred,
			effect: _grayscaleEffect,
			blendState: BlendState.AlphaBlend,
			samplerState: SamplerState.PointClamp,
			depthStencilState: DepthStencilState.None,
			rasterizerState: RasterizerState.CullCounterClockwise);
		_batch.Draw(_texture1, Vector2.Zero, Color.White);
		_batch.End();

		_sdfShapeBatch.Begin();

		_sdfShapeBatch.DrawCircle(new Vector2(190, 270), 10, Color.Black, Color.Orange, 2f);
		_sdfShapeBatch.DrawCircle(new Vector2(220, 270), 10, Color.Black, Color.Red, 2f);

		_sdfShapeBatch.DrawRectangle(new Vector2(235, 500), new Vector2(125, 40), Color.Red, Color.Green);
		_sdfShapeBatch.FillRectangle(new Vector2(235, 500 + 45), new Vector2(125, 40), Color.Green);
		_sdfShapeBatch.BorderRectangle(new Vector2(235, 500 + 45 * 2), new Vector2(125, 40), Color.Green);

		_sdfShapeBatch.DrawLine(new Vector2(235, 640), new Vector2(300, 640), 2, Color.Red, Color.Green);
		_sdfShapeBatch.FillLine(new Vector2(235, 650), new Vector2(300, 650), 2, Color.Green);
		_sdfShapeBatch.BorderLine(new Vector2(235, 660), new Vector2(300, 660), 2, Color.Green);

		_sdfShapeBatch.FillLine(new Vector2(235, 670), new Vector2(300, 670), 0, Color.Green);
		_sdfShapeBatch.DrawLine(new Vector2(235, 680), new Vector2(300, 680), 0, Color.Green, Color.Green);

		_sdfShapeBatch.End();

		base.Draw(gameTime);
	}
}
