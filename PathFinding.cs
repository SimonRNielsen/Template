using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace Template
{
    internal class PathFinding
    {

        public class Tile : GameObject<Enum>
        {
            private bool walkable = true;
            private bool fencePath = false;
            private HashSet<Edge> edges = new HashSet<Edge>();
            private bool discovered = false;
            private Tile parent;

            public int G { get; set; }
            public int H { get; set; }
            public int F => G + H;

            public HashSet<Edge> Edges { get => edges; }
            public bool Discovered { get => discovered; set => discovered = value; }
            public Tile Parent { get => parent; set => parent = value; }
            public bool Walkable
            {
                get => walkable;
                set
                {
                    if (value == false && walkable == true && type.Equals(TileTypes.FencePath))
                    {
                        walkable = value;
                    }
                }
            }
            public bool FencePath { get => fencePath; }

            public Tile(Enum type, Vector2 spawnPos, bool fencePath) : base(type, spawnPos)
            {
                Type = type;
                position = spawnPos;
                switch (type)
                {
                    case TileTypes.Stone:
                    case TileTypes.Fence:
                        walkable = false;
                        break;
                    case TileTypes.FencePath:
                        this.fencePath = true;
                        break;
                    default:
                        break;
                }
            }


            public void CreateEdges(List<Tile> list)
            {
                if (walkable)
                    foreach (Tile other in list)
                    {
                        if (this != other && other.Walkable)
                        {
                            float distance = Vector2.Distance(position, other.Position);
                            if (distance < 91)
                            {
                                int weight;
                                if (distance < 65)
                                    weight = 10;
                                else
                                    weight = 14;
                                Edges.Add(new Edge(weight, this, other));
                            }
                        }
                    }

            }

            public override void LoadContent(ContentManager content)
            {
                //throw new NotImplementedException();
            }
        }

        public class Edge
        {

            #region Fields

            private int weight;
            private Tile from;
            private Tile to;

            #endregion
            #region Properties

            public int Weight { get => weight; private set => weight = value; }
            public Tile From { get => from; }
            //public Tile To { get => to; }
            public Tile To
            {
                get
                {
                    if (to.Walkable)
                        return to;
                    else
                        return default;
                }
            }

            #endregion
            #region Constructor

            public Edge(int weight, Tile from, Tile to)
            {
                this.weight = weight;
                this.from = from;
                this.to = to;
            }

            #endregion

        }

        internal class AStar
        {

            public AStar()
            {

            }


            //public static Dictionary<Vector2, Tile> Cells { get => cells; set => cells = value; }

            public static List<Tile> AStarFindPath(Vector2 startVector, Vector2 endVector, Dictionary<Vector2, Tile> cells)
            {

                HashSet<Tile> openList = new HashSet<Tile>();
                HashSet<Tile> closedList = new HashSet<Tile>();
                //Ryder tidligere data
                openList.Clear();
                closedList.Clear();


                // Sikrer at punkterne findes i cellerne
                if (!cells.ContainsKey(startVector) || !cells.ContainsKey(endVector))
                {
                    return null;
                }

                Tile startTile = cells[startVector];
                Tile endTile = cells[endVector];
                openList.Add(cells[startVector]);

                while (openList.Count > 0)
                {
                    Tile curCell = openList.First();
                    foreach (var t in openList)
                    {
                        if (t.F < curCell.F || t.F == curCell.F && t.H < curCell.H)
                        {
                            curCell = t;
                        }
                    }
                    openList.Remove(curCell);
                    closedList.Add(curCell);

                    if (curCell.Position.X == endVector.X && curCell.Position.Y == endVector.Y)
                    {
                        return AStarRetracePath(cells[startVector], cells[endVector]);
                    }

                    List<Tile> neighbours = AStarGetNeighbours(curCell, cells);
                    foreach (var neighbour in neighbours)
                    {
                        if (closedList.Contains(neighbour))
                            continue;

                        int newMovementCostToNeighbour = curCell.G + AStarGetDistance(curCell.Position, neighbour.Position);

                        if (newMovementCostToNeighbour < neighbour.G || !openList.Contains(neighbour))
                        {
                            neighbour.G = newMovementCostToNeighbour;
                            //udregner H med manhatten princip
                            neighbour.H = (((int)Math.Abs(neighbour.Position.X - endVector.X) + (int)Math.Abs(endVector.Y - neighbour.Position.Y)) * 10);
                            neighbour.Parent = curCell;

                            if (!openList.Contains(neighbour))
                            {
                                openList.Add(neighbour);
                            }
                        }
                    }
                }

                return null;

            }

            public static List<Tile> AStarRetracePath(Tile startVector, Tile endVector)
            {
                List<Tile> path = new List<Tile>();
                Tile currentNode = endVector;

                while (currentNode != startVector)
                {
                    path.Add(currentNode);
                    currentNode = currentNode.Parent;
                }
                path.Add(startVector);
                path.Reverse();

                return path;
            }

            public static int AStarGetDistance(Vector2 neighbourPosition, Vector2 endVector)
            {
                int dstX = Math.Abs((int)neighbourPosition.X - (int)endVector.X);
                int dstY = Math.Abs((int)neighbourPosition.Y - (int)endVector.Y);

                if (dstX > dstY)
                {
                    return 14 * dstY + 10 * (dstX - dstY);
                }
                return 14 * dstX + 10 * (dstY - dstX);
            }



            public static List<Tile> AStarGetNeighbours(Tile curCell, Dictionary<Vector2, Tile> cells)
            {
                List<Tile> neighbours = new List<Tile>(8);
                //var wallSprite = TileTypes.Stone;
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0)
                        {
                            continue;
                        }

                        Tile curNeighbour;
                        if (cells.TryGetValue(new Vector2((int)curCell.Position.X + (i * curCell.Sprite.Width), (int)curCell.Position.Y + (curCell.Sprite.Height * j)), out var cell))
                        {
                            curNeighbour = cell;
                        }
                        else
                        {
                            continue;
                        }

                        if (!curNeighbour.Walkable)  // Hvis en tile ikke længere er walkable
                        {
                            continue; // Spring den over
                        }

                        //hjørner
                        switch (i)
                        {
                            case -1 when j == 1 && (cells[curCell.Position + new Vector2(i * 64, 0)].Type.Equals(TileTypes.Stone) || cells[curCell.Position + new Vector2(0, j * 64)].Type.Equals(TileTypes.Stone)):
                            case 1 when j == 1 && (cells[curCell.Position + new Vector2(i * 64, 0)].Type.Equals(TileTypes.Stone) || cells[curCell.Position + new Vector2(0, j * 64)].Type.Equals(TileTypes.Stone)):
                            case -1 when j == -1 && (cells[curCell.Position + new Vector2(i * 64, 0)].Type.Equals(TileTypes.Stone) || cells[curCell.Position + new Vector2(0, j * 64)].Type.Equals(TileTypes.Stone)):
                            case 1 when j == -1 && (cells[curCell.Position + new Vector2(i * 64, 0)].Type.Equals(TileTypes.Stone) || cells[curCell.Position + new Vector2(0, j * 64)].Type.Equals(TileTypes.Stone)):
                            case -1 when j == 1 && (cells[curCell.Position + new Vector2(i * 64, 0)].Type.Equals(TileTypes.Fence) || cells[curCell.Position + new Vector2(0, j * 64)].Type.Equals(TileTypes.Fence)):
                            case 1 when j == 1 && (cells[curCell.Position + new Vector2(i * 64, 0)].Type.Equals(TileTypes.Fence) || cells[curCell.Position + new Vector2(0, j * 64)].Type.Equals(TileTypes.Fence)):
                            case -1 when j == -1 && (cells[curCell.Position + new Vector2(i * 64, 0)].Type.Equals(TileTypes.Fence) || cells[curCell.Position + new Vector2(0, j * 64)].Type.Equals(TileTypes.Fence)):
                            case 1 when j == -1 && (cells[curCell.Position + new Vector2(i * 64, 0)].Type.Equals(TileTypes.Fence) || cells[curCell.Position + new Vector2(0, j * 64)].Type.Equals(TileTypes.Fence)):
                                continue;
                            default:
                                neighbours.Add(curNeighbour);
                                break;
                        }
                    }

                }

                return neighbours;
            }
        }


        public static Tile BFSMethod(Tile startNode, Tile endNode)
        {
            Queue<Tile> queue = new Queue<Tile>();
            startNode.Discovered = true; //The first node is marked as discovered, so it is not processed agaain
            queue.Enqueue(startNode); //The first node is added to the queue.
            while (queue.Count > 0) //Runs as long as the queue isn't empty
            {
                Tile currentTile = queue.Dequeue(); //The current tile is taken from the queue. 
                if (currentTile == endNode) //If the current tile is the endnode, the destination has ben reached, and is returned.
                {
                    return currentTile;
                }
                foreach (Edge e in currentTile.Edges) //Looks at all the edges from the current tile
                {
                    if (e.To.Walkable) //To make sure there is no mistakes, as non-walkable tiles returns null and should be ignored. 
                    {
                        Tile neighbor = e.To; //The neighbor of the current tile is the destination of the edge
                        if (!neighbor.Discovered) //If the neighbor isn't already discovered, it is marked ad discovered, the current tile is set as parent, and it is added to the queue to be processed later. 
                        {
                            neighbor.Discovered = true;
                            neighbor.Parent = currentTile;
                            queue.Enqueue(neighbor);
                        }
                    }
                }

            }
            return null;

        }
        /// <summary>
        /// Returns a list of tiles, following the path generated by the BFSMethod.
        /// </summary>
        /// <param name="endNode">The tile where the BFS ended</param>
        /// <param name="startNode">The Tile where the BFS started.</param>
        /// <returns></returns>
        public static List<Tile> BFSFindPath(Tile endNode, Tile startNode)
        {
            List<Tile> pathOfNodes = new List<Tile>();
            pathOfNodes.Add(endNode);
            while (endNode != startNode)
            {
                pathOfNodes.Add(endNode.Parent);
                endNode = endNode.Parent;
            }
            pathOfNodes.Reverse();
            return pathOfNodes;
        }
    }
}

