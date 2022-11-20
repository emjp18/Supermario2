using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperMario;

namespace Supermario
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        GameManager m_gamemanager;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            m_gamemanager = new GameManager(this);
        }

        protected override void Initialize()
        {
            Components.Add(m_gamemanager.GetResourceManager());
            Components.Add(m_gamemanager.GetGameObjectManager());
            Components.Add(m_gamemanager.GetLevelManager());
            _graphics.PreferredBackBufferWidth = GameManager.GetRes(true);
            _graphics.PreferredBackBufferHeight = GameManager.GetRes(false);
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            m_gamemanager.LoadLevel(GameManager.GetCurrentLevel());
        }

        protected override void Update(GameTime gameTime)
        {
           
            KeyMouseReader.Update();
            m_gamemanager.UpdateGameState();
            if (Keyboard.GetState().IsKeyDown(Keys.Enter)&&GameManager.GetCurrentLevel() == SuperMario.LEVEL_TYPE.LEVELE)
            {
                m_gamemanager.SaveLevel();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)&&m_gamemanager.GetState()==GAME_STATE.MENU)
                Exit();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

          

            base.Draw(gameTime);
        }
    }
}