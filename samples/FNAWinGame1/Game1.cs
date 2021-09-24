using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using SDL2;

namespace FNAWinGame1;

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
		/* This is a nice place to start up the engine, after
		 * loading configuration stuff in the constructor
		 */
		base.Initialize();

		Window.KeyDown += (o, e) =>
		{
			if (Window.ImmService == null) return;

			if (e.Key == Keys.F1)
			{
				if (Window.ImmService.IsTextInputActive)
					Window.ImmService.StopTextInput();
				else
					Window.ImmService.StartTextInput();
			}
		};

		if (Window.ImmService != null)
		{
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
				var candidateList = Window.ImmService.CandidateList();
				for (int i = 0; i < candidateList.Length; i++)
				{
					if (i == Window.ImmService.CandidateSelection)
						Console.WriteLine($"*{i+1}.Candidates: {candidateList[i]}");
					else
						Console.WriteLine($"{i+1}.Candidates: {candidateList[i]}");
				}
				Console.WriteLine($"Candidate Size: {candidateList.Length}");
				Console.WriteLine($"Candidate Selection: {Window.ImmService.CandidateSelection}");
				Console.WriteLine("==================================");
			};
		}
	}

	protected override void LoadContent()
	{
		// Create the batch...
		_batch = new SpriteBatch(GraphicsDevice);

		base.LoadContent();
	}

	protected override void UnloadContent()
	{
		// Clean up after yourself!
		base.UnloadContent();
	}

	private SpriteBatch _batch;

	protected override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.CornflowerBlue);

		base.Draw(gameTime);
	}
}
