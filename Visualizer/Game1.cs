using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using GraphsLibrary;
using System.Linq;

namespace Visualizer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Dictionary<Node<Vector2>, Color> Nodes;
        UnweightedDirectedGraph<Vector2> graph;
        Vector2 GridSize;
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
            // TODO: Add your initialization logic here
            Nodes = new Dictionary<Node<Vector2>, Color>();
            GridSize = new Vector2(20, 20);
            InitNodes(GridSize);
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

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            // TODO: Add your drawing code here
            DrawNodes(GridSize);
            base.Draw(gameTime);
            spriteBatch.End();
        }

        void InitNodes(Vector2 gridSize)
        {
            Dictionary<(int X, int Y), Node<Vector2>> nodes = new Dictionary<(int, int), Node<Vector2>>();

            for (int i = 0; i < gridSize.X; i++)
            {
                for (int j = 0; j < gridSize.Y; j++)
                {
                    Node<Vector2> currentNode;
                    if (!nodes.ContainsKey((i, j)))
                    {
                        nodes.Add((i, j), new Node<Vector2>(new Vector2(i, j)));
                        
                    }

                    currentNode = nodes[(i, j)];
                    graph.AddNode(currentNode.Value);

                     
                    Node<Vector2> currentNeighbor;

                    for (int y = (int)(currentNode.Value.Y-1); y < (int)(currentNode.Value.Y + 2); y++)
                    {
                        for (int x = (int)(currentNode.Value.X - 1); x < (int)(currentNode.Value.X + 2); x++)
                        {
                            if(new Vector2(x, y) != currentNode.Value && y >= 0 && y < gridSize.Y && x >= 0 && x < gridSize.X)
                            {
                                if(!nodes.ContainsKey((x, y)))
                                {
                                    nodes.Add((x, y), new Node<Vector2>(new Vector2(x, y)));
                                }
                                currentNeighbor = nodes[(x, y)];

                                graph.AddEdge(currentNode.Value, currentNeighbor.Value, 1);
                            }
                        }
                    }
                }
            }


            foreach(var kvp in nodes)
            {
                Nodes.Add(kvp.Value, Color.Gray);
            }
        }

        void DrawNodes(Vector2 gridSize)
        {
            Rectangle node = new Rectangle(0, 0, Window.ClientBounds.X/(int)gridSize.X, Window.ClientBounds.Y / (int)gridSize.Y);
            Color[] blankColorData = new Color[node.Width * node.Height];
            Texture2D nodeTexture = new Texture2D(GraphicsDevice, node.Width, node.Height);

            for (int i = 0; i < blankColorData.Length; ++i)
            {
                blankColorData[i] = Color.White;
            }

            nodeTexture.SetData(blankColorData);

            for (int x = 0; x < gridSize.X; x++)
            {
                for (int y = 0; y < gridSize.Y; y++)
                {
                    spriteBatch.Draw(nodeTexture, positionHelper(gridSize, node, new Vector2(x, y)), Nodes[getNodeHelper(new Vector2(x, y))]);
                }
            }

            //spriteBatch.Draw(nodeTexture, new Vector2(10, 10), Color.White);
            //spriteBatch.Draw(nodeTexture, new Vector2(100, 10), Color.Red);

            //Texture2D rectTexture = new Texture2D(graphics.GraphicsDevice, 20, 20);
            ////Color colorData[];
            ////for (int i = 0; i < data.Length; i++)
            ////{
            ////    data[i] = Color.White;
            ////}
            ////rectTexture.SetData(data);
            //spriteBatch.Draw(rectTexture, new Vector2(60, 60), Color.White);
            //spriteBatch.Draw(rectTexture, new Vector2(100, 100), Color.Red);
        }

        Vector2 positionHelper(Vector2 gridSize, Rectangle rect, Vector2 postion)
        {
            return new Vector2(postion.X*(Window.ClientBounds.X / (int)gridSize.X), postion.Y*(Window.ClientBounds.Y / (int)gridSize.Y));
        }

        Node<Vector2> getNodeHelper(Vector2 position)
        {
            return Nodes.Where(kvp => kvp.Key.Value == position).First().Key;
            //foreach(var kvp in Nodes)
            //{
            //    if(kvp.Key.Value == position)
            //    {
            //        return kvp.Key;
            //    }
            //}
            //return null;
        }
    }
}
