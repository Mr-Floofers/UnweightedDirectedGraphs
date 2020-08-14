using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using GraphsLibrary;
using System.Linq;
using System;

namespace Visualizer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private const int screenWidth = 900;
        private const int screenHeight = 900;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<VisualizerNode> Nodes;
        UnweightedDirectedGraph<Vector2> graph;
        Vector2 GridSize;
        Rectangle nodeRect;
        Texture2D pixel;
        VisualizerNode startNode;
        VisualizerNode endNode;
        int count;
        MouseState oldMouseState;
        Pathfinding<Vector2> pathfinder;
        TimeSpan timeSpan;
        bool done;
        Queue<Node<Vector2>> visitedNodes;
        Stack<Node<Vector2>> path;

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
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.ApplyChanges();
            // TODO: Add your initialization logic here
            GridSize = new Vector2(20, 20);

            nodeRect = new Rectangle(0, 0, (Window.ClientBounds.Size.X / (int)GridSize.X) - 1, (Window.ClientBounds.Size.Y / (int)GridSize.Y) - 1);

            graph = new UnweightedDirectedGraph<Vector2>();
            Nodes = new List<VisualizerNode>();
            InitNodes(GridSize);
            IsMouseVisible = true;

            count = 0;
            oldMouseState = Mouse.GetState();
            pathfinder = new Pathfinding<Vector2>(graph);
            done = false;
            timeSpan = new TimeSpan(0);
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

            Color[] blankColorData = new Color[] { Color.White };
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(blankColorData);

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
            var mouseState = Mouse.GetState();
            var keyboardState = Keyboard.GetState();
            timeSpan += gameTime.ElapsedGameTime;

            //Nodes.Keys.Select(k => Nodes[k] = Color.Red);

            //foreach(var edge in Nodes[76].PointingTo)
            //{
            //    getNodeHelper(edge.ToNode).Color = Color.Blue;
            //}
            //Nodes[76].Color = Color.Brown;


            if (done && timeSpan.TotalMilliseconds >= 20 && visitedNodes.Count > 0)
            {
                timeSpan = new TimeSpan(0);
                ((VisualizerNode)visitedNodes.Dequeue()).Color = Color.LightBlue;
            }

            //Nodes.Select(p => p.Value = Color.Red);
            foreach (var node in Nodes)
            {
                //ChangeNodeColor(nodeKvp, Color.Red);
                //node.Color = Color.Red;

                //if(node.Visited)
                //{
                //    node.Color = Color.LightBlue;
                //}
                if (MouseSingleClick(node, mouseState, oldMouseState))
                {
                    //node.Color = Color.Red;
                    if (count == 0)
                    {
                        startNode = node;
                        node.Color = Color.Red;
                        count++;
                    }
                    else if (count == 1)
                    {
                        endNode = node;
                        node.Color = Color.Green;
                        count++;
                    }
                    // do something
                }
                if (MouseClicked(node, mouseState) && count > 1 && node != startNode && node != endNode)
                {
                    node.Color = Color.DarkGray;
                    WallHelper(node);
                }
            }

            if (startNode != null && endNode != null && keyboardState.IsKeyDown(Keys.Enter))
            {
                (path, visitedNodes) = pathfinder.AStar(startNode, endNode, Pathfinding<Vector2>.Manhattan);
                done = true;
            }

            if (done && visitedNodes.Count <= 0)
            {
                foreach (var node in path)
                {
                    ((VisualizerNode)node).Color = Color.DarkBlue;
                }
                //while (path.Count > 0)
                //{
                //    var node = (VisualizerNode)path.Pop();
                //    node.Color = Color.DarkBlue;
                //}
                //startNode.Color = Color.Red;
                //endNode.Color = Color.Green;

            }

            if (startNode != null)
            {
                startNode.Color = Color.Red;
            }
            if (endNode != null)
            {
                endNode.Color = Color.Green;
            }

            oldMouseState = mouseState;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            // TODO: Add your drawing code here
            DrawNodes(GridSize, nodeRect, pixel);
            base.Draw(gameTime);
            spriteBatch.End();
        }

        void InitNodes(Vector2 gridSize)
        {
            Dictionary<(int X, int Y), VisualizerNode> nodes = new Dictionary<(int, int), VisualizerNode>();

            for (int i = 0; i < gridSize.X; i++)
            {
                for (int j = 0; j < gridSize.Y; j++)
                {
                    VisualizerNode currentNode;
                    if (!nodes.ContainsKey((i, j)))
                    {
                        nodes.Add((i, j), new VisualizerNode(new Vector2(i, j)));
                        graph.AddNode(nodes[(i, j)]);
                    }

                    currentNode = nodes[(i, j)];


                    VisualizerNode currentNeighbor;
                    Vector2 neighborPosition = Vector2.Zero;


                    for (int y = (int)(currentNode.Value.Y - 1); y < (int)(currentNode.Value.Y + 2); y++)
                    {
                        for (int x = (int)(currentNode.Value.X - 1); x < (int)(currentNode.Value.X + 2); x++)
                        {
                            neighborPosition.X = x;
                            neighborPosition.Y = y;
                            if (y >= 0 && y < gridSize.Y && x >= 0 && x < gridSize.X)
                            {
                                if (!nodes.ContainsKey((x, y)))
                                {
                                    nodes.Add((x, y), new VisualizerNode(neighborPosition));
                                    graph.AddNode(nodes[(x, y)]);
                                }
                                currentNeighbor = nodes[(x, y)];
                                if (currentNeighbor != currentNode)
                                {
                                    float weight = 1;
                                    if (currentNode.Value.X != neighborPosition.X && currentNode.Value.Y != neighborPosition.Y)
                                    {
                                        weight = (float)System.Math.Sqrt(2);
                                    }
                                    graph.AddEdge(currentNode.Value, currentNeighbor.Value, weight);
                                }
                            }
                        }
                    }


                }
            }


            foreach (var kvp in nodes)
            {

                Nodes.Add(kvp.Value);
                Nodes.Last().Color = Color.LightGray;
                var position = positionHelper(gridSize, Nodes.Last().Value);
                Nodes.Last().HitBox = new Rectangle(position.ToPoint(), nodeRect.Size.ToVector2().ToPoint());
            }
        }

        void DrawNodes(Vector2 gridSize, Rectangle nodeRect, Texture2D pixel)
        {

            VisualizerNode currentNode;


            for (int x = 0; x < gridSize.X; x++)
            {
                for (int y = 0; y < gridSize.Y; y++)
                {
                    currentNode = getNodeHelper(new Vector2(x, y));
                    //spriteBatch.Draw(pixel, positionHelper(gridSize, new Vector2(x, y)), Nodes[getNodeHelper(new Vector2(x, y))]);
                    spriteBatch.Draw(pixel, positionHelper(gridSize, new Vector2(x, y)), currentNode.HitBox, currentNode.Color);
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

        Vector2 positionHelper(Vector2 gridSize, Vector2 postion)
        {
            return new Vector2(postion.X * (Window.ClientBounds.Size.X / (int)gridSize.X), postion.Y * (Window.ClientBounds.Size.Y / (int)gridSize.Y));
        }

        VisualizerNode getNodeHelper(Vector2 position)
        {
            return Nodes.Where(node => node.Value == position).First();
            //foreach(var kvp in Nodes)
            //{
            //    if(kvp.Key.Value == position)
            //    {
            //        return kvp.Key;
            //    }
            //}
            //return null;
        }

        VisualizerNode getNodeHelper(Node<Vector2> node)
        {
            return Nodes.Where(vNode => vNode.Value == node.Value).First();
        }

        public bool MouseClicked(VisualizerNode node, MouseState state)
        {
            var hitbox = node.HitBox;

            if (hitbox.Contains(state.Position) && state.LeftButton == ButtonState.Pressed)
            {
                //node.Color = Color.Red;
                return true;
            }
            return false;
        }

        public bool MouseSingleClick(VisualizerNode node, MouseState state, MouseState oldState)
        {
            var hitbox = node.HitBox;

            if (hitbox.Contains(state.Position) && state.LeftButton == ButtonState.Pressed && oldState.LeftButton != ButtonState.Pressed)
            {
                //node.Color = Color.Red;
                return true;
            }
            return false;
        }

        public void WallHelper(VisualizerNode wallNode)
        {
            if (!graph.Contains(new Vector2(wallNode.Value.X - 1, wallNode.Value.Y - 1)))
            {
                //otherWall = Nodes.Find(node => node.Value == new Vector2(wallNode.Value.X - 1, wallNode.Value.Y - 1));
                graph.RemoveEdge(graph.Search(new Vector2(wallNode.Value.X, wallNode.Value.Y - 1)).PointingTo.Find(edge => edge.ToNode == graph.Search(new Vector2(wallNode.Value.X - 1, wallNode.Value.Y))));
                graph.RemoveEdge(graph.Search(new Vector2(wallNode.Value.X-1, wallNode.Value.Y)).PointingTo.Find(edge => edge.ToNode == graph.Search(new Vector2(wallNode.Value.X, wallNode.Value.Y-1))));
            }
            if (!graph.Contains(new Vector2(wallNode.Value.X + 1, wallNode.Value.Y - 1)))
            {

            }
            if (!graph.Contains(new Vector2(wallNode.Value.X + 1, wallNode.Value.Y + 1)))
            {

            }
            if (!graph.Contains(new Vector2(wallNode.Value.X - 1, wallNode.Value.Y + 1)))
            {

            }
            graph.Remove(wallNode);
        }


    }
}
